using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServiceApi.Domain.Entities.Products
{
    public class ProductOption
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ProductId { get; set; }

        public string Name { get; set; } = null!;

        public int DisplayOrder { get; set; } = 0;

        public virtual Product Product { get; set; } = null!;

        public virtual ICollection<ProductOptionValue> Values { get; set; }
            = new List<ProductOptionValue>();
    }
}
