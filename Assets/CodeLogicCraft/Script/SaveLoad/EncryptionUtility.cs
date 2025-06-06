using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class EncryptionUtility
{
    private static string key = "KodeRahasia12345"; // Panjang 16 karakter = 128-bit key

    public static string Encrypt(string plainText)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.GenerateIV();
            using (var encryptor = aes.CreateEncryptor())
            {
                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                // Gabungkan IV + CipherText
                byte[] combined = new byte[aes.IV.Length + cipherBytes.Length];
                Array.Copy(aes.IV, 0, combined, 0, aes.IV.Length);
                Array.Copy(cipherBytes, 0, combined, aes.IV.Length, cipherBytes.Length);

                return Convert.ToBase64String(combined);
            }
        }
    }

    public static string Decrypt(string encryptedText)
    {
        byte[] combined = Convert.FromBase64String(encryptedText);
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);

        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;

            // Ambil IV dan cipher text
            byte[] iv = new byte[16];
            byte[] cipherText = new byte[combined.Length - 16];
            Array.Copy(combined, 0, iv, 0, iv.Length);
            Array.Copy(combined, iv.Length, cipherText, 0, cipherText.Length);

            aes.IV = iv;

            using (var decryptor = aes.CreateDecryptor())
            {
                byte[] plainBytes = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);
                return Encoding.UTF8.GetString(plainBytes);
            }
        }
    }
}
