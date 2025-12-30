using System;
using System.Security.Cryptography;
using System.Text;
using Android.Util;

namespace EmptyProject2025Extended.Security
{
    public static class EncryptionUtils
    {
        // ===============================================
        //  GET DEVICE ID  (Base key for encryption)
        // ===============================================
        public static string GetDeviceId()
        {
            return Android.Provider.Settings.Secure.GetString(
                Android.App.Application.Context.ContentResolver,
                Android.Provider.Settings.Secure.AndroidId
            );
        }

        // ===============================================
        //  TURN DEVICE ID INTO 32-BYTE AES KEY (SHA-256)
        // ===============================================
        private static byte[] DeriveKey(string baseKey)
        {
            using (SHA256 sha = SHA256.Create())
            {
                return sha.ComputeHash(Encoding.UTF8.GetBytes(baseKey));
            }
        }

        // ===============================================
        //  AES ENCRYPTION
        // ===============================================
        public static string Encrypt(string plainText, string baseKey)
        {
            if (plainText == null)
                return null;

            byte[] key = DeriveKey(baseKey);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.GenerateIV(); // new IV for every encryption

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                // combine IV + cipher
                byte[] combined = new byte[aes.IV.Length + cipherBytes.Length];
                Buffer.BlockCopy(aes.IV, 0, combined, 0, aes.IV.Length);
                Buffer.BlockCopy(cipherBytes, 0, combined, aes.IV.Length, cipherBytes.Length);

                string final = Convert.ToBase64String(combined);

                Log.Debug("AES", "Encrypted: " + final);

                return final;
            }
        }

        // ===============================================
        //  AES DECRYPTION
        // ===============================================
        public static string Decrypt(string encryptedText, string baseKey)
        {
            try
            {
                if (encryptedText == null)
                    return null;

                byte[] combined = Convert.FromBase64String(encryptedText);
                byte[] key = DeriveKey(baseKey);

                using (Aes aes = Aes.Create())
                {
                    aes.Key = key;

                    // extract IV (first 16 bytes)
                    byte[] iv = new byte[16];
                    byte[] cipher = new byte[combined.Length - 16];

                    Buffer.BlockCopy(combined, 0, iv, 0, 16);
                    Buffer.BlockCopy(combined, 16, cipher, 0, cipher.Length);

                    aes.IV = iv;

                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                    byte[] plainBytes = decryptor.TransformFinalBlock(cipher, 0, cipher.Length);

                    string final = Encoding.UTF8.GetString(plainBytes);

                    Log.Debug("AES", "Decrypted: " + final);

                    return final;
                }
            }
            catch (Exception ex)
            {
                Log.Error("AES", "Decrypt ERROR: " + ex.Message);
                return "DECRYPT_ERROR";
            }
        }
    }
}
