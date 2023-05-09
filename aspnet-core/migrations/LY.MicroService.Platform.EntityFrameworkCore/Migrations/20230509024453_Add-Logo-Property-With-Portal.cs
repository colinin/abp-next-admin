using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LY.MicroService.Platform.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class AddLogoPropertyWithPortal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "AppPlatformEnterprises",
                type: "varchar(512)",
                maxLength: 512,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logo",
                table: "AppPlatformEnterprises");
        }
    }
}
