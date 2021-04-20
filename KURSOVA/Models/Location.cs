using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KURSOVA.Models
{
    public partial class Location
    {
        public Location()
        {
            Delivery = new HashSet<Delivery>();
            Restaurant = new HashSet<Restaurant>();
        }
        [Display(Name = "Місцезнаходження")]
        public int LocationId { get; set; }
        [Display(Name = "Вулиця")]
        public int StreetId { get; set; }
        [Display(Name = "Будинок")]
        public string BuildingNbr { get; set; }
        [Display(Name = "№ квартири/кімнати")]
        public string Room { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
        [Display(Name = "Вулиця")]
        public virtual Street Street { get; set; }
        [Display(Name = "Район")]
        public virtual ICollection<Delivery> Delivery { get; set; }
        [Display(Name = "Ресторан")]
        public virtual ICollection<Restaurant> Restaurant { get; set; }
    }
}
