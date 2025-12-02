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

        public CreatePasswordDialog(Context context, MainPresenter presenter)
        {
            // Store context (needed to show UI)
            this.context = context;

            // Create access to the database
            db = new DBHelper(context);

            // Presenter reference so we can refresh the list after creation
            this.presenter = presenter;
        }

        public void Show()
        {
            // Create popup window
            Dialog dialog = new Dialog(context);

            // Attach our XML layout to it
            dialog.SetContentView(Resource.Layout.CreatePassword);

            // Connect XML fields to variables
            EditText siteInput = dialog.FindViewById<EditText>(Resource.Id.editCreateSite);
            EditText usernameInput = dialog.FindViewById<EditText>(Resource.Id.editCreateUsername);
            EditText passwordInput = dialog.FindViewById<EditText>(Resource.Id.editCreatePassword);
            Button createButton = dialog.FindViewById<Button>(Resource.Id.buttonCreate);

            // Handle save button click
            createButton.Click += (s, e) =>
            {
                string site = siteInput.Text;
                string username = usernameInput.Text;
                string password = passwordInput.Text;

                // Validate fields
                if (string.IsNullOrWhiteSpace(site) ||
                    string.IsNullOrWhiteSpace(username) ||
                    string.IsNullOrWhiteSpace(password))
                {
                    Toast.MakeText(context, "All fields must be filled", ToastLength.Short).Show();
                    return;
                }

                // Create a new password record object
                PasswordInfo data = new PasswordInfo(0, username, password, site);

                // Save to database
                db.Create(data);

                // Close popup
                dialog.Dismiss();

                // Refresh UI list
                presenter.LoadPasswords();
            };

            dialog.Show();
        }
    }
}
