using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftSwift.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class Update_Education_Properties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SchoolName",
                table: "Educations",
                newName: "UniversityName");

            migrationBuilder.RenameColumn(
                name: "LevelOfEducation",
                table: "Educations",
                newName: "Level");

            migrationBuilder.RenameColumn(
                name: "FieldOfStudy",
                table: "Educations",
                newName: "Faculty");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UniversityName",
                table: "Educations",
                newName: "SchoolName");

            migrationBuilder.RenameColumn(
                name: "Level",
                table: "Educations",
                newName: "LevelOfEducation");

            migrationBuilder.RenameColumn(
                name: "Faculty",
                table: "Educations",
                newName: "FieldOfStudy");
        }
    }
}
