using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LY.MicroService.BackendAdmin.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class FixMySqlIndexKeyTooLangForResourcePermissionGrant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AbpResourcePermissionGrants_TenantId_Name_ResourceName_Resou~",
                table: "AbpResourcePermissionGrants");

            migrationBuilder.AlterColumn<string>(
                name: "ResourceName",
                table: "AbpResourcePermissionGrants",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(256)",
                oldMaxLength: 256)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ResourceKey",
                table: "AbpResourcePermissionGrants",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(256)",
                oldMaxLength: 256)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AbpResourcePermissionGrants_TenantId_Name_ResourceName_Resou~",
                table: "AbpResourcePermissionGrants",
                columns: new[] { "TenantId", "Name", "ResourceName", "ResourceKey", "ProviderName", "ProviderKey" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AbpResourcePermissionGrants_TenantId_Name_ResourceName_Resou~",
                table: "AbpResourcePermissionGrants");

            migrationBuilder.AlterColumn<string>(
                name: "ResourceName",
                table: "AbpResourcePermissionGrants",
                type: "varchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ResourceKey",
                table: "AbpResourcePermissionGrants",
                type: "varchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AbpResourcePermissionGrants_TenantId_Name_ResourceName_Resou~",
                table: "AbpResourcePermissionGrants",
                columns: new[] { "TenantId", "ResourceName", "ResourceKey", "ProviderName", "ProviderKey" },
                unique: true);
        }
    }
}
