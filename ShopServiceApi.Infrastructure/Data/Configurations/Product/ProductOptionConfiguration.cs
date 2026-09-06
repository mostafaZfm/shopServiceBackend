using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopServiceApi.Domain.Entities.Products;

namespace ShopServiceApi.Infrastructure.Data.Configurations.Product
{
    public class ProductOptionConfiguration : IEntityTypeConfiguration<ProductOption>

    {
        public void Configure(EntityTypeBuilder<ProductOption> builder)
        {
            builder.ToTable("ProductOptions");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.DisplayOrder)
                .IsRequired()
                .HasDefaultValue(0);

            // Relationship: ProductOption -> Product
            builder.HasOne(x => x.Product)
                .WithMany(x => x.Options)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => new { x.ProductId, x.Name })
                .IsUnique();

            builder.HasIndex(x => new { x.ProductId, x.DisplayOrder });
        }
    }
}
