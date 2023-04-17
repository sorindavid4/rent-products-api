using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace rent_products_api.Migrations
{
    public partial class rentchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Confirmed",
                table: "Rents");

            migrationBuilder.DropColumn(
                name: "IsRejected",
                table: "Rents");

            migrationBuilder.AddColumn<DateTime>(
                name: "ConfirmationTime",
                table: "Rents",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Rents",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmationTime",
                table: "Rents");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Rents");

            migrationBuilder.AddColumn<bool>(
                name: "Confirmed",
                table: "Rents",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRejected",
                table: "Rents",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
