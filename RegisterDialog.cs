using Android.App;
using Android.Content;
using Android.Widget;
using EmptyProject2025Extended.Data;
using EmptyProject2025Extended.Models;
using EmptyProject2025Extended.Security;

namespace EmptyProject2025Extended
{
    public class RegisterDialog
    {
        // Holds the context of the screen that opened the dialog (example: LoginActivity)
        private readonly Context context;

        // Database helper instance we will use to insert the new user
        private readonly DBHelper db;

        public RegisterDialog(Context context)
        {
            // Store context so we can build UI elements or access system features
            this.context = context;

            // Create a DBHelper instance to interact with the SQLite database
            db = new DBHelper(context);
        }

        // Displays the popup on screen
        public void Show()
        {
            // Create a dialog instance (popup window)
            Dialog dialog = new Dialog(context);

            // Attach the XML layout we created earlier to this dialog
            dialog.SetContentView(Resource.Layout.Register);

            // Connect UI elements from the XML to C# variables
            EditText usernameInput = dialog.FindViewById<EditText>(Resource.Id.editPopupUsername);
            EditText passwordInput = dialog.FindViewById<EditText>(Resource.Id.editPopupPassword);
            Button registerButton = dialog.FindViewById<Button>(Resource.Id.buttonPopupRegister);

            // Runs when user clicks the "Register" button
            registerButton.Click += (s, e) =>
            {
                // Read text from input fields
                string username = usernameInput.Text;
                string password = passwordInput.Text;

                // Basic validation: ensure user didn't leave fields empty
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    Toast.MakeText(context, "Fields cannot be empty", ToastLength.Short).Show();
                    return; // Stop the process
                }

                // Check if username already exists in the database
                var existingUser = db.GetUser(username);
                if (existingUser != null)
                {
                    Toast.MakeText(context, "User already exists", ToastLength.Short).Show();
                    return;
                }

                // Generate a salt for security (temporary system)
                string salt = SecurityUtils.GenerateSalt();

                // Hash the password together with the salt
                string hash = SecurityUtils.HashPassword(password, salt);

                // Create a new User object and insert it into the database
                db.InsertUser(new User(0, username, hash, salt));

                // Notify the user that registration was successful
                Toast.MakeText(context, "User Created!", ToastLength.Short).Show();

                // Close the popup window
                dialog.Dismiss();
            };

            // Finally show the dialog
            dialog.Show();
        }
    }
}
