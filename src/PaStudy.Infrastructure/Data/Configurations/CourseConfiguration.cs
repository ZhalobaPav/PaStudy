using Microsoft.EntityFrameworkCore;
using PaStudy.Core.Entities;

namespace PaStudy.Infrastructure.Data.Configurations;
public class CourseConfiguration: IEntityTypeConfiguration<Course>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Course> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(c => c.Enrollments)
            .WithOne(e => e.Course)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.TeacherCourses)
            .WithOne(tc => tc.Course)
            .HasForeignKey(tc => tc.CourseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

