using ShopServiceApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServiceApi.Domain.Entities.Orders
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string UserId { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? PaidAt { get; set; }

        public DateTime? ShippedAt { get; set; }

        public DateTime? CompletedAt { get; set; }

        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

        public decimal TotalPrice { get; set; }

        public virtual ICollection<OrderItem> Items { get; set; }
            = new List<OrderItem>();
    }
}
