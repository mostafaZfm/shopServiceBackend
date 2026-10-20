using Microsoft.EntityFrameworkCore;
using ShopServiceApi.Domain.Entities.Orders;
using ShopServiceApi.Domain.Entities.Products;
using ShopServiceApi.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore.Storage;

namespace ShopServiceApi.Abstractions.Interfaces
{
    public interface IApplicationDbContext
    {
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        DbSet<Product> Products { get; }

        DbSet<Category> Categories { get; }

        DbSet<Order> Orders { get; }

        DbSet<OrderItem> OrderItems { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        DbSet<RefreshToken> RefreshTokens { get; }
    }
}
