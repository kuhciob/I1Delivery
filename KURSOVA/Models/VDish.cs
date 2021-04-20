using System;
using System.Collections.Generic;

namespace KURSOVA.Models
{
    public partial class VDish
    {
        public int DishId { get; set; }
        public string Title { get; set; }
        public string DishType { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public string UnitOfMeasure { get; set; }
    }
}
