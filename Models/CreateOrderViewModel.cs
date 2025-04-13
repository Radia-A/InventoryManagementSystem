using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Models
{
    public class CreateOrderViewModel
    {
        [Required(ErrorMessage = "Guest name is required.")]
        public string GuestName { get; set; }

        [Required(ErrorMessage = "Guest email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string GuestEmail { get; set; }

        public List<OrderItemViewModel> OrderItems { get; set; } = new List<OrderItemViewModel>();
    }
}