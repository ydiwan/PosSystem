using System;
using System.Collections.Generic;

namespace Pos.Api.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; } = "paid";

        // Multi-location
        public int LocationId { get; set; }
        public Location Location { get; set; } = null!;

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }

    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
    }
}

