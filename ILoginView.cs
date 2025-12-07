namespace EmptyProject2025Extended
{
    public interface ILoginView
    {
        string Username { get; }
        string Password { get; }

        void ShowMessage(string message);

        // Sends the logged-in username
        void NavigateToMain(string owner);

        void ClearInputFields();
    }
}
