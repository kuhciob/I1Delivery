using System;
using System.Collections.Generic;

namespace KURSOVA.Models
{
    public partial class Account
    {
        public int AccountId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual User User { get; set; }
    }
}
