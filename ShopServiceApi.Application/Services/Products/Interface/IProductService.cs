using ShopServiceApi.Application.DTOs.Site.Products;
using ShopServiceApi.Application.DTOs.Site.Products.Categories;
using ShopServiceApi.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServiceApi.Application.Services.Products.Interface
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();

        Task<ProductDto?> GetByIdAsync(Guid id);

        Task<ProductDto> CreateAsync(CreateProductDto request);

        Task<ProductResponseDto> GetLazyLoadProduct(ProductQueryDto query);

        Task<bool> DeleteAsync(Guid id);

        #region Category
        Task<CategoryDto> CreateAsync(CreateCategoryDto request);


        #endregion

    }
}
