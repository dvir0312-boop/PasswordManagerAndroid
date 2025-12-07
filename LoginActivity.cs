using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using EmptyProject2025Extended.Data;
using EmptyProject2025Extended.Presenters;

namespace EmptyProject2025Extended
{
    [Activity(Label = "Login", MainLauncher = true)]
    public class LoginActivity : Activity, ILoginView
    {
        EditText editUsername;
        EditText editPassword;
        Button btnLogin;
        TextView txtRegister;

        LoginPresenter presenter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // ✅ MATCHES Login.xml
            SetContentView(Resource.Layout.Login);

            editUsername = FindViewById<EditText>(Resource.Id.editTextUsername);
            editPassword = FindViewById<EditText>(Resource.Id.editTextPassword);
            btnLogin = FindViewById<Button>(Resource.Id.buttonLogin);
            txtRegister = FindViewById<TextView>(Resource.Id.textRegisterLink);

            presenter = new LoginPresenter(this, new DBHelper(this));

            btnLogin.Click += (s, e) =>
            {
                presenter.Login(Username, Password);
            };

            txtRegister.Click += (s, e) =>
            {
                new RegisterDialog(this).Show();
            };
        }

        public string Username => editUsername.Text;
        public string Password => editPassword.Text;

        public void ShowMessage(string message)
        {
            Toast.MakeText(this, message, ToastLength.Short).Show();
        }

        public void NavigateToMain(string owner)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            intent.PutExtra("owner", owner);
            StartActivity(intent);
            Finish();
        }

        public void ClearInputFields()
        {
            editPassword.Text = "";
        }
    }
}
