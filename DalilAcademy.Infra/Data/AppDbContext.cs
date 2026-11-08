using DalilAcademy.Core.Entities;
using DalilAcademy.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DalilAcademy.Infra.Data
{
    // Options (provider + connection string) come from DI - see AddDbContext in Program.cs
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        // DbSet
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Instructor> Instructors => Set<Instructor>();
        public DbSet<Enrollment> Enrollments => Set<Enrollment>();
        public DbSet<User> Users => Set<User>();

        // Finds every IEntityTypeConfiguration<T> in this project
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Runs automatically on Update-Database
            optionsBuilder.UseSeeding((context, _) =>
            {
                // Students
                if (!context.Set<Student>().Any())
                {
                    context.Set<Student>().AddRange(
                        new Student { FullName = "Lina Haddad", Email = "lina@example.com" },
                        new Student { FullName = "Omar Khalil", Email = "omar@example.com" },
                        new Student { FullName = "Sara Nasser", Email = "sara@example.com" });
                    context.SaveChanges();
                }

                // Instructors
                if (!context.Set<Instructor>().Any())
                {
                    var ahmad = new Instructor { FullName = "Ahmad Saleh" };
                    var lina = new Instructor { FullName = "Lina Haddad" };

                    // Courses
                    context.Set<Course>().AddRange(
                    new Course { Title = "C# Fundamentals", Price = 50, Instructor = ahmad },
                    new Course { Title = "ASP.NET Core Web API", Price = 80, Instructor = ahmad },
                    new Course { Title = "PostgreSQL Essentials", Price = 60, Instructor = lina }
                    );
                    context.SaveChanges();
                }

                // Users
                if (!context.Set<User>().Any())
                {
                    context.Set<User>().Add(new User
                    {
                        Email = "admin@dalil.jo",
                        FullName = "Admin User",
                        Role = Roles.Admin,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123") // wrapped in a class later
                    });
                    context.SaveChanges();
                }
            });
        }
    }
}