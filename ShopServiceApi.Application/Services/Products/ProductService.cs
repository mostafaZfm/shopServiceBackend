
using Microsoft.EntityFrameworkCore;
using ShopServiceApi.Application.DTOs.Site.Products;
using ShopServiceApi.Abstractions.Interfaces;
using ShopServiceApi.Application.Services.Products.Interface;
using ShopServiceApi.Domain.Entities.Products;
using ShopServiceApi.Domain.Entities.Products.Variants;
using ShopServiceApi.Domain.Utility;

namespace ShopServiceApi.Application.Services.Products;

public class ProductService : IProductService
{
    private readonly IApplicationDbContext _context;

    public ProductService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        return await _context.Products
            .AsNoTracking()
            .Where(x => x.IsActive)
            .Select(x => new ProductDto
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                Price = x.Price,
                StockQuantity = x.StockQuantity,
                IsActive = x.IsActive,
                CategoryId = x.CategoryId,

                PrimaryImageUrl = x.Images
                    .Where(i => i.IsPrimary)
                    .Select(i => i.ImageUrl)
                    .FirstOrDefault()
            })
            .ToListAsync();
    }

    public async Task<ProductDto?> GetByIdAsync(Guid id)
    {
        return await _context.Products
            .AsNoTracking()
            .Where(x => x.Id == id && x.IsActive)
            .Select(x => new ProductDto
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                Price = x.Price,
                StockQuantity = x.StockQuantity,
                IsActive = x.IsActive,
                CategoryId = x.CategoryId,

                PrimaryImageUrl = x.Images
                    .Where(i => i.IsPrimary)
                    .Select(i => i.ImageUrl)
                    .FirstOrDefault(),

                Images = x.Images
                    .OrderBy(i => i.DisplayOrder)
                    .Select(i => new ProductImageDto
                    {
                        Id = i.Id,
                        ImageUrl = i.ImageUrl,
                        DisplayOrder = i.DisplayOrder,
                        IsPrimary = i.IsPrimary
                    })
                    .ToList(),

                Options = x.Options
                    .OrderBy(o => o.DisplayOrder)
                    .Select(o => new ProductOptionDto
                    {
                        Id = o.Id,
                        Name = o.Name,
                        DisplayOrder = o.DisplayOrder,

                        Values = o.Values
                            .OrderBy(v => v.DisplayOrder)
                            .Select(v => new ProductOptionValueDto
                            {
                                Id = v.Id,
                                Value = v.Value,
                                DisplayOrder = v.DisplayOrder
                            })
                            .ToList()
                    })
                    .ToList(),

                Variants = x.Variants
                    .Select(v => new ProductVariantDto
                    {
                        Id = v.Id,
                        SKU = v.SKU,
                        Price = v.Price,
                        StockQuantity = v.StockQuantity,
                        IsActive = v.IsActive,

                        OptionValueIds = v.OptionValues
                            .Select(vov => vov.ProductOptionValueId)
                            .ToList()
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto request)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Slug = request.Slug,
            Description = request.Description,
            Price = request.Price,
            StockQuantity = request.StockQuantity,
            CategoryId = request.CategoryId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        foreach (var imageRequest in request.Images)
        {
            product.Images.Add(new ProductImage
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                ImageUrl = imageRequest.ImageUrl,
                DisplayOrder = imageRequest.DisplayOrder,
                IsPrimary = imageRequest.IsPrimary
            });
        }

        foreach (var optionRequest in request.Options)
        {
            var option = new ProductOption
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                Name = optionRequest.Name,
                DisplayOrder = optionRequest.DisplayOrder
            };

            foreach (var valueRequest in optionRequest.Values)
            {
                option.Values.Add(new ProductOptionValue
                {
                    Id = Guid.NewGuid(),
                    ProductOptionId = option.Id,
                    Value = valueRequest.Value,
                    DisplayOrder = valueRequest.DisplayOrder
                });
            }

            product.Options.Add(option);
        }

        foreach (var variantRequest in request.Variants)
        {
            var variant = new ProductVariant
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                SKU = variantRequest.SKU,
                Price = variantRequest.Price,
                StockQuantity = variantRequest.StockQuantity,
                IsActive = true
            };

            foreach (var optionValueId in variantRequest.OptionValueIds)
            {
                variant.OptionValues.Add(new ProductVariantOptionValue
                {
                    ProductVariantId = variant.Id,
                    ProductOptionValueId = optionValueId
                });
            }

            product.Variants.Add(variant);
        }

        _context.Products.Add(product);

        await _context.SaveChangesAsync();

        return (await GetByIdAsync(product.Id))!;
    }

    public async Task<ProductResponseDto> GetLazyLoadProduct(ProductQueryDto query)
    {
        var products = _context.Products
            .AsNoTracking()
            .Where(x => x.IsActive);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();

            products = products.Where(x =>
                x.Name.Contains(search) ||
                x.Description.Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(query.SortField))
        {
            bool isAscending =
                string.Equals(
                    query.SortOrder,
                    "asc",
                    StringComparison.OrdinalIgnoreCase);

            var allowedColumns = new[]
            {
                "Name",
                "Price",
                "CreatedAt",
                "StockQuantity"
            };

            if (allowedColumns.Contains(query.SortField))
            {
                products = products.OrderByDynamic(
                    query.SortField,
                    isAscending);
            }
        }
        else
        {
            products = products.OrderByDescending(x => x.CreatedAt);
        }

        var totalCount = await products.CountAsync();

        var page = query.Page < 1 ? 1 : query.Page;
        var pageSize = query.PageSize < 1 ? 10 : query.PageSize;

        var data = await products
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new ProductDto
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                Price = x.Price,
                StockQuantity = x.StockQuantity,
                IsActive = x.IsActive,
                CategoryId = x.CategoryId,

                PrimaryImageUrl = x.Images
                    .Where(i => i.IsPrimary)
                    .Select(i => i.ImageUrl)
                    .FirstOrDefault()
            })
            .ToListAsync();

        return new ProductResponseDto
        {
            TotalCount = totalCount,
            Products = data
        };
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(x => x.Id == id);

        if (product == null)
            return false;

        product.IsActive = false;

        return await _context.SaveChangesAsync() > 0;
    }
}
