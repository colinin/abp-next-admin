using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LINGYUN.Abp.MicroService.TaskService.Migrations
{
    /// <inheritdoc />
    public partial class Initial_Task_Service : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TK_BackgroundJobActions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    JobId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    Paramters = table.Column<string>(type: "text", nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TK_BackgroundJobActions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TK_BackgroundJobLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    JobId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    JobName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    JobGroup = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    JobType = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Message = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    RunTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Exception = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TK_BackgroundJobLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TK_BackgroundJobs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Group = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Result = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Args = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    LockTimeOut = table.Column<int>(type: "integer", nullable: false),
                    BeginTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastRunTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    NextRunTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    JobType = table.Column<int>(type: "integer", nullable: false),
                    Cron = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Source = table.Column<int>(type: "integer", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    TriggerCount = table.Column<int>(type: "integer", nullable: false),
                    TryCount = table.Column<int>(type: "integer", nullable: false),
                    MaxTryCount = table.Column<int>(type: "integer", nullable: false),
                    MaxCount = table.Column<int>(type: "integer", nullable: false),
                    Interval = table.Column<int>(type: "integer", nullable: false),
                    IsAbandoned = table.Column<bool>(type: "boolean", nullable: false),
                    NodeName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TK_BackgroundJobs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TK_BackgroundJobActions_Name",
                table: "TK_BackgroundJobActions",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_TK_BackgroundJobLogs_JobGroup_JobName",
                table: "TK_BackgroundJobLogs",
                columns: new[] { "JobGroup", "JobName" });

            migrationBuilder.CreateIndex(
                name: "IX_TK_BackgroundJobs_Name_Group",
                table: "TK_BackgroundJobs",
                columns: new[] { "Name", "Group" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TK_BackgroundJobActions");

            migrationBuilder.DropTable(
                name: "TK_BackgroundJobLogs");

            migrationBuilder.DropTable(
                name: "TK_BackgroundJobs");
        }
    }
}
