using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LINGYUN.Abp.MicroService.AllInOne.EntityFrameworkCore.MySql.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldWithTextTemplateDefinition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocalizationResourceName",
                table: "AbpTextTemplateDefinitions",
                type: "varchar(64)",
                maxLength: 64,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocalizationResourceName",
                table: "AbpTextTemplateDefinitions");
        }
    }
}
