using Microsoft.EntityFrameworkCore.Migrations;

namespace mvc.Migrations
{
    public partial class EidtPermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Permissions");

            migrationBuilder.AddColumn<string>(
                name: "PermissionDisplayName",
                table: "Permissions",
                maxLength: 32,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PermissionDisplayName",
                table: "Permissions");

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Permissions",
                nullable: false,
                defaultValue: 0);
        }
    }
}
