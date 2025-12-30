using Android.App;
using Android.Content;
using Android.Widget;
using EmptyProject2025Extended.Data;
using EmptyProject2025Extended.Models;
using EmptyProject2025Extended.Presenters;

namespace EmptyProject2025Extended
{
    public class CreatePasswordDialog
    {
        private readonly Context context;
        private readonly DBHelper db;
        private readonly MainPresenter presenter;
        private readonly string owner;

        public CreatePasswordDialog(Context context, MainPresenter presenter, string owner)
        {
            this.context = context;
            this.presenter = presenter;
            this.owner = owner;

            db = new DBHelper(context);
        }

        public void Show()
        {
            Dialog dialog = new Dialog(context);
            dialog.SetContentView(Resource.Layout.CreatePassword);

            EditText siteInput = dialog.FindViewById<EditText>(Resource.Id.editCreateSite);
            EditText usernameInput = dialog.FindViewById<EditText>(Resource.Id.editCreateUsername);
            EditText passwordInput = dialog.FindViewById<EditText>(Resource.Id.editCreatePassword);
            Button btnCreate = dialog.FindViewById<Button>(Resource.Id.buttonCreate);

            btnCreate.Click += (s, e) =>
            {
                string site = siteInput.Text;
                string username = usernameInput.Text;
                string password = passwordInput.Text;

                if (string.IsNullOrWhiteSpace(site) ||
                    string.IsNullOrWhiteSpace(username) ||
                    string.IsNullOrWhiteSpace(password))
                {
                    Toast.MakeText(context, "All fields are required", ToastLength.Short).Show();
                    return;
                }

                // DBHelper already encrypts internally
                PasswordInfo info = new PasswordInfo(
                    0,
                    username,
                    password,
                    site,
                    owner
                );

                db.Create(info);

                presenter.LoadPasswords();
                dialog.Dismiss();
            };

            dialog.Show();
        }
    }
}
