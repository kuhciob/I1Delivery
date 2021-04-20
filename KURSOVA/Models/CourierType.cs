using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KURSOVA.Models
{
    public partial class CourierType
    {
        public CourierType()
        {
            Courier = new HashSet<Courier>();
        }
        [Display(Name = "Кур'єр")]

        public int CourierTypeId { get; set; }
        [Display(Name = "Тип")]

        public string Type { get; set; }
        [Display(Name = "Ставка")]

        public decimal Rate { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual ICollection<Courier> Courier { get; set; }
    }
}
