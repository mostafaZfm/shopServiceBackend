using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopServiceApi.Domain.Entities.Products;

namespace ShopServiceApi.Infrastructure.Data.Configurations.Product
{
    public class ProductOptionValueConfiguration : IEntityTypeConfiguration<ProductOptionValue>
    {
        public void Configure(EntityTypeBuilder<ProductOptionValue> builder)
        {
            builder.ToTable("ProductOptionValues");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Value)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.DisplayOrder)
                .IsRequired()
                .HasDefaultValue(0);

            // Relationship:
            // ProductOptionValue -> ProductOption
            builder.HasOne(x => x.ProductOption)
                .WithMany(x => x.Values)
                .HasForeignKey(x => x.ProductOptionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => new
            {
                x.ProductOptionId,
                x.Value
            })
            .IsUnique();

            builder.HasIndex(x => new
            {
                x.ProductOptionId,
                x.DisplayOrder
            });
        }
    }
}
