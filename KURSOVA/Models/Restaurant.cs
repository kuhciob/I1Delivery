using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KURSOVA.Models
{
    public partial class Restaurant
    {
        public Restaurant()
        {
            RestaurantDishRelation = new HashSet<RestaurantDishRelation>();
        }
        [Display(Name = "Ресторан")]
        public int RestaurantId { get; set; }
        [Display(Name = "Назва")]
        public string Title { get; set; }
        [Display(Name = "Телефон")]
        public string Phone { get; set; }
        [Display(Name = "Місцезнаходженн")]
        public int LocationId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual Location Location { get; set; }
        public virtual ICollection<RestaurantDishRelation> RestaurantDishRelation { get; set; }
    }
}
