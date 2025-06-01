using EcommerceApp.Data;
using EcommerceApp.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EcommerceApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cart
        public async Task<IActionResult> Index()
        {
            string cartId = GetCartId();
            
            var cartItems = await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.CartId == cartId)
                .ToListAsync();

            decimal total = cartItems.Sum(item => item.Quantity * item.Product.Price);
            
            ViewBag.Total = total;
            
            return View(cartItems);
        }

        // POST: Cart/AddToCart
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            string cartId = GetCartId();
            
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.CartId == cartId && c.ProductId == productId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    CartId = cartId,
                    ProductId = productId,
                    Quantity = quantity,
                    DateCreated = DateTime.Now
                };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        // POST: Cart/UpdateQuantity
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int id, int quantity)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }

            if (quantity <= 0)
            {
                _context.CartItems.Remove(cartItem);
            }
            else
            {
                cartItem.Quantity = quantity;
            }

            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        // POST: Cart/RemoveFromCart
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        // GET: Cart/Checkout
        public IActionResult Checkout()
        {
            return View();
        }

        // Helper method to get or create cart ID
        private string GetCartId()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("CartId")))
            {
                string cartId = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("CartId", cartId);
                return cartId;
            }

            return HttpContext.Session.GetString("CartId");
        }
    }
}
