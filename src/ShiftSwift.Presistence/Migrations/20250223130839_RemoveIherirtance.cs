using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftSwift.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIherirtance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Account_Id",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Members_Account_Id",
                table: "Members");

            migrationBuilder.AddColumn<string>(
                name: "AccountId",
                table: "Members",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AccountId",
                table: "Companies",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Members_AccountId",
                table: "Members",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_AccountId",
                table: "Companies",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Account_AccountId",
                table: "Companies",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Account_AccountId",
                table: "Members",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Account_AccountId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Members_Account_AccountId",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Members_AccountId",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Companies_AccountId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Companies");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Account_Id",
                table: "Companies",
                column: "Id",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Account_Id",
                table: "Members",
                column: "Id",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
