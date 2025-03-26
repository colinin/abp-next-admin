using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LY.MicroService.Applications.Single.EntityFrameworkCore.MySql.Migrations
{
    /// <inheritdoc />
    public partial class RenameAllowPropertiesToAccessedProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AllowProperties",
                table: "AbpAuthRoleEntityRules",
                newName: "AccessedProperties");

            migrationBuilder.RenameColumn(
                name: "AllowProperties",
                table: "AbpAuthOrganizationUnitEntityRules",
                newName: "AccessedProperties");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccessedProperties",
                table: "AbpAuthRoleEntityRules",
                newName: "AllowProperties");

            migrationBuilder.RenameColumn(
                name: "AccessedProperties",
                table: "AbpAuthOrganizationUnitEntityRules",
                newName: "AllowProperties");
        }
    }
}
