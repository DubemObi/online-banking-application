using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace banking.Migrations
{
    /// <inheritdoc />
    public partial class RemovedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_Users_UserId",
                table: "BankAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Users_UserId",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Users_UserId",
                table: "Loans");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Loans_UserId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Cards_UserId",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_BankAccounts_UserId",
                table: "BankAccounts");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Loans",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminRemarks",
                table: "LoanRequests",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "LoanRequests",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ReviewedAt",
                table: "LoanRequests",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "LoanRequests",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Cards",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "CardRequests",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "BankAccounts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Loans_UserId1",
                table: "Loans",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_UserId1",
                table: "Cards",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_UserId1",
                table: "BankAccounts",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_AspNetUsers_UserId1",
                table: "BankAccounts",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_AspNetUsers_UserId1",
                table: "Cards",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_AspNetUsers_UserId1",
                table: "Loans",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_AspNetUsers_UserId1",
                table: "BankAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Cards_AspNetUsers_UserId1",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_AspNetUsers_UserId1",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_UserId1",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Cards_UserId1",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_BankAccounts_UserId1",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "AdminRemarks",
                table: "LoanRequests");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "LoanRequests");

            migrationBuilder.DropColumn(
                name: "ReviewedAt",
                table: "LoanRequests");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "LoanRequests");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "BankAccounts");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CardRequests",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    MobileNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Loans_UserId",
                table: "Loans",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_UserId",
                table: "Cards",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_UserId",
                table: "BankAccounts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_Users_UserId",
                table: "BankAccounts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Users_UserId",
                table: "Cards",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Users_UserId",
                table: "Loans",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
