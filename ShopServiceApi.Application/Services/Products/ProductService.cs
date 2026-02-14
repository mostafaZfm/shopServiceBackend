using Microsoft.EntityFrameworkCore;
using ShopServiceApi.Application.Services.Products.Interface;
using ShopServiceApi.Infrastructure.Data;
using ShopServiceApi.Infrastructure.Site.Products;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
