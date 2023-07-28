using ESY_MVC.Data;
using ESY_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ESY_MVC.Controllers
{
    public class LogInController : Controller
    {
        private readonly DataContext _db;

        public LogInController(DataContext db)
        {
            _db = db;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User credentials)
        {  

            if (ModelState.IsValid)
            {
                var user = _db.Users.FirstOrDefault(u => u.Username == credentials.Username && u.Password == credentials.Password);

                if (user != null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            return View(credentials);
        }
    }

}
