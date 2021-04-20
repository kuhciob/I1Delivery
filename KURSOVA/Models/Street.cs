using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KURSOVA.Models
{
    public partial class Street
    {
        public Street()
        {
            Location = new HashSet<Location>();
        }
        [Display(Name = "Вулиця")]
        public int StreetId { get; set; }
        [Display(Name = "Район")]
        public int DistrictId { get; set; }
        [Display(Name = "Вулиця")]
        public string Street1 { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual District District { get; set; }
        public virtual ICollection<Location> Location { get; set; }
    }
}
