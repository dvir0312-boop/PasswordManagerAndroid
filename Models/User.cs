using System;

namespace EmptyProject2025Extended.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; }      // username field
        public string PasswordHash { get; set; }  // hashed password
        public string Salt { get; set; }          // salt for hashing

        // Empty constructor
        public User() { }

        // Constructor used when reading from database
        public User(long id, string username, string passwordHash, string salt)
        {
            Id = id;
            Username = username;
            PasswordHash = passwordHash;
            Salt = salt;
        }
    }
}
