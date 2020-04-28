using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mb_back.Models
{
    public class User
    {

        public string Email { get; set; }
        public string Password { get; set; }
        public string Img { get; set; }

        public User() { }
        public User(string email, string password)
        {
            this.Email = email;
            this.Password = password;
            this.Img = null;
        }
    }
}
