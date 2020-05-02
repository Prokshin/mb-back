using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mb_back.Models
{
    public class Operation
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public decimal amount { get; set; }
        public DateTime date { get; set; }
        public long accountInId { get; set; }
        public long accountOutId { get; set; }
        public Operation() { }

    }
}
