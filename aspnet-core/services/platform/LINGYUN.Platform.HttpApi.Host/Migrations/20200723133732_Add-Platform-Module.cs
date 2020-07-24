using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LINGYUN.Platform.Migrations
{
    public partial class AddPlatformModule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppPlatformRoleRoute",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: true),
                    RoleName = table.Column<string>(maxLength: 256, nullable: false),
                    RouteId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPlatformRoleRoute", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppPlatformRoute",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ExtraProperties = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: true),
                    Code = table.Column<string>(maxLength: 95, nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    FullName = table.Column<string>(maxLength: 128, nullable: true),
                    DisplayName = table.Column<string>(maxLength: 128, nullable: false),
                    PlatformType = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    Icon = table.Column<string>(maxLength: 128, nullable: true),
                    LinkUrl = table.Column<string>(maxLength: 255, nullable: false),
                    IsMenu = table.Column<bool>(nullable: false),
                    IsToolBar = table.Column<bool>(nullable: false),
                    IsSideBar = table.Column<bool>(nullable: false),
                    IsPublic = table.Column<bool>(nullable: false),
                    IsStatic = table.Column<bool>(nullable: false),
                    AlwaysShow = table.Column<bool>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPlatformRoute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppPlatformRoute_AppPlatformRoute_ParentId",
                        column: x => x.ParentId,
                        principalTable: "AppPlatformRoute",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppPlatformUserRoute",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    RouteId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPlatformUserRoute", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppPlatformVersion",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ExtraProperties = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: true),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    Version = table.Column<string>(maxLength: 20, nullable: false),
                    Description = table.Column<string>(maxLength: 2048, nullable: true),
                    Level = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPlatformVersion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppPlatformVersionFile",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Version = table.Column<string>(maxLength: 20, nullable: false),
                    Size = table.Column<long>(nullable: false),
                    FileType = table.Column<int>(nullable: false),
                    SHA256 = table.Column<string>(maxLength: 32, nullable: false),
                    DownloadCount = table.Column<int>(nullable: false),
                    AppVersionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPlatformVersionFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppPlatformVersionFile_AppPlatformVersion_AppVersionId",
                        column: x => x.AppVersionId,
                        principalTable: "AppPlatformVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppPlatformRoleRoute_RoleName_RouteId",
                table: "AppPlatformRoleRoute",
                columns: new[] { "RoleName", "RouteId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppPlatformRoute_Code",
                table: "AppPlatformRoute",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_AppPlatformRoute_ParentId",
                table: "AppPlatformRoute",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPlatformUserRoute_UserId_RouteId",
                table: "AppPlatformUserRoute",
                columns: new[] { "UserId", "RouteId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppPlatformVersion_Version",
                table: "AppPlatformVersion",
                column: "Version");

            migrationBuilder.CreateIndex(
                name: "IX_AppPlatformVersionFile_AppVersionId",
                table: "AppPlatformVersionFile",
                column: "AppVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPlatformVersionFile_Name_Version",
                table: "AppPlatformVersionFile",
                columns: new[] { "Name", "Version" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppPlatformRoleRoute");

            migrationBuilder.DropTable(
                name: "AppPlatformRoute");

            migrationBuilder.DropTable(
                name: "AppPlatformUserRoute");

            migrationBuilder.DropTable(
                name: "AppPlatformVersionFile");

            migrationBuilder.DropTable(
                name: "AppPlatformVersion");
        }
    }
}
