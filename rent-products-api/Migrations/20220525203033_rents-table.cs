using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace rent_products_api.Migrations
{
    public partial class rentstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rents",
                columns: table => new
                {
                    RentId = table.Column<Guid>(type: "char(36)", nullable: false),
                    RentedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    StartingHour = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    EndingHour = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    PaymentType = table.Column<int>(type: "int", nullable: false),
                    Confirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PaymentConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ProductId = table.Column<Guid>(type: "char(36)", nullable: false),
                    RentedByUserId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rents", x => x.RentId);
                    table.ForeignKey(
                        name: "FK_Rents_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rents_Users_RentedByUserId",
                        column: x => x.RentedByUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rents_ProductId",
                table: "Rents",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Rents_RentedByUserId",
                table: "Rents",
                column: "RentedByUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rents");
        }
    }
}
