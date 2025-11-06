namespace EmptyProject2025Extended
{
    internal interface IPresenter
    {
        void UserClick(int row, int col, int player, int level);
        bool CheckWinner(int i, int j, int player);
        void RestartGame();
    }
}