using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace rent_products_api.Migrations
{
    public partial class payments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentConfirmed",
                table: "Rents");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "StartingHour",
                table: "Rents",
                type: "time(6)",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EndingHour",
                table: "Rents",
                type: "time(6)",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PaymentId",
                table: "Rents",
                type: "char(36)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PricePerDay",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PricePerHour",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    PaymentConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ForRentId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payments_Rents_ForRentId",
                        column: x => x.ForRentId,
                        principalTable: "Rents",
                        principalColumn: "RentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ForRentId",
                table: "Payments",
                column: "ForRentId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Rents");

            migrationBuilder.DropColumn(
                name: "PricePerDay",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PricePerHour",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "StartingHour",
                table: "Rents",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time(6)");

            migrationBuilder.AlterColumn<string>(
                name: "EndingHour",
                table: "Rents",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time(6)");

            migrationBuilder.AddColumn<bool>(
                name: "PaymentConfirmed",
                table: "Rents",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
