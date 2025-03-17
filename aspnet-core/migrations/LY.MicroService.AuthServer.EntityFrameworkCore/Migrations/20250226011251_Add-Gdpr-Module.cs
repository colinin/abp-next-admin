using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LY.MicroService.AuthServer.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class AddGdprModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbpGdprRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ReadyTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ExtraProperties = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpGdprRequests", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AbpGdprInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RequestId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Data = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Provider = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpGdprInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpGdprInfos_AbpGdprRequests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "AbpGdprRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AbpGdprInfos_RequestId",
                table: "AbpGdprInfos",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpGdprRequests_UserId",
                table: "AbpGdprRequests",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbpGdprInfos");

            migrationBuilder.DropTable(
                name: "AbpGdprRequests");
        }
    }
}
