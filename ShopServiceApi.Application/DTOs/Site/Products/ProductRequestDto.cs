using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShopServiceApi.Application.DTOs.Site.Products
{
    public class CreateProductDto
    {
        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Slug { get; set; } = null!;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Range(typeof(decimal), "0.01", "999999999")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        public List<CreateProductImageDto> Images { get; set; } = new();

        public List<CreateProductOptionDto> Options { get; set; } = new();

        public List<CreateProductVariantDto> Variants { get; set; } = new();
    }

    public class CreateProductImageDto
    {
        [Required]
        [Url]
        public string ImageUrl { get; set; } = null!;

        [Range(0, int.MaxValue)]
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

        public List<CreateProductVariantOptionDto> Options { get; set; } = new();
    }

    public class CreateProductVariantOptionDto
    {
        public string OptionName { get; set; } = null!;

        public string Value { get; set; } = null!;
    }

}
