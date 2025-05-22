using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftSwift.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class AddJobQuestionsAndAnswers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    QuestionType = table.Column<int>(type: "int", nullable: false),
                    JobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobQuestions_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobQuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnswerText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnswerBool = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationAnswers_JobApplications_JobApplicationId",
                        column: x => x.JobApplicationId,
                        principalTable: "JobApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationAnswers_JobQuestions_JobQuestionId",
                        column: x => x.JobQuestionId,
                        principalTable: "JobQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationAnswers_JobApplicationId",
                table: "ApplicationAnswers",
                column: "JobApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationAnswers_JobQuestionId",
                table: "ApplicationAnswers",
                column: "JobQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_JobQuestions_JobId",
                table: "JobQuestions",
                column: "JobId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationAnswers");

            migrationBuilder.DropTable(
                name: "JobQuestions");
        }
    }
}
