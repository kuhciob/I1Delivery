using System;
using System.Collections.Generic;

namespace KURSOVA.Models
{
    public partial class VInvoice
    {
        public int InvoiceId { get; set; }
        public decimal TotalAmt { get; set; }
        public DateTime DateTime { get; set; }
        public decimal DeliveryPrice { get; set; }
        public decimal OrderTotal { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
    }
}
