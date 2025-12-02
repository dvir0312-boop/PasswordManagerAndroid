using Android.App;
using Android.Content;
using EmptyProject2025Extended.Data;
using EmptyProject2025Extended.Models;
using EmptyProject2025Extended.Security;

namespace EmptyProject2025Extended.Presenters
{
    public class LoginPresenter
    {
        private readonly ILoginView view;
        private readonly DBHelper db;

        //**********************************************************
        // CONSTRUCTOR
        //**********************************************************
        public LoginPresenter(ILoginView view, Context context)
        {
            this.view = view;
            db = new DBHelper(context);
        }

        //**********************************************************
        // LOGIN CLICKED
        //**********************************************************
        public void OnLoginClicked()
        {
            // Empty username
            if (string.IsNullOrWhiteSpace(view.Username))
            {
                view.ShowMessage("Please enter username");
                return;
            }

            // Empty password
            if (string.IsNullOrWhiteSpace(view.Password))
            {
                view.ShowMessage("Please enter password");
                return;
            }

            //**********************************************************
            // READ USER FROM DATABASE
            //**********************************************************
            User user = db.GetUser(view.Username);

            if (user == null)
            {
                view.ShowMessage("User does not exist");
                return;
            }

            //**********************************************************
            // VERIFY PASSWORD (TEMP SECURITY)
            //**********************************************************
            bool isValid = SecurityUtils.VerifyPassword(
                view.Password,
                user.PasswordHash,
                user.Salt
            );

            if (isValid)
            {
                view.NavigateToMain();
            }
            else
            {
                view.ShowMessage("Wrong password");
            }
        }
    }
}
