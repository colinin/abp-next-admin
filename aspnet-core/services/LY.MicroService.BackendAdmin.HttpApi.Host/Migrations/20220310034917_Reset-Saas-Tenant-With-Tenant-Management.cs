using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LY.MicroService.BackendAdmin.Migrations
{
    public partial class ResetSaasTenantWithTenantManagement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DisableTime",
                table: "AbpTenants",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EditionId",
                table: "AbpTenants",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "EnableTime",
                table: "AbpTenants",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AbpTenants",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_AbpTenants_EditionId",
                table: "AbpTenants",
                column: "EditionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpTenants_AbpEditions_EditionId",
                table: "AbpTenants",
                column: "EditionId",
                principalTable: "AbpEditions",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpTenants_AbpEditions_EditionId",
                table: "AbpTenants");

            migrationBuilder.DropIndex(
                name: "IX_AbpTenants_EditionId",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "DisableTime",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "EditionId",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "EnableTime",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AbpTenants");
        }
    }
}
