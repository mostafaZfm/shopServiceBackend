using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServiceApi.Infrastructure.Site.Products
{
public class ProductImage
    {
        public Guid Id { get; set; }

        public string ImageUrl { get; set; } = null!;

        public int DisplayOrder { get; set; }

        public bool IsPrimary { get; set; } = false;

        public Guid ProductId { get; set; }

        public Product Product { get; set; } = null!;
    }

}
