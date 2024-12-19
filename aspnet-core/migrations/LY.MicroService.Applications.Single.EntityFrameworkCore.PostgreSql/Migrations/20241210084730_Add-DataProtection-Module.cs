using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LY.MicroService.Applications.Single.EntityFrameworkCore.PostgreSql.Migrations
{
    /// <inheritdoc />
    public partial class AddDataProtectionModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbpAuthEntitites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    TypeFullName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    IsAuditEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpAuthEntitites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpAuthEntityProperties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    TypeFullName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ValueRange = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    TypeInfoId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpAuthEntityProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpAuthEntityProperties_AbpAuthEntitites_TypeInfoId",
                        column: x => x.TypeInfoId,
                        principalTable: "AbpAuthEntitites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpAuthOrganizationUnitEntityRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrgId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrgCode = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    Operation = table.Column<int>(type: "integer", nullable: false),
                    FilterGroup = table.Column<string>(type: "text", nullable: true),
                    EntityTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    EntityTypeFullName = table.Column<string>(type: "text", nullable: true),
                    AllowProperties = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpAuthOrganizationUnitEntityRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpAuthOrganizationUnitEntityRules_AbpAuthEntitites_EntityT~",
                        column: x => x.EntityTypeId,
                        principalTable: "AbpAuthEntitites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpAuthRoleEntityRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    Operation = table.Column<int>(type: "integer", nullable: false),
                    FilterGroup = table.Column<string>(type: "text", nullable: true),
                    EntityTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    EntityTypeFullName = table.Column<string>(type: "text", nullable: true),
                    AllowProperties = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpAuthRoleEntityRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpAuthRoleEntityRules_AbpAuthEntitites_EntityTypeId",
                        column: x => x.EntityTypeId,
                        principalTable: "AbpAuthEntitites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbpAuthEntitites_TypeFullName",
                table: "AbpAuthEntitites",
                column: "TypeFullName");

            migrationBuilder.CreateIndex(
                name: "IX_AbpAuthEntityProperties_TypeInfoId_TypeFullName",
                table: "AbpAuthEntityProperties",
                columns: new[] { "TypeInfoId", "TypeFullName" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpAuthOrganizationUnitEntityRules_EntityTypeId",
                table: "AbpAuthOrganizationUnitEntityRules",
                column: "EntityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpAuthRoleEntityRules_EntityTypeId",
                table: "AbpAuthRoleEntityRules",
                column: "EntityTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbpAuthEntityProperties");

            migrationBuilder.DropTable(
                name: "AbpAuthOrganizationUnitEntityRules");

            migrationBuilder.DropTable(
                name: "AbpAuthRoleEntityRules");

            migrationBuilder.DropTable(
                name: "AbpAuthEntitites");
        }
    }
}
