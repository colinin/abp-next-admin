-- basic script

CREATE SCHEMA IF NOT EXISTS "Elsa";

SET search_path TO "Elsa";

CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" varchar(150) NOT NULL,
    "ProductVersion" varchar(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "Bookmarks" (
    "Id" varchar(255) NOT NULL,
    "TenantId" varchar(255) NULL,
    "Hash" varchar(255) NOT NULL,
    "Model" text NOT NULL,
    "ModelType" text NOT NULL,
    "ActivityType" varchar(255) NOT NULL,
    "ActivityId" varchar(255) NOT NULL,
    "WorkflowInstanceId" varchar(255) NOT NULL,
    "CorrelationId" varchar(255) NULL,
    CONSTRAINT "PK_Bookmarks" PRIMARY KEY ("Id")
);

CREATE TABLE "WorkflowDefinitions" (
    "Id" varchar(255) NOT NULL,
    "DefinitionId" varchar(255) NOT NULL,
    "TenantId" varchar(255) NULL,
    "Name" varchar(255) NULL,
    "DisplayName" text NULL,
    "Description" text NULL,
    "Version" integer NOT NULL,
    "IsSingleton" boolean NOT NULL,
    "PersistenceBehavior" integer NOT NULL,
    "DeleteCompletedInstances" boolean NOT NULL,
    "IsPublished" boolean NOT NULL,
    "IsLatest" boolean NOT NULL,
    "Tag" varchar(255) NULL,
    "Data" text NULL,
    CONSTRAINT "PK_WorkflowDefinitions" PRIMARY KEY ("Id")
);

CREATE TABLE "WorkflowExecutionLogRecords" (
    "Id" varchar(255) NOT NULL,
    "TenantId" varchar(255) NULL,
    "WorkflowInstanceId" varchar(255) NOT NULL,
    "ActivityId" varchar(255) NOT NULL,
    "ActivityType" varchar(255) NOT NULL,
    "Timestamp" timestamp with time zone NOT NULL,
    "EventName" text NULL,
    "Message" text NULL,
    "Source" text NULL,
    "Data" text NULL,
    CONSTRAINT "PK_WorkflowExecutionLogRecords" PRIMARY KEY ("Id")
);

CREATE TABLE "WorkflowInstances" (
    "Id" varchar(255) NOT NULL,
    "DefinitionId" varchar(255) NOT NULL,
    "TenantId" varchar(255) NULL,
    "Version" integer NOT NULL,
    "WorkflowStatus" integer NOT NULL,
    "CorrelationId" varchar(255) NULL,
    "ContextType" varchar(255) NULL,
    "ContextId" varchar(255) NULL,
    "Name" varchar(255) NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "LastExecutedAt" timestamp with time zone NULL,
    "FinishedAt" timestamp with time zone NULL,
    "CancelledAt" timestamp with time zone NULL,
    "FaultedAt" timestamp with time zone NULL,
    "Data" text NULL,
    CONSTRAINT "PK_WorkflowInstances" PRIMARY KEY ("Id")
);

CREATE INDEX "IX_Bookmark_ActivityId" ON "Bookmarks" ("ActivityId");
CREATE INDEX "IX_Bookmark_ActivityType" ON "Bookmarks" ("ActivityType");
CREATE INDEX "IX_Bookmark_ActivityType_TenantId_Hash" ON "Bookmarks" ("ActivityType", "TenantId", "Hash");
CREATE INDEX "IX_Bookmark_CorrelationId" ON "Bookmarks" ("CorrelationId");
CREATE INDEX "IX_Bookmark_Hash" ON "Bookmarks" ("Hash");
CREATE INDEX "IX_Bookmark_Hash_CorrelationId_TenantId" ON "Bookmarks" ("Hash", "CorrelationId", "TenantId");
CREATE INDEX "IX_Bookmark_TenantId" ON "Bookmarks" ("TenantId");
CREATE INDEX "IX_Bookmark_WorkflowInstanceId" ON "Bookmarks" ("WorkflowInstanceId");

CREATE UNIQUE INDEX "IX_WorkflowDefinition_DefinitionId_VersionId" ON "WorkflowDefinitions" ("DefinitionId", "Version");
CREATE INDEX "IX_WorkflowDefinition_IsLatest" ON "WorkflowDefinitions" ("IsLatest");
CREATE INDEX "IX_WorkflowDefinition_IsPublished" ON "WorkflowDefinitions" ("IsPublished");
CREATE INDEX "IX_WorkflowDefinition_Name" ON "WorkflowDefinitions" ("Name");
CREATE INDEX "IX_WorkflowDefinition_Tag" ON "WorkflowDefinitions" ("Tag");
CREATE INDEX "IX_WorkflowDefinition_TenantId" ON "WorkflowDefinitions" ("TenantId");
CREATE INDEX "IX_WorkflowDefinition_Version" ON "WorkflowDefinitions" ("Version");

CREATE INDEX "IX_WorkflowExecutionLogRecord_ActivityId" ON "WorkflowExecutionLogRecords" ("ActivityId");
CREATE INDEX "IX_WorkflowExecutionLogRecord_ActivityType" ON "WorkflowExecutionLogRecords" ("ActivityType");
CREATE INDEX "IX_WorkflowExecutionLogRecord_TenantId" ON "WorkflowExecutionLogRecords" ("TenantId");
CREATE INDEX "IX_WorkflowExecutionLogRecord_Timestamp" ON "WorkflowExecutionLogRecords" ("Timestamp");
CREATE INDEX "IX_WorkflowExecutionLogRecord_WorkflowInstanceId" ON "WorkflowExecutionLogRecords" ("WorkflowInstanceId");

CREATE INDEX "IX_WorkflowInstance_ContextId" ON "WorkflowInstances" ("ContextId");
CREATE INDEX "IX_WorkflowInstance_ContextType" ON "WorkflowInstances" ("ContextType");
CREATE INDEX "IX_WorkflowInstance_CorrelationId" ON "WorkflowInstances" ("CorrelationId");
CREATE INDEX "IX_WorkflowInstance_CreatedAt" ON "WorkflowInstances" ("CreatedAt");
CREATE INDEX "IX_WorkflowInstance_DefinitionId" ON "WorkflowInstances" ("DefinitionId");
CREATE INDEX "IX_WorkflowInstance_FaultedAt" ON "WorkflowInstances" ("FaultedAt");
CREATE INDEX "IX_WorkflowInstance_FinishedAt" ON "WorkflowInstances" ("FinishedAt");
CREATE INDEX "IX_WorkflowInstance_LastExecutedAt" ON "WorkflowInstances" ("LastExecutedAt");
CREATE INDEX "IX_WorkflowInstance_Name" ON "WorkflowInstances" ("Name");
CREATE INDEX "IX_WorkflowInstance_TenantId" ON "WorkflowInstances" ("TenantId");
CREATE INDEX "IX_WorkflowInstance_WorkflowStatus" ON "WorkflowInstances" ("WorkflowStatus");
CREATE INDEX "IX_WorkflowInstance_WorkflowStatus_DefinitionId" ON "WorkflowInstances" ("WorkflowStatus", "DefinitionId");
CREATE INDEX "IX_WorkflowInstance_WorkflowStatus_DefinitionId_Version" ON "WorkflowInstances" ("WorkflowStatus", "DefinitionId", "Version");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20210523093427_Initial', '5.0.10');

COMMIT;

START TRANSACTION;

ALTER TABLE "WorkflowInstances" 
ALTER COLUMN "CorrelationId" SET DEFAULT '',
ALTER COLUMN "CorrelationId" SET NOT NULL;

ALTER TABLE "WorkflowInstances" ADD "LastExecutedActivityId" text NULL;

ALTER TABLE "WorkflowDefinitions" ADD "OutputStorageProviderName" text NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20210611200027_Update21', '5.0.10');

COMMIT;

START TRANSACTION;

ALTER TABLE "WorkflowDefinitions" DROP COLUMN "OutputStorageProviderName";

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20210923112211_Update23', '5.0.10');

COMMIT;

START TRANSACTION;

ALTER TABLE "WorkflowInstances" ADD "DefinitionVersionId" text NOT NULL;

ALTER TABLE "Bookmarks" 
ALTER COLUMN "CorrelationId" SET DEFAULT '',
ALTER COLUMN "CorrelationId" SET NOT NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20211215100204_Update24', '5.0.10');

COMMIT;

START TRANSACTION;

ALTER TABLE "WorkflowInstances" 
ALTER COLUMN "DefinitionVersionId" TYPE varchar(255);

CREATE INDEX "IX_WorkflowInstance_DefinitionVersionId" ON "WorkflowInstances" ("DefinitionVersionId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220120170050_Update241', '5.0.10');

COMMIT;

START TRANSACTION;

CREATE TABLE "Triggers" (
    "Id" varchar(255) NOT NULL,
    "TenantId" varchar(255) NULL,
    "Hash" varchar(255) NOT NULL,
    "Model" text NOT NULL,
    "ModelType" text NOT NULL,
    "ActivityType" varchar(255) NOT NULL,
    "ActivityId" varchar(255) NOT NULL,
    "WorkflowDefinitionId" varchar(255) NOT NULL,
    CONSTRAINT "PK_Triggers" PRIMARY KEY ("Id")
);

CREATE INDEX "IX_Trigger_ActivityId" ON "Triggers" ("ActivityId");
CREATE INDEX "IX_Trigger_ActivityType" ON "Triggers" ("ActivityType");
CREATE INDEX "IX_Trigger_ActivityType_TenantId_Hash" ON "Triggers" ("ActivityType", "TenantId", "Hash");
CREATE INDEX "IX_Trigger_Hash" ON "Triggers" ("Hash");
CREATE INDEX "IX_Trigger_Hash_TenantId" ON "Triggers" ("Hash", "TenantId");
CREATE INDEX "IX_Trigger_TenantId" ON "Triggers" ("TenantId");
CREATE INDEX "IX_Trigger_WorkflowDefinitionId" ON "Triggers" ("WorkflowDefinitionId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220120204150_Update25', '5.0.10');

COMMIT;

START TRANSACTION;

ALTER TABLE "WorkflowDefinitions" ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT '0001-01-01 00:00:00';

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220512203646_Update28', '5.0.10');

COMMIT;


-- webhooks

CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" varchar(150) NOT NULL,
    "ProductVersion" varchar(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "WebhookDefinitions" (
    "Id" varchar(255) NOT NULL,
    "TenantId" varchar(255) NULL,
    "Name" varchar(255) NOT NULL,
    "Path" varchar(255) NOT NULL,
    "Description" varchar(255) NULL,
    "PayloadTypeName" varchar(255) NULL,
    "IsEnabled" boolean NOT NULL,
    CONSTRAINT "PK_WebhookDefinitions" PRIMARY KEY ("Id")
);

CREATE INDEX "IX_WebhookDefinition_Description" ON "WebhookDefinitions" ("Description");
CREATE INDEX "IX_WebhookDefinition_IsEnabled" ON "WebhookDefinitions" ("IsEnabled");
CREATE INDEX "IX_WebhookDefinition_Name" ON "WebhookDefinitions" ("Name");
CREATE INDEX "IX_WebhookDefinition_Path" ON "WebhookDefinitions" ("Path");
CREATE INDEX "IX_WebhookDefinition_PayloadTypeName" ON "WebhookDefinitions" ("PayloadTypeName");
CREATE INDEX "IX_WebhookDefinition_TenantId" ON "WebhookDefinitions" ("TenantId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20210604065041_Initial', '5.0.10');

COMMIT;


-- workflow-settings

CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" varchar(150) NOT NULL,
    "ProductVersion" varchar(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "WorkflowSettings" (
    "Id" varchar(255) NOT NULL,
    "WorkflowBlueprintId" varchar(255) NULL,
    "Key" varchar(255) NULL,
    "Value" varchar(255) NULL,
    CONSTRAINT "PK_WorkflowSettings" PRIMARY KEY ("Id")
);

CREATE INDEX "IX_WorkflowSetting_Key" ON "WorkflowSettings" ("Key");
CREATE INDEX "IX_WorkflowSetting_Value" ON "WorkflowSettings" ("Value");
CREATE INDEX "IX_WorkflowSetting_WorkflowBlueprintId" ON "WorkflowSettings" ("WorkflowBlueprintId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20210730112043_Initial', '5.0.10');

COMMIT;


-- secrets

CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" varchar(150) NOT NULL,
    "ProductVersion" varchar(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "Secrets" (
    "Id" varchar(255) NOT NULL,
    "Type" text NOT NULL,
    "Name" text NOT NULL,
    "DisplayName" text NULL,
    "Data" text NULL,
    CONSTRAINT "PK_Secrets" PRIMARY KEY ("Id")
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230709045349_Initial', '5.0.10');

COMMIT;