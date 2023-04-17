using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace rent_products_api.Migrations
{
    public partial class rejectrents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRejected",
                table: "Rents",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "RejectionTime",
                table: "Rents",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRejected",
                table: "Rents");

            migrationBuilder.DropColumn(
                name: "RejectionTime",
                table: "Rents");
        }
    }
}
