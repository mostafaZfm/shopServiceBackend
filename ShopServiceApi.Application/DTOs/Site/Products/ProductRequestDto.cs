using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServiceApi.Application.DTOs.Site.Products
{
    public class CreateProductDto
    {
        public string Name { get; set; } = null!;

        public string Slug { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public Guid CategoryId { get; set; }

        public List<CreateProductImageDto> Images { get; set; } = new();

        public List<CreateProductOptionDto> Options { get; set; } = new();

        public List<CreateProductVariantDto> Variants { get; set; } = new();
    }

    public class CreateProductImageDto
    {
        public string ImageUrl { get; set; } = null!;

        public int DisplayOrder { get; set; }

        public bool IsPrimary { get; set; }
    }

    public class CreateProductOptionDto
    {
        public string Name { get; set; } = null!;

        public int DisplayOrder { get; set; }

        public List<CreateProductOptionValueDto> Values { get; set; } = new();
    }

    public class CreateProductOptionValueDto
    {
        public string Value { get; set; } = null!;

        public int DisplayOrder { get; set; }
    }

    public class CreateProductVariantDto
    {
        public string SKU { get; set; } = null!;

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public List<Guid> OptionValueIds { get; set; } = new();
    }
}
