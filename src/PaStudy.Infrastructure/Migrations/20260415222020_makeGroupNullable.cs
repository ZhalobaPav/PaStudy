using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaStudy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class makeGroupNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Teacher_GroupId",
                table: "Teacher");

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "Teacher",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Teacher_GroupId",
                table: "Teacher",
                column: "GroupId",
                unique: true,
                filter: "[GroupId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Teacher_GroupId",
                table: "Teacher");

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "Teacher",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teacher_GroupId",
                table: "Teacher",
                column: "GroupId",
                unique: true);
        }
    }
}
