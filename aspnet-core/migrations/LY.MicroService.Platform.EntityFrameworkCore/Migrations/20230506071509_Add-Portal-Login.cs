using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LY.MicroService.Platform.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class AddPortalLogin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppPlatformEnterprises",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EnglishName = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LegalMan = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TaxCode = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OrganizationCode = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RegistrationCode = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RegistrationDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ExtraProperties = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    DeletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPlatformEnterprises", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppPlatformEnterprises");
        }
    }
}
