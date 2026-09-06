using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServiceApi.Domain.Entities.Products.Variants
{
    public class ProductVariant
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ProductId { get; set; }

        public string SKU { get; set; } = null!;

        public decimal Price { get; set; }

        public int StockQuantity { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public virtual Product Product { get; set; } = null!;
        public virtual ICollection<ProductVariantOptionValue> OptionValues { get; set; }
            = new List<ProductVariantOptionValue>();
    }
}
