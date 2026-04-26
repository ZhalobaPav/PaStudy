using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PaStudy.Core.Entities.Assignments.Submission;

namespace PaStudy.Infrastructure.Data.Configurations;

public class QuestionAnswerConfiguration : IEntityTypeConfiguration<QuestionAnswer>
{
    public void Configure(EntityTypeBuilder<QuestionAnswer> builder)
    {
        builder.HasKey(qa => qa.Id);

        builder.HasDiscriminator<string>("AnswerType")
            .HasValue<ChoiceAnswer>("Choice")
            .HasValue<MatchingAnswer>("Matching");

        builder.Property(qa => qa.PointsAwarded).HasPrecision(5, 2);

        builder.HasOne<QuizSubmission>()
            .WithMany(qs => qs.Answers)
            .HasForeignKey(qa => qa.QuizSubmissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class MatchingAnswerPairConfiguration : IEntityTypeConfiguration<MatchingAnswerPair>
{
    public void Configure(EntityTypeBuilder<MatchingAnswerPair> builder)
    {
        builder.HasKey(map => map.Id);

        builder.Property(map => map.SelectedRightSideValue).IsRequired();

        builder.HasOne(map => map.MatchingAnswer)
            .WithMany(ma => ma.SelectedPairs)
            .HasForeignKey(map => map.MatchingAnswerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(map => map.MatchingPair)
            .WithMany()
            .HasForeignKey(map => map.MatchingPairId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}