using ShopServiceApi.Infrastructure.Site.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServiceApi.Application.Services.Products.Interface
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(Guid id);
        Task<Product> CreateAsync(Product product);
        Task<Product?> UpdateAsync(Product product);
        Task<bool> DeleteAsync(Guid id);
    }
}
