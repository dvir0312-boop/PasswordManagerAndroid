namespace EmptyProject2025Extended.Models
{
    public class PasswordInfo
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Site { get; set; }
        public string Owner { get; set; }

        public PasswordInfo(long id, string username, string password, string site, string owner)
        {
            Id = id;
            Username = username;
            Password = password;
            Site = site;
            Owner = owner;
        }
    }
}
