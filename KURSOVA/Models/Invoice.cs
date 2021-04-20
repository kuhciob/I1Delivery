using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KURSOVA.Models
{
    public partial class Invoice
    {
        [Display(Name = "Рахунок")]
        public int InvoiceId { get; set; }
        [Display(Name = "Замовлення")]
        public int OrderId { get; set; }
        [Display(Name = "Ціна за доставку")]
        public decimal DeliveryPrice { get; set; }
        [Display(Name = "Загальна сума")]
        public decimal TotalAmt { get; set; }
        [Display(Name = "Знижка %")]
        [Range(0, 100, ErrorMessage = "Знижка має бути у проміжку [0;100]")]
        public decimal Discount { get; set; }
        [Display(Name = "Дата")]
        public DateTime DateTime { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
        [Display(Name = "Замовлення")]
        public virtual Order Order { get; set; }
    }
}
