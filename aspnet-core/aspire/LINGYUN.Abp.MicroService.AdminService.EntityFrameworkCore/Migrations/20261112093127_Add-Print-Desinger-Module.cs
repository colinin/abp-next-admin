using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LINGYUN.Abp.MicroService.AdminService.Migrations
{
    /// <inheritdoc />
    public partial class AddPrintDesingerModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbpPrintDesignerCustomElements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpPrintDesignerCustomElements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpPrintDesignerTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpPrintDesignerTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbpPrintDesignerCustomElementDatas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    ElementId = table.Column<Guid>(type: "uuid", nullable: false),
                    ElementData = table.Column<string>(type: "text", nullable: true),
                    ExtendedData = table.Column<string>(type: "text", nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpPrintDesignerCustomElementDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpPrintDesignerCustomElementDatas_AbpPrintDesignerCustomEl~",
                        column: x => x.ElementId,
                        principalTable: "AbpPrintDesignerCustomElements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpPrintDesignerTemplateDatas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    TemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    DesignerData = table.Column<string>(type: "text", nullable: true),
                    ExtendedData = table.Column<string>(type: "text", nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpPrintDesignerTemplateDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpPrintDesignerTemplateDatas_AbpPrintDesignerTemplates_Tem~",
                        column: x => x.TemplateId,
                        principalTable: "AbpPrintDesignerTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbpPrintDesignerCustomElementDatas_ElementId",
                table: "AbpPrintDesignerCustomElementDatas",
                column: "ElementId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpPrintDesignerCustomElements_Name",
                table: "AbpPrintDesignerCustomElements",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpPrintDesignerTemplateDatas_TemplateId",
                table: "AbpPrintDesignerTemplateDatas",
                column: "TemplateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpPrintDesignerTemplates_Name",
                table: "AbpPrintDesignerTemplates",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbpPrintDesignerCustomElementDatas");

            migrationBuilder.DropTable(
                name: "AbpPrintDesignerTemplateDatas");

            migrationBuilder.DropTable(
                name: "AbpPrintDesignerCustomElements");

            migrationBuilder.DropTable(
                name: "AbpPrintDesignerTemplates");
        }
    }
}
