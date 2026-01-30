using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaStudy.Core.Entities;

namespace PaStudy.Infrastructure.Data.Configurations;

public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
{
    public void Configure(EntityTypeBuilder<Assignment> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.Description)
            .HasMaxLength(2000);

        builder.Property(a => a.MaxPoints)
            .HasDefaultValue(100);

        builder.HasMany(a => a.Attachments)
            .WithOne(at => at.Assignment)
            .HasForeignKey(at => at.AssignmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Section)
            .WithMany(s => s.Assignments)
            .HasForeignKey(s => s.SectionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}