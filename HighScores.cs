using AndroidX.AppCompat.App;
using EmptyProject2025Extended.Data;

namespace EmptyProject2025Extended
{
    [Activity(Label = "HighScores", Theme = "@style/AppTheme")]
    public class HighScores : AppCompatActivity
    {
        public static List<Score> scoreList { get; set; }
        ScoreAdapter scoreAdapter;
        ListView scoreListView;
        DBHelper dbHelper;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.highscores);
            // Create your application here
            InitObjects();
            InitViews();
        }

        private void InitObjects()
        {
            dbHelper = new DBHelper(this);
        }

        private void InitViews()
        {
            // *** you have to build your score items in database.
            // *** for Example:
            // Android.Graphics.Bitmap pic = BitmapFactory.DecodeResource(Application.Context.Resources, Resource.Drawable.image);
            // Score score = new Score(pic, "Doron", "LEVEL 1", 2250);
            // dbHelper.Insert(score); // add new data to SQLite database
            // ***

            scoreList = dbHelper.SelectAll(); // read all data from SQLite databae

            scoreAdapter = new ScoreAdapter(this, scoreList);

            scoreListView = FindViewById<ListView>(Resource.Id.lvScores);
            scoreListView.Adapter = scoreAdapter;

            scoreListView.ItemClick += ScoreListView_ItemClick;
            scoreListView.ItemLongClick += ScoreListView_ItemLongClick;
        }

        private void ScoreListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            // your code for listview item click action
        }

        private void ScoreListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            // your code for long listview item click action
        }

        protected override void OnResume()
        {
            if (scoreAdapter != null)
            {
                scoreAdapter.NotifyDataSetChanged();  // refresh list after On Resume takes place
            }
            base.OnResume();
        }
    }
}