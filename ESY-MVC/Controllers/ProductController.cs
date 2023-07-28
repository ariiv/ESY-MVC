using ESY_MVC.Data;
using ESY_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ESY_MVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly DataContext _db;
        private static int? userId;

        public ProductController(DataContext db)
        {
            _db = db;
        }

        public IActionResult Index(User user)
        {
            var products = _db.Products.ToList();
            userId = user.Id;

            ProductModel model = new ProductModel()
            {
                Products = products, 
                IsAdmin = user.IsAdmin
            };
            return View(model);
        }
 
        public void Audit(Product product, string action)
        {
            
            var auditLog = new Audit
            {
                UserId = (int)userId,
                TimeStamp = DateTime.Now.ToString(),
                Action = "User (id:" + Convert.ToInt32(userId) + ") " + action + "ed " + product.ProductName + " (product id:" + product.Id + ")"
            };

            _db.Audits.Add(auditLog);
            _db.SaveChanges();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _db.Products.Add(product);
                await _db.SaveChangesAsync();
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                Audit(product, "Create");
                return RedirectToAction(nameof(Index));
            }
            
            return View(product);
        }

        public async Task<IActionResult> Read(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Entry(product).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
