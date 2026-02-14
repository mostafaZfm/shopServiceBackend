using ShopServiceApi.Infrastructure.Site.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServiceApi.Infrastructure.Site.Orders
{
    public class OrderItem
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; } // ذخیره قیمت لحظه‌ای Product
    }
}
