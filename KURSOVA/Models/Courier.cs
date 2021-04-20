using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KURSOVA.Models
{
    public partial class Courier
    {
        public Courier()
        {
            Delivery = new HashSet<Delivery>();
            SalaryPayment = new HashSet<SalaryPayment>();
        }
        [Display(Name = "Кур'єр")]
        public int CourierId { get; set; }
        [Display(Name = "Ім'я")]
        public string Name { get; set; }
        [Display(Name = "Прізвище")]
        public string Surname { get; set; }
        [Display(Name = "Номер телефону")]
        public string Phone { get; set; }
        [Display(Name = "Тип")]

        public int CourierTypeId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
        [Display(Name = "Тип")]
        public virtual CourierType CourierType { get; set; }
        [Display(Name = "Доставка")]

        public virtual ICollection<Delivery> Delivery { get; set; }
        [Display(Name = "Заробітня плата")]

        public virtual ICollection<SalaryPayment> SalaryPayment { get; set; }
    }
}
