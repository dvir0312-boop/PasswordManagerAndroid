namespace EmptyProject2025Extended
{
    public interface ILoginView
    {
        string Username { get; }
        string Password { get; }

        void ShowMessage(string message);
        void NavigateToMain(string owner);
        void ClearInputFields();
    }
}
