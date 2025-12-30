using Android.App;
using Android.Content;
using Android.Widget;
using EmptyProject2025Extended.Presenters;

namespace EmptyProject2025Extended
{
    public class ResetDialog
    {
        private readonly Context context;
        private readonly LoginPresenter presenter;

        public ResetDialog(Context context, LoginPresenter presenter)
        {
            this.context = context;
            this.presenter = presenter;
        }

        public void Show()
        {
            Dialog dialog = new Dialog(context);
            dialog.SetContentView(Resource.Layout.Reset);

            EditText username = dialog.FindViewById<EditText>(Resource.Id.editResetUsername);
            EditText words = dialog.FindViewById<EditText>(Resource.Id.editRecoveryWords);
            EditText newPassword = dialog.FindViewById<EditText>(Resource.Id.editNewPassword);
            Button reset = dialog.FindViewById<Button>(Resource.Id.buttonResetPassword);

            reset.Click += (s, e) =>
            {
                presenter.ResetPassword(
                    username.Text,
                    words.Text,
                    newPassword.Text
                );
            };

            dialog.Show();
        }
    }
}
