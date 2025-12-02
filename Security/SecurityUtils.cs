namespace EmptyProject2025Extended.Security
{
    public static class SecurityUtils
    {
        //**********************************************************
        // GENERATE SALT (TEMPORARY)
        //**********************************************************
        public static string GenerateSalt()
        {
            // Temporary salt until advanced encryption is added
            return "static_salt";
        }

        //**********************************************************
        // HASH PASSWORD (TEMPORARY)
        //**********************************************************
        public static string HashPassword(string password, string salt)
        {
            // Temporary hash (simple) - will be replaced later
            return password + salt;
        }

        //**********************************************************
        // VERIFY PASSWORD (TEMPORARY)
        //**********************************************************
        public static bool VerifyPassword(string inputPassword, string storedHash, string storedSalt)
        {
            // Temporary comparison - will be replaced with PBKDF2
            string newHash = HashPassword(inputPassword, storedSalt);
            return newHash.Equals(storedHash);
        }
    }
}
