using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Net.Crypto
{
    public class AesEncryptor
    {
        AesManaged m_AesManager;
        ICryptoTransform m_AesEncryptor;
        public AesEncryptor(byte[] key, byte[] iv)
        {
            m_AesManager = new AesManaged { Mode = CipherMode.ECB, Padding = PaddingMode.None };
            m_AesEncryptor = new CounterModeCryptoTransform(m_AesManager, key, iv);
        }
        public byte[] Encrypt(byte[] data)
        {
            return m_AesEncryptor.TransformFinalBlock(data, 0, data.Length);
        }
    }
}