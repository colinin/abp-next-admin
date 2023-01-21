using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LY.MicroService.LocalizationManagement.DbMigrator.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultCultureNameWithResourceRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DefaultCultureName",
                table: "AbpLocalizationResources",
                type: "varchar(64)",
                maxLength: 64,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultCultureName",
                table: "AbpLocalizationResources");
        }
    }
}
