using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServiceApi.Application.DTOs.Site.Products
{
    public class ProductResponseDto
    {
        public int TotalCount { get; set; }
        public List<ProductDto> Products { get; set; }
    }

    public class ProductDto
    {
        public int Quantity { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

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
