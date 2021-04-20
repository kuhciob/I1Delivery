using KURSOVA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;

namespace KURSOVA.Controllers
{
    public class LoginController : Controller
    {
        private readonly I1Delivery_KursovaContext _context;

        public LoginController(I1Delivery_KursovaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {           
            return View();
        }
        public string GetCulture(string code = "")
        {
            if (!String.IsNullOrEmpty(code))
            {
                CultureInfo.CurrentCulture = new CultureInfo(code);
                CultureInfo.CurrentUICulture = new CultureInfo(code);
            }
            return $"CurrentCulture:{CultureInfo.CurrentCulture.Name}, CurrentUICulture:{CultureInfo.CurrentUICulture.Name}";
        }

        [HttpPost]
        public async Task<IActionResult> Login(string login, string password)
        {
            var user = _context.Account
                .Include(a => a.User)
                .ThenInclude(a=> a.UserType)
                .Where(a => a.Login == login
                && a.Password == password)             
                .FirstOrDefault();
            if(user != null)
            {
                AccessInfo.UserID = user.User.UserId;
                AccessInfo.UserType = user.User.UserType.Title;
                AccessInfo.CurrUser = user.User;

                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            return RedirectToRoute(new { controller = "Login", action = "Index" });

        }
      
        public async Task<IActionResult> Logout()
        {
            AccessInfo.UserID = null;
            AccessInfo.UserType = "";
            AccessInfo.CurrUser = null;
            return RedirectToRoute(new { controller = "Login", action = "Index" });

        }
    }
}
