using EmptyProject2025Extended.Data;
using EmptyProject2025Extended.Models;
using System.Collections.Generic;

namespace EmptyProject2025Extended.Presenters
{
    public class MainPresenter
    {
        private readonly IMainView view;
        private readonly DBHelper db;
        private readonly string owner;

        public MainPresenter(IMainView view, DBHelper db, string owner)
        {
            this.view = view;
            this.db = db;
            this.owner = owner;
        }

        // Load all passwords for current owner
        public void LoadPasswords()
        {
            List<PasswordInfo> list = db.ReadAll(owner);
            view.DisplayPasswords(list);
        }

        // Simple search by site (local filter, no DB change!)
        public void Search(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                LoadPasswords();
                return;
            }

            List<PasswordInfo> all = db.ReadAll(owner);
            List<PasswordInfo> filtered = all.FindAll(p =>
                p.Site != null &&
                p.Site.ToLower().Contains(text.ToLower())
            );

            view.DisplayPasswords(filtered);
        }

        public void DeletePassword(long id)
        {
            db.DeleteById(id);
            LoadPasswords();
        }
    }
}
