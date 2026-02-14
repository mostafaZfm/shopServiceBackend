using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServiceApi.Infrastructure.Site.Products
{
    public class Category
    {
            public Guid Id { get; set; } = Guid.NewGuid();
            public string Name { get; set; } = null!;
            public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
