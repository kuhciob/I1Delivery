using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KURSOVA.Models
{
    public static class AccessInfo
    {
        public static int? UserID { get; set; }
        public static string UserType { get; set; }
        public static User CurrUser { get; set; }
    }
}
