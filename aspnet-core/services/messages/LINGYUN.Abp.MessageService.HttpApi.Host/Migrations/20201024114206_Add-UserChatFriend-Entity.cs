using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LINGYUN.Abp.MessageService.Migrations
{
    public partial class AddUserChatFriendEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUserChatFriends",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    FrientId = table.Column<Guid>(nullable: false),
                    Black = table.Column<bool>(nullable: false),
                    DontDisturb = table.Column<bool>(nullable: false),
                    SpecialFocus = table.Column<bool>(nullable: false),
                    RemarkName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserChatFriends", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserChatFriends_TenantId_UserId_FrientId",
                table: "AppUserChatFriends",
                columns: new[] { "TenantId", "UserId", "FrientId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserChatFriends");
        }
    }
}
