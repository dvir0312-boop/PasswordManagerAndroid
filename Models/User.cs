namespace EmptyProject2025Extended.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; }

        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }

        public string SecurityQuestion { get; set; }
        public string SecurityAnswerHash { get; set; }
        public string SecurityAnswerSalt { get; set; }

        // Empty constructor
        public User() { }

        // Constructor for login / register
        public User(long id,
                    string username,
                    string passwordHash,
                    string passwordSalt,
                    string securityQuestion,
                    string securityAnswerHash,
                    string securityAnswerSalt)
        {
            Id = id;
            Username = username;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            SecurityQuestion = securityQuestion;
            SecurityAnswerHash = securityAnswerHash;
            SecurityAnswerSalt = securityAnswerSalt;
        }
    }
}
