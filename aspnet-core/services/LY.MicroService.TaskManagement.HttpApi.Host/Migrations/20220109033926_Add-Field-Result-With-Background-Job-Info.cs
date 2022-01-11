using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LY.MicroService.TaskManagement.Migrations
{
    public partial class AddFieldResultWithBackgroundJobInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Result",
                table: "TK_BackgroundJobs",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Result",
                table: "TK_BackgroundJobs");
        }
    }
}
