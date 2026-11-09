using DalilAcademy.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DalilAcademy.Infra.Data.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.FullName).IsRequired().HasMaxLength(200);
            builder.Property(s => s.Email).IsRequired().HasMaxLength(255);
            builder.Property(s => s.PhoneNumber).HasMaxLength(20);
            builder.HasIndex(s => s.Email).IsUnique();
            builder.HasIndex(s => s.UserId).IsUnique();
            builder.HasOne(s => s.User)
                .WithOne()
                .HasForeignKey<Student>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade); // or DeleteBehavior.SetNull, depending on your requirements
        }
    }
}
