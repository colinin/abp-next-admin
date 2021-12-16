using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LY.MicroService.WorkflowManagement.Migrations
{
    public partial class AddModuleWorkflowManagement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WF_Compensate",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    WorkflowId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StepType = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CancelCondition = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ErrorBehavior = table.Column<int>(type: "int", nullable: true),
                    RetryInterval = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    Saga = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ParentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Inputs = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Outputs = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SelectNextStep = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WF_Compensate", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WF_Definition",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    IsEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Version = table.Column<int>(type: "int", nullable: false),
                    ErrorBehavior = table.Column<int>(type: "int", nullable: false),
                    ErrorRetryInterval = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    ExtraProperties = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WF_Definition", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WF_Event",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    EventName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EventKey = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EventData = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsProcessed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WF_Event", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WF_ExecutionError",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    WorkflowId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ExecutionPointerId = table.Column<Guid>(type: "char(50)", maxLength: 50, nullable: false, collation: "ascii_general_ci"),
                    ErrorTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Message = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WF_ExecutionError", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WF_ScheduledCommand",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CommandName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Data = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExecuteTime = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WF_ScheduledCommand", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WF_Step",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    WorkflowId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StepType = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CancelCondition = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ErrorBehavior = table.Column<int>(type: "int", nullable: true),
                    RetryInterval = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    Saga = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ParentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Inputs = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Outputs = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SelectNextStep = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WF_Step", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WF_Subscription",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    WorkflowId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StepId = table.Column<int>(type: "int", nullable: false),
                    ExecutionPointerId = table.Column<Guid>(type: "char(50)", maxLength: 50, nullable: false, collation: "ascii_general_ci"),
                    EventName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EventKey = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SubscribeAsOf = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SubscriptionData = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExternalToken = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExternalWorkerId = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExternalTokenExpiry = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WF_Subscription", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WF_Workflow",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    WorkflowDefinitionId = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Version = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Reference = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NextExecution = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompleteTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ExtraProperties = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WF_Workflow", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WF_ExecutionPointer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(50)", maxLength: 50, nullable: false, collation: "ascii_general_ci"),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    WorkflowId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StepId = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SleepUntil = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    PersistenceData = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    EventName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EventKey = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EventPublished = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    EventData = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StepName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RetryCount = table.Column<int>(type: "int", nullable: false),
                    Children = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContextItem = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PredecessorId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Outcome = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Scope = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WF_ExecutionPointer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WF_ExecutionPointer_WF_Workflow_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "WF_Workflow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WF_ExtensionAttribute",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ExecutionPointerId = table.Column<Guid>(type: "char(50)", maxLength: 50, nullable: false, collation: "ascii_general_ci"),
                    Key = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WF_ExtensionAttribute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WF_ExtensionAttribute_WF_ExecutionPointer_ExecutionPointerId",
                        column: x => x.ExecutionPointerId,
                        principalTable: "WF_ExecutionPointer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_WF_Event_CreationTime",
                table: "WF_Event",
                column: "CreationTime");

            migrationBuilder.CreateIndex(
                name: "IX_WF_Event_EventName_EventKey",
                table: "WF_Event",
                columns: new[] { "EventName", "EventKey" });

            migrationBuilder.CreateIndex(
                name: "IX_WF_Event_IsProcessed",
                table: "WF_Event",
                column: "IsProcessed");

            migrationBuilder.CreateIndex(
                name: "IX_WF_ExecutionPointer_WorkflowId",
                table: "WF_ExecutionPointer",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_WF_ExtensionAttribute_ExecutionPointerId",
                table: "WF_ExtensionAttribute",
                column: "ExecutionPointerId");

            migrationBuilder.CreateIndex(
                name: "IX_WF_ScheduledCommand_CommandName_Data",
                table: "WF_ScheduledCommand",
                columns: new[] { "CommandName", "Data" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WF_ScheduledCommand_ExecuteTime",
                table: "WF_ScheduledCommand",
                column: "ExecuteTime");

            migrationBuilder.CreateIndex(
                name: "IX_WF_Subscription_EventKey",
                table: "WF_Subscription",
                column: "EventKey");

            migrationBuilder.CreateIndex(
                name: "IX_WF_Subscription_EventName",
                table: "WF_Subscription",
                column: "EventName");

            migrationBuilder.CreateIndex(
                name: "IX_WF_Workflow_NextExecution",
                table: "WF_Workflow",
                column: "NextExecution");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WF_Compensate");

            migrationBuilder.DropTable(
                name: "WF_Definition");

            migrationBuilder.DropTable(
                name: "WF_Event");

            migrationBuilder.DropTable(
                name: "WF_ExecutionError");

            migrationBuilder.DropTable(
                name: "WF_ExtensionAttribute");

            migrationBuilder.DropTable(
                name: "WF_ScheduledCommand");

            migrationBuilder.DropTable(
                name: "WF_Step");

            migrationBuilder.DropTable(
                name: "WF_Subscription");

            migrationBuilder.DropTable(
                name: "WF_ExecutionPointer");

            migrationBuilder.DropTable(
                name: "WF_Workflow");
        }
    }
}
