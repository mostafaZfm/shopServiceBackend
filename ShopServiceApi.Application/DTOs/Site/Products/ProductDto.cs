using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServiceApi.Application.DTOs.Site.Products
{
    public class ProductResponseDto
    {
        public int TotalCount { get; set; }

        public List<ProductDto> Products { get; set; } = new();
    }

    public class ProductDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Slug { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public bool IsActive { get; set; }

        public Guid CategoryId { get; set; }

        public string? PrimaryImageUrl { get; set; }

        public List<ProductImageDto> Images { get; set; } = new();

        public List<ProductOptionDto> Options { get; set; } = new();

        public List<ProductVariantDto> Variants { get; set; } = new();
    }

    public class ProductImageDto
    {
        public Guid Id { get; set; }

        public string ImageUrl { get; set; } = null!;

        public int DisplayOrder { get; set; }

        public bool IsPrimary { get; set; }
    }

    public class ProductOptionDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public int DisplayOrder { get; set; }

        public List<ProductOptionValueDto> Values { get; set; } = new();
    }

    public class ProductOptionValueDto
    {
        public Guid Id { get; set; }

        public string Value { get; set; } = null!;

        public int DisplayOrder { get; set; }
    }

    public class ProductVariantDto
    {
        public Guid Id { get; set; }

        public string SKU { get; set; } = null!;

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public bool IsActive { get; set; }

        public List<Guid> OptionValueIds { get; set; } = new();
    }

    public class ProductQueryDto
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public string? Search { get; set; }

        public string? SortField { get; set; }

        public string? SortOrder { get; set; }
    }



}
