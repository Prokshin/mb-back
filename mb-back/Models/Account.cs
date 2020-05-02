using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mb_back.Models
{
    public class Account
    {
        public long Id { get; set; }
        public double Balance { get; set; }
        public Account() { }

        internal List<Account> ToList()
        {
            throw new NotImplementedException();
        }
    }
}
