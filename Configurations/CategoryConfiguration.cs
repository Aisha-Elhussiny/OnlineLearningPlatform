using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineLearningPlatform.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineLearningPlatform.Configurations
{
    public class CategoryConfiguration
       : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Description)
                .HasMaxLength(500);

            builder.HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasData(
    new Category
    {
        Id = 1,
        Name = "Programming"
    },
    new Category
    {
        Id = 2,
        Name = "Backend",
        ParentCategoryId = 1
    },
    new Category
    {
        Id = 3,
        Name = "ASP.NET",
        ParentCategoryId = 2
    },
    new Category
    {
        Id = 4,
        Name = "Frontend",
        ParentCategoryId = 1
    },
    new Category
    {
        Id = 5,
        Name = "Design"
    }
);
        }


      
    }
}
