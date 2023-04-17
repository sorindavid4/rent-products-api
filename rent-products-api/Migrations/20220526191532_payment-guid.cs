using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace rent_products_api.Migrations
{
    public partial class paymentguid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Rents_ForRentId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_ForRentId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ForRentId",
                table: "Payments");

            migrationBuilder.AlterColumn<Guid>(
                name: "PaymentId",
                table: "Payments",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.CreateIndex(
                name: "IX_Rents_PaymentId",
                table: "Rents",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rents_Payments_PaymentId",
                table: "Rents",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "PaymentId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rents_Payments_PaymentId",
                table: "Rents");

            migrationBuilder.DropIndex(
                name: "IX_Rents_PaymentId",
                table: "Rents");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentId",
                table: "Payments",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<Guid>(
                name: "ForRentId",
                table: "Payments",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ForRentId",
                table: "Payments",
                column: "ForRentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Rents_ForRentId",
                table: "Payments",
                column: "ForRentId",
                principalTable: "Rents",
                principalColumn: "RentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
