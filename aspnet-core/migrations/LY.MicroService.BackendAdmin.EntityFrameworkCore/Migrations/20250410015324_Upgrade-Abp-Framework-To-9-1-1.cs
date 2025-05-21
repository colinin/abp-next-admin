using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LY.MicroService.BackendAdmin.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class UpgradeAbpFrameworkTo911 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValueRange",
                table: "AbpAuthEntityProperties");

            migrationBuilder.RenameColumn(
                name: "AllowProperties",
                table: "AbpAuthRoleEntityRules",
                newName: "AccessedProperties");

            migrationBuilder.RenameColumn(
                name: "AllowProperties",
                table: "AbpAuthOrganizationUnitEntityRules",
                newName: "AccessedProperties");

            migrationBuilder.AddColumn<string>(
                name: "JavaScriptType",
                table: "AbpAuthEntityProperties",
                type: "varchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AbpAuthEntityEnums",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayName = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PropertyInfoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpAuthEntityEnums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpAuthEntityEnums_AbpAuthEntityProperties_PropertyInfoId",
                        column: x => x.PropertyInfoId,
                        principalTable: "AbpAuthEntityProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AbpAuthSubjectStrategys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IsEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    SubjectName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SubjectId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Strategy = table.Column<int>(type: "int", nullable: false),
                    ExtraProperties = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpAuthSubjectStrategys", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AbpAuthEntityEnums_PropertyInfoId_Name",
                table: "AbpAuthEntityEnums",
                columns: new[] { "PropertyInfoId", "Name" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbpAuthEntityEnums");

            migrationBuilder.DropTable(
                name: "AbpAuthSubjectStrategys");

            migrationBuilder.DropColumn(
                name: "JavaScriptType",
                table: "AbpAuthEntityProperties");

            migrationBuilder.RenameColumn(
                name: "AccessedProperties",
                table: "AbpAuthRoleEntityRules",
                newName: "AllowProperties");

            migrationBuilder.RenameColumn(
                name: "AccessedProperties",
                table: "AbpAuthOrganizationUnitEntityRules",
                newName: "AllowProperties");

            migrationBuilder.AddColumn<string>(
                name: "ValueRange",
                table: "AbpAuthEntityProperties",
                type: "varchar(512)",
                maxLength: 512,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
