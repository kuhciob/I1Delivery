using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KURSOVA.Models
{
    public partial class District
    {
        public District()
        {
            Street = new HashSet<Street>();
        }
        [Display(Name = "Район")]
        public int DistrictId { get; set; }
        [Display(Name = "Район")]
        public string District1 { get; set; }
        [Display(Name = "Місто")]
        public int CityId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
        [Display(Name = "Місто")]
        public virtual City City { get; set; }
        public virtual ICollection<Street> Street { get; set; }
    }
}
