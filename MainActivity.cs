using Android.App;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using EmptyProject2025Extended.Data;
using EmptyProject2025Extended.Models;
using EmptyProject2025Extended.Presenters;
using System.Collections.Generic;

namespace EmptyProject2025Extended
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class MainActivity : Activity, IMainView
    {
        private MainPresenter presenter;

        // רכיבי XML
        private Button btnAdd;
        private EditText editSearch;
        private ImageView imgProfile;
        private LinearLayout passwordContainer;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            // חיבור רכיבים מה-XML
            btnAdd = FindViewById<Button>(Resource.Id.btnAdd);
            editSearch = FindViewById<EditText>(Resource.Id.editSearch);
            imgProfile = FindViewById<ImageView>(Resource.Id.imgProfile);
            passwordContainer = FindViewById<LinearLayout>(Resource.Id.passwordContainer);

            // יצירת Presenter
            presenter = new MainPresenter(this, new DBHelper(this));

            // טעינת כל הסיסמאות במסך
            presenter.LoadPasswords();

            // אירועים
            btnAdd.Click += (s, e) =>
            {
                new CreatePasswordDialog(this, presenter).Show();
            };


            editSearch.TextChanged += (s, e) =>
            {
                presenter.Search(editSearch.Text);
            };

            imgProfile.Click += (s, e) =>
            {
                Toast.MakeText(this, "PROFILE MENU", ToastLength.Short).Show();
                // פה נפתח תפריט פרופיל
            };
        }

        // הצגת הודעות
        public void ShowMessage(string message)
        {
            Toast.MakeText(this, message, ToastLength.Short).Show();
        }

        // הצגת רשימת סיסמאות על המסך
        public void DisplayPasswords(List<PasswordInfo> passwords)
        {
            passwordContainer.RemoveAllViews(); // מנקה את הרשימה

            foreach (var item in passwords)
            {
                // מנפחים את השורה
                var row = LayoutInflater.Inflate(Resource.Layout.PasswordRow, null);

                // מוצאים את הכפתורים והטקסט מהשורה
                TextView txtSite = row.FindViewById<TextView>(Resource.Id.txtSiteName);
                Button btnView = row.FindViewById<Button>(Resource.Id.buttonView);
                Button btnEdit = row.FindViewById<Button>(Resource.Id.buttonEdit);
                Button btnDelete = row.FindViewById<Button>(Resource.Id.buttonDelete);

                txtSite.Text = item.Site;

                btnView.Click += (s, e) =>
                {
                    ShowMessage("Viewing: " + item.Site);
                };

                btnEdit.Click += (s, e) =>
                {
                    ShowMessage("Editing: " + item.Site);
                };

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
            Toast.MakeText(this, "ADD POPUP", ToastLength.Short).Show();
        }

        public void OpenEditPopup(PasswordInfo pw)
        {
            Toast.MakeText(this, "EDIT POPUP FOR: " + pw.Site, ToastLength.Short).Show();
        }

        public void ClearInputFields()
        {
            // לא בשימוש במסך הזה – השאר ריק
        }

    }
}
