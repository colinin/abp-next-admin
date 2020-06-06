using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LINGYUN.Abp.MessageService.Migrations
{
    public partial class AddChatMessageEntites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ReadStatus",
                table: "AppUserNotifications",
                nullable: false,
                oldClrType: typeof(sbyte),
                oldType: "tinyint");

            migrationBuilder.CreateTable(
                name: "AppChatGroupAdmins",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: true),
                    GroupId = table.Column<long>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    IsSuperAdmin = table.Column<bool>(nullable: false),
                    AllowSilence = table.Column<bool>(nullable: false),
                    AllowKickPeople = table.Column<bool>(nullable: false),
                    AllowAddPeople = table.Column<bool>(nullable: false),
                    AllowSendNotice = table.Column<bool>(nullable: false),
                    AllowDissolveGroup = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppChatGroupAdmins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppChatGroups",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: true),
                    GroupId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(maxLength: 20, nullable: false),
                    Tag = table.Column<string>(maxLength: 512, nullable: true),
                    Address = table.Column<string>(maxLength: 256, nullable: true),
                    Notice = table.Column<string>(maxLength: 64, nullable: true),
                    MaxUserCount = table.Column<int>(nullable: false),
                    AllowAnonymous = table.Column<bool>(nullable: false),
                    AllowSendMessage = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppChatGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppGroupChatBlacks",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: true),
                    GroupId = table.Column<long>(nullable: false),
                    ShieldUserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppGroupChatBlacks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppGroupMessages",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: true),
                    MessageId = table.Column<long>(nullable: false),
                    SendUserName = table.Column<string>(maxLength: 64, nullable: false),
                    Content = table.Column<string>(maxLength: 1048576, nullable: false),
                    Type = table.Column<int>(nullable: false),
                    SendState = table.Column<sbyte>(nullable: false),
                    GroupId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppGroupMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserChatBlacks",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    ShieldUserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserChatBlacks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserChatGroups",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    GroupId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserChatGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserChatSettings",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<Guid>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    AllowAnonymous = table.Column<bool>(nullable: false),
                    AllowAddFriend = table.Column<bool>(nullable: false),
                    RequireAddFriendValition = table.Column<bool>(nullable: false),
                    AllowReceiveMessage = table.Column<bool>(nullable: false),
                    AllowSendMessage = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserChatSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserMessages",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: true),
                    MessageId = table.Column<long>(nullable: false),
                    SendUserName = table.Column<string>(maxLength: 64, nullable: false),
                    Content = table.Column<string>(maxLength: 1048576, nullable: false),
                    Type = table.Column<int>(nullable: false),
                    SendState = table.Column<sbyte>(nullable: false),
                    ReceiveUserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserSpecialFocuss",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    FocusUserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserSpecialFocuss", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppChatGroupAdmins_TenantId_GroupId",
                table: "AppChatGroupAdmins",
                columns: new[] { "TenantId", "GroupId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppChatGroups_TenantId_Name",
                table: "AppChatGroups",
                columns: new[] { "TenantId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_AppGroupChatBlacks_TenantId_GroupId",
                table: "AppGroupChatBlacks",
                columns: new[] { "TenantId", "GroupId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppGroupMessages_TenantId_GroupId",
                table: "AppGroupMessages",
                columns: new[] { "TenantId", "GroupId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserChatBlacks_TenantId_UserId",
                table: "AppUserChatBlacks",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserChatGroups_TenantId_GroupId_UserId",
                table: "AppUserChatGroups",
                columns: new[] { "TenantId", "GroupId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserChatSettings_TenantId_UserId",
                table: "AppUserChatSettings",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserMessages_TenantId_ReceiveUserId",
                table: "AppUserMessages",
                columns: new[] { "TenantId", "ReceiveUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserSpecialFocuss_TenantId_UserId",
                table: "AppUserSpecialFocuss",
                columns: new[] { "TenantId", "UserId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppChatGroupAdmins");

            migrationBuilder.DropTable(
                name: "AppChatGroups");

            migrationBuilder.DropTable(
                name: "AppGroupChatBlacks");

            migrationBuilder.DropTable(
                name: "AppGroupMessages");

            migrationBuilder.DropTable(
                name: "AppUserChatBlacks");

            migrationBuilder.DropTable(
                name: "AppUserChatGroups");

            migrationBuilder.DropTable(
                name: "AppUserChatSettings");

            migrationBuilder.DropTable(
                name: "AppUserMessages");

            migrationBuilder.DropTable(
                name: "AppUserSpecialFocuss");

            migrationBuilder.AlterColumn<sbyte>(
                name: "ReadStatus",
                table: "AppUserNotifications",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
