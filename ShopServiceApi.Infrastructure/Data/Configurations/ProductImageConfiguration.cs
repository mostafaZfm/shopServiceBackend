using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopServiceApi.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServiceApi.Infrastructure.Data.Configurations
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("ProductImages"); 
            builder.HasKey(x => x.Id); 
            builder.Property(x => x.ImageUrl)
                .IsRequired()
                .HasMaxLength(500);
            builder.Property(x => x.IsPrimary)
                .IsRequired()
                .HasDefaultValue(false); 
            builder.Property(x => x.DisplayOrder)
                .IsRequired()
                .HasDefaultValue(0); 
            builder.HasOne(x => x.Product)
                .WithMany(x => x.Images)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade); 
            builder.HasIndex(x => new { x.ProductId, x.DisplayOrder }); 
            // هر محصول فقط یک تصویر اصلی داشته باشد
            builder.HasIndex(x => x.ProductId).IsUnique().HasFilter("\"IsPrimary\" = true");
        }
    }
}
