using System;
using System.Security.Cryptography;

namespace SerialUpgrader
{
    public static class FirmwareSecurity
    {
        // 需与下位机 AES_KEY / AES_IV 保持完全一致
        private static readonly byte[] AesKey = new byte[] {
            0x44, 0x33, 0x22, 0x11, 0x88, 0x77, 0x66, 0x55,
            0xCC, 0xBB, 0xAA, 0x99, 0x00, 0xFF, 0xEE, 0xDD
        };

        private static readonly byte[] AesIv = new byte[] {
            0x03, 0x02, 0x01, 0x00, 0x07, 0x06, 0x05, 0x04,
            0x0B, 0x0A, 0x09, 0x08, 0x0F, 0x0E, 0x0D, 0x0C
        };

        // 修改点：这里参数改为了 byte[] plainBin
        public static byte[] EncryptFirmware(byte[] plainBin)
        {
            // 注意：因为直接传入了 byte[]，原本这里的 byte[] plainBin = File.ReadAllBytes(binFilePath); 就被删除了

            // AES CBC 模式要求数据长度为 16 的倍数，末尾用 0xFF 填充补齐
            int paddingLen = 16 - (plainBin.Length % 16);
            if (paddingLen != 16)
            {
                Array.Resize(ref plainBin, plainBin.Length + paddingLen);
                for (int i = plainBin.Length - paddingLen; i < plainBin.Length; i++)
                {
                    plainBin[i] = 0xFF;
                }
            }

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = AesKey;
                aesAlg.IV = AesIv;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.None; // 已手动填充，关闭自带Padding

                using (ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                {
                    return encryptor.TransformFinalBlock(plainBin, 0, plainBin.Length);
                }
            }
        }
    }
}