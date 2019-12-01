using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCore3_api.Infrastructure.Migrations
{
    public partial class PaymentMapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentCharge_Payments_PaymentId",
                table: "PaymentCharge");

            migrationBuilder.AlterColumn<long>(
                name: "PaymentId",
                table: "PaymentCharge",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentCharge_Payments_PaymentId",
                table: "PaymentCharge",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentCharge_Payments_PaymentId",
                table: "PaymentCharge");

            migrationBuilder.AlterColumn<long>(
                name: "PaymentId",
                table: "PaymentCharge",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentCharge_Payments_PaymentId",
                table: "PaymentCharge",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
