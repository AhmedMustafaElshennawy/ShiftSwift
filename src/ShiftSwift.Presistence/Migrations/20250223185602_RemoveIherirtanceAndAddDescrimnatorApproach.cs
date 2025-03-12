using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftSwift.Presistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIherirtanceAndAddDescrimnatorApproach : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accomplishments_Members_MemberId",
                table: "Accomplishments");

            migrationBuilder.DropForeignKey(
                name: "FK_Educations_Members_MemberId",
                table: "Educations");

            migrationBuilder.DropForeignKey(
                name: "FK_Experiences_Members_MemberId",
                table: "Experiences");

            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_Members_MemberId",
                table: "JobApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Companies_CompanyId",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Companies_CompanyId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_SavedJobs_Members_MemberId",
                table: "SavedJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Members_MemberId",
                table: "Skills");

            migrationBuilder.DropForeignKey(
                name: "FK_UserClaims_Account_UserId",
                table: "UserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLogins_Account_UserId",
                table: "UserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Account_UserId",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTokens_Account_UserId",
                table: "UserTokens");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Account",
                table: "Account");

            migrationBuilder.RenameTable(
                name: "Account",
                newName: "Accounts");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Accounts",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Accounts",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Accounts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Accounts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Accounts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "Accounts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProfileViews",
                table: "Accounts",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "Accounts",
                type: "float",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accomplishments_Accounts_MemberId",
                table: "Accomplishments",
                column: "MemberId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Educations_Accounts_MemberId",
                table: "Educations",
                column: "MemberId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Experiences_Accounts_MemberId",
                table: "Experiences",
                column: "MemberId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_Accounts_MemberId",
                table: "JobApplications",
                column: "MemberId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Accounts_CompanyId",
                table: "Jobs",
                column: "CompanyId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Accounts_CompanyId",
                table: "Ratings",
                column: "CompanyId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SavedJobs_Accounts_MemberId",
                table: "SavedJobs",
                column: "MemberId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Accounts_MemberId",
                table: "Skills",
                column: "MemberId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserClaims_Accounts_UserId",
                table: "UserClaims",
                column: "UserId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogins_Accounts_UserId",
                table: "UserLogins",
                column: "UserId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Accounts_UserId",
                table: "UserRoles",
                column: "UserId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTokens_Accounts_UserId",
                table: "UserTokens",
                column: "UserId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accomplishments_Accounts_MemberId",
                table: "Accomplishments");

            migrationBuilder.DropForeignKey(
                name: "FK_Educations_Accounts_MemberId",
                table: "Educations");

            migrationBuilder.DropForeignKey(
                name: "FK_Experiences_Accounts_MemberId",
                table: "Experiences");

            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_Accounts_MemberId",
                table: "JobApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Accounts_CompanyId",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Accounts_CompanyId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_SavedJobs_Accounts_MemberId",
                table: "SavedJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Accounts_MemberId",
                table: "Skills");

            migrationBuilder.DropForeignKey(
                name: "FK_UserClaims_Accounts_UserId",
                table: "UserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLogins_Accounts_UserId",
                table: "UserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Accounts_UserId",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTokens_Accounts_UserId",
                table: "UserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "ProfileViews",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Accounts");

            migrationBuilder.RenameTable(
                name: "Accounts",
                newName: "Account");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Account",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Account",
                table: "Account",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Rating = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileViews = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Members_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_AccountId",
                table: "Companies",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_AccountId",
                table: "Members",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accomplishments_Members_MemberId",
                table: "Accomplishments",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Educations_Members_MemberId",
                table: "Educations",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Experiences_Members_MemberId",
                table: "Experiences",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_Members_MemberId",
                table: "JobApplications",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Companies_CompanyId",
                table: "Jobs",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Companies_CompanyId",
                table: "Ratings",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SavedJobs_Members_MemberId",
                table: "SavedJobs",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Members_MemberId",
                table: "Skills",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserClaims_Account_UserId",
                table: "UserClaims",
                column: "UserId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogins_Account_UserId",
                table: "UserLogins",
                column: "UserId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Account_UserId",
                table: "UserRoles",
                column: "UserId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTokens_Account_UserId",
                table: "UserTokens",
                column: "UserId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
