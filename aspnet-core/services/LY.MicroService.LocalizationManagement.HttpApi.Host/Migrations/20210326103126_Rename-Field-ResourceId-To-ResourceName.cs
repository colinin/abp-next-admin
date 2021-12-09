using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LY.MicroService.LocalizationManagement.Migrations
{
    public partial class RenameFieldResourceIdToResourceName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResourceId",
                table: "AbpLocalizationTexts");

            migrationBuilder.AddColumn<string>(
                name: "ResourceName",
                table: "AbpLocalizationTexts",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResourceName",
                table: "AbpLocalizationTexts");

            migrationBuilder.AddColumn<Guid>(
                name: "ResourceId",
                table: "AbpLocalizationTexts",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
