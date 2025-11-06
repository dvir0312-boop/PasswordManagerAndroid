using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Provider;
using Android.Runtime;
using Android.Util;
using Android.Views;
using System.Timers;
using AndroidX.AppCompat.App;


namespace EmptyProject2025Extended
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IView
    {
        private Presenter presenter;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            presenter = new Presenter(view: this);
        }

        public override bool OnCreateOptionsMenu(IMenu? menu)
        {
            MenuInflater.Inflate(Resource.Menu.main_menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_profile)
            {
                Toast.MakeText(this, "YOU PRESSED PROFILE ICON", ToastLength.Short).Show();
            }
            if (id == Resource.Id.action_start)
            {
                Toast.MakeText(this, "YOU PRESSED START", ToastLength.Short).Show();
            }
            if (id == Resource.Id.action_highScore)
            {
                Toast.MakeText(this, "YOU PRESSED HIGH SCORES", ToastLength.Short).Show();
                Intent intent = new Intent(this, typeof(HighScores));
                StartActivity(intent);
            }
            return base.OnOptionsItemSelected(item);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        // Interface methods
        public void MarkButton(int i, int j, char letter)
        {
            // your code here
        }

        public void DisplayMessage(string message)
        {
            Toast.MakeText(this, message, ToastLength.Short).Show();
        }

        public void ClearBoard()
        {
            // your code here
        }
    }
}