using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftSwift.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyProfileFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Accounts",
                newName: "Overview");

            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "Accounts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Accounts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Accounts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfEstablish",
                table: "Accounts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Field",
                table: "Accounts",
                type: "nvarchar(155)",
                maxLength: 155,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Area",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "DateOfEstablish",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Field",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "Overview",
                table: "Accounts",
                newName: "Description");
        }
    }
}
