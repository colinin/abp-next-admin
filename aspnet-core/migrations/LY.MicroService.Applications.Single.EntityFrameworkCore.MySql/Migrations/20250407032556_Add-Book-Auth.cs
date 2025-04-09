using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LY.MicroService.Applications.Single.EntityFrameworkCore.MySql.Migrations
{
    /// <inheritdoc />
    public partial class AddBookAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ExtraProperties",
                table: "Demo_Books",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "json")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Demo_BooksAuths",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    EntityId = table.Column<Guid>(type: "char(64)", maxLength: 64, nullable: false, collation: "ascii_general_ci"),
                    EntityType = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Role = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OrganizationUnit = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Demo_BooksAuths", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Demo_BooksAuths_Demo_Books_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Demo_Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Demo_BooksAuths_EntityId",
                table: "Demo_BooksAuths",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Demo_BooksAuths_OrganizationUnit",
                table: "Demo_BooksAuths",
                column: "OrganizationUnit");

            migrationBuilder.CreateIndex(
                name: "IX_Demo_BooksAuths_Role",
                table: "Demo_BooksAuths",
                column: "Role");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Demo_BooksAuths");

            migrationBuilder.AlterColumn<string>(
                name: "ExtraProperties",
                table: "Demo_Books",
                type: "json",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
