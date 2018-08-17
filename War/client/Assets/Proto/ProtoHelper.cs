using ProtoBuf;
using ProtoBuf.Meta;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace ProtoBuf
{
    public class ProtoHelper
    {
        public static byte[] EncodeWithName(object p)
        {
            var type = p.GetType();
            var name = type.Name;
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, p);
                var nbs = Encoding.UTF8.GetBytes(name);
                int nblen = nbs.Length;
                if (nblen > 255)
                {
                    throw new Exception("PB:name->" + name + " is To Long " + nblen + " > 255");
                }
                var buffer = new byte[ms.Length + nbs.Length + 1];
                buffer[0] = (byte)((nblen >> 0) & 0xFF);
                Buffer.BlockCopy(nbs, 0, buffer, 1, nblen);
                ms.Position = 0;
                ms.Read(buffer, 1 + nblen, (int)ms.Length);
                return buffer;
            }
        }

        public static object DecodeWithName(byte[] b, out string name)
        {
            var bytesLen = b[0];
            name = Encoding.UTF8.GetString(b, 1, bytesLen);
            using (var ms = new MemoryStream(b, 1 + bytesLen, b.Length - 1 - bytesLen))
            {
                Type T = Type.GetType("mmopb." + name);
                return Serializer.Deserialize(T, ms);
            }
        }
    }
}
