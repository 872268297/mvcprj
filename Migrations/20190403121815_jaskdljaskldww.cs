using Microsoft.EntityFrameworkCore.Migrations;

namespace mvc.Migrations
{
    public partial class jaskdljaskldww : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "UserAssets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Sex",
                table: "UserAssets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sign",
                table: "UserAssets",
                maxLength: 60,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImgUrl",
                table: "LiveClasses",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "UserAssets");

            migrationBuilder.DropColumn(
                name: "Sex",
                table: "UserAssets");

            migrationBuilder.DropColumn(
                name: "Sign",
                table: "UserAssets");

            migrationBuilder.DropColumn(
                name: "ImgUrl",
                table: "LiveClasses");
        }
    }
}
