using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaStudy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentRelationToAttempt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "QuizAttempt",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuizAttempt_StudentId",
                table: "QuizAttempt",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizAttempt_Student_StudentId",
                table: "QuizAttempt",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizAttempt_Student_StudentId",
                table: "QuizAttempt");

            migrationBuilder.DropIndex(
                name: "IX_QuizAttempt_StudentId",
                table: "QuizAttempt");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "QuizAttempt");
        }
    }
}
