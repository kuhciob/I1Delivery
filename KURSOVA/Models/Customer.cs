using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KURSOVA.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Order = new HashSet<Order>();
        }
        [Display(Name = "Замовник")]
        public int CustomerId { get; set; }
        [Display(Name = "Ім'я")]
        public string Name { get; set; }
        [Display(Name = "Прізвище")]
        public string Surname { get; set; }
        [Display(Name = "Номер Телефону")]
        public string Phone { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
        [Display(Name = "Замовлення")]
        public virtual ICollection<Order> Order { get; set; }
    }
}
