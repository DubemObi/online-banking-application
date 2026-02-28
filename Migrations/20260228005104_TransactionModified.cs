using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace banking.Migrations
{
    /// <inheritdoc />
    public partial class TransactionModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_BankAccounts_BankAccountAccountId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_BankAccountAccountId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "BankAccountAccountId",
                table: "Transactions");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "RecipientAccountId",
                table: "Transactions",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "Transactions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "AccountBalance",
                table: "BankAccounts",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "BankAccounts",
                type: "BLOB",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountId",
                table: "Transactions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_RecipientAccountId",
                table: "Transactions",
                column: "RecipientAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_BankAccounts_AccountId",
                table: "Transactions",
                column: "AccountId",
                principalTable: "BankAccounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_BankAccounts_RecipientAccountId",
                table: "Transactions",
                column: "RecipientAccountId",
                principalTable: "BankAccounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_BankAccounts_AccountId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_BankAccounts_RecipientAccountId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_AccountId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_RecipientAccountId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "RecipientAccountId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Reference",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "BankAccounts");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Transactions",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "BankAccountAccountId",
                table: "Transactions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "AccountBalance",
                table: "BankAccounts",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BankAccountAccountId",
                table: "Transactions",
                column: "BankAccountAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_BankAccounts_BankAccountAccountId",
                table: "Transactions",
                column: "BankAccountAccountId",
                principalTable: "BankAccounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
