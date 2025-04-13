using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using InventoryManagementSystem.Models;
using InventoryManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Create()
        {
            var model = new CreateOrderViewModel();

            var products = await _context.Products.ToListAsync();
            foreach (var product in products)
            {
                model.OrderItems.Add(new OrderItemViewModel
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price ?? 0,
                    Quantity = 0
                });
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrderViewModel model)
        {
            foreach (var item in model.OrderItems)
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
                if (product != null)
                {
                    item.ProductName = product.Name;
                    item.Price = product.Price ?? 0;
                }
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var order = new Order
            {
                GuestName = model.GuestName,
                GuestEmail = model.GuestEmail,
                OrderDate = DateTime.UtcNow
            };

            foreach (var item in model.OrderItems.Where(i => i.Quantity > 0))
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
                if (product != null && product.Quantity >= item.Quantity)
                {
                    product.Quantity -= item.Quantity;

                    order.Items.Add(new OrderItem
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Price = item.Price,
                        Quantity = item.Quantity
                    });
                }
            }

            order.TotalPrice = order.Items.Sum(i => i.SubTotal);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return RedirectToAction("Summary", new { id = order.Id });
        }

        public async Task<IActionResult> Summary(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);

            return order == null ? NotFound() : View(order);
        }
        
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders.Include(o => o.Items).ToListAsync();
            return View(orders);
        }

    }
}