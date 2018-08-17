using Net.Crypto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Net.Tcp
{
    internal class TcpTools
    {
        internal static byte[] Encode(byte[] arr, AesEncryptor aesEncryptor, byte sendIdx)
        {
            bool ziped = false;
            if (arr.Length >= 1024)
            {
                arr = ZLib.Zip(arr);
                ziped = true;
            }
            bool aesed = (new Random().Next() % 10) < 3;
            if (aesed)
            {
                arr = aesEncryptor.Encrypt(arr);
            }
            int crc32 = 0;
            bool crced = (new Random().Next() % 10) < 3;
            if (crced)
            {
                crc32 = Crc.Crc32(arr);
            }

            int alllen = arr.Length + 1 + 4;
            byte[] balllen = BitConverter.GetBytes(alllen);
            byte flag = sendIdx;
            if (ziped)
            {
                flag |= 0x80;
            }
            if (aesed)
            {
                flag |= 0x40;
            }
            if (crced)
            {
                flag |= 0x20;
            }
            var m2 = BitConverter.GetBytes(crc32);
            var os = new MemoryStream();
            os.Write(balllen, 0, 4); //allLen
            os.WriteByte(flag);//flag
            os.Write(m2, 0, 4);//crc
            os.Write(arr, 0, arr.Length);
            return os.ToArray();
        }

        internal static byte[] Decode(byte[] arr, AesDecryptor aesDecryptor, ref int recvIdx)
        {
            byte flag = arr[0];
            bool ziped = ((flag & 0x80) == 0x80);
            bool aesed = ((flag & 0x40) == 0x40);
            bool crced = ((flag & 0x20) == 0x20);
            int idx = flag & 0x1F;
            if (recvIdx == idx)
            {
                recvIdx++;
                if (recvIdx > 0x1F)
                {
                    recvIdx = 0;
                }
                Byte[] bcrc = new Byte[4];
                Buffer.BlockCopy(arr, 1, bcrc, 0, 4);
                int crc32 = BitConverter.ToInt32(bcrc, 0);
                Byte[] data = new Byte[arr.Length - 1 - 4];
                Buffer.BlockCopy(arr, 1 + 4, data, 0, data.Length);
                int ncrc32 = 0;
                if (crced)
                {
                    ncrc32 = Crc.Crc32(data);
                }
                if (ncrc32 == crc32)
                {
                    if (aesed)
                    {
                        data = aesDecryptor.Decrypt(data);
                    }
                    if (ziped)
                    {
                        data = ZLib.UnZip(data);
                    }
                    if (data != null)
                    {
                        return data;
                    }
                    else
                    {
                        TcpLogger.LogError("Recv Decode data null");
                    }
                }
                else
                {
                    TcpLogger.LogError("Recv error crc32 " + crc32 + "   ncrc32" + ncrc32);
                }
            }
            else
            {
                TcpLogger.LogError("Recv error idx " + idx + "   lidx" + recvIdx);
            }
            return null;
        }

        internal static void SplitPack(ref byte[] recvBuffer,ref int receivedSize, ref int bufferSize,Action<byte[]> push)
        {
            //包头大小4字节
            var headSize = 4;

            //当前buf位置指针
            var offset = 0;
            //在mBuffer中可能有多个逻辑数据包，逐个解出
            while (receivedSize - offset > headSize)
            {
                //解包大小
                var packSize = BitConverter.ToInt32(recvBuffer, offset);

                if (receivedSize - offset - headSize >= packSize) //已经接收了一个完整的包
                {
                    //当前buf指针加下包头偏移
                    offset += headSize;

                    //包体大小
                    var pack = new byte[packSize];

                    //解MsgBody
                    Buffer.BlockCopy(recvBuffer, offset, pack, 0, packSize);
                    //当前buf指针加下Body偏移
                    offset += packSize;


                    //存起来
                    push(pack);
                }
                else if (bufferSize < packSize + headSize) //收到的包比buff大,需要做Buff的扩容
                {
                    //要扩容到的Buff大小
                    var newBuffSize = packSize + headSize;

                    //下面这段Baidu的 快速求 > newBuffSize 的 最小的2的幂次方数(原理近似快速的把最高为的1复制到右边所有的位置上然后+1)
                    newBuffSize |= (newBuffSize >> 1);
                    newBuffSize |= (newBuffSize >> 2);
                    newBuffSize |= (newBuffSize >> 4);
                    newBuffSize |= (newBuffSize >> 8);
                    newBuffSize |= (newBuffSize >> 16);
                    newBuffSize++;
                    if (newBuffSize < 0)
                    {
                        newBuffSize >>= 1;
                    }

                    var newBuff = new byte[newBuffSize];

                    //拷贝剩余的有效内容到新的buff
                    //Buffer中真正剩余的有效内容
                    receivedSize -= offset;
                    Buffer.BlockCopy(recvBuffer, offset, newBuff, 0, receivedSize);
                    bufferSize = newBuffSize;
                    recvBuffer = newBuff;
                    offset = 0;
                    break;
                }
                else //收到的包不完整 直接Break
                {
                    break;
                }
            }
            receivedSize -= offset;
            if (receivedSize > 0)
            {
                //buf内容前移
                Buffer.BlockCopy(recvBuffer, offset, recvBuffer, 0, receivedSize);
            }
        }
    }
}
