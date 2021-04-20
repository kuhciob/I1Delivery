using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KURSOVA.Models
{
    public partial class SalaryPayment
    {
        [Display(Name = "Виплата зарплати")]
        public int SalaryPaymentId { get; set; }
        [Display(Name = "Кур'єр")]
        public int CourierId { get; set; }
        [Display(Name = "Початок періоду")]
        public DateTime StartPeriodDate { get; set; }
        [Display(Name = "Кінець періоду")]
        public DateTime EndPeriodDate { get; set; }
        [Display(Name = "К-кість доставок")]
        public int DeliveriesCount { get; set; }
        [Display(Name = "Виплати за доставки")]
        public decimal PaymentForDeliveries { get; set; }
        [Display(Name = "Надбавка")]
        public decimal? Premium { get; set; }
        [Display(Name = "Штрафи")]
        public decimal? FineAmt { get; set; }
        [Display(Name = "Сума виплати")]
        public decimal PaymentAmt { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
        [Display(Name = "Кур'єр")]
        public virtual Courier Courier { get; set; }
    }
}
