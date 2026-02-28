using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LINGYUN.Abp.MicroService.LocalizationService.Migrations
{
    /// <inheritdoc />
    public partial class Initial_Localization_Service : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbpLocalizationLanguages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Enable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CultureName = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    UiCultureName = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    TwoLetterISOLanguageName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpLocalizationLanguages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpLocalizationResources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Enable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Description = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    DefaultCultureName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpLocalizationResources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpLocalizationTexts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CultureName = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Key = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    Value = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    ResourceName = table.Column<string>(type: "text", nullable: true)
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

        /// <inheritdoc />
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
