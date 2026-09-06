using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServiceApi.Domain.Entities.Products.Variants
{
    public class ProductVariantOptionValue
    {
        public Guid ProductVariantId { get; set; }

        public Guid ProductOptionValueId { get; set; }

        public virtual ProductVariant ProductVariant { get; set; } = null!;

        public virtual ProductOptionValue ProductOptionValue { get; set; } = null!;
    }
}
