using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaStudy.Core.Entities.Attachments;

namespace PaStudy.Infrastructure.Data.Configurations;

public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder.Property(a => a.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(a => a.FileUrl)
            .IsRequired()
            .HasMaxLength(2048);

        builder.Property(a => a.ContentType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.FileSize)
            .IsRequired();

        builder.HasDiscriminator<string>("AttachmentType")
            .HasValue<ImageAttachment>("Image")
            .HasValue<DocumentAttachment>("Document");

        builder.Property(a => (a as ImageAttachment).Width)
        .HasColumnName("Width");

        builder.Property(a => (a as ImageAttachment).Height)
            .HasColumnName("Height");
    }
}
