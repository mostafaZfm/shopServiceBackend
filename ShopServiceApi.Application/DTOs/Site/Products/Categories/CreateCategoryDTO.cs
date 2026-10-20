using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShopServiceApi.Application.DTOs.Site.Products.Categories
{
    public class CreateCategoryDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(120, MinimumLength = 2)]
        [RegularExpression(@"^[a-zA-Z0-9]+(?:-[a-zA-Z0-9]+)*$")]
        public string Slug { get; set; } = null!;

        [StringLength(500)]
        public string? Description { get; set; }

        public Guid? ParentCategoryId { get; set; }
    }
    public class CategoryDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Slug { get; set; } = null!;

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public Guid? ParentCategoryId { get; set; }
    }
}
