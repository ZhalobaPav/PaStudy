using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaStudy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class coursedescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Course",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Course",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Course_CategoryId",
                table: "Course",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Category_CategoryId",
                table: "Course",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_Category_CategoryId",
                table: "Course");

            migrationBuilder.DropIndex(
                name: "IX_Course_CategoryId",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Course");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Course",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);
        }
    }
}
