using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LY.MicroService.Applications.Single.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class UpgradeAbpFrameworkTo813 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tenant_Text_Template_Name",
                table: "AbpTextTemplates");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AbpTextTemplates");

            migrationBuilder.AddColumn<int>(
                name: "TimeoutDuration",
                table: "AbpWebhooksSubscriptions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "AbpTenants",
                type: "varchar(64)",
                maxLength: 64,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Tenant_Text_Template_Name",
                table: "AbpTextTemplates",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_AbpTenants_NormalizedName",
                table: "AbpTenants",
                column: "NormalizedName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tenant_Text_Template_Name",
                table: "AbpTextTemplates");

            migrationBuilder.DropIndex(
                name: "IX_AbpTenants_NormalizedName",
                table: "AbpTenants");

            migrationBuilder.DropColumn(
                name: "TimeoutDuration",
                table: "AbpWebhooksSubscriptions");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "AbpTenants");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "AbpTextTemplates",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Tenant_Text_Template_Name",
                table: "AbpTextTemplates",
                columns: new[] { "TenantId", "Name" });
        }
    }
}
