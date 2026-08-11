using Microsoft.EntityFrameworkCore;
using ShopServiceApi.Application.DTOs.Site.Products;
using ShopServiceApi.Application.Services.Products.Interface;
using ShopServiceApi.Domain.Utility;
using ShopServiceApi.Infrastructure.Data;
using ShopServiceApi.Infrastructure.Site.Products;
using System;
using System.Collections.Generic;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ShopServiceApi.Application.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            product.Id = Guid.NewGuid();
            product.CreatedAt = DateTime.UtcNow;
            product.IsActive = true;

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await _context.Products
                          // .Include(p => p.Category) // اگر بخوای Category رو هم بگیری
                           .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
        }

        public async Task<Product?> UpdateAsync(Product product)
        {
            var existing = await _context.Products.FindAsync(product.Id);
            if (existing == null) return null;

            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;
            existing.ImageUrl = product.ImageUrl;
            existing.IsActive = product.IsActive;
            existing.CategoryId = product.CategoryId;

            await _context.SaveChangesAsync();
            return existing;
        }

      

        public async Task<ProductResponseDto> GetLazyLoadProduct(ProductQueryDto query)
        {
            var products = _context.Products.AsQueryable();

            // 🔎 Search
            if (!string.IsNullOrEmpty(query.Search))
            {
                products = products.Where(p =>
                    p.Name.Contains(query.Search) ||
                    p.Description.Contains(query.Search));
            }

            // 🔄 Sorting
            if (!string.IsNullOrEmpty(query.SortField))
            {
                bool isAscending = query.SortOrder == "asc";
                var allowedColumns = new[] { "Name", "Price", "CreatedAt", "Quantity" };

                if (allowedColumns.Contains(query.SortField))
                {
                    products = products.OrderByDynamic(query.SortField, isAscending);
                }
            }
            var count = await products.CountAsync();
            var data = await products
                .Skip(query.Page * query.PageSize)
                .Take(query.PageSize).Select(x => new ProductDto()
                {
                    Name = x.Name,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    Description = x.Description,
                   ImageUrl = x.ImageUrl,
                })
                .ToListAsync();
            return new ProductResponseDto()
            {
                TotalCount = count,
                Products = data,
            };
         
           
        }
    }
}
