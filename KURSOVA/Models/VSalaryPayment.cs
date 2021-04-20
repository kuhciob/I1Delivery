using System;
using System.Collections.Generic;

namespace KURSOVA.Models
{
    public partial class VSalaryPayment
    {
        public int SalaryPaymentId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Type { get; set; }
        public decimal Rate { get; set; }
        public int DeliveriesCount { get; set; }
        public decimal? Premium { get; set; }
        public decimal? FineAmt { get; set; }
        public decimal PaymentAmt { get; set; }
        public DateTime StartPeriodDate { get; set; }
        public DateTime EndPeriodDate { get; set; }
    }
}
