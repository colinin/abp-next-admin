using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LY.MicroService.LocalizationManagement.Migrations
{
    public partial class RmoveFieldTenantId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AbpLocalizationTexts");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AbpLocalizationResources");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AbpLocalizationLanguages");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "AbpLocalizationTexts",
                type: "char(36)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "AbpLocalizationResources",
                type: "char(36)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "AbpLocalizationLanguages",
                type: "char(36)",
                nullable: true);
        }
    }
}
