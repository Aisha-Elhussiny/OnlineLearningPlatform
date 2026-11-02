using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineLearningPlatform.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineLearningPlatform.Configurations
{
    public class InstructorConfiguration
          : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.FullName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(i => i.Email)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(i => i.Bio)
                .HasMaxLength(500);

            builder.Property(i => i.JoinedAt)
                .IsRequired();
            builder.HasData(
    new Instructor
    {
        Id = 1,
        FullName = "Ahmed Ali",
        Email = "ahmed@mail.com",
        JoinedAt = new DateTime(2026, 1, 1)
    },
    new Instructor
    {
        Id = 2,
        FullName = "Mona Hassan",
        Email = "mona@mail.com",
        JoinedAt = new DateTime(2026, 1, 2)
    },
    new Instructor
    {
        Id = 3,
        FullName = "Omar Adel",
        Email = "omar@mail.com",
        JoinedAt = new DateTime(2026, 1, 3)
    }
);
        }

        
    }
}
