using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KURSOVA.Models
{
    public partial class OrderLine
    {
        [Display(Name = "Замовлення позиції")]
        public int OrderLineId { get; set; }
        [Display(Name = "Замовлення")]
        public int OrderId { get; set; }
        [Display(Name = "Позиція з меню")]
        public int RestaurantDishRelationId { get; set; }
        [Display(Name = "Кількість")]
        public decimal Quantity { get; set; }
        [Display(Name = "Дата")]
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
        [Display(Name = "Замовлення")]
        public virtual Order Order { get; set; }
        [Display(Name = "Меню")]
        public virtual RestaurantDishRelation RestaurantDishRelation { get; set; }
    }
}
