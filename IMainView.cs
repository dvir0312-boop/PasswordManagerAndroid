using EmptyProject2025Extended.Models;
using EmptyProject2025Extended.Presenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EmptyProject2025Extended
{
    public interface IMainView : IView
    {
        void ClearList();
        void OpenAddPopup();
        void OpenEditPopup(PasswordInfo pw);
    }

}
