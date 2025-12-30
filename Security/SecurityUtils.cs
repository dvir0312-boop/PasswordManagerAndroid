using Android.Content;
using System;
using System.Security.Cryptography;
using System.Text;
using Android.Provider;
namespace EmptyProject2025Extended.Security
{
    public static class SecurityUtils
    {
        //**********************************************************
        // GENERATE SALT
        //**********************************************************
        public static string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        //**********************************************************
        // HASH PASSWORD   
        //**********************************************************
        public static string HashPassword(string password, string salt)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password + salt);
                byte[] hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        //**********************************************************
        // VERIFY PASSWORD 
        //**********************************************************
        public static bool VerifyPassword(string typedPassword, string storedHash, string storedSalt)
        {
            string newHash = HashPassword(typedPassword, storedSalt);
            return newHash == storedHash;
        }
        public static string GetAndroidId(Context context)
        {
            return Settings.Secure.GetString(
                context.ContentResolver,
                Settings.Secure.AndroidId
            );
        }
        private const string APP_SALT = "MySuperSalt2025!";

        public static byte[] GenerateAesKey(string owner, string androidId)
        {
            // Combine secret values into a single string
            string combined = owner + androidId + APP_SALT;

            // Hash the combined value using SHA256 to create a 256-bit AES key
            using (SHA256 sha = SHA256.Create())
            {
                return sha.ComputeHash(Encoding.UTF8.GetBytes(combined));
            }
        }


    }
}

