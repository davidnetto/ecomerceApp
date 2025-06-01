using EcommerceApp.Data;
using EcommerceApp.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceApp.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var userId = User.Identity.Name;
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
                
            return View(orders);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.Identity.Name;
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);
                
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            if (ModelState.IsValid)
            {
                var cartId = HttpContext.Session.GetString("CartId");
                if (string.IsNullOrEmpty(cartId))
                {
                    return RedirectToAction("Index", "Cart");
                }

                var cartItems = await _context.CartItems
                    .Include(c => c.Product)
                    .Where(c => c.CartId == cartId)
                    .ToListAsync();

                if (!cartItems.Any())
                {
                    return RedirectToAction("Index", "Cart");
                }

                order.UserId = User.Identity.Name;
                order.OrderDate = DateTime.Now;
                order.Status = "Pendente";
                order.TotalAmount = cartItems.Sum(item => item.Quantity * item.Product.Price);

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                foreach (var item in cartItems)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.Product.Price
                    };

                    _context.OrderItems.Add(orderItem);
                    
                    // Atualizar estoque
                    var product = await _context.Products.FindAsync(item.ProductId);
                    if (product != null)
                    {
                        product.StockQuantity -= item.Quantity;
                        _context.Update(product);
                    }
                }

                // Limpar carrinho
                _context.CartItems.RemoveRange(cartItems);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Details), new { id = order.Id });
            }
            
            return View(order);
        }

        // GET: Orders/Checkout
        public async Task<IActionResult> Checkout()
        {
            var cartId = HttpContext.Session.GetString("CartId");
            if (string.IsNullOrEmpty(cartId))
            {
                return RedirectToAction("Index", "Cart");
            }

            var cartItems = await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.CartId == cartId)
                .ToListAsync();

            if (!cartItems.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            decimal total = cartItems.Sum(item => item.Quantity * item.Product.Price);
            ViewBag.Total = total;
            ViewBag.CartItems = cartItems;

            return View();
        }

        // POST: Orders/ProcessPayment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessPayment(Order order)
        {
            if (ModelState.IsValid)
            {
                // Simulação de processamento de pagamento
                order.PaymentMethod = "Cartão de Crédito";
                order.PaymentDate = DateTime.Now;
                order.TransactionId = Guid.NewGuid().ToString().Substring(0, 8);
                order.Status = "Pago";

                return await Create(order);
            }
            
            return View("Checkout", order);
        }
    }
}
