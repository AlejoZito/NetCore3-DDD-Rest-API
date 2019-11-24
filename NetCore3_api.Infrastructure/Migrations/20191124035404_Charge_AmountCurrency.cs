using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCore3_api.Infrastructure.Migrations
{
    public partial class Charge_AmountCurrency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "Charges",
                newName: "Amount_Currency");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Charges",
                newName: "Amount_Amount");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount_Amount",
                table: "Charges",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount_Currency",
                table: "Charges",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "Amount_Amount",
                table: "Charges",
                newName: "Amount");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Charges",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);
        }
    }
}
