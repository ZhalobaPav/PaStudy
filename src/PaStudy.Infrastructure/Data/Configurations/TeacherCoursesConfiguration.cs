using Microsoft.EntityFrameworkCore;
using PaStudy.Core.Entities.ConnectionEntities;

namespace PaStudy.Infrastructure.Data.Configurations;
public class TeacherCoursesConfiguration: IEntityTypeConfiguration<TeacherCourses>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<TeacherCourses> builder)
    {
        builder.HasKey(tc => new { tc.TeacherId, tc.CourseId });

        builder
            .HasOne(tc => tc.Teacher)
            .WithMany(t => t.TeacherCourses)
            .HasForeignKey(tc => tc.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(tc => tc.Course)
            .WithMany(c => c.TeacherCourses)
            .HasForeignKey(tc => tc.CourseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

