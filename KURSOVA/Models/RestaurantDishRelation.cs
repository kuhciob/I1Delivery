using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KURSOVA.Models
{
    public partial class RestaurantDishRelation
    {
        public RestaurantDishRelation()
        {
            OrderLine = new HashSet<OrderLine>();
        }
        [Display(Name = "Позиція меню")]
        public int RestaurantDishRelationId { get; set; }
        [Display(Name = "Ресторан")]
        public int RestaurantId { get; set; }
        [Display(Name = "Страва")]
        public int DishId { get; set; }
        [Display(Name = "Ціна")]
        public decimal Price { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
        [Display(Name = "Страва")]
        public virtual Dish Dish { get; set; }
        [Display(Name = "Ресторан")]
        public virtual Restaurant Restaurant { get; set; }
        public virtual ICollection<OrderLine> OrderLine { get; set; }
    }
}
