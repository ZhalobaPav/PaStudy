using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaStudy.Core.Entities.Notification;

namespace PaStudy.Infrastructure.Data.Configurations;

public class NotificationConfiguration: IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Title)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(n => n.Message)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(n => n.ClickActionUrl)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(n => n.Type)
            .IsRequired();

        builder.Property(n => n.IsRead)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(n => n.RecipientUserId)
            .HasMaxLength(450) 
            .IsRequired(false);

        builder.HasOne(n => n.Course)
            .WithMany()
            .HasForeignKey(n => n.CourseId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);

        builder.HasIndex(n => new { n.RecipientUserId, n.IsRead })
            .HasDatabaseName("IX_Notification_Recipient_IsRead")
            .HasFilter("[RecipientUserId] IS NOT NULL");

        builder.HasIndex(n => n.CourseId)
            .HasDatabaseName("IX_Notification_CourseId")
            .HasFilter("[CourseId] IS NOT NULL");
    }
}
