using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PaStudy.Core.Entities;

namespace PaStudy.Infrastructure.Data.Configurations;
public class TeacherConfiguration: IEntityTypeConfiguration<Teacher>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Teacher> builder)
    {
        builder.HasKey(t => t.Id);

        builder
            .HasOne<IdentityUser>()
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(t => t.TeacherCourses)
            .WithOne(tc => tc.Teacher)
            .HasForeignKey(t => t.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.
            HasOne(t =>t.GroupOfCurator)
            .WithOne(t=> t.CuratorOfGroup)
            .HasForeignKey<Teacher>(t => t.GroupId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}

