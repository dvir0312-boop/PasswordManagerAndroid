using Android.Views;


namespace EmptyProject2025Extended
{
    internal class Presenter : IPresenter
    {
        private readonly IView view;
        private readonly Model model;

        public Presenter(IView view)
        {
            this.view = view;
            this.model = new Model();
        }

        // Interface methods
        public void UserClick(int row, int col, int player, int level)
        {
            // your code here
        }

        public bool CheckWinner(int i, int j, int player)
        {
            return true; // temporary, Check relevance.
        }

        public void RestartGame()
        {
            // your code here
        }
    }
}