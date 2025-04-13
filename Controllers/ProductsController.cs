using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Data;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ApplicationDbContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ----------------------------
        // Create
        // ----------------------------
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create() => View();

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Products.Add(product);
                    _context.SaveChanges();
                    TempData["ToastMessage"] = "Product created successfully!";
                    return RedirectToAction("List");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating product.");
                    ModelState.AddModelError("", "An error occurred while saving the product.");
                }
            }

            return View(product);
        }

        // ----------------------------
        // List & AJAX Filter
        // ----------------------------
        [HttpGet]
        public IActionResult List()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult FilteredList(string searchString, string categoryFilter, decimal? minPrice, decimal? maxPrice)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
                query = query.Where(p => p.Name.Contains(searchString));

            if (!string.IsNullOrWhiteSpace(categoryFilter))
                query = query.Where(p => p.Category == categoryFilter);

            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice);

            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice);

            return PartialView("_ProductListPartial", query.ToList());
        }

        // ----------------------------
        // Edit (Full Page)
        // ----------------------------
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return product == null ? NotFound() : View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product)
        {
            if (!ModelState.IsValid)
                return View(product);

            try
            {
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                TempData["ToastMessage"] = "Product updated successfully!";
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product.");
                ModelState.AddModelError("", "Failed to update product.");
                return View(product);
            }
        }

        // ----------------------------
        // Delete (Full Page)
        // ----------------------------
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            try
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                TempData["ToastMessage"] = "Product deleted successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product.");
                TempData["ToastMessage"] = "Error occurred while deleting the product.";
            }

            return RedirectToAction("List");
        }

        // ----------------------------
        // AJAX Modals: Edit & Delete
        // ----------------------------
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditModal(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return product == null ? NotFound() : PartialView("_EditModalPartial", product);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> DeleteModal(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return product == null ? NotFound() : PartialView("_DeleteConfirmPartial", product);
        }

        // ----------------------------
        // AJAX Handlers
        // ----------------------------
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditAjax(Product product)
        {
            if (!ModelState.IsValid) return BadRequest();

            try
            {
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Product updated successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AJAX update failed.");
                return StatusCode(500, "Failed to update product.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteAjax(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            try
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Product deleted successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AJAX delete failed.");
                return StatusCode(500, "Failed to delete product.");
            }
        }
    }
}
