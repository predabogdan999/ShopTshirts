using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopTshirts.Migrations
{
    public partial class MyFirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "categories",
                table: "Products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "categories",
                table: "Products",
                nullable: false,
                defaultValue: 0);
        }
    }
}
