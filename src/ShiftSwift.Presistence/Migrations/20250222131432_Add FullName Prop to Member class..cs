using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftSwift.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFullNameProptoMemberclass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Account");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Members",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Members",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "Members",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Companies");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Account",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
