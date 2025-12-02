using EmptyProject2025Extended.Presenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptyProject2025Extended
{
    public interface ILoginView : IView
    {
        /// <summary>
        /// שם המשתמש שהוזן במסך הלוגין
        /// </summary>
        string Username { get; }

        /// <summary>
        /// הסיסמה שהוזנה במסך הלוגין
        /// </summary>
        string Password { get; }

        /// <summary>
        /// מעבר למסך הראשי (MainActivity)
        /// </summary>
        void NavigateToMain();
    }
}
