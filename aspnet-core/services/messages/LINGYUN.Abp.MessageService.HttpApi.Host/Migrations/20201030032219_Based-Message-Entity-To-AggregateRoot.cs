using Microsoft.EntityFrameworkCore.Migrations;

namespace LINGYUN.Abp.MessageService.Migrations
{
    public partial class BasedMessageEntityToAggregateRoot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "AppUserMessages",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "AppUserMessages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "AppGroupMessages",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "AppGroupMessages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "AppUserMessages");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "AppUserMessages");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "AppGroupMessages");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "AppGroupMessages");
        }
    }
}
