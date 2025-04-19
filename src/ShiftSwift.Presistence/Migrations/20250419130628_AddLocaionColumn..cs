using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftSwift.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLocaionColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Accounts");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Accounts");

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "Accounts",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);
        }
    }
}
