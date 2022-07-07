using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LY.MicroService.TaskManagement.Migrations
{
    public partial class AddFieldIsEnabledWithBackgroundJobAction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "TK_BackgroundJobActions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "TK_BackgroundJobActions");
        }
    }
}
