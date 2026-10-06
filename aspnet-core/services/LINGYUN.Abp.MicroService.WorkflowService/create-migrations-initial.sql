-- basic script

CREATE DATABASE `Workflow-V70` CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_general_ci';

USE `Workflow-V70`;

CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `Bookmarks` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `TenantId` varchar(255) CHARACTER SET utf8mb4 NULL,
    `Hash` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Model` longtext CHARACTER SET utf8mb4 NOT NULL,
    `ModelType` longtext CHARACTER SET utf8mb4 NOT NULL,
    `ActivityType` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ActivityId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `WorkflowInstanceId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `CorrelationId` varchar(255) CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_Bookmarks` PRIMARY KEY (`Id`)
) CHARACTER SET utf8mb4;

CREATE TABLE `WorkflowDefinitions` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `DefinitionId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `TenantId` varchar(255) CHARACTER SET utf8mb4 NULL,
    `Name` varchar(255) CHARACTER SET utf8mb4 NULL,
    `DisplayName` longtext CHARACTER SET utf8mb4 NULL,
    `Description` longtext CHARACTER SET utf8mb4 NULL,
    `Version` int NOT NULL,
    `IsSingleton` tinyint(1) NOT NULL,
    `PersistenceBehavior` int NOT NULL,
    `DeleteCompletedInstances` tinyint(1) NOT NULL,
    `IsPublished` tinyint(1) NOT NULL,
    `IsLatest` tinyint(1) NOT NULL,
    `Tag` varchar(255) CHARACTER SET utf8mb4 NULL,
    `Data` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_WorkflowDefinitions` PRIMARY KEY (`Id`)
) CHARACTER SET utf8mb4;

CREATE TABLE `WorkflowExecutionLogRecords` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `TenantId` varchar(255) CHARACTER SET utf8mb4 NULL,
    `WorkflowInstanceId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ActivityId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ActivityType` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Timestamp` datetime(6) NOT NULL,
    `EventName` longtext CHARACTER SET utf8mb4 NULL,
    `Message` longtext CHARACTER SET utf8mb4 NULL,
    `Source` longtext CHARACTER SET utf8mb4 NULL,
    `Data` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_WorkflowExecutionLogRecords` PRIMARY KEY (`Id`)
) CHARACTER SET utf8mb4;

CREATE TABLE `WorkflowInstances` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `DefinitionId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `TenantId` varchar(255) CHARACTER SET utf8mb4 NULL,
    `Version` int NOT NULL,
    `WorkflowStatus` int NOT NULL,
    `CorrelationId` varchar(255) CHARACTER SET utf8mb4 NULL,
    `ContextType` varchar(255) CHARACTER SET utf8mb4 NULL,
    `ContextId` varchar(255) CHARACTER SET utf8mb4 NULL,
    `Name` varchar(255) CHARACTER SET utf8mb4 NULL,
    `CreatedAt` datetime(6) NOT NULL,
    `LastExecutedAt` datetime(6) NULL,
    `FinishedAt` datetime(6) NULL,
    `CancelledAt` datetime(6) NULL,
    `FaultedAt` datetime(6) NULL,
    `Data` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_WorkflowInstances` PRIMARY KEY (`Id`)
) CHARACTER SET utf8mb4;

CREATE INDEX `IX_Bookmark_ActivityId` ON `Bookmarks` (`ActivityId`);

CREATE INDEX `IX_Bookmark_ActivityType` ON `Bookmarks` (`ActivityType`);

CREATE INDEX `IX_Bookmark_ActivityType_TenantId_Hash` ON `Bookmarks` (`ActivityType`, `TenantId`, `Hash`);

CREATE INDEX `IX_Bookmark_CorrelationId` ON `Bookmarks` (`CorrelationId`);

CREATE INDEX `IX_Bookmark_Hash` ON `Bookmarks` (`Hash`);

CREATE INDEX `IX_Bookmark_Hash_CorrelationId_TenantId` ON `Bookmarks` (`Hash`, `CorrelationId`, `TenantId`);

CREATE INDEX `IX_Bookmark_TenantId` ON `Bookmarks` (`TenantId`);

CREATE INDEX `IX_Bookmark_WorkflowInstanceId` ON `Bookmarks` (`WorkflowInstanceId`);

CREATE UNIQUE INDEX `IX_WorkflowDefinition_DefinitionId_VersionId` ON `WorkflowDefinitions` (`DefinitionId`, `Version`);

CREATE INDEX `IX_WorkflowDefinition_IsLatest` ON `WorkflowDefinitions` (`IsLatest`);

CREATE INDEX `IX_WorkflowDefinition_IsPublished` ON `WorkflowDefinitions` (`IsPublished`);

CREATE INDEX `IX_WorkflowDefinition_Name` ON `WorkflowDefinitions` (`Name`);

CREATE INDEX `IX_WorkflowDefinition_Tag` ON `WorkflowDefinitions` (`Tag`);

CREATE INDEX `IX_WorkflowDefinition_TenantId` ON `WorkflowDefinitions` (`TenantId`);

CREATE INDEX `IX_WorkflowDefinition_Version` ON `WorkflowDefinitions` (`Version`);

CREATE INDEX `IX_WorkflowExecutionLogRecord_ActivityId` ON `WorkflowExecutionLogRecords` (`ActivityId`);

CREATE INDEX `IX_WorkflowExecutionLogRecord_ActivityType` ON `WorkflowExecutionLogRecords` (`ActivityType`);

CREATE INDEX `IX_WorkflowExecutionLogRecord_TenantId` ON `WorkflowExecutionLogRecords` (`TenantId`);

CREATE INDEX `IX_WorkflowExecutionLogRecord_Timestamp` ON `WorkflowExecutionLogRecords` (`Timestamp`);

CREATE INDEX `IX_WorkflowExecutionLogRecord_WorkflowInstanceId` ON `WorkflowExecutionLogRecords` (`WorkflowInstanceId`);

CREATE INDEX `IX_WorkflowInstance_ContextId` ON `WorkflowInstances` (`ContextId`);

CREATE INDEX `IX_WorkflowInstance_ContextType` ON `WorkflowInstances` (`ContextType`);

CREATE INDEX `IX_WorkflowInstance_CorrelationId` ON `WorkflowInstances` (`CorrelationId`);

CREATE INDEX `IX_WorkflowInstance_CreatedAt` ON `WorkflowInstances` (`CreatedAt`);

CREATE INDEX `IX_WorkflowInstance_DefinitionId` ON `WorkflowInstances` (`DefinitionId`);

CREATE INDEX `IX_WorkflowInstance_FaultedAt` ON `WorkflowInstances` (`FaultedAt`);

CREATE INDEX `IX_WorkflowInstance_FinishedAt` ON `WorkflowInstances` (`FinishedAt`);

CREATE INDEX `IX_WorkflowInstance_LastExecutedAt` ON `WorkflowInstances` (`LastExecutedAt`);

CREATE INDEX `IX_WorkflowInstance_Name` ON `WorkflowInstances` (`Name`);

CREATE INDEX `IX_WorkflowInstance_TenantId` ON `WorkflowInstances` (`TenantId`);

CREATE INDEX `IX_WorkflowInstance_WorkflowStatus` ON `WorkflowInstances` (`WorkflowStatus`);

CREATE INDEX `IX_WorkflowInstance_WorkflowStatus_DefinitionId` ON `WorkflowInstances` (`WorkflowStatus`, `DefinitionId`);

CREATE INDEX `IX_WorkflowInstance_WorkflowStatus_DefinitionId_Version` ON `WorkflowInstances` (`WorkflowStatus`, `DefinitionId`, `Version`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20210523093427_Initial', '5.0.10');

COMMIT;

START TRANSACTION;

ALTER TABLE `WorkflowInstances` MODIFY COLUMN `CorrelationId` varchar(255) CHARACTER SET utf8mb4 NOT NULL DEFAULT '';

ALTER TABLE `WorkflowInstances` ADD `LastExecutedActivityId` longtext CHARACTER SET utf8mb4 NULL;

ALTER TABLE `WorkflowDefinitions` ADD `OutputStorageProviderName` longtext CHARACTER SET utf8mb4 NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20210611200027_Update21', '5.0.10');

COMMIT;

START TRANSACTION;

ALTER TABLE `WorkflowDefinitions` DROP COLUMN `OutputStorageProviderName`;

ALTER TABLE `WorkflowInstances` RENAME `WorkflowInstances`;

ALTER TABLE `WorkflowExecutionLogRecords` RENAME `WorkflowExecutionLogRecords`;

ALTER TABLE `WorkflowDefinitions` RENAME `WorkflowDefinitions`;

ALTER TABLE `Bookmarks` RENAME `Bookmarks`;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20210923112211_Update23', '5.0.10');

COMMIT;

START TRANSACTION;

ALTER TABLE `WorkflowInstances` ADD `DefinitionVersionId` longtext CHARACTER SET utf8mb4 NOT NULL;

ALTER TABLE `Bookmarks` MODIFY COLUMN `CorrelationId` varchar(255) CHARACTER SET utf8mb4 NOT NULL DEFAULT '';

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20211215100204_Update24', '5.0.10');

COMMIT;

START TRANSACTION;

ALTER TABLE `WorkflowInstances` MODIFY COLUMN `DefinitionVersionId` varchar(255) CHARACTER SET utf8mb4 NOT NULL;

CREATE INDEX `IX_WorkflowInstance_DefinitionVersionId` ON `WorkflowInstances` (`DefinitionVersionId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20220120170050_Update241', '5.0.10');

