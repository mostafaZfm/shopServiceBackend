using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductEntity = ShopServiceApi.Domain.Entities.Products.Product;
namespace ShopServiceApi.Infrastructure.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.ToTable("Products"); 
            // Primary Key
            builder.HasKey(x => x.Id); 
            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Slug).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Description).IsRequired(); 
            builder.Property(x => x.Price).IsRequired().HasPrecision(18, 2);
            builder.Property(x => x.StockQuantity).IsRequired()
        .HasDefaultValue(0);

            builder.Property(x => x.IsActive) .IsRequired() .HasDefaultValue(true); 
            builder.Property(x => x.CreatedAt) .IsRequired(); 
            // Relationship: Product -> Category
            builder.HasOne(x => x.Category) .WithMany(x => x.Products) .HasForeignKey(x => x.CategoryId) .OnDelete(DeleteBehavior.Restrict); 
            // Indexes
            builder.HasIndex(x => x.Slug) .IsUnique(); 
            builder.HasIndex(x => x.CategoryId);
            builder.HasIndex(x => x.IsActive);
        }
    }
}
