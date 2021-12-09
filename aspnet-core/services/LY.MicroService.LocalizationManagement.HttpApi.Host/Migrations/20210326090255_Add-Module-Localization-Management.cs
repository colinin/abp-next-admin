using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LY.MicroService.LocalizationManagement.Migrations
{
    public partial class AddModuleLocalizationManagement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbpLocalizationLanguages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true),
                    Enable = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    CultureName = table.Column<string>(type: "varchar(20) CHARACTER SET utf8mb4", maxLength: 20, nullable: false),
                    UiCultureName = table.Column<string>(type: "varchar(20) CHARACTER SET utf8mb4", maxLength: 20, nullable: false),
                    DisplayName = table.Column<string>(type: "varchar(64) CHARACTER SET utf8mb4", maxLength: 64, nullable: false),
                    FlagIcon = table.Column<string>(type: "varchar(30) CHARACTER SET utf8mb4", maxLength: 30, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpLocalizationLanguages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpLocalizationResources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true),
                    Enable = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    Name = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: false),
                    DisplayName = table.Column<string>(type: "varchar(64) CHARACTER SET utf8mb4", maxLength: 64, nullable: true),
                    Description = table.Column<string>(type: "varchar(64) CHARACTER SET utf8mb4", maxLength: 64, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpLocalizationResources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpLocalizationTexts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true),
                    CultureName = table.Column<string>(type: "varchar(20) CHARACTER SET utf8mb4", maxLength: 20, nullable: false),
                    Key = table.Column<string>(type: "varchar(512) CHARACTER SET utf8mb4", maxLength: 512, nullable: false),
                    Value = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", maxLength: 2048, nullable: true),
                    ResourceId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpLocalizationTexts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbpLocalizationLanguages_CultureName",
                table: "AbpLocalizationLanguages",
                column: "CultureName");

            migrationBuilder.CreateIndex(
                name: "IX_AbpLocalizationResources_Name",
                table: "AbpLocalizationResources",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_AbpLocalizationTexts_Key",
                table: "AbpLocalizationTexts",
                column: "Key");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbpLocalizationLanguages");

            migrationBuilder.DropTable(
                name: "AbpLocalizationResources");

            migrationBuilder.DropTable(
                name: "AbpLocalizationTexts");
        }
    }
}
