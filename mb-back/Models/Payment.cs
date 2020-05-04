using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mb_back.Models
{
    public class Payment
    {
        public long Account_Out_Id { get; set; }
        public decimal Amount { get; set; }
        public Requisite Requisite { get; set; }

        public string Purpose;
    }
}
