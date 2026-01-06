using Android.App;
using Android.OS;
using Android.Widget;
using EmptyProject2025Extended.Data;
using EmptyProject2025Extended.Models;
using EmptyProject2025Extended.Presenters;
using EmptyProject2025Extended.Security;
using System.Collections.Generic;

namespace EmptyProject2025Extended
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class MainActivity : Activity, IMainView
    {
        private MainPresenter presenter;

        // UI components
        private Button btnAdd;
        private EditText editSearch;
        private ImageView imgProfile;
        private LinearLayout passwordContainer;

        // Logged-in username (OWNER)
        private string currentOwner;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            // Get OWNER from LoginActivity
            currentOwner = Intent.GetStringExtra("owner");
            presenter = new MainPresenter(this, new DBHelper(this), currentOwner);
 
            // Safety check (prevents crashes)
            if (string.IsNullOrWhiteSpace(currentOwner))
            {
                Toast.MakeText(this, "User not identified. Please login again.", ToastLength.Long).Show();
                Finish();
                return;
            }

            // Bind UI
            btnAdd = FindViewById<Button>(Resource.Id.btnAdd);
            editSearch = FindViewById<EditText>(Resource.Id.editSearch);
            imgProfile = FindViewById<ImageView>(Resource.Id.imgProfile);
            passwordContainer = FindViewById<LinearLayout>(Resource.Id.passwordContainer);


            // Initial load
            presenter.LoadPasswords();

            // ---------------- EVENTS ----------------

            // Add password
            btnAdd.Click += (s, e) =>
            {
                new CreatePasswordDialog(this, presenter, currentOwner).Show();
            };

            // Live search
            editSearch.TextChanged += (s, e) =>
            {
                presenter.Search(editSearch.Text);
            };

            // Profile (future use)
            imgProfile.Click += (s, e) =>
            {
                Toast.MakeText(this, "PROFILE MENU", ToastLength.Short).Show();
            };
        }

        // ---------------- IMainView ----------------

        public void ShowMessage(string message)
        {
            Toast.MakeText(this, message, ToastLength.Short).Show();
        }

        public void DisplayPasswords(List<PasswordInfo> passwords)
        {
            passwordContainer.RemoveAllViews();

            foreach (var item in passwords)
            {
                var row = LayoutInflater.Inflate(Resource.Layout.PasswordRow, null);

                TextView txtSite = row.FindViewById<TextView>(Resource.Id.txtSiteName);
                Button btnView = row.FindViewById<Button>(Resource.Id.buttonView);
                Button btnEdit = row.FindViewById<Button>(Resource.Id.buttonEdit);
                Button btnDelete = row.FindViewById<Button>(Resource.Id.buttonDelete);

                txtSite.Text = item.Site;

                // VIEW
                btnView.Click += (s, e) =>
                {
                    new ViewEditDialog(this, item, presenter, currentOwner).Show(false);
                };

                // EDIT
                btnEdit.Click += (s, e) =>
                {
                    new ViewEditDialog(this, item, presenter, currentOwner).Show(true);
                };

                // DELETE
                btnDelete.Click += (s, e) =>
                {
                    presenter.DeletePassword(item.Id);
                };

                passwordContainer.AddView(row);
            }
        }

        public void ClearList()
        {
            passwordContainer.RemoveAllViews();
        }

        public void OpenAddPopup()
        {
            // Not used
        }

        public void OpenEditPopup(PasswordInfo pw)
        {
            // Not used
        }

        public void ClearInputFields()
        {
            // Not used
        }
    }
}
