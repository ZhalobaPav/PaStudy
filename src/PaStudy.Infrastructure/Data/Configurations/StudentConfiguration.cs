using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaStudy.Core.Entities;

namespace PaStudy.Infrastructure.Data.Configurations;
public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.GroupId)
            .IsRequired();

        builder.Property(s => s.UserId)
            .IsRequired();

        builder
            .HasOne(s => s.Group)
            .WithMany(g => g.Students)
            .HasForeignKey(s => s.GroupId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<IdentityUser>()
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .HasPrincipalKey("Id")
            .OnDelete(DeleteBehavior.Restrict);
    }
}

