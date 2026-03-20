using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PaStudy.Core.Entities.Assignments.Questions;

namespace PaStudy.Infrastructure.Data.Configurations;

public class AnswerOptionConfiguration : IEntityTypeConfiguration<AwnserOption>
{
    public void Configure(EntityTypeBuilder<AwnserOption> builder)
    {
        builder.ToTable("AnswerOptions");

        builder.Property(o => o.Text)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(o => o.IsCorrect)
            .HasDefaultValue(false);
    }
}

public class MatchingPairConfiguration : IEntityTypeConfiguration<MatchingPair>
{
    public void Configure(EntityTypeBuilder<MatchingPair> builder)
    {
        builder.ToTable("MatchingPairs");

        builder.Property(p => p.LeftSide)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(p => p.RightSide)
            .IsRequired()
            .HasMaxLength(500);
    }
}