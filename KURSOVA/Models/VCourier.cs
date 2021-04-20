using System;
using System.Collections.Generic;

namespace KURSOVA.Models
{
    public partial class VCourier
    {
        public int CourierId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Type { get; set; }
        public decimal Rate { get; set; }
    }
}
