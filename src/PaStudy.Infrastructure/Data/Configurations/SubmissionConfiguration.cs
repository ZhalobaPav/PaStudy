using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaStudy.Core.Entities.Assignments.Submission;

namespace PaStudy.Infrastructure.Data.Configurations;

public class SubmissionConfiguration : IEntityTypeConfiguration<Submission>
{
    public void Configure(EntityTypeBuilder<Submission> builder)
    {
        builder.HasKey(s => s.Id);

        builder.HasDiscriminator<string>("SubmissionType")
            .HasValue<TaskSubmission>("Task")
            .HasValue<QuizSubmission>("Quiz");

        builder.Property(s => s.TeacherFeedback)
            .HasMaxLength(1000);

        builder.Property(s => s.Grade)
            .HasPrecision(5, 2);

        builder.HasOne(s => s.Assignment)
            .WithMany(a => a.Submissions)
            .HasForeignKey(s => s.AssignmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.Student)
            .WithMany()
            .HasForeignKey(s => s.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(s => new { s.AssignmentId, s.StudentId }).IsUnique();
    }
}

public class TaskSubmissionConfiguration : IEntityTypeConfiguration<TaskSubmission>
{
    public void Configure(EntityTypeBuilder<TaskSubmission> builder)
    {
        builder.Property(t => t.StudentNotes)
            .HasMaxLength(10000);

        builder.HasMany(t => t.Attachments)
            .WithOne()
            .HasForeignKey("TaskSubmissionId")
            .OnDelete(DeleteBehavior.Restrict);
    }
}