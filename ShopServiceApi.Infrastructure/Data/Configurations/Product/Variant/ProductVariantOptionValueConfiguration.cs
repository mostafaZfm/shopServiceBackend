using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopServiceApi.Domain.Entities.Products.Variants;

namespace ShopServiceApi.Infrastructure.Data.Configurations.Product.Variant
{
    public class ProductVariantOptionValueConfiguration : IEntityTypeConfiguration<ProductVariantOptionValue>
    {
        public void Configure(
        EntityTypeBuilder<ProductVariantOptionValue> builder)
        {
            builder.ToTable("ProductVariantOptionValues");

            // Composite Primary Key
            builder.HasKey(x => new
            {
                x.ProductVariantId,
                x.ProductOptionValueId
            });

            // Relationship:
            // ProductVariantOptionValue -> ProductVariant
            builder.HasOne(x => x.ProductVariant)
                .WithMany(x => x.OptionValues)
                .HasForeignKey(x => x.ProductVariantId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship:
            // ProductVariantOptionValue -> ProductOptionValue
            builder.HasOne(x => x.ProductOptionValue)
                .WithMany(x => x.Variants)
                .HasForeignKey(x => x.ProductOptionValueId)
                .OnDelete(DeleteBehavior.Cascade);

            // Index
            builder.HasIndex(x => x.ProductOptionValueId);
        }
    }
}
