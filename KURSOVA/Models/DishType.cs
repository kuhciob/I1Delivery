using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KURSOVA.Models
{
    public partial class DishType
    {
        public DishType()
        {
            Dish = new HashSet<Dish>();
        }
        [Display(Name = "Тип Страви")]
        public int DishTypeId { get; set; }
        [Display(Name = "Тип")]
        public string DishType1 { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual ICollection<Dish> Dish { get; set; }
    }
}
