using ShopServiceApi.Domain.Entities.Products;
using ShopServiceApi.Domain.Entities.Products.Variants;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServiceApi.Domain.Entities.Orders
{
    public class OrderItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid OrderId { get; set; }

        public Guid ProductId { get; set; }

        public Guid? ProductVariantId { get; set; }

        public int Quantity { get; set; } = 1;

        public decimal UnitPrice { get; set; }

        public virtual Order Order { get; set; } = null!;

        public virtual Product Product { get; set; } = null!;

        public virtual ProductVariant? ProductVariant { get; set; }
    }
}
