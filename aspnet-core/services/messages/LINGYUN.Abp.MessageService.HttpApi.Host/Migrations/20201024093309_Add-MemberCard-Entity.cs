using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LINGYUN.Abp.MessageService.Migrations
{
    public partial class AddMemberCardEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppChatGroupAdmins");

            migrationBuilder.AddColumn<Guid>(
                name: "AdminUserId",
                table: "AppChatGroups",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "AppUserChatCards",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ExtraProperties = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: false),
                    Sex = table.Column<int>(nullable: false),
                    Sign = table.Column<string>(maxLength: 30, nullable: true),
                    NickName = table.Column<string>(maxLength: 256, nullable: true),
                    Description = table.Column<string>(maxLength: 50, nullable: true),
                    AvatarUrl = table.Column<string>(maxLength: 512, nullable: true),
                    Birthday = table.Column<DateTime>(nullable: true),
                    Age = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserChatCards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserGroupCards",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ExtraProperties = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    NickName = table.Column<string>(maxLength: 256, nullable: true),
                    IsAdmin = table.Column<bool>(nullable: false),
                    SilenceEnd = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserGroupCards", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserChatCards_TenantId_UserId",
                table: "AppUserChatCards",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserGroupCards_TenantId_UserId",
                table: "AppUserGroupCards",
                columns: new[] { "TenantId", "UserId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserChatCards");

            migrationBuilder.DropTable(
                name: "AppUserGroupCards");

            migrationBuilder.DropColumn(
                name: "AdminUserId",
                table: "AppChatGroups");

            migrationBuilder.CreateTable(
                name: "AppChatGroupAdmins",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AllowAddPeople = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowDissolveGroup = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowKickPeople = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowSendNotice = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllowSilence = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<string>(type: "char(36)", nullable: true),
                    GroupId = table.Column<long>(type: "bigint", nullable: false),
                    IsSuperAdmin = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierId = table.Column<string>(type: "char(36)", nullable: true),
                    TenantId = table.Column<string>(type: "char(36)", nullable: true),
                    UserId = table.Column<string>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppChatGroupAdmins", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppChatGroupAdmins_TenantId_GroupId",
                table: "AppChatGroupAdmins",
                columns: new[] { "TenantId", "GroupId" });
        }
    }
}
