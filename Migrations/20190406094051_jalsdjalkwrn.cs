using Microsoft.EntityFrameworkCore.Migrations;

namespace mvc.Migrations
{
    public partial class jalsdjalkwrn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NickName",
                table: "UserAssets",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "BroadcastRooms",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CoverUrl",
                table: "BroadcastRooms",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBan",
                table: "BroadcastRooms",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Notice",
                table: "BroadcastRooms",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StreamCode",
                table: "BroadcastRooms",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NickName",
                table: "UserAssets");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "BroadcastRooms");

            migrationBuilder.DropColumn(
                name: "CoverUrl",
                table: "BroadcastRooms");

            migrationBuilder.DropColumn(
                name: "IsBan",
                table: "BroadcastRooms");

            migrationBuilder.DropColumn(
                name: "Notice",
                table: "BroadcastRooms");

            migrationBuilder.DropColumn(
                name: "StreamCode",
                table: "BroadcastRooms");
        }
    }
}
