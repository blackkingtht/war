using System.Collections;
using System.Collections.Generic;
using System;

namespace Net.Crypto
{
    public class ZLib
    {
        public static byte[] Zip(byte[] unzip)
        {
            var ziped = LZ4.LZ4Codec.Encode(unzip, 0, unzip.Length);
            var ulength = unzip.Length;
            var buffer = new byte[ziped.Length + 4];
            Buffer.BlockCopy(ziped, 0, buffer, 4, ziped.Length);
            buffer[0] = (byte)((ulength >> 24) & 0xFF);
            buffer[1] = (byte)((ulength >> 16) & 0xFF);
            buffer[2] = (byte)((ulength >> 8) & 0xFF);
            buffer[3] = (byte)((ulength >> 0) & 0xFF);
            return buffer;
        }
        public static byte[] UnZip(byte[] zip)
        {
            int length = (int)(((zip[0] << 24) | (zip[1] << 16) | (zip[2] << 8) | (zip[3] << 0)) & 0xFFFFFFFF);
            return LZ4.LZ4Codec.Decode(zip, 4, zip.Length - 4, length);
        }
    }
}
