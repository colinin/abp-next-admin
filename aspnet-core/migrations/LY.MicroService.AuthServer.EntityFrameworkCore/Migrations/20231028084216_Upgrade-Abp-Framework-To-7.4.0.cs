using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LY.MicroService.AuthServer.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class UpgradeAbpFrameworkTo740 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "OpenIddictTokens",
                keyColumn: "ExtraProperties",
                keyValue: null,
                column: "ExtraProperties",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ExtraProperties",
                table: "OpenIddictTokens",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "OpenIddictTokens",
                keyColumn: "ConcurrencyStamp",
                keyValue: null,
                column: "ConcurrencyStamp",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "OpenIddictTokens",
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
                table: "OpenIddictScopes",
                keyColumn: "ExtraProperties",
                keyValue: null,
                column: "ExtraProperties",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ExtraProperties",
                table: "OpenIddictScopes",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "OpenIddictScopes",
                keyColumn: "ConcurrencyStamp",
                keyValue: null,
                column: "ConcurrencyStamp",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "OpenIddictScopes",
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
                table: "OpenIddictAuthorizations",
                keyColumn: "ExtraProperties",
                keyValue: null,
                column: "ExtraProperties",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ExtraProperties",
                table: "OpenIddictAuthorizations",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "OpenIddictAuthorizations",
                keyColumn: "ConcurrencyStamp",
                keyValue: null,
                column: "ConcurrencyStamp",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "OpenIddictAuthorizations",
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
                table: "OpenIddictApplications",
                keyColumn: "ExtraProperties",
                keyValue: null,
                column: "ExtraProperties",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ExtraProperties",
                table: "OpenIddictApplications",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "OpenIddictApplications",
                keyColumn: "ConcurrencyStamp",
                keyValue: null,
                column: "ConcurrencyStamp",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "OpenIddictApplications",
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
                table: "AbpUsers",
                keyColumn: "ExtraProperties",
                keyValue: null,
                column: "ExtraProperties",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ExtraProperties",
                table: "AbpUsers",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AbpUsers",
                keyColumn: "ConcurrencyStamp",
                keyValue: null,
                column: "ConcurrencyStamp",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "AbpUsers",
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
                table: "AbpSecurityLogs",
                keyColumn: "ExtraProperties",
                keyValue: null,
                column: "ExtraProperties",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ExtraProperties",
                table: "AbpSecurityLogs",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AbpSecurityLogs",
                keyColumn: "ConcurrencyStamp",
                keyValue: null,
                column: "ConcurrencyStamp",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "AbpSecurityLogs",
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
                table: "AbpRoles",
                keyColumn: "ExtraProperties",
                keyValue: null,
                column: "ExtraProperties",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ExtraProperties",
                table: "AbpRoles",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AbpRoles",
                keyColumn: "ConcurrencyStamp",
                keyValue: null,
                column: "ConcurrencyStamp",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "AbpRoles",
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
                table: "AbpOrganizationUnits",
                keyColumn: "ExtraProperties",
                keyValue: null,
                column: "ExtraProperties",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ExtraProperties",
                table: "AbpOrganizationUnits",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AbpOrganizationUnits",
                keyColumn: "ConcurrencyStamp",
                keyValue: null,
                column: "ConcurrencyStamp",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "AbpOrganizationUnits",
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
                table: "AbpClaimTypes",
                keyColumn: "ExtraProperties",
                keyValue: null,
                column: "ExtraProperties",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ExtraProperties",
                table: "AbpClaimTypes",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AbpClaimTypes",
                keyColumn: "ConcurrencyStamp",
                keyValue: null,
                column: "ConcurrencyStamp",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "AbpClaimTypes",
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
                table: "OpenIddictTokens",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "OpenIddictTokens",
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
                table: "OpenIddictScopes",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "OpenIddictScopes",
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
                table: "OpenIddictAuthorizations",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "OpenIddictAuthorizations",
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
                table: "OpenIddictApplications",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "OpenIddictApplications",
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
                table: "AbpUsers",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "AbpUsers",
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
                table: "AbpSecurityLogs",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "AbpSecurityLogs",
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
                table: "AbpRoles",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "AbpRoles",
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
                table: "AbpOrganizationUnits",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "AbpOrganizationUnits",
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
                table: "AbpClaimTypes",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "AbpClaimTypes",
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
