using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineLearningPlatform.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineLearningPlatform.Configurations
{
    public class CourseConfiguration
        : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(c => c.Description)
                .HasMaxLength(1000);

            builder.Property(c => c.Price)
                .HasPrecision(10, 2);

            builder.Property(c => c.Level)
                .IsRequired();

            builder.Property(c => c.IsPublished)
                .IsRequired();

            builder.HasOne(c => c.Instructor)
                .WithMany(i => i.Courses)
                .HasForeignKey(c => c.InstructorId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(c => c.Category)
                .WithMany(c => c.Courses)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasData(
    new Course
    {
        Id = 1,
        Title = "C# Basics",
        Price = 500m,
        Level = CourseLevel.Beginner,
        IsPublished = true,
        PublishedAt = new DateTime(2026, 2, 1),
        InstructorId = 1,
        CategoryId = 2
    },
    new Course
    {
        Id = 2,
        Title = "EF Core",
        Price = 900m,
        Level = CourseLevel.Intermediate,
        IsPublished = true,
        PublishedAt = new DateTime(2026, 2, 2),
        InstructorId = 1,
        CategoryId = 3
    },
    new Course
    {
        Id = 3,
        Title = "ASP.NET Advanced",
        Price = 1500m,
        Level = CourseLevel.Advanced,
        IsPublished = false,
        InstructorId = 2,
        CategoryId = 3
    },
    new Course
    {
        Id = 4,
        Title = "HTML and CSS",
        Price = 400m,
        Level = CourseLevel.Beginner,
        IsPublished = true,
        PublishedAt = new DateTime(2026, 2, 3),
        InstructorId = 2,
        CategoryId = 4
    },
    new Course
    {
        Id = 5,
        Title = "Angular",
        Price = 1000m,
        Level = CourseLevel.Intermediate,
        IsPublished = false,
        InstructorId = 3,
        CategoryId = 4
    },
    new Course
    {
        Id = 6,
        Title = "UI Design",
        Price = 700m,
        Level = CourseLevel.Advanced,
        IsPublished = true,
        PublishedAt = new DateTime(2026, 2, 4),
        InstructorId = 3,
        CategoryId = 5
    }
);

        }

       
    }
}
