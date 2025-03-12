using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftSwift.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class updatepropertyname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Companies_CompanyId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "CompanytId",
                table: "Ratings");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyId",
                table: "Ratings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Companies_CompanyId",
                table: "Ratings",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Companies_CompanyId",
                table: "Ratings");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyId",
                table: "Ratings",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "CompanytId",
                table: "Ratings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Companies_CompanyId",
                table: "Ratings",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
        }
    }
}
