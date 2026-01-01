using Android.App;
using Android.Content;
using Android.Widget;
using EmptyProject2025Extended.Presenters;
using System.Collections.Generic;

namespace EmptyProject2025Extended
{
    public class RegisterDialog
    {
        private readonly Context context;
        private readonly LoginPresenter presenter;
        private Spinner securityQuestionSpinner;

        public RegisterDialog(Context context, LoginPresenter presenter)
        {
            this.context = context;
            this.presenter = presenter;
        }

        public void Show()
        {
            Dialog dialog = new Dialog(context);
            dialog.SetContentView(Resource.Layout.Register);

            EditText usernameInput = dialog.FindViewById<EditText>(Resource.Id.editPopupUsername);
            EditText passwordInput = dialog.FindViewById<EditText>(Resource.Id.editPopupPassword);
            EditText securityAnswerInput = dialog.FindViewById<EditText>(Resource.Id.editSecurityAnswer);
            securityQuestionSpinner = dialog.FindViewById<Spinner>(Resource.Id.securityQuestionSpinner);
            Button registerButton = dialog.FindViewById<Button>(Resource.Id.buttonPopupRegister);

            // Security questions list (UI only)
            var questions = new List<string>
            {
                "What is the name of your first pet?",
                "What city were you born in?",
                "What is your favorite food?"
            };

            var adapter = new ArrayAdapter<string>(
                context,
                Android.Resource.Layout.SimpleSpinnerItem,
                questions
            );

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            securityQuestionSpinner.Adapter = adapter;

            registerButton.Click += (s, e) =>
            {
                // Call presenter. Only if success -> show recovery words dialog.
                bool ok = presenter.Register(
                    usernameInput.Text,
                    passwordInput.Text,
                    securityQuestionSpinner.SelectedItem.ToString(),
                    securityAnswerInput.Text,
                    out List<string> words
                );

                if (!ok)
                    return;

                // NOTE: Do NOT dismiss register dialog here.
                // Recovery dialog will close BOTH dialogs on Continue.
                new RecoveryWordsDialog(context, dialog, words).Show();
            };

            dialog.Show();
        }
    }
}
