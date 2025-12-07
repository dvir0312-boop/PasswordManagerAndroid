using EmptyProject2025Extended.Data;
using EmptyProject2025Extended.Security;
using EmptyProject2025Extended.Models;

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

        public void Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password))
            {
                view.ShowMessage("Please fill all fields");
                return;
            }

            username = username.Trim().ToLower();

            User user = db.GetUser(username);
            if (user == null)
            {
                view.ShowMessage("User does not exist");
                return;
            }

            string hash = SecurityUtils.HashPassword(password, user.Salt);
            if (hash != user.PasswordHash)
            {
                view.ShowMessage("Incorrect password");
                return;
            }

            // ✅ PASS OWNER
            view.NavigateToMain(username);
            view.ClearInputFields();
        }
    }
}
