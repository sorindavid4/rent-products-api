using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace rent_products_api.Migrations
{
    public partial class updateblob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "ProductImages");

            migrationBuilder.AddColumn<byte[]>(
                name: "Data",
                table: "ProductImages",
                type: "longblob",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "ProductImages");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "ProductImages",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
