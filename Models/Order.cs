using System;
using System.Collections.Generic;

namespace InventoryManagementSystem.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string GuestName { get; set; }
        public string GuestEmail { get; set; }
        public DateTime OrderDate { get; set; }

        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        public decimal TotalPrice { get; set; }
    }

    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal SubTotal => Price * Quantity;
    }
}