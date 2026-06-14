using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaStudy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class notificationStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InvitationStatus",
                table: "Notification",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvitationStatus",
                table: "Notification");
        }
    }
}
