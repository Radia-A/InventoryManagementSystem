using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models
{
    public class OrderItemViewModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be 0 or more.")]
        public int Quantity { get; set; }

        public decimal SubTotal => Price * Quantity;
    }
}