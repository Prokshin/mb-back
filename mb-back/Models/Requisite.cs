using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mb_back.Models
{
    public class Requisite
    {
        public int Id { get; set; }
        public string Payment_name { get; set;}
        public string Target_name { get; set; }
        public string Target_email { get; set; }
    }
}
