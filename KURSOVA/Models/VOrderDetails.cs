using System;
using System.Collections.Generic;

namespace KURSOVA.Models
{
    public partial class VOrderDetails
    {
        public int OrderId { get; set; }
        public int OrderLineId { get; set; }
        public string OrderDescription { get; set; }
        public int RestaurantId { get; set; }
        public decimal? LineAmt { get; set; }
        public decimal PortionSize { get; set; }
        public decimal Price { get; set; }
        public string Title { get; set; }
        public string DishType { get; set; }
        public string DishDescription { get; set; }
        public decimal Quantity { get; set; }
        public string UnitOfMeasure { get; set; }
    }
}
