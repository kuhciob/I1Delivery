using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KURSOVA.Models
{
    public partial class City
    {
        public City()
        {
            District = new HashSet<District>();
        }
        [Display(Name = "Місто")]
        public int CityId { get; set; }
        [Display(Name = "Місто")]
        public string City1 { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual ICollection<District> District { get; set; }
    }
}
