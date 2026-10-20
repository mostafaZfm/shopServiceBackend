
using Microsoft.EntityFrameworkCore;
using ShopServiceApi.Application.DTOs.Site.Products;
using ShopServiceApi.Abstractions.Interfaces;
using ShopServiceApi.Application.Services.Products.Interface;
using ShopServiceApi.Domain.Entities.Products;
using ShopServiceApi.Domain.Entities.Products.Variants;
using ShopServiceApi.Domain.Utility;
using ShopServiceApi.Application.DTOs.Site.Products.Categories;

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
        // اعتبارسنجی اولیه
        if (request.Price <= 0)
            throw new ArgumentException("Price must be greater than zero.");

        if (request.StockQuantity < 0)
            throw new ArgumentException("Stock quantity cannot be negative.");

        if (!await _context.Categories.AnyAsync(c => c.Id == request.CategoryId))
            throw new ArgumentException("Category not found.");

        if (await _context.Products.AnyAsync(p => p.Slug == request.Slug))
            throw new ArgumentException("Slug already exists.");

        if (request.Images.Count(i => i.IsPrimary) > 1)
            throw new ArgumentException("Only one primary image is allowed.");

        var duplicateSku = request.Variants
            .GroupBy(v => v.SKU)
            .FirstOrDefault(g => g.Count() > 1);

        if (duplicateSku != null)
            throw new ArgumentException($"Duplicate SKU: {duplicateSku.Key}");

        if (await _context.Products
            .SelectMany(p => p.Variants)
            .AnyAsync(v => request.Variants.Select(x => x.SKU).Contains(v.SKU)))
        {
            throw new ArgumentException("One or more SKUs already exist.");
        }

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Slug = request.Slug.Trim().ToLowerInvariant(),
            Description = request.Description,
            Price = request.Price,
            StockQuantity = request.StockQuantity,
            CategoryId = request.CategoryId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // اگر تصویر اصلی مشخص نشده بود، اولین تصویر را اصلی کن
        if (request.Images.Any() && request.Images.All(i => !i.IsPrimary))
            request.Images[0].IsPrimary = true;

        foreach (var imageRequest in request.Images.OrderBy(i => i.DisplayOrder))
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

        // دیکشنری برای پیدا کردن OptionValueها
        var optionValueMap = new Dictionary<(string Option, string Value), ProductOptionValue>();

        foreach (var optionRequest in request.Options.OrderBy(o => o.DisplayOrder))
        {
            var option = new ProductOption
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                Name = optionRequest.Name.Trim(),
                DisplayOrder = optionRequest.DisplayOrder
            };

            foreach (var valueRequest in optionRequest.Values.OrderBy(v => v.DisplayOrder))
            {
                var value = new ProductOptionValue
                {
                    Id = Guid.NewGuid(),
                    ProductOptionId = option.Id,
                    Value = valueRequest.Value.Trim(),
                    DisplayOrder = valueRequest.DisplayOrder
                };

                option.Values.Add(value);

                optionValueMap[(option.Name.ToLowerInvariant(),
                    value.Value.ToLowerInvariant())] = value;
            }

            product.Options.Add(option);
        }

        foreach (var variantRequest in request.Variants)
        {
            if (variantRequest.Price <= 0)
                throw new ArgumentException($"Variant {variantRequest.SKU} has invalid price.");

            if (variantRequest.StockQuantity < 0)
                throw new ArgumentException($"Variant {variantRequest.SKU} has invalid stock.");

            var variant = new ProductVariant
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                SKU = variantRequest.SKU.Trim(),
                Price = variantRequest.Price,
                StockQuantity = variantRequest.StockQuantity,
                IsActive = true
            };

            foreach (var selectedOption in variantRequest.Options)
            {
                var key = (
                    selectedOption.OptionName.Trim().ToLowerInvariant(),
                    selectedOption.Value.Trim().ToLowerInvariant());

                if (!optionValueMap.TryGetValue(key, out var optionValue))
                {
                    throw new ArgumentException(
                        $"Option '{selectedOption.OptionName}' with value '{selectedOption.Value}' was not found.");
                }

                variant.OptionValues.Add(new ProductVariantOptionValue
                {
                    ProductVariantId = variant.Id,
                    ProductOptionValueId = optionValue.Id
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

        // Search
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();

            products = products.Where(x =>
                x.Name.Contains(search) ||
                (x.Description != null && x.Description.Contains(search)));
        }

        // Count before pagination
        var totalCount = await products.CountAsync();

        // Pagination
        var page = query.Page < 1 ? 1 : query.Page;

        var pageSize = query.PageSize < 1
            ? 10
            : Math.Min(query.PageSize, 100);

        // Sorting
        bool isAscending = string.Equals(
            query.SortOrder,
            "asc",
            StringComparison.OrdinalIgnoreCase);

        IOrderedQueryable<Product> orderedProducts;

        switch (query.SortField?.Trim().ToLowerInvariant())
        {
            case "name":
                orderedProducts = isAscending
                    ? products.OrderBy(x => x.Name)
                    : products.OrderByDescending(x => x.Name);
                break;

            case "price":
                orderedProducts = isAscending
                    ? products.OrderBy(x => x.Price)
                    : products.OrderByDescending(x => x.Price);
                break;

            case "createdat":
                orderedProducts = isAscending
                    ? products.OrderBy(x => x.CreatedAt)
                    : products.OrderByDescending(x => x.CreatedAt);
                break;

            case "stockquantity":
                orderedProducts = isAscending
                    ? products.OrderBy(x => x.StockQuantity)
                    : products.OrderByDescending(x => x.StockQuantity);
                break;

            default:
                orderedProducts = products.OrderByDescending(x => x.CreatedAt);
                break;
        }

        // Fetch current page
        var data = await orderedProducts
            .ThenBy(x => x.Id)
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

        // Return response
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

    public async Task<CategoryDto> CreateAsync(
        CreateCategoryDto request)
    {
        var name = request.Name.Trim();
        var slug = request.Slug.Trim().ToLowerInvariant();

        if (await _context.Categories.AnyAsync(
            c => c.Slug.ToLower() == slug))
        {
            throw new ArgumentException(
                "دسته بندی مورد نظر شما تکراری می باشد");
        }

        if (request.ParentCategoryId.HasValue)
        {
            var parent = await _context.Categories
                .FirstOrDefaultAsync(
                    c => c.Id == request.ParentCategoryId.Value);

            if (parent is null)
            {
                throw new ArgumentException(
                    "Parent category not found.");
            }

            if (!parent.IsActive)
            {
                throw new ArgumentException(
                    "Parent category is inactive.");
            }
        }

        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = name,
            Slug = slug,
            Description = request.Description?.Trim(),
            ParentCategoryId = request.ParentCategoryId,
            IsActive = true
        };

        _context.Categories.Add(category);

        await _context.SaveChangesAsync();

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Slug = category.Slug,
            Description = category.Description,
            IsActive = category.IsActive,
            ParentCategoryId = category.ParentCategoryId
        };
    }
}
