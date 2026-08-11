using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopServiceApi.Infrastructure.Site.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServiceApi.Infrastructure.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Slug)
                .IsRequired()
                .HasMaxLength(150)
                .ValueGeneratedNever();

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.HasIndex(x => x.Slug)
                .IsUnique();


            builder.HasOne(x => x.ParentCategory)
                .WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Products)
                .WithOne(x => x.Category)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
