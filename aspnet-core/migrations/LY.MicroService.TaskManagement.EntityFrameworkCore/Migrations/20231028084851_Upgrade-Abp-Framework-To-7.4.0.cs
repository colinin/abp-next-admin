using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LY.MicroService.TaskManagement.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class UpgradeAbpFrameworkTo740 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TK_BackgroundJobs",
                keyColumn: "ExtraProperties",
                keyValue: null,
                column: "ExtraProperties",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ExtraProperties",
                table: "TK_BackgroundJobs",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "TK_BackgroundJobs",
                keyColumn: "ConcurrencyStamp",
                keyValue: null,
                column: "ConcurrencyStamp",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "TK_BackgroundJobs",
                type: "varchar(40)",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(40)",
                oldMaxLength: 40,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "TK_BackgroundJobActions",
                keyColumn: "ExtraProperties",
                keyValue: null,
                column: "ExtraProperties",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ExtraProperties",
                table: "TK_BackgroundJobActions",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "TK_BackgroundJobActions",
                keyColumn: "ConcurrencyStamp",
                keyValue: null,
                column: "ConcurrencyStamp",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "TK_BackgroundJobActions",
                type: "varchar(40)",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(40)",
                oldMaxLength: 40,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ExtraProperties",
                table: "TK_BackgroundJobs",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "TK_BackgroundJobs",
                type: "varchar(40)",
                maxLength: 40,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(40)",
                oldMaxLength: 40)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ExtraProperties",
                table: "TK_BackgroundJobActions",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "TK_BackgroundJobActions",
                type: "varchar(40)",
                maxLength: 40,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(40)",
                oldMaxLength: 40)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
