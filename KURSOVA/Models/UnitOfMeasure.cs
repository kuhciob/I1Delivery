using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KURSOVA.Models
{
    public partial class UnitOfMeasure
    {
        public UnitOfMeasure()
        {
            Dish = new HashSet<Dish>();
        }
        [Display(Name = "Одиниця виміру")]
        public int UnitOfMeasureId { get; set; }
        [Display(Name = "Одиниця виміру")]
        public string UnitOfMeasure1 { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual ICollection<Dish> Dish { get; set; }
    }
}
