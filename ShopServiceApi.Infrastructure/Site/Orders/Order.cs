using ShopServiceApi.Infrastructure.Identity;
using ShopServiceApi.Infrastructure.Site.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServiceApi.Infrastructure.Site.Orders
{
    public class Order
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = null!; // Foreign Key به AspNetUsers
        public ApplicationUser User { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending; // Pending, Paid, Shipped, Completed

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
