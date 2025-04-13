using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Category { get; set; }

        public decimal? Price { get; set; }

        public int? Quantity { get; set; }

        public int? LowStockThreshold { get; set; }
    }
}