using Microsoft.EntityFrameworkCore.Migrations;

namespace LINGYUN.ApiGateway.HttpApi.Host.Migrations
{
    public partial class ModifyReRouteIndexUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppApiGatewayReRoute_DownstreamPathTemplate_UpstreamPathTemp~",
                table: "AppApiGatewayReRoute");

            migrationBuilder.CreateIndex(
                name: "IX_AppApiGatewayReRoute_AppId_DownstreamPathTemplate_UpstreamPa~",
                table: "AppApiGatewayReRoute",
                columns: new[] { "AppId", "DownstreamPathTemplate", "UpstreamPathTemplate" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppApiGatewayReRoute_AppId_DownstreamPathTemplate_UpstreamPa~",
                table: "AppApiGatewayReRoute");

            migrationBuilder.CreateIndex(
                name: "IX_AppApiGatewayReRoute_DownstreamPathTemplate_UpstreamPathTemp~",
                table: "AppApiGatewayReRoute",
                columns: new[] { "DownstreamPathTemplate", "UpstreamPathTemplate" },
                unique: true);
        }
    }
}
