using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PaStudy.Core.Entities.Assignments;
using System.Text.Json;

namespace PaStudy.Infrastructure.Data.Configurations;

public class QuizAttemptConfiguration : IEntityTypeConfiguration<QuizAttempt>
{
    public void Configure(EntityTypeBuilder<QuizAttempt> builder)
    {
        builder.HasKey(qa => qa.Id);

        builder.HasOne(qa => qa.Quiz)
            .WithMany()
            .HasForeignKey(qa => qa.QuizId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(qa => qa.UserId)
            .IsRequired();

        // Зв'язок із відповідями
        builder.HasMany(qa => qa.Answers)
            .WithOne(a => a.Attempt)
            .HasForeignKey(a => a.QuizAttemptId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(qa => qa.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(qa => qa.TotalScore)
            .HasPrecision(18, 2);

        builder.Property(qa => qa.StartedAt)
            .IsRequired();
    }
}
public class QuizAttemptAnswerConfiguration : IEntityTypeConfiguration<QuizAttemptAnswer>
{
    public void Configure(EntityTypeBuilder<QuizAttemptAnswer> builder)
    {
        var intListConverter = new ValueConverter<List<int>?, string>(
            v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
            v => JsonSerializer.Deserialize<List<int>>(v, JsonSerializerOptions.Default) ?? new List<int>());

        var dictConverter = new ValueConverter<Dictionary<string, string>?, string>(
            v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
            v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, JsonSerializerOptions.Default) ?? new Dictionary<string, string>());

        builder.Property(qaa => qaa.SelectedOptionIds)
            .HasConversion(intListConverter);

        builder.Property(qaa => qaa.MatchingAnswers)
            .HasConversion(dictConverter);

        builder.Property(qaa => qaa.PointsEarned)
            .HasPrecision(18, 2);

        builder.HasOne(qaa => qaa.Question)
            .WithMany()
            .HasForeignKey(qaa => qaa.QuestionId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(qaa => qaa.TextResponse)
            .HasMaxLength(4000);
    }
}