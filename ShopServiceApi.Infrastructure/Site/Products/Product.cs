using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServiceApi.Infrastructure.Site.Products
{
public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Slug { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public bool IsActive { get; set; } = true;

        public Guid CategoryId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Category Category { get; set; } = null!;

        public ICollection<ProductImage> Images { get; set; }
            = new List<ProductImage>();
    }
}
