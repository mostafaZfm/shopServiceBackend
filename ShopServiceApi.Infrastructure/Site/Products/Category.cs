using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServiceApi.Infrastructure.Site.Products
{
    public class Category
    {
            public Guid Id { get; set; } = Guid.NewGuid();
            public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;

        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public Guid? ParentCategoryId { get; set; }

        public Category? ParentCategory { get; set; }

        public ICollection<Category> Children { get; set; }
            = new List<Category>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
