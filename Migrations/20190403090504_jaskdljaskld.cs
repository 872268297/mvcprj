using Microsoft.EntityFrameworkCore.Migrations;

namespace mvc.Migrations
{
    public partial class jaskdljaskld : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HeadIcon",
                table: "UserAssets",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeadIcon",
                table: "UserAssets");
        }
    }
}
