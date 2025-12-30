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
            Spinner securityQuestionSpinner = dialog.FindViewById<Spinner>(Resource.Id.securityQuestionSpinner);
            Button registerButton = dialog.FindViewById<Button>(Resource.Id.buttonPopupRegister);

            var questions = new List<string>
            {
                "Choose a security question",
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
                presenter.Register(
                    usernameInput.Text,
                    passwordInput.Text,
                    securityQuestionSpinner.SelectedItem.ToString(),
                    securityAnswerInput.Text
                );

                // 🔐 פתיחת RecoveryWords – הוא אחראי על הסגירה
                new RecoveryWordsDialog(context, dialog).Show();
            };

            dialog.Show();
        }
    }
}
