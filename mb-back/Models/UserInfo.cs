using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace mb_back.Models
{
    public class UserInfo
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }

        public UserInfo() { }

        public UserInfo(string email, string name, string img) 
        {
            this.Email = email;
            this.Name = name;
            this.Img = img;
        }
    }
}
