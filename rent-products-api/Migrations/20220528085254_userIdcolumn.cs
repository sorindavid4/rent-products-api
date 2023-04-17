using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace rent_products_api.Migrations
{
    public partial class userIdcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserPayingId",
                table: "Payments",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UserPayingId",
                table: "Payments",
                column: "UserPayingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Users_UserPayingId",
                table: "Payments",
                column: "UserPayingId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Users_UserPayingId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_UserPayingId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "UserPayingId",
                table: "Payments");
        }
    }
}
