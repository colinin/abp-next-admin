using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LINGYUN.ApiGateway.HttpApi.Host.Migrations
{
    public partial class MigrationApiGatewayMySql : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppApiGatewayAggregate",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ExtraProperties = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    AppId = table.Column<string>(maxLength: 50, nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ReRouteId = table.Column<long>(nullable: false),
                    ReRouteKeys = table.Column<string>(maxLength: 1000, nullable: true),
                    UpstreamPathTemplate = table.Column<string>(maxLength: 1000, nullable: true),
                    UpstreamHost = table.Column<string>(maxLength: 1000, nullable: true),
                    ReRouteIsCaseSensitive = table.Column<bool>(nullable: false, defaultValue: false),
                    Aggregator = table.Column<string>(maxLength: 256, nullable: true),
                    Priority = table.Column<int>(nullable: true),
                    UpstreamHttpMethod = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppApiGatewayAggregate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppApiGatewayDynamicReRoute",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ExtraProperties = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    DynamicReRouteId = table.Column<long>(nullable: false),
                    ServiceName = table.Column<string>(maxLength: 100, nullable: false),
                    DownstreamHttpVersion = table.Column<string>(maxLength: 30, nullable: true),
                    AppId = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppApiGatewayDynamicReRoute", x => x.Id);
                    table.UniqueConstraint("AK_AppApiGatewayDynamicReRoute_DynamicReRouteId", x => x.DynamicReRouteId);
                });

            migrationBuilder.CreateTable(
                name: "AppApiGatewayGlobalConfiguration",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ExtraProperties = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    ItemId = table.Column<long>(nullable: false),
                    RequestIdKey = table.Column<string>(maxLength: 100, nullable: true),
                    BaseUrl = table.Column<string>(maxLength: 256, nullable: false),
                    DownstreamScheme = table.Column<string>(maxLength: 100, nullable: true),
                    DownstreamHttpVersion = table.Column<string>(maxLength: 30, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(nullable: false),
                    AppId = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppApiGatewayGlobalConfiguration", x => x.Id);
                    table.UniqueConstraint("AK_AppApiGatewayGlobalConfiguration_ItemId", x => x.ItemId);
                });

            migrationBuilder.CreateTable(
                name: "AppApiGatewayHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ReRouteId = table.Column<long>(nullable: false),
                    Key = table.Column<string>(maxLength: 50, nullable: true),
                    Value = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppApiGatewayHeaders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppApiGatewayHostAndPort",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ReRouteId = table.Column<long>(nullable: false),
                    Host = table.Column<string>(maxLength: 50, nullable: false),
                    Port = table.Column<int>(nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppApiGatewayHostAndPort", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppApiGatewayReRoute",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ExtraProperties = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    ReRouteId = table.Column<long>(nullable: false),
                    ReRouteName = table.Column<string>(maxLength: 50, nullable: false),
                    DownstreamPathTemplate = table.Column<string>(maxLength: 100, nullable: false),
                    ChangeDownstreamPathTemplate = table.Column<string>(maxLength: 1000, nullable: true),
                    DownstreamHttpMethod = table.Column<string>(maxLength: 100, nullable: true),
                    UpstreamPathTemplate = table.Column<string>(maxLength: 100, nullable: false),
                    UpstreamHttpMethod = table.Column<string>(maxLength: 50, nullable: false),
                    AddHeadersToRequest = table.Column<string>(maxLength: 1000, nullable: true),
                    UpstreamHeaderTransform = table.Column<string>(maxLength: 1000, nullable: true),
                    DownstreamHeaderTransform = table.Column<string>(maxLength: 1000, nullable: true),
                    AddClaimsToRequest = table.Column<string>(maxLength: 1000, nullable: true),
                    RouteClaimsRequirement = table.Column<string>(maxLength: 1000, nullable: true),
                    AddQueriesToRequest = table.Column<string>(maxLength: 1000, nullable: true),
                    RequestIdKey = table.Column<string>(maxLength: 100, nullable: true),
                    ReRouteIsCaseSensitive = table.Column<bool>(nullable: false),
                    ServiceName = table.Column<string>(maxLength: 100, nullable: true),
                    ServiceNamespace = table.Column<string>(maxLength: 100, nullable: true),
                    DownstreamScheme = table.Column<string>(maxLength: 100, nullable: true),
                    DownstreamHostAndPorts = table.Column<string>(maxLength: 1000, nullable: true),
                    DelegatingHandlers = table.Column<string>(maxLength: 1000, nullable: true),
                    UpstreamHost = table.Column<string>(maxLength: 100, nullable: true),
                    Key = table.Column<string>(maxLength: 100, nullable: true),
                    Priority = table.Column<int>(nullable: true),
                    Timeout = table.Column<int>(nullable: true),
                    DangerousAcceptAnyServerCertificateValidator = table.Column<bool>(nullable: false),
                    DownstreamHttpVersion = table.Column<string>(maxLength: 30, nullable: true),
                    AppId = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppApiGatewayReRoute", x => x.Id);
                    table.UniqueConstraint("AK_AppApiGatewayReRoute_ReRouteId", x => x.ReRouteId);
                });

            migrationBuilder.CreateTable(
                name: "AppApiGatewayRouter",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ExtraProperties = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    AppId = table.Column<string>(maxLength: 50, nullable: false),
                    AppName = table.Column<string>(maxLength: 100, nullable: false),
                    AppIpAddress = table.Column<string>(maxLength: 256, nullable: false),
                    Description = table.Column<string>(maxLength: 256, nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppApiGatewayRouter", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppApiGatewayAggregateConfig",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ReRouteId = table.Column<long>(nullable: false),
                    ReRouteKey = table.Column<string>(maxLength: 256, nullable: true),
                    Parameter = table.Column<string>(maxLength: 1000, nullable: true),
                    JsonPath = table.Column<string>(maxLength: 256, nullable: true),
                    AggregateReRouteId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppApiGatewayAggregateConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppApiGatewayAggregateConfig_AppApiGatewayAggregate_Aggregat~",
                        column: x => x.AggregateReRouteId,
                        principalTable: "AppApiGatewayAggregate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppApiGatewayDiscovery",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ItemId = table.Column<long>(nullable: false),
                    Host = table.Column<string>(maxLength: 50, nullable: true),
                    Port = table.Column<int>(nullable: true),
                    Type = table.Column<string>(maxLength: 128, nullable: true),
                    Token = table.Column<string>(maxLength: 256, nullable: true),
                    ConfigurationKey = table.Column<string>(maxLength: 256, nullable: true),
                    PollingInterval = table.Column<int>(nullable: true),
                    Namespace = table.Column<string>(maxLength: 128, nullable: true),
                    Scheme = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppApiGatewayDiscovery", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppApiGatewayDiscovery_AppApiGatewayGlobalConfiguration_Item~",
                        column: x => x.ItemId,
                        principalTable: "AppApiGatewayGlobalConfiguration",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppApiGatewayRateLimitOptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ItemId = table.Column<long>(nullable: false),
                    ClientIdHeader = table.Column<string>(maxLength: 50, nullable: true, defaultValue: "ClientId"),
                    QuotaExceededMessage = table.Column<string>(maxLength: 256, nullable: true),
                    RateLimitCounterPrefix = table.Column<string>(maxLength: 50, nullable: true, defaultValue: "ocelot"),
                    DisableRateLimitHeaders = table.Column<bool>(nullable: false),
                    HttpStatusCode = table.Column<int>(nullable: true, defaultValue: 429)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppApiGatewayRateLimitOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppApiGatewayRateLimitOptions_AppApiGatewayGlobalConfigurati~",
                        column: x => x.ItemId,
                        principalTable: "AppApiGatewayGlobalConfiguration",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppApiGatewayAuthOptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ReRouteId = table.Column<long>(nullable: false),
                    AuthenticationProviderKey = table.Column<string>(maxLength: 100, nullable: true),
                    AllowedScopes = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppApiGatewayAuthOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppApiGatewayAuthOptions_AppApiGatewayReRoute_ReRouteId",
                        column: x => x.ReRouteId,
                        principalTable: "AppApiGatewayReRoute",
                        principalColumn: "ReRouteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppApiGatewayBalancerOptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ItemId = table.Column<long>(nullable: true),
                    ReRouteId = table.Column<long>(nullable: true),
                    Type = table.Column<string>(maxLength: 100, nullable: true),
                    Key = table.Column<string>(maxLength: 100, nullable: true),
                    Expiry = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppApiGatewayBalancerOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppApiGatewayBalancerOptions_AppApiGatewayGlobalConfiguratio~",
                        column: x => x.ItemId,
                        principalTable: "AppApiGatewayGlobalConfiguration",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppApiGatewayBalancerOptions_AppApiGatewayReRoute_ReRouteId",
                        column: x => x.ReRouteId,
                        principalTable: "AppApiGatewayReRoute",
                        principalColumn: "ReRouteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppApiGatewayCacheOptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ReRouteId = table.Column<long>(nullable: false),
                    TtlSeconds = table.Column<int>(nullable: true),
                    Region = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppApiGatewayCacheOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppApiGatewayCacheOptions_AppApiGatewayReRoute_ReRouteId",
                        column: x => x.ReRouteId,
                        principalTable: "AppApiGatewayReRoute",
                        principalColumn: "ReRouteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppApiGatewayHttpOptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ItemId = table.Column<long>(nullable: true),
                    ReRouteId = table.Column<long>(nullable: true),
                    MaxConnectionsPerServer = table.Column<int>(nullable: true),
                    AllowAutoRedirect = table.Column<bool>(nullable: false),
                    UseCookieContainer = table.Column<bool>(nullable: false),
                    UseTracing = table.Column<bool>(nullable: false),
                    UseProxy = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppApiGatewayHttpOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppApiGatewayHttpOptions_AppApiGatewayGlobalConfiguration_It~",
                        column: x => x.ItemId,
                        principalTable: "AppApiGatewayGlobalConfiguration",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppApiGatewayHttpOptions_AppApiGatewayReRoute_ReRouteId",
                        column: x => x.ReRouteId,
                        principalTable: "AppApiGatewayReRoute",
                        principalColumn: "ReRouteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppApiGatewayQoSOptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ItemId = table.Column<long>(nullable: true),
                    ReRouteId = table.Column<long>(nullable: true),
                    ExceptionsAllowedBeforeBreaking = table.Column<int>(nullable: true),
                    DurationOfBreak = table.Column<int>(nullable: true),
                    TimeoutValue = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppApiGatewayQoSOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppApiGatewayQoSOptions_AppApiGatewayGlobalConfiguration_Ite~",
                        column: x => x.ItemId,
                        principalTable: "AppApiGatewayGlobalConfiguration",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppApiGatewayQoSOptions_AppApiGatewayReRoute_ReRouteId",
                        column: x => x.ReRouteId,
                        principalTable: "AppApiGatewayReRoute",
                        principalColumn: "ReRouteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppApiGatewayRateLimitRule",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ReRouteId = table.Column<long>(nullable: true),
                    DynamicReRouteId = table.Column<long>(nullable: true),
                    ClientWhitelist = table.Column<string>(maxLength: 1000, nullable: true),
                    EnableRateLimiting = table.Column<bool>(nullable: false),
                    Period = table.Column<string>(maxLength: 50, nullable: true),
                    PeriodTimespan = table.Column<double>(nullable: true),
                    Limit = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppApiGatewayRateLimitRule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppApiGatewayRateLimitRule_AppApiGatewayDynamicReRoute_Dynam~",
                        column: x => x.DynamicReRouteId,
                        principalTable: "AppApiGatewayDynamicReRoute",
                        principalColumn: "DynamicReRouteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppApiGatewayRateLimitRule_AppApiGatewayReRoute_ReRouteId",
                        column: x => x.ReRouteId,
                        principalTable: "AppApiGatewayReRoute",
                        principalColumn: "ReRouteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppApiGatewaySecurityOptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ReRouteId = table.Column<long>(nullable: false),
                    IPAllowedList = table.Column<string>(maxLength: 1000, nullable: true),
                    IPBlockedList = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppApiGatewaySecurityOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppApiGatewaySecurityOptions_AppApiGatewayReRoute_ReRouteId",
                        column: x => x.ReRouteId,
                        principalTable: "AppApiGatewayReRoute",
                        principalColumn: "ReRouteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppApiGatewayAggregateConfig_AggregateReRouteId",
                table: "AppApiGatewayAggregateConfig",
                column: "AggregateReRouteId");

            migrationBuilder.CreateIndex(
                name: "IX_AppApiGatewayAuthOptions_ReRouteId",
                table: "AppApiGatewayAuthOptions",
                column: "ReRouteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppApiGatewayBalancerOptions_ItemId",
                table: "AppApiGatewayBalancerOptions",
                column: "ItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppApiGatewayBalancerOptions_ReRouteId",
                table: "AppApiGatewayBalancerOptions",
                column: "ReRouteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppApiGatewayCacheOptions_ReRouteId",
                table: "AppApiGatewayCacheOptions",
                column: "ReRouteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppApiGatewayDiscovery_ItemId",
                table: "AppApiGatewayDiscovery",
                column: "ItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppApiGatewayHttpOptions_ItemId",
                table: "AppApiGatewayHttpOptions",
                column: "ItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppApiGatewayHttpOptions_ReRouteId",
                table: "AppApiGatewayHttpOptions",
                column: "ReRouteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppApiGatewayQoSOptions_ItemId",
                table: "AppApiGatewayQoSOptions",
                column: "ItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppApiGatewayQoSOptions_ReRouteId",
                table: "AppApiGatewayQoSOptions",
                column: "ReRouteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppApiGatewayRateLimitOptions_ItemId",
                table: "AppApiGatewayRateLimitOptions",
                column: "ItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppApiGatewayRateLimitRule_DynamicReRouteId",
                table: "AppApiGatewayRateLimitRule",
                column: "DynamicReRouteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppApiGatewayRateLimitRule_ReRouteId",
                table: "AppApiGatewayRateLimitRule",
                column: "ReRouteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppApiGatewayReRoute_DownstreamPathTemplate_UpstreamPathTemp~",
                table: "AppApiGatewayReRoute",
                columns: new[] { "DownstreamPathTemplate", "UpstreamPathTemplate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppApiGatewayRouter_AppId_AppName_AppIpAddress",
                table: "AppApiGatewayRouter",
                columns: new[] { "AppId", "AppName", "AppIpAddress" });

            migrationBuilder.CreateIndex(
                name: "IX_AppApiGatewaySecurityOptions_ReRouteId",
                table: "AppApiGatewaySecurityOptions",
                column: "ReRouteId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppApiGatewayAggregateConfig");

            migrationBuilder.DropTable(
                name: "AppApiGatewayAuthOptions");

            migrationBuilder.DropTable(
                name: "AppApiGatewayBalancerOptions");

            migrationBuilder.DropTable(
                name: "AppApiGatewayCacheOptions");

            migrationBuilder.DropTable(
                name: "AppApiGatewayDiscovery");

            migrationBuilder.DropTable(
                name: "AppApiGatewayHeaders");

            migrationBuilder.DropTable(
                name: "AppApiGatewayHostAndPort");

            migrationBuilder.DropTable(
                name: "AppApiGatewayHttpOptions");

            migrationBuilder.DropTable(
                name: "AppApiGatewayQoSOptions");

            migrationBuilder.DropTable(
                name: "AppApiGatewayRateLimitOptions");

            migrationBuilder.DropTable(
                name: "AppApiGatewayRateLimitRule");

            migrationBuilder.DropTable(
                name: "AppApiGatewayRouter");

            migrationBuilder.DropTable(
                name: "AppApiGatewaySecurityOptions");

            migrationBuilder.DropTable(
                name: "AppApiGatewayAggregate");

            migrationBuilder.DropTable(
                name: "AppApiGatewayGlobalConfiguration");

            migrationBuilder.DropTable(
                name: "AppApiGatewayDynamicReRoute");

            migrationBuilder.DropTable(
                name: "AppApiGatewayReRoute");
        }
    }
}
