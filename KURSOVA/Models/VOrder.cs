using System;
using System.Collections.Generic;

namespace KURSOVA.Models
{
    public partial class VOrder
    {
        public int OrderId { get; set; }
        public string Description { get; set; }
        public decimal TotalAmt { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
