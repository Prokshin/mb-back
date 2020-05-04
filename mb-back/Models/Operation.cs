using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mb_back.Models
{
    public class Operation
    {
        public int Id { get; set; }
        //Тип операции
        public string Operation_type { get; set; }
        //Сумма операции
        public decimal Amount { get; set; }
        //Дата операции
        public DateTime Date { get; set; }
        // Id счёта, на который приходит операция
        public long Account_in_id { get; set; }
        // Id счёта, с которого списывется сумма
        public long Account_out_id { get; set; }

        public Requisite Requisite { get; set; }
        //Назначение платежа
        public string Purpose { get; set; }
        public Operation() {}

        public Operation(string operation_type, decimal amount, long accountInId, long accountOutId, Requisite requisite, string purpose) {
            Operation_type = operation_type;
            Amount = amount;
            Account_in_id = accountInId;
            Account_out_id = accountOutId;
            Requisite = requisite;
            Purpose = purpose;
        }
    }
}
