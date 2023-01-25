using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopTshirts.Migrations
{
    public partial class new1Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "productImg",
                table: "Products",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(256)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "productImg",
                table: "Products",
                type: "NVARCHAR(256)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
