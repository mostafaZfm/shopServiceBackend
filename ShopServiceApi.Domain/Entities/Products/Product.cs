using ShopServiceApi.Domain.Entities.Products.Variants;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServiceApi.Domain.Entities.Products
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = null!;

        public string Slug { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public bool IsActive { get; set; } = true;

        public Guid CategoryId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual Category Category { get; set; } = null!;

        public virtual ICollection<ProductImage> Images { get; set; }
            = new List<ProductImage>();

        public virtual ICollection<ProductOption> Options { get; set; }
            = new List<ProductOption>();

        public virtual ICollection<ProductVariant> Variants { get; set; }
            = new List<ProductVariant>();
    }
}
