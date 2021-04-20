using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KURSOVA.Models
{
    public partial class Dish
    {
        public Dish()
        {
            RestaurantDishRelation = new HashSet<RestaurantDishRelation>();
        }
        [Display(Name = "Страва")]
        public int DishId { get; set; }
        [Display(Name = "Назва")]
        public string Title { get; set; }
        [Display(Name = "Розмір порції")]
        public decimal Quantity { get; set; }
        [Display(Name = "Одиниця виміру")]
        public int UnitOfMeasureId { get; set; }
        [Display(Name = "Тип")]
        public int DishTypeId { get; set; }
        [Display(Name = "Опис")]
        public string Description { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
        [Display(Name = "Вартість")]
        public decimal? Cost { get; set; }
        [Display(Name = "Тип Страви")]
        public virtual DishType DishType { get; set; }
        [Display(Name = "Одиниці виміру")]
        public virtual UnitOfMeasure UnitOfMeasure { get; set; }
        [Display(Name = "Меню")]
        public virtual ICollection<RestaurantDishRelation> RestaurantDishRelation { get; set; }
    }
}
