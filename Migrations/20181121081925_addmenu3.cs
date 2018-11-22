using Microsoft.EntityFrameworkCore.Migrations;

namespace mvc.Migrations
{
    public partial class addmenu3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "MenuItems",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Index",
                table: "MenuItems");
        }
    }
}
