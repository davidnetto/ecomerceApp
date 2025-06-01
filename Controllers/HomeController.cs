using EcommerceApp.Data;
using EcommerceApp.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var featuredProducts = await Task.FromResult(_context.Products
                .Include(p => p.Category)
                .Where(p => p.StockQuantity > 0)
                .OrderByDescending(p => p.CreatedAt)
                .Take(6)
                .ToList());

            var categories = await Task.FromResult(_context.Categories.ToList());

            ViewBag.Categories = categories;
            
            return View(featuredProducts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("sobre-nos")]
        public IActionResult About()
        {
            return View();
        }

        [Route("contato")]
        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
