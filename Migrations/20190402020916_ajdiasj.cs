using Microsoft.EntityFrameworkCore.Migrations;

namespace mvc.Migrations
{
    public partial class ajdiasj : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFinal",
                table: "LiveClasses");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "LiveClasses",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "LiveClasses");

            migrationBuilder.AddColumn<bool>(
                name: "IsFinal",
                table: "LiveClasses",
                nullable: false,
                defaultValue: false);
        }
    }
}
