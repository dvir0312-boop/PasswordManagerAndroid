using EmptyProject2025Extended.Data;
using EmptyProject2025Extended.Models;
using EmptyProject2025Extended.Security;

namespace EmptyProject2025Extended.Presenters
{
    public class LoginPresenter
    {
        private readonly ILoginView view;
        private readonly DBHelper db;

        public LoginPresenter(ILoginView view, DBHelper db)
        {
            this.view = view;
            this.db = db;
        }

        // ================= LOGIN =================
        public void Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                view.ShowMessage("Please fill all fields");
                return;
            }
                    
            username = username.Trim().ToLower();
            User user = db.GetUser(username);

            if (user == null)
            {
                view.ShowMessage("User not found");
                return;
            }

            string hash = SecurityUtils.HashPassword(password, user.PasswordSalt);

            if (hash != user.PasswordHash)
            {
                view.ShowMessage("Wrong password");
                return;
            }

            view.NavigateToMain(user.Username);
        }

        // ================= REGISTER =================
        public void Register(
            string username,
            string password,
            string securityQuestion,
            string securityAnswer
        )
        {
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(securityAnswer) ||
                securityQuestion == "Choose a security question")
            {
                view.ShowMessage("Please fill all fields");
                return;
            }

            username = username.Trim().ToLower();

            if (db.GetUser(username) != null)
            {
                view.ShowMessage("User already exists");
                return;
            }

            // 🔐 Password
            string passwordSalt = SecurityUtils.GenerateSalt();
            string passwordHash = SecurityUtils.HashPassword(password, passwordSalt);

            // 🔐 Security Answer
            string answerSalt = SecurityUtils.GenerateSalt();
            string answerHash = SecurityUtils.HashPassword(securityAnswer, answerSalt);

            User user = new User(
                0,
                username,
                passwordHash,
                passwordSalt,
                securityQuestion,
                answerHash,
                answerSalt
            );

            db.InsertUser(user);

            // ❌ אין כאן OpenRecoveryWords
            // UI Flow שייך ל-RegisterDialog
        }
        public void ResetPassword(string username, string recoveryWords, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(recoveryWords) ||
                string.IsNullOrWhiteSpace(newPassword))
            {
                view.ShowMessage("All fields are required");
                return;
            }

            var user = db.GetUser(username.ToLower());
            if (user == null)
            {
                view.ShowMessage("User not found");
                return;
            }

            // Hash recovery words
            string wordsHash = SecurityUtils.HashPassword(
                recoveryWords.Trim().ToLower(),
                user.SecurityAnswerSalt
            );

            if (wordsHash != user.SecurityAnswerHash)
            {
                view.ShowMessage("Recovery words are incorrect");
                return;
            }

            // Create new password
            string newSalt = SecurityUtils.GenerateSalt();
            string newHash = SecurityUtils.HashPassword(newPassword, newSalt);

            user.PasswordSalt = newSalt;
            user.PasswordHash = newHash;

            db.UpdateUserPassword(user);

            view.ShowMessage("Password reset successful");
        }


    }
}
