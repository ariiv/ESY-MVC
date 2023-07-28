using Azure.Core;
using ESY_MVC.Data;
using ESY_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ESY_MVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly DataContext _db;

        public LoginController(DataContext db)
        {
            _db = db;
        }

        public ActionResult Login()
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
                    return RedirectToAction("Index", "Product", user);
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid username or password.";
                }
            }

            return View(credentials);
        }
    }

}
