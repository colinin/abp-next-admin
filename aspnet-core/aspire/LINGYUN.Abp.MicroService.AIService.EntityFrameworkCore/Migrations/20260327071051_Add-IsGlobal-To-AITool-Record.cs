using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LINGYUN.Abp.MicroService.AIService.Migrations
{
    /// <inheritdoc />
    public partial class AddIsGlobalToAIToolRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsGlobal",
                table: "AbpAIAIToolDefinitionRecords",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGlobal",
                table: "AbpAIAIToolDefinitionRecords");
        }
    }
}
