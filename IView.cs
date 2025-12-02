using System.Collections.Generic;
using EmptyProject2025Extended.Models;

namespace EmptyProject2025Extended.Presenters
{
    public interface IView
    {
        /// <summary>
        /// מציג הודעה למשתמש (למשל Toast או Alert)
        /// </summary>
        /// <param name="message">הטקסט להצגה</param>
        void ShowMessage(string message);

        /// <summary>
        /// מציג רשימת סיסמאות על המסך (לדוגמה בתוך ListView / RecyclerView)
        /// </summary>
        /// <param name="passwords">רשימת סיסמאות שמגיעה מה-DB</param>
        void DisplayPasswords(List<PasswordInfo> passwords);

        /// <summary>
        /// מנקה את השדות במסך לאחר שמירה או מחיקה
        /// </summary>
        void ClearInputFields();
    }
}
