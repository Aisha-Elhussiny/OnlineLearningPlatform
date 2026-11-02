using Microsoft.EntityFrameworkCore;
using OnlineLearningPlatform.Configurations;
using OnlineLearningPlatform.Models;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace OnlineLearningPlatform.Data
{
    public class PlatformDbContext : DbContext
    {
        public DbSet<Instructor> Instructors { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=10.211.55.2,1434;Database=OnlineLearningPlatformDb;User Id=sa;Password=Route@2026SQL;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(
                new InstructorConfiguration());

            modelBuilder.ApplyConfiguration(
                new CategoryConfiguration());

            modelBuilder.ApplyConfiguration(
                new CourseConfiguration());

            modelBuilder.ApplyConfiguration(
                new StudentConfiguration());

            modelBuilder.ApplyConfiguration(
                new EnrollmentConfiguration());
        }
    }
}
