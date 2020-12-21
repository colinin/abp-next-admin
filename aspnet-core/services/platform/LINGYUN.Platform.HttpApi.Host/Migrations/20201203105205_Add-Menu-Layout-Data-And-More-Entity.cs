using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LINGYUN.Platform.Migrations
{
    public partial class AddMenuLayoutDataAndMoreEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppPlatformRoleRoute");

            migrationBuilder.DropTable(
                name: "AppPlatformRoute");

            migrationBuilder.DropTable(
                name: "AppPlatformUserRoute");

            migrationBuilder.CreateTable(
                name: "AppPlatformDatas",
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
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    Code = table.Column<string>(maxLength: 1024, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 128, nullable: false),
                    Description = table.Column<string>(maxLength: 128, nullable: true),
                    ParentId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPlatformDatas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppPlatformLayouts",
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
                    Path = table.Column<string>(maxLength: 255, nullable: true),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 128, nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Redirect = table.Column<string>(maxLength: 255, nullable: true),
                    Code = table.Column<string>(nullable: true),
                    PlatformType = table.Column<int>(nullable: false),
                    DataId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPlatformLayouts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppPlatformMenus",
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
                    Path = table.Column<string>(maxLength: 255, nullable: true),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 128, nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Redirect = table.Column<string>(maxLength: 255, nullable: true),
                    PlatformType = table.Column<int>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Component = table.Column<string>(nullable: true),
                    ParentId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPlatformMenus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppPlatformMenus_AppPlatformMenus_ParentId",
                        column: x => x.ParentId,
                        principalTable: "AppPlatformMenus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppPlatformRoleMenus",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: true),
                    MenuId = table.Column<Guid>(nullable: false),
                    RoleName = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPlatformRoleMenus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppPlatformUserMenus",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: true),
                    MenuId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPlatformUserMenus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppPlatformDataItems",
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
                    Name = table.Column<string>(maxLength: 30, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(maxLength: 128, nullable: false),
                    Description = table.Column<string>(maxLength: 128, nullable: true),
                    ValueType = table.Column<int>(nullable: false),
                    DataId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPlatformDataItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppPlatformDataItems_AppPlatformDatas_DataId",
                        column: x => x.DataId,
                        principalTable: "AppPlatformDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppPlatformDataItems_DataId",
                table: "AppPlatformDataItems",
                column: "DataId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPlatformDataItems_Name",
                table: "AppPlatformDataItems",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_AppPlatformDatas_Name",
                table: "AppPlatformDatas",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_AppPlatformMenus_ParentId",
                table: "AppPlatformMenus",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPlatformRoleMenus_RoleName_MenuId",
                table: "AppPlatformRoleMenus",
                columns: new[] { "RoleName", "MenuId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppPlatformUserMenus_UserId_MenuId",
                table: "AppPlatformUserMenus",
                columns: new[] { "UserId", "MenuId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppPlatformDataItems");

            migrationBuilder.DropTable(
                name: "AppPlatformLayouts");

            migrationBuilder.DropTable(
                name: "AppPlatformMenus");

            migrationBuilder.DropTable(
                name: "AppPlatformRoleMenus");

            migrationBuilder.DropTable(
                name: "AppPlatformUserMenus");

            migrationBuilder.DropTable(
                name: "AppPlatformDatas");

            migrationBuilder.CreateTable(
                name: "AppPlatformRoleRoute",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<string>(type: "char(36)", nullable: true),
                    DeleterId = table.Column<string>(type: "char(36)", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierId = table.Column<string>(type: "char(36)", nullable: true),
                    RoleName = table.Column<string>(type: "varchar(256) CHARACTER SET utf8mb4", maxLength: 256, nullable: false),
                    RouteId = table.Column<string>(type: "char(36)", nullable: false),
                    TenantId = table.Column<string>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPlatformRoleRoute", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppPlatformRoute",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(36)", nullable: false),
                    AlwaysShow = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Code = table.Column<string>(type: "varchar(95) CHARACTER SET utf8mb4", maxLength: 95, nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(40) CHARACTER SET utf8mb4", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<string>(type: "char(36)", nullable: true),
                    DeleterId = table.Column<string>(type: "char(36)", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Description = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    DisplayName = table.Column<string>(type: "varchar(128) CHARACTER SET utf8mb4", maxLength: 128, nullable: false),
                    ExtraProperties = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    FullName = table.Column<string>(type: "varchar(128) CHARACTER SET utf8mb4", maxLength: 128, nullable: true),
                    Icon = table.Column<string>(type: "varchar(128) CHARACTER SET utf8mb4", maxLength: 128, nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    IsMenu = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsPublic = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsSideBar = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsStatic = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsToolBar = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierId = table.Column<string>(type: "char(36)", nullable: true),
                    LinkUrl = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "varchar(64) CHARACTER SET utf8mb4", maxLength: 64, nullable: false),
                    ParentId = table.Column<string>(type: "char(36)", nullable: true),
                    PlatformType = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<string>(type: "char(36)", nullable: true)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<string>(type: "char(36)", nullable: true),
                    DeleterId = table.Column<string>(type: "char(36)", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierId = table.Column<string>(type: "char(36)", nullable: true),
                    RouteId = table.Column<string>(type: "char(36)", nullable: false),
                    TenantId = table.Column<string>(type: "char(36)", nullable: true),
                    UserId = table.Column<string>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPlatformUserRoute", x => x.Id);
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
        }
    }
}
