using Android.App;
using Android.App.Job;
using Android.Content;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using EmptyProject2025Extended.Models;
using EmptyProject2025Extended.Presenters;

namespace EmptyProject2025Extended // תבדוק שזה אותו namespace כמו ב-MainActivity
{
    [Activity(Label = "Login", Theme = "@style/AppTheme", MainLauncher = true)]
    public class LoginActivity : AppCompatActivity, ILoginView
    {
        EditText editTextUsername;
        EditText editTextPassword;
        Button buttonLogin;

        LoginPresenter presenter;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //     // מחבר את המסך לקובץ העיצוב של מסך ההתחברות

            SetContentView(Resource.Layout.Login);

            // מאתר את שדה שם המשתמש מתוך העיצוב
            editTextUsername = FindViewById<EditText>(Resource.Id.editTextUsername);
            // מאתר את שדה הסיסמה מתוך העיצוב

            editTextPassword = FindViewById<EditText>(Resource.Id.editTextPassword);
            // מאתר את כפתור ההתחברות מתוך העיצוב

            buttonLogin = FindViewById<Button>(Resource.Id.buttonLogin);
            presenter = new LoginPresenter(this, this);

            buttonLogin.Click += (s, e) => presenter.OnLoginClicked();

            // Find the register text link
            TextView registerLink = FindViewById<TextView>(Resource.Id.textRegisterLink);
            registerLink.PaintFlags = registerLink.PaintFlags | Android.Graphics.PaintFlags.UnderlineText;

            // Make it clickable like a hyperlink
            registerLink.Click += (s, e) =>
            {
                // Open the Register popup dialog
                RegisterDialog registerDialog = new RegisterDialog(this);
                registerDialog.Show();
            };

        }
        private void OnLoginButtonClicked(object sender, EventArgs e)
        {
            presenter.OnLoginClicked();
        }

        public string Username => editTextUsername.Text;

        public string Password => editTextPassword.Text;

        public void ShowMessage(string message)
        {
            Toast.MakeText(this, message, ToastLength.Short).Show();
        }

        public void NavigateToMain()
        {
            StartActivity(new Intent(this, typeof(MainActivity)));
            Finish();
        }

        // כי ירשנו IView
        public void DisplayPasswords(List<PasswordInfo> passwords)
        {
            // לא רלוונטי ללוגין — משאירים ריק
        }

        public void ClearInputFields()
        {
            // אופציונלי; בלוגין לרוב לא משתמשים.
            editTextUsername.Text = "";
            editTextPassword.Text = "";
        }

    }
}
