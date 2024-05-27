using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LY.MicroService.BackendAdmin.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class RemoteTenantIdWithTextTemplate : Migration
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

            migrationBuilder.CreateIndex(
                name: "IX_Tenant_Text_Template_Name",
                table: "AbpTextTemplates",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tenant_Text_Template_Name",
                table: "AbpTextTemplates");

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