COMMIT;

START TRANSACTION;

CREATE TABLE `Triggers` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `TenantId` varchar(255) CHARACTER SET utf8mb4 NULL,
    `Hash` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Model` longtext CHARACTER SET utf8mb4 NOT NULL,
    `ModelType` longtext CHARACTER SET utf8mb4 NOT NULL,
    `ActivityType` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ActivityId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `WorkflowDefinitionId` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Triggers` PRIMARY KEY (`Id`)
) CHARACTER SET utf8mb4;

CREATE INDEX `IX_Trigger_ActivityId` ON `Triggers` (`ActivityId`);

CREATE INDEX `IX_Trigger_ActivityType` ON `Triggers` (`ActivityType`);

CREATE INDEX `IX_Trigger_ActivityType_TenantId_Hash` ON `Triggers` (`ActivityType`, `TenantId`, `Hash`);

CREATE INDEX `IX_Trigger_Hash` ON `Triggers` (`Hash`);

CREATE INDEX `IX_Trigger_Hash_TenantId` ON `Triggers` (`Hash`, `TenantId`);

CREATE INDEX `IX_Trigger_TenantId` ON `Triggers` (`TenantId`);

CREATE INDEX `IX_Trigger_WorkflowDefinitionId` ON `Triggers` (`WorkflowDefinitionId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20220120204150_Update25', '5.0.10');

COMMIT;

START TRANSACTION;

ALTER TABLE `WorkflowDefinitions` ADD `CreatedAt` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00';

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20220512203646_Update28', '5.0.10');

COMMIT;


-- webhooks

CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `WebhookDefinitions` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `TenantId` varchar(255) CHARACTER SET utf8mb4 NULL,
    `Name` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Path` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Description` varchar(255) CHARACTER SET utf8mb4 NULL,
    `PayloadTypeName` varchar(255) CHARACTER SET utf8mb4 NULL,
    `IsEnabled` tinyint(1) NOT NULL,
    CONSTRAINT `PK_WebhookDefinitions` PRIMARY KEY (`Id`)
) CHARACTER SET utf8mb4;

CREATE INDEX `IX_WebhookDefinition_Description` ON `WebhookDefinitions` (`Description`);

CREATE INDEX `IX_WebhookDefinition_IsEnabled` ON `WebhookDefinitions` (`IsEnabled`);

CREATE INDEX `IX_WebhookDefinition_Name` ON `WebhookDefinitions` (`Name`);

CREATE INDEX `IX_WebhookDefinition_Path` ON `WebhookDefinitions` (`Path`);

CREATE INDEX `IX_WebhookDefinition_PayloadTypeName` ON `WebhookDefinitions` (`PayloadTypeName`);

CREATE INDEX `IX_WebhookDefinition_TenantId` ON `WebhookDefinitions` (`TenantId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20210604065041_Initial', '5.0.10');

COMMIT;


-- workflow-settings

CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `WorkflowSettings` (
    `Id` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `WorkflowBlueprintId` varchar(255) CHARACTER SET utf8mb4 NULL,
    `Key` varchar(255) CHARACTER SET utf8mb4 NULL,
    `Value` varchar(255) CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_WorkflowSettings` PRIMARY KEY (`Id`)
) CHARACTER SET utf8mb4;

CREATE INDEX `IX_WorkflowSetting_Key` ON `WorkflowSettings` (`Key`);

CREATE INDEX `IX_WorkflowSetting_Value` ON `WorkflowSettings` (`Value`);

CREATE INDEX `IX_WorkflowSetting_WorkflowBlueprintId` ON `WorkflowSettings` (`WorkflowBlueprintId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20210730112043_Initial', '5.0.10');

COMMIT;
