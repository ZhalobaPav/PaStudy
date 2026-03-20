using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaStudy.Core.Entities.Assignments.Questions;

namespace PaStudy.Infrastructure.Data.Configurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.ToTable("Questions")
            .HasDiscriminator<string>("QuestionType")
            .HasValue<ChoiceQuestion>("Choice")
            .HasValue<MatchingQuestion>("Matching");

        builder.Property(q => q.Text)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(q => q.Feedback)
            .HasMaxLength(1000);

        builder.Property(q => q.Points)
            .IsRequired();

        builder.HasMany(q => q.Attachments)
            .WithMany()
            .UsingEntity(j => j.ToTable("QuestionAttachments"));
    }
}

public class ChoiceQuestionConfiguration : IEntityTypeConfiguration<ChoiceQuestion>
{
    public void Configure(EntityTypeBuilder<ChoiceQuestion> builder)
    {

        builder.HasMany(q => q.Options)
            .WithOne()
            .HasForeignKey(o => o.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}

public class MatchingQuestionConfiguration : IEntityTypeConfiguration<MatchingQuestion>
{
    public void Configure(EntityTypeBuilder<MatchingQuestion> builder)
    {

        builder.HasMany(q => q.Pairs)
            .WithOne()
            .HasForeignKey(p => p.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}