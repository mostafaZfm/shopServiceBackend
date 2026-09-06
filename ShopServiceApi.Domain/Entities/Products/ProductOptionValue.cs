using ShopServiceApi.Domain.Entities.Products.Variants;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServiceApi.Domain.Entities.Products
{
    public class ProductOptionValue
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ProductOptionId { get; set; }

        public string Value { get; set; } = null!;

        public int DisplayOrder { get; set; } = 0;

        public virtual ProductOption ProductOption { get; set; } = null!;

        public virtual ICollection<ProductVariantOptionValue> Variants { get; set; }
    = new List<ProductVariantOptionValue>();
    }
}
