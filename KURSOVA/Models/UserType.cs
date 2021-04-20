using System;
using System.Collections.Generic;

namespace KURSOVA.Models
{
    public partial class UserType
    {
        public UserType()
        {
            User = new HashSet<User>();
        }

        public int UserTypeId { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }

        public virtual ICollection<User> User { get; set; }
    }
}
