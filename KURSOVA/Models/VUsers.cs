using System;
using System.Collections.Generic;

namespace KURSOVA.Models
{
    public partial class VUsers
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Title { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
