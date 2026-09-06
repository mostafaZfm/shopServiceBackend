using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServiceApi.Domain.Entities.Products
{
    public class ProductImage
    {
        public Guid Id { get; set; }

        public string ImageUrl { get; set; } = null!;

        public int DisplayOrder { get; set; }

        public bool IsPrimary { get; set; } = false;

        public Guid ProductId { get; set; }

        public virtual Product Product { get; set; } = null!;
    }

}
