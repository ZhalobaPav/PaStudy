using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaStudy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSectionAssignmentRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignment_Course_CourseId",
                table: "Assignment");

            migrationBuilder.DropIndex(
                name: "IX_Assignment_CourseId",
                table: "Assignment");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Assignment");

            migrationBuilder.AddColumn<int>(
                name: "SectionId",
                table: "Assignment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Section",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Section", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Section_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_SectionId",
                table: "Assignment",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Section_CourseId",
                table: "Section",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignment_Section_SectionId",
                table: "Assignment",
                column: "SectionId",
                principalTable: "Section",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignment_Section_SectionId",
                table: "Assignment");

            migrationBuilder.DropTable(
                name: "Section");

            migrationBuilder.DropIndex(
                name: "IX_Assignment_SectionId",
                table: "Assignment");

            migrationBuilder.DropColumn(
                name: "SectionId",
                table: "Assignment");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Assignment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_CourseId",
                table: "Assignment",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignment_Course_CourseId",
                table: "Assignment",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
