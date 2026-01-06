using Android.App;
using Android.Content;
using Android.Text;
using Android.Widget;
using EmptyProject2025Extended.Data;
using EmptyProject2025Extended.Models;
using EmptyProject2025Extended.Presenters;

namespace EmptyProject2025Extended
{
    public class ViewEditDialog
    {
        private readonly Context context;           // MainActivity
        private readonly DBHelper db;              // Database helper
        private readonly PasswordInfo data;        // Selected password entry
        private readonly MainPresenter presenter;  // To refresh list after update
        private readonly string owner;             // Logged-in username

        // *** FIXED CONSTRUCTOR — 4 PARAMETERS ***
        public ViewEditDialog(Context context, PasswordInfo data, MainPresenter presenter, string owner)
        {
            this.context = context;
            this.data = data;
            this.presenter = presenter;
            this.owner = owner;
            this.db = new DBHelper(context);
        }

        // Show popup
        public void Show(bool startInEditMode = false)
        {
            Dialog dialog = new Dialog(context);
            dialog.SetContentView(Resource.Layout.ViewEditPassword);


            // Connect UI elements
            TextView title = dialog.FindViewById<TextView>(Resource.Id.textTitle);
            EditText siteInput = dialog.FindViewById<EditText>(Resource.Id.editSite);
            EditText usernameInput = dialog.FindViewById<EditText>(Resource.Id.editUsername);
            EditText passwordInput = dialog.FindViewById<EditText>(Resource.Id.editPassword);
            ImageButton toggleBtn = dialog.FindViewById<ImageButton>(Resource.Id.btnTogglePassword);

            Button updateButton = dialog.FindViewById<Button>(Resource.Id.buttonUpdate);
            Button editButton = dialog.FindViewById<Button>(Resource.Id.buttonToggleEdit);
            Button closeButton = dialog.FindViewById<Button>(Resource.Id.buttonClose);

            // Fill fields with existing data
            siteInput.Text = data.Site;
            usernameInput.Text = data.Username;
            passwordInput.Text = data.Password;
            bool isVisible = false;
            passwordInput.InputType = InputTypes.ClassText | InputTypes.TextVariationPassword;
            passwordInput.SetSelection(passwordInput.Text.Length);
            toggleBtn.Click += (s, e) =>
            {
                isVisible = !isVisible;

                if (isVisible)
                {
                    passwordInput.InputType = InputTypes.ClassText | InputTypes.TextVariationVisiblePassword;
                    toggleBtn.SetImageResource(Resource.Drawable.ic_eye_open);
                }
                else
                {
                    passwordInput.InputType = InputTypes.ClassText | InputTypes.TextVariationPassword;
                    toggleBtn.SetImageResource(Resource.Drawable.ic_eye_closed);
                }

                passwordInput.SetSelection(passwordInput.Text.Length);
            };
            // Mode handler
            void SetMode(bool editMode)
            {
                siteInput.Enabled = editMode;
                usernameInput.Enabled = editMode;
                passwordInput.Enabled = editMode;

                if (editMode)
                {
                    // Masked password in edit mode
                    passwordInput.InputType =
                        Android.Text.InputTypes.ClassText |
                        Android.Text.InputTypes.TextVariationPassword;
                }
                else
                {
                    // Show plain text in view mode
                    passwordInput.InputType = Android.Text.InputTypes.ClassText;
                }

                updateButton.Visibility = editMode ? Android.Views.ViewStates.Visible : Android.Views.ViewStates.Gone;
                editButton.Visibility = editMode ? Android.Views.ViewStates.Gone : Android.Views.ViewStates.Visible;
                title.Text = editMode ? "Edit Password" : "View Password";
            }

            // Apply requested mode
            SetMode(startInEditMode);

            // Enter edit mode when pressing "Edit"
            editButton.Click += (s, e) =>
            {
                SetMode(true);
            };

            // Save changes
            updateButton.Click += (s, e) =>
            {
                data.Site = siteInput.Text.Trim();
                data.Username = usernameInput.Text.Trim();
                data.Password = passwordInput.Text.Trim();
                data.Owner = owner;  // ensure correct user

                db.Update(data);
                presenter.LoadPasswords();

                Toast.MakeText(context, "Password updated!", ToastLength.Short).Show();
                dialog.Dismiss();
            };

            // Close dialog
            closeButton.Click += (s, e) =>
            {
                dialog.Dismiss();
            };

            dialog.Show();
        }
    }
}
