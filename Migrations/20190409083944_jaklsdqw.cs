using Microsoft.EntityFrameworkCore.Migrations;

namespace mvc.Migrations
{
    public partial class jaklsdqw : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCustomCover",
                table: "BroadcastRooms",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCustomCover",
                table: "BroadcastRooms");
        }
    }
}
