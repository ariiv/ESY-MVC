using ESY_MVC.Data;
using ESY_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace ESY_MVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly DataContext _dbContext;
        private static int? userId;
        private readonly IMemoryCache _memoryCache;

        public ProductController(DataContext db, IMemoryCache memoryCache)
        {
            _dbContext = db;
            _memoryCache = memoryCache;
        }

        public IActionResult Index(User user)
        {
            var products = _dbContext.Products.ToList();
            userId = user.Id;

            if (!_memoryCache.TryGetValue("CacheKey", out bool cachedData))
            {
                cachedData = user.IsAdmin;

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
                    SlidingExpiration = TimeSpan.FromMinutes(10)
                };

                _memoryCache.Set("CacheKey", cachedData, cacheEntryOptions);
            }

            var isAdmin = _memoryCache.Get("CacheKey");

            ProductModel model = new ProductModel()
            {
                Products = products,
                UserId = (int)userId,
                IsAdmin = (bool)isAdmin
            };
            return View(model);
        }
 
        public void Audit(Product product, string action)
        {
            var auditLog = new Audit
            {
                UserId = (int)userId,
                TimeStamp = DateTime.Now.ToString(),
                Action = "User (id:" + (int)userId + ") performed action '" + action + "' to a product " + product.ProductName + " (id:" + product.Id + ")"
            };

            _dbContext.Audits.Add(auditLog);
            _dbContext.SaveChanges();
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
                _dbContext.Products.Add(product);
                await _dbContext.SaveChangesAsync();
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                Audit(product, "Create");
                var isAdmin = _memoryCache.Get("CacheKey");
                return RedirectToAction(nameof(Index), new { userId, isAdmin });
            }

            return View(product);
        }
        public async Task<IActionResult> Read(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
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

            var product = await _dbContext.Products.FindAsync(id);
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
                _dbContext.Entry(product).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                Audit(product, "Update");
                var isAdmin = _memoryCache.Get("CacheKey");
                return RedirectToAction(nameof(Index), new { userId, isAdmin });
            }

            return View(product);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
            Audit(product, "Delete");
            var isAdmin = _memoryCache.Get("CacheKey");
            return RedirectToAction(nameof(Index), new { userId, isAdmin });
        }

        public IActionResult Logout()
        {
            _memoryCache.Remove("CacheKey");
            return RedirectToAction("Login", "Login");
        }
    }
}
