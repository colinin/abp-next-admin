using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthServer.Host.Migrations
{
    public partial class UpgradeAbp400 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IdentityServerApiClaims_IdentityServerApiResources_ApiResour~",
                table: "IdentityServerApiClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityServerApiScopeClaims_IdentityServerApiScopes_ApiReso~",
                table: "IdentityServerApiScopeClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityServerApiScopes_IdentityServerApiResources_ApiResour~",
                table: "IdentityServerApiScopes");

            migrationBuilder.DropTable(
                name: "IdentityServerApiSecrets");

            migrationBuilder.DropTable(
                name: "IdentityServerIdentityClaims");

            migrationBuilder.DropIndex(
                name: "IX_IdentityServerDeviceFlowCodes_UserCode",
                table: "IdentityServerDeviceFlowCodes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityServerClientProperties",
                table: "IdentityServerClientProperties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityServerApiScopes",
                table: "IdentityServerApiScopes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityServerApiScopeClaims",
                table: "IdentityServerApiScopeClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityServerApiClaims",
                table: "IdentityServerApiClaims");

            migrationBuilder.DropColumn(
                name: "Properties",
                table: "IdentityServerIdentityResources");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "IdentityServerApiScopeClaims");

            migrationBuilder.DropColumn(
                name: "Properties",
                table: "IdentityServerApiResources");

            migrationBuilder.RenameTable(
                name: "IdentityServerApiClaims",
                newName: "IdentityServerApiResourceClaims");

            migrationBuilder.RenameColumn(
                name: "ApiResourceId",
                table: "IdentityServerApiScopes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ApiResourceId",
                table: "IdentityServerApiScopeClaims",
                newName: "ApiScopeId");

            migrationBuilder.AddColumn<DateTime>(
                name: "ConsumedTime",
                table: "IdentityServerPersistedGrants",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "IdentityServerPersistedGrants",
                type: "varchar(200) CHARACTER SET utf8mb4",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "IdentityServerPersistedGrants",
                type: "varchar(100) CHARACTER SET utf8mb4",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "IdentityServerDeviceFlowCodes",
                type: "varchar(200) CHARACTER SET utf8mb4",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "IdentityServerDeviceFlowCodes",
                type: "varchar(100) CHARACTER SET utf8mb4",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AllowedIdentityTokenSigningAlgorithms",
                table: "IdentityServerClients",
                type: "varchar(100) CHARACTER SET utf8mb4",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequireRequestObject",
                table: "IdentityServerClients",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "IdentityServerClientProperties",
                type: "varchar(300) CHARACTER SET utf8mb4",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(2000) CHARACTER SET utf8mb4",
                oldMaxLength: 2000);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "IdentityServerApiScopes",
                type: "varchar(40) CHARACTER SET utf8mb4",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "IdentityServerApiScopes",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "IdentityServerApiScopes",
                type: "char(36)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "IdentityServerApiScopes",
                type: "char(36)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "IdentityServerApiScopes",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                table: "IdentityServerApiScopes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "IdentityServerApiScopes",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "IdentityServerApiScopes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "IdentityServerApiScopes",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                table: "IdentityServerApiScopes",
                type: "char(36)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AllowedAccessTokenSigningAlgorithms",
                table: "IdentityServerApiResources",
                type: "varchar(100) CHARACTER SET utf8mb4",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowInDiscoveryDocument",
                table: "IdentityServerApiResources",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityServerClientProperties",
                table: "IdentityServerClientProperties",
                columns: new[] { "ClientId", "Key", "Value" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityServerApiScopes",
                table: "IdentityServerApiScopes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityServerApiScopeClaims",
                table: "IdentityServerApiScopeClaims",
                columns: new[] { "ApiScopeId", "Type" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityServerApiResourceClaims",
                table: "IdentityServerApiResourceClaims",
                columns: new[] { "ApiResourceId", "Type" });

            migrationBuilder.CreateTable(
                name: "AbpLinkUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    SourceUserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    SourceTenantId = table.Column<Guid>(type: "char(36)", nullable: true),
                    TargetUserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    TargetTenantId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpLinkUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerApiResourceProperties",
                columns: table => new
                {
                    ApiResourceId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Key = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: false),
                    Value = table.Column<string>(type: "varchar(300) CHARACTER SET utf8mb4", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerApiResourceProperties", x => new { x.ApiResourceId, x.Key, x.Value });
                    table.ForeignKey(
                        name: "FK_IdentityServerApiResourceProperties_IdentityServerApiResourc~",
                        column: x => x.ApiResourceId,
                        principalTable: "IdentityServerApiResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerApiResourceScopes",
                columns: table => new
                {
                    ApiResourceId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Scope = table.Column<string>(type: "varchar(200) CHARACTER SET utf8mb4", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerApiResourceScopes", x => new { x.ApiResourceId, x.Scope });
                    table.ForeignKey(
                        name: "FK_IdentityServerApiResourceScopes_IdentityServerApiResources_A~",
                        column: x => x.ApiResourceId,
                        principalTable: "IdentityServerApiResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerApiResourceSecrets",
                columns: table => new
                {
                    Type = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: false),
                    Value = table.Column<string>(type: "varchar(300) CHARACTER SET utf8mb4", maxLength: 300, nullable: false),
                    ApiResourceId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Description = table.Column<string>(type: "varchar(1000) CHARACTER SET utf8mb4", maxLength: 1000, nullable: true),
                    Expiration = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerApiResourceSecrets", x => new { x.ApiResourceId, x.Type, x.Value });
                    table.ForeignKey(
                        name: "FK_IdentityServerApiResourceSecrets_IdentityServerApiResources_~",
                        column: x => x.ApiResourceId,
                        principalTable: "IdentityServerApiResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerApiScopeProperties",
                columns: table => new
                {
                    ApiScopeId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Key = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: false),
                    Value = table.Column<string>(type: "varchar(300) CHARACTER SET utf8mb4", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerApiScopeProperties", x => new { x.ApiScopeId, x.Key, x.Value });
                    table.ForeignKey(
                        name: "FK_IdentityServerApiScopeProperties_IdentityServerApiScopes_Api~",
                        column: x => x.ApiScopeId,
                        principalTable: "IdentityServerApiScopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerIdentityResourceClaims",
                columns: table => new
                {
                    Type = table.Column<string>(type: "varchar(200) CHARACTER SET utf8mb4", maxLength: 200, nullable: false),
                    IdentityResourceId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerIdentityResourceClaims", x => new { x.IdentityResourceId, x.Type });
                    table.ForeignKey(
                        name: "FK_IdentityServerIdentityResourceClaims_IdentityServerIdentityR~",
                        column: x => x.IdentityResourceId,
                        principalTable: "IdentityServerIdentityResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerIdentityResourceProperties",
                columns: table => new
                {
                    IdentityResourceId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Key = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: false),
                    Value = table.Column<string>(type: "varchar(300) CHARACTER SET utf8mb4", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerIdentityResourceProperties", x => new { x.IdentityResourceId, x.Key, x.Value });
                    table.ForeignKey(
                        name: "FK_IdentityServerIdentityResourceProperties_IdentityServerIdent~",
                        column: x => x.IdentityResourceId,
                        principalTable: "IdentityServerIdentityResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IdentityServerPersistedGrants_SubjectId_SessionId_Type",
                table: "IdentityServerPersistedGrants",
                columns: new[] { "SubjectId", "SessionId", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_IdentityServerDeviceFlowCodes_UserCode",
                table: "IdentityServerDeviceFlowCodes",
                column: "UserCode");

            migrationBuilder.CreateIndex(
                name: "IX_AbpLinkUsers_SourceUserId_SourceTenantId_TargetUserId_Target~",
                table: "AbpLinkUsers",
                columns: new[] { "SourceUserId", "SourceTenantId", "TargetUserId", "TargetTenantId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityServerApiResourceClaims_IdentityServerApiResources_A~",
                table: "IdentityServerApiResourceClaims",
                column: "ApiResourceId",
                principalTable: "IdentityServerApiResources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityServerApiScopeClaims_IdentityServerApiScopes_ApiScop~",
                table: "IdentityServerApiScopeClaims",
                column: "ApiScopeId",
                principalTable: "IdentityServerApiScopes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IdentityServerApiResourceClaims_IdentityServerApiResources_A~",
                table: "IdentityServerApiResourceClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_IdentityServerApiScopeClaims_IdentityServerApiScopes_ApiScop~",
                table: "IdentityServerApiScopeClaims");

            migrationBuilder.DropTable(
                name: "AbpLinkUsers");

            migrationBuilder.DropTable(
                name: "IdentityServerApiResourceProperties");

            migrationBuilder.DropTable(
                name: "IdentityServerApiResourceScopes");

            migrationBuilder.DropTable(
                name: "IdentityServerApiResourceSecrets");

            migrationBuilder.DropTable(
                name: "IdentityServerApiScopeProperties");

            migrationBuilder.DropTable(
                name: "IdentityServerIdentityResourceClaims");

            migrationBuilder.DropTable(
                name: "IdentityServerIdentityResourceProperties");

            migrationBuilder.DropIndex(
                name: "IX_IdentityServerPersistedGrants_SubjectId_SessionId_Type",
                table: "IdentityServerPersistedGrants");

            migrationBuilder.DropIndex(
                name: "IX_IdentityServerDeviceFlowCodes_UserCode",
                table: "IdentityServerDeviceFlowCodes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityServerClientProperties",
                table: "IdentityServerClientProperties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityServerApiScopes",
                table: "IdentityServerApiScopes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityServerApiScopeClaims",
                table: "IdentityServerApiScopeClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityServerApiResourceClaims",
                table: "IdentityServerApiResourceClaims");

            migrationBuilder.DropColumn(
                name: "ConsumedTime",
                table: "IdentityServerPersistedGrants");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "IdentityServerPersistedGrants");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "IdentityServerPersistedGrants");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "IdentityServerDeviceFlowCodes");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "IdentityServerDeviceFlowCodes");

            migrationBuilder.DropColumn(
                name: "AllowedIdentityTokenSigningAlgorithms",
                table: "IdentityServerClients");

            migrationBuilder.DropColumn(
                name: "RequireRequestObject",
                table: "IdentityServerClients");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "IdentityServerApiScopes");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "IdentityServerApiScopes");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "IdentityServerApiScopes");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "IdentityServerApiScopes");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "IdentityServerApiScopes");

            migrationBuilder.DropColumn(
                name: "Enabled",
                table: "IdentityServerApiScopes");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "IdentityServerApiScopes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "IdentityServerApiScopes");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "IdentityServerApiScopes");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "IdentityServerApiScopes");

            migrationBuilder.DropColumn(
                name: "AllowedAccessTokenSigningAlgorithms",
                table: "IdentityServerApiResources");

            migrationBuilder.DropColumn(
                name: "ShowInDiscoveryDocument",
                table: "IdentityServerApiResources");

            migrationBuilder.RenameTable(
                name: "IdentityServerApiResourceClaims",
                newName: "IdentityServerApiClaims");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "IdentityServerApiScopes",
                newName: "ApiResourceId");

            migrationBuilder.RenameColumn(
                name: "ApiScopeId",
                table: "IdentityServerApiScopeClaims",
                newName: "ApiResourceId");

            migrationBuilder.AddColumn<string>(
                name: "Properties",
                table: "IdentityServerIdentityResources",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "IdentityServerClientProperties",
                type: "varchar(2000) CHARACTER SET utf8mb4",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(300) CHARACTER SET utf8mb4",
                oldMaxLength: 300);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "IdentityServerApiScopeClaims",
                type: "varchar(200) CHARACTER SET utf8mb4",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Properties",
                table: "IdentityServerApiResources",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityServerClientProperties",
                table: "IdentityServerClientProperties",
                columns: new[] { "ClientId", "Key" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityServerApiScopes",
                table: "IdentityServerApiScopes",
                columns: new[] { "ApiResourceId", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityServerApiScopeClaims",
                table: "IdentityServerApiScopeClaims",
                columns: new[] { "ApiResourceId", "Name", "Type" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityServerApiClaims",
                table: "IdentityServerApiClaims",
                columns: new[] { "ApiResourceId", "Type" });

            migrationBuilder.CreateTable(
                name: "IdentityServerApiSecrets",
                columns: table => new
                {
                    ApiResourceId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Type = table.Column<string>(type: "varchar(250) CHARACTER SET utf8mb4", maxLength: 250, nullable: false),
                    Value = table.Column<string>(type: "varchar(300) CHARACTER SET utf8mb4", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "varchar(2000) CHARACTER SET utf8mb4", maxLength: 2000, nullable: true),
                    Expiration = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerApiSecrets", x => new { x.ApiResourceId, x.Type, x.Value });
                    table.ForeignKey(
                        name: "FK_IdentityServerApiSecrets_IdentityServerApiResources_ApiResou~",
                        column: x => x.ApiResourceId,
                        principalTable: "IdentityServerApiResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityServerIdentityClaims",
                columns: table => new
                {
                    IdentityResourceId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Type = table.Column<string>(type: "varchar(200) CHARACTER SET utf8mb4", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityServerIdentityClaims", x => new { x.IdentityResourceId, x.Type });
                    table.ForeignKey(
                        name: "FK_IdentityServerIdentityClaims_IdentityServerIdentityResources~",
                        column: x => x.IdentityResourceId,
                        principalTable: "IdentityServerIdentityResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IdentityServerDeviceFlowCodes_UserCode",
                table: "IdentityServerDeviceFlowCodes",
                column: "UserCode",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityServerApiClaims_IdentityServerApiResources_ApiResour~",
                table: "IdentityServerApiClaims",
                column: "ApiResourceId",
                principalTable: "IdentityServerApiResources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityServerApiScopeClaims_IdentityServerApiScopes_ApiReso~",
                table: "IdentityServerApiScopeClaims",
                columns: new[] { "ApiResourceId", "Name" },
                principalTable: "IdentityServerApiScopes",
                principalColumns: new[] { "ApiResourceId", "Name" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityServerApiScopes_IdentityServerApiResources_ApiResour~",
                table: "IdentityServerApiScopes",
                column: "ApiResourceId",
                principalTable: "IdentityServerApiResources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
