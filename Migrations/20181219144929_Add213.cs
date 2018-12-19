using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace mvc.Migrations
{
    public partial class Add213 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Anchors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    Follower = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anchors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BroadcastRooms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    AnchorId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 32, nullable: true),
                    RoomNum = table.Column<int>(nullable: false),
                    IsLiving = table.Column<bool>(nullable: false),
                    LastLiveTime = table.Column<DateTime>(nullable: true),
                    Viewer = table.Column<int>(nullable: false),
                    StreamChannel = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BroadcastRooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComsumeRecords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    GoodGuid = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<decimal>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    Count = table.Column<int>(nullable: false),
                    TotalPrice = table.Column<decimal>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComsumeRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GiftReceiveRecords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SendUserId = table.Column<int>(nullable: false),
                    ReceiveUserId = table.Column<int>(nullable: false),
                    GiftId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiftReceiveRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gifts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Guid = table.Column<string>(nullable: true),
                    Type = table.Column<string>(maxLength: 32, nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    ImgUrl = table.Column<string>(nullable: true),
                    IsShow = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gifts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LiveClasses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ParentId = table.Column<int>(nullable: false),
                    IsFinal = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveClasses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    GoodGuid = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    UnitPrice = table.Column<decimal>(nullable: false),
                    PayWay = table.Column<string>(nullable: true),
                    Count = table.Column<int>(nullable: false),
                    ToTalPrice = table.Column<decimal>(nullable: false),
                    RecevieMoney = table.Column<decimal>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CompleteTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RechargeRecords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<decimal>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    BZ = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Count = table.Column<decimal>(nullable: false),
                    PayWay = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RechargeRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Servers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IpAddress = table.Column<string>(nullable: true),
                    Port = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAssets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    Level = table.Column<int>(nullable: false),
                    Exp = table.Column<int>(nullable: false),
                    Gold = table.Column<decimal>(nullable: false),
                    Silver = table.Column<decimal>(nullable: false),
                    RechargeAmount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAssets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserGifts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    GiftId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGifts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Anchors");

            migrationBuilder.DropTable(
                name: "BroadcastRooms");

            migrationBuilder.DropTable(
                name: "ComsumeRecords");

            migrationBuilder.DropTable(
                name: "GiftReceiveRecords");

            migrationBuilder.DropTable(
                name: "Gifts");

            migrationBuilder.DropTable(
                name: "LiveClasses");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "RechargeRecords");

            migrationBuilder.DropTable(
                name: "Servers");

            migrationBuilder.DropTable(
                name: "UserAssets");

            migrationBuilder.DropTable(
                name: "UserGifts");
        }
    }
}
