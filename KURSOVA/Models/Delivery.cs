using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KURSOVA.Models
{
    public partial class Delivery
    {
        [Display(Name = "Доставка")]
        public int DeliveryId { get; set; }
        [Display(Name = "Замовлення")]
        public int OrderId { get; set; }
        [Display(Name = "Кур'єр")]
        public int CourierId { get; set; }
        [Display(Name = "Місцезнаходження")]
        public int LocationId { get; set; }
        [Display(Name = "Опис")]
        public string Description { get; set; }
        [Display(Name = "Початок")]
        public DateTime? StartTime { get; set; }
        [Display(Name = "Кінець")]
        public DateTime? EndTime { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
        [Display(Name = "Вага")]
        public decimal? Weight { get; set; }
        [Display(Name = "Статус")]
        [DefaultValue(DeliveryStatus.Created)]

        public DeliveryStatus? Status { get; set; }
        [Display(Name = "Кур'єр")]
        public virtual Courier Courier { get; set; }
        [Display(Name = "Місцзнаходження")]
        public virtual Location Location { get; set; }
        [Display(Name = "Замовлення")]
        public virtual Order Order { get; set; }
    }
}
