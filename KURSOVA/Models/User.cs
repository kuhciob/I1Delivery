using System;
using System.Collections.Generic;

namespace KURSOVA.Models
{
    public partial class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int UserTypeId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual Account UserNavigation { get; set; }
        public virtual UserType UserType { get; set; }
    }
}
