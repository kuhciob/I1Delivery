using System;
using System.Collections.Generic;

namespace KURSOVA.Models
{
    public partial class VRestaurant
    {
        public int RestaurantId { get; set; }
        public string Title { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Street { get; set; }
        public string BuildingNbr { get; set; }
        public string Phone { get; set; }
    }
}
