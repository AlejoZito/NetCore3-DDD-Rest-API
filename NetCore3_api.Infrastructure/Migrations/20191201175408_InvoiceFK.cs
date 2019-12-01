using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCore3_api.Infrastructure.Migrations
{
    public partial class InvoiceFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "InvoiceId",
                table: "Charges",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Charges_InvoiceId",
                table: "Charges",
                column: "InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Charges_Invoices_InvoiceId",
                table: "Charges",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Charges_Invoices_InvoiceId",
                table: "Charges");

            migrationBuilder.DropIndex(
                name: "IX_Charges_InvoiceId",
                table: "Charges");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "Charges");
        }
    }
}
