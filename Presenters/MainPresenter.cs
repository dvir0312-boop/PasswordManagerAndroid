using EmptyProject2025Extended.Data;
using EmptyProject2025Extended.Models;
using System.Collections.Generic;

namespace EmptyProject2025Extended.Presenters
{
    public class MainPresenter
    {
        private readonly IMainView view;
        private readonly DBHelper db;

        public MainPresenter(IMainView view, DBHelper db)
        {
            this.view = view;
            this.db = db;
        }

        // Load all passwords from DB and display them
        public void LoadPasswords()
        {
            List<PasswordInfo> all = db.ReadAll();
            view.DisplayPasswords(all);
        }

        // Search passwords by text
        public void Search(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                LoadPasswords();
                return;
            }

            List<PasswordInfo> results = db.Read("site", new string[] { text });

            if (results.Count == 0)
            {
                view.ClearList();
                view.ShowMessage("No results");
                return;
            }

            view.DisplayPasswords(results);
        }

        // Delete a password by ID
        public void DeletePassword(long id)
        {
            db.DeleteById(id);
            LoadPasswords();
            view.ShowMessage("Password deleted");
        }

        // Open add-password popup
        public void AddPassword()
        {
            view.OpenAddPopup();
        }

        // Open edit-password popup
        public void EditPassword(PasswordInfo pw)
        {
            view.OpenEditPopup(pw);
        }
    }
}
