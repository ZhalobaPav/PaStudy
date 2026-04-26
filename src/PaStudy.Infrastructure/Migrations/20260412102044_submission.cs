using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaStudy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class submission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Submission");

            migrationBuilder.DropColumn(
                name: "FileUrl",
                table: "Submission");

            migrationBuilder.AddColumn<int>(
                name: "AttemptNumber",
                table: "Submission",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "Status",
                table: "Submission",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "StudentNotes",
                table: "Submission",
                type: "nvarchar(max)",
                maxLength: 10000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubmissionType",
                table: "Submission",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimeTaken",
                table: "Submission",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaskSubmissionId",
                table: "Attachment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "QuestionAnswer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    QuizSubmissionId = table.Column<int>(type: "int", nullable: false),
                    PointsAwarded = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    AnswerType = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    SelectedOptionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionAnswer_Submission_QuizSubmissionId",
                        column: x => x.QuizSubmissionId,
                        principalTable: "Submission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchingAnswerPair",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MatchingAnswerId = table.Column<int>(type: "int", nullable: false),
                    MatchingPairId = table.Column<int>(type: "int", nullable: false),
                    SelectedRightSideValue = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchingAnswerPair", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchingAnswerPair_MatchingPairs_MatchingPairId",
                        column: x => x.MatchingPairId,
                        principalTable: "MatchingPairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MatchingAnswerPair_QuestionAnswer_MatchingAnswerId",
                        column: x => x.MatchingAnswerId,
                        principalTable: "QuestionAnswer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_TaskSubmissionId",
                table: "Attachment",
                column: "TaskSubmissionId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchingAnswerPair_MatchingAnswerId",
                table: "MatchingAnswerPair",
                column: "MatchingAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchingAnswerPair_MatchingPairId",
                table: "MatchingAnswerPair",
                column: "MatchingPairId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswer_QuizSubmissionId",
                table: "QuestionAnswer",
                column: "QuizSubmissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachment_Submission_TaskSubmissionId",
                table: "Attachment",
                column: "TaskSubmissionId",
                principalTable: "Submission",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachment_Submission_TaskSubmissionId",
                table: "Attachment");

            migrationBuilder.DropTable(
                name: "MatchingAnswerPair");

            migrationBuilder.DropTable(
                name: "QuestionAnswer");

            migrationBuilder.DropIndex(
                name: "IX_Attachment_TaskSubmissionId",
                table: "Attachment");

            migrationBuilder.DropColumn(
                name: "AttemptNumber",
                table: "Submission");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Submission");

            migrationBuilder.DropColumn(
                name: "StudentNotes",
                table: "Submission");

            migrationBuilder.DropColumn(
                name: "SubmissionType",
                table: "Submission");

            migrationBuilder.DropColumn(
                name: "TimeTaken",
                table: "Submission");

            migrationBuilder.DropColumn(
                name: "TaskSubmissionId",
                table: "Attachment");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Submission",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileUrl",
                table: "Submission",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }
    }
}
