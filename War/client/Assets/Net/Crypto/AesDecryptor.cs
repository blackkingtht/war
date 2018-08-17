using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Net.Crypto
{
    public class AesDecryptor
    {
        AesManaged m_AesManager;
        ICryptoTransform m_AesDecryptor;
        public AesDecryptor(byte[] key, byte[] iv) {
            m_AesManager = new AesManaged { Mode = CipherMode.ECB, Padding = PaddingMode.None };
            m_AesDecryptor = new CounterModeCryptoTransform(m_AesManager, key, iv);
        }
        public byte[] Decrypt(byte[] data)
        {
            return m_AesDecryptor.TransformFinalBlock(data, 0, data.Length);
        }
    }
}