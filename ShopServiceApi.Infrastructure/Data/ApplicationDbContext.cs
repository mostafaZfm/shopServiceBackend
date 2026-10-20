using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopServiceApi.Abstractions.Interfaces;
using ShopServiceApi.Domain.Entities.Orders;
using ShopServiceApi.Domain.Entities.Products;
using ShopServiceApi.Infrastructure.Identity;
using ShopServiceApi.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore.Storage;

namespace ShopServiceApi.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>,IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();

        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await Database.BeginTransactionAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(
                typeof(ApplicationDbContext).Assembly);

            builder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Token)
                    .IsRequired();

                entity.HasIndex(x => x.Token)
                    .IsUnique();

                entity.Property(x => x.UserId)
                    .IsRequired();
            });
        }

    }
}
