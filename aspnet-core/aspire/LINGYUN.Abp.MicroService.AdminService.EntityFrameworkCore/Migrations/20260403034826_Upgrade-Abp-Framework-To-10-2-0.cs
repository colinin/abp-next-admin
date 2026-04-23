using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LINGYUN.Abp.MicroService.AdminService.Migrations
{
    /// <inheritdoc />
    public partial class UpgradeAbpFrameworkTo1020 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AbpPermissions_Name",
                table: "AbpPermissions");

            migrationBuilder.AlterColumn<string>(
                name: "GroupName",
                table: "AbpPermissions",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.AddColumn<string>(
                name: "ManagementPermissionName",
                table: "AbpPermissions",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResourceName",
                table: "AbpPermissions",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AbpResourcePermissionGrants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ProviderName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ProviderKey = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ResourceName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ResourceKey = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpResourcePermissionGrants", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbpPermissions_ResourceName_Name",
                table: "AbpPermissions",
                columns: new[] { "ResourceName", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpResourcePermissionGrants_TenantId_Name_ResourceName_Reso~",
                table: "AbpResourcePermissionGrants",
                columns: new[] { "TenantId", "ResourceName", "ResourceKey", "ProviderName", "ProviderKey" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbpResourcePermissionGrants");

            migrationBuilder.DropIndex(
                name: "IX_AbpPermissions_ResourceName_Name",
                table: "AbpPermissions");

            migrationBuilder.DropColumn(
                name: "ManagementPermissionName",
                table: "AbpPermissions");

            migrationBuilder.DropColumn(
                name: "ResourceName",
                table: "AbpPermissions");

            migrationBuilder.AlterColumn<string>(
                name: "GroupName",
                table: "AbpPermissions",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpPermissions_Name",
                table: "AbpPermissions",
                column: "Name",
                unique: true);
        }
    }
}
