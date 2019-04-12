using Microsoft.EntityFrameworkCore.Migrations;

namespace mvc.Migrations
{
    public partial class add_user_follow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserFollows",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    AnchorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFollows", x => new { x.UserId, x.AnchorId });
                    table.UniqueConstraint("AK_UserFollows_AnchorId_UserId", x => new { x.AnchorId, x.UserId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFollows");
        }
    }
}
