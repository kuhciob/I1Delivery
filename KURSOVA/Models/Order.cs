using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KURSOVA.Models
{
    public partial class Order
    {
        public Order()
        {
            Delivery = new HashSet<Delivery>();
            Invoice = new HashSet<Invoice>();
            OrderLine = new HashSet<OrderLine>();
        }
        [Display(Name = "Замовлення")]
        public int OrderId { get; set; }
        [Display(Name = "Замовник")]
        public int CustomerId { get; set; }
        [Display(Name = "Загальна сума")]
        public decimal TotalAmt { get; set; }
        [Display(Name = "Опис")]
        public string Description { get; set; }
        [Display(Name = "Дата")]
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
        [Display(Name = "Статус")]
        [DefaultValue(OrderStatus.Created)]
        public OrderStatus? Status { get; set; }
        [Display(Name = "Замовник")]
        public virtual Customer Customer { get; set; }
        public virtual ICollection<Delivery> Delivery { get; set; }
        public virtual ICollection<Invoice> Invoice { get; set; }
        public virtual ICollection<OrderLine> OrderLine { get; set; }
    }
}
