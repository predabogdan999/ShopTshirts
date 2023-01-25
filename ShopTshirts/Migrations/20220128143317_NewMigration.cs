using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopTshirts.Migrations
{
    public partial class NewMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "productImg",
                table: "Products",
                type: "NVARCHAR(256)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "productImg",
                table: "Products",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(256)",
                oldNullable: true);
        }
    }
}
