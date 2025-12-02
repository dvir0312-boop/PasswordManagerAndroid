using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptyProject2025Extended.Models
{
    public class PasswordInfo
    {
        public long Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Site { get; set; }


        public PasswordInfo(long id, string username, string password, string site)
        {
            this.Id = id;
            this.Username = username;
            this.Password = password;
            this.Site = site;
        }

    }
}
