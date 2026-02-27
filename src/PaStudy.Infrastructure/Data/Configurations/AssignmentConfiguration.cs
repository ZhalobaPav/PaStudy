using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaStudy.Core.Entities.Assignments;

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
            .WithMany()
            .UsingEntity(j => j.ToTable("AssignmentAttachments"));

        builder.HasOne(a => a.Section)
            .WithMany(s => s.Assignments)
            .HasForeignKey(s => s.SectionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}