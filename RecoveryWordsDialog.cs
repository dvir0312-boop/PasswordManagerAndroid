using Android.App;
using Android.Content;
using Android.Widget;
using System;
using System.Collections.Generic;

namespace EmptyProject2025Extended
{
    public class RecoveryWordsDialog
    {
        private readonly Context context;
        private readonly Dialog registerDialog;
        private Dialog dialog;

        public RecoveryWordsDialog(Context context, Dialog registerDialog)
        {
            this.context = context;
            this.registerDialog = registerDialog;
        }

        public void Show()
        {
            dialog = new Dialog(context);
            dialog.SetContentView(Resource.Layout.RecoveryWords);

            TextView[] wordViews =
            {
                dialog.FindViewById<TextView>(Resource.Id.textWord1),
                dialog.FindViewById<TextView>(Resource.Id.textWord2),
                dialog.FindViewById<TextView>(Resource.Id.textWord3),
                dialog.FindViewById<TextView>(Resource.Id.textWord4),
                dialog.FindViewById<TextView>(Resource.Id.textWord5),
                dialog.FindViewById<TextView>(Resource.Id.textWord6),
                dialog.FindViewById<TextView>(Resource.Id.textWord7),
                dialog.FindViewById<TextView>(Resource.Id.textWord8),
                dialog.FindViewById<TextView>(Resource.Id.textWord9),
                dialog.FindViewById<TextView>(Resource.Id.textWord10)
            };

            List<string> words = GenerateRecoveryWords();
            for (int i = 0; i < 10; i++)
            {
                wordViews[i].Text = $"{i + 1}. {words[i]}";
            }

            CheckBox checkSaved = dialog.FindViewById<CheckBox>(Resource.Id.checkSavedWords);
            Button btnContinue = dialog.FindViewById<Button>(Resource.Id.buttonContinueRecovery);
            Button btnCancel = dialog.FindViewById<Button>(Resource.Id.buttonCancelRecovery);

            btnContinue.Enabled = false;

            checkSaved.CheckedChange += (s, e) =>
            {
                btnContinue.Enabled = e.IsChecked;
            };

            btnContinue.Click += (s, e) =>
            {
                dialog.Dismiss();
                registerDialog.Dismiss();
            };

            btnCancel.Click += (s, e) =>
            {
                dialog.Dismiss();
            };

            dialog.Show();
        }

        private List<string> GenerateRecoveryWords()
        {
            string[] pool =
            {
                "apple","river","stone","cloud","moon","forest","gold","storm","fire","shadow",
                "eagle","wind","night","sun","wolf","water","mountain","iron","light","earth"
            };

            Random rnd = new Random();
            var result = new List<string>();

            while (result.Count < 10)
            {
                string word = pool[rnd.Next(pool.Length)];
                if (!result.Contains(word))
                    result.Add(word);
            }

            return result;
        }
    }
}
