using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineLearningPlatform.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineLearningPlatform.Configurations
{
    public class StudentConfiguration
        : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.FullName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(s => s.DateOfBirth)
                .IsRequired()
                .HasColumnType("date");
        }

      
    }
}
