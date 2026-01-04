using EmptyProject2025Extended.Data;
using EmptyProject2025Extended.Models;
using EmptyProject2025Extended.Security;
using NBitcoin;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public bool Register(
            string username,
            string password,
            string securityQuestion,
            string securityAnswer,
            out List<string> recoveryWordsOut
        )
        {
            recoveryWordsOut = null;
            if( (password.Length < 6) || (!password.Any(ch=> !char.IsLetterOrDigit(ch))))
            {
                view.ShowMessage("Password does not meet the requirments");
                return false;
            }
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(securityAnswer) ||
                securityQuestion == "Choose a security question")
            {
                view.ShowMessage("Please fill all fields");
                return false;
            }

            username = username.Trim().ToLower();

            if (db.GetUser(username) != null)
            {
                view.ShowMessage("User already exists");
                return false;
            }

            // Password
            string passwordSalt = SecurityUtils.GenerateSalt();
            string passwordHash = SecurityUtils.HashPassword(password, passwordSalt);

            // Security answer
            string answerSalt = SecurityUtils.GenerateSalt();
            string answerHash = SecurityUtils.HashPassword(securityAnswer.Trim().ToLower(), answerSalt);

            // Recovery words
            recoveryWordsOut = GenerateRecoveryWords10();
            string recoveryWordsJoined = string.Join(" ", recoveryWordsOut);

            User user = new User(
                0,
                username,
                passwordHash,
                passwordSalt,
                securityQuestion,
                answerHash,
                answerSalt
            );

            db.InsertUser(user, recoveryWordsJoined);

            view.ShowMessage("User Created!");
            return true;
        }

        // ================= RESET PASSWORD =================
        public void ResetPassword(string username, string recoveryWordsInput, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(recoveryWordsInput) ||
                string.IsNullOrWhiteSpace(newPassword))
            {
                view.ShowMessage("Please fill all fields");
                return;
            }

            username = username.Trim().ToLower();
            var user = db.GetUser(username);

            if (user == null)
            {
                view.ShowMessage("User not found");
                return;
            }

            string savedWords = db.GetRecoveryWords(username);
            if (string.IsNullOrWhiteSpace(savedWords))
            {
                view.ShowMessage("No recovery words found");
                return;
            }

            if (NormalizeWords(savedWords) != NormalizeWords(recoveryWordsInput))
            {
                view.ShowMessage("Recovery words do not match");
                return;
            }

            string newSalt = SecurityUtils.GenerateSalt();
            string newHash = SecurityUtils.HashPassword(newPassword, newSalt);

            db.UpdateUserPassword(username, newHash, newSalt);
            view.ShowMessage("Password reset successfully");
        }

        // ================= HELPERS =================

        private static string NormalizeWords(string s)
        {
            var parts = s.Trim().ToLower()
                .Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            return string.Join(" ", parts);
        }

        private static List<string> GenerateRecoveryWords10()
        {
            // ✅ FIX: Convert ReadOnlyCollection to List
            List<string> pool = Wordlist.English.GetWords().ToList();

            Random rnd = new Random();
            HashSet<string> chosen = new HashSet<string>();

            while (chosen.Count < 10)
            {
                string w = pool[rnd.Next(pool.Count)];
                chosen.Add(w);
            }

            return chosen.ToList();
        }
    }
}
