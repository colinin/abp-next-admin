
USE [${DataBase}];

-- Elsa Schema
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'Elsa')
BEGIN
    EXEC('CREATE SCHEMA Elsa');
END;

-- ----------------------------
-- Table structure for __EFMigrationsHistory
-- ----------------------------
IF OBJECT_ID(N'[Elsa].[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [Elsa].[__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

-- ----------------------------
-- Table structure for bookmarks
-- ----------------------------

CREATE TABLE [Elsa].[Bookmarks] (
  [Id] nvarchar(450) NOT NULL,
  [TenantId] nvarchar(450) NULL,
  [Hash] nvarchar(450) NOT NULL,
  [Model] nvarchar(max) NOT NULL,
  [ModelType] nvarchar(max) NOT NULL,
  [ActivityType] nvarchar(450) NOT NULL,
  [ActivityId] nvarchar(450) NOT NULL,
  [WorkflowInstanceId] nvarchar(450) NOT NULL,
  [CorrelationId] nvarchar(450) NOT NULL
)


-- ----------------------------
-- Table structure for workflowdefinitions
-- ----------------------------

CREATE TABLE [Elsa].[WorkflowDefinitions] (
  [Id] nvarchar(450) NOT NULL,
  [DefinitionId] nvarchar(450) NOT NULL,
  [TenantId] nvarchar(450) NULL,
  [Name] nvarchar(450) NULL,
  [DisplayName] nvarchar(max) NULL,
  [Description] nvarchar(max) NULL,
  [Version] int NOT NULL,
  [IsSingleton] bit NOT NULL,
  [PersistenceBehavior] int NOT NULL,
  [DeleteCompletedInstances] bit NOT NULL,
  [IsPublished] bit NOT NULL,
  [IsLatest] bit NOT NULL,
  [Tag] nvarchar(450) NULL,
  [Data] nvarchar(max) NULL,
  [OutputStorageProviderName] nvarchar(max) NULL,
  [CreatedAt] datetimeoffset NOT NULL
)


-- ----------------------------
-- Table structure for workflowexecutionlogrecords
-- ----------------------------

CREATE TABLE [Elsa].[WorkflowExecutionLogRecords] (
  [Id] nvarchar(450) NOT NULL,
  [TenantId] nvarchar(450) NULL,
  [WorkflowInstanceId] nvarchar(450) NOT NULL,
  [ActivityId] nvarchar(450) NOT NULL,
  [ActivityType] nvarchar(450) NOT NULL,
  [Timestamp] datetimeoffset NOT NULL,
  [EventName] nvarchar(max) NULL,
  [Message] nvarchar(max) NULL,
  [Source] nvarchar(max) NULL,
  [Data] nvarchar(max) NULL
)


-- ----------------------------
-- Table structure for workflowinstances
-- ----------------------------

CREATE TABLE [Elsa].[WorkflowInstances] (
  [Id] nvarchar(450) NOT NULL,
  [DefinitionId] nvarchar(450) NOT NULL,
  [TenantId] nvarchar(450) NULL,
  [Version] int NOT NULL,
  [WorkflowStatus] int NOT NULL,
  [CorrelationId] nvarchar(450) NOT NULL,
  [ContextType] nvarchar(450) NULL,
  [ContextId] nvarchar(450) NULL,
  [Name] nvarchar(450) NULL,
  [CreatedAt] datetimeoffset NOT NULL,
  [LastExecutedAt] datetimeoffset NULL,
  [FinishedAt] datetimeoffset NULL,
  [CancelledAt] datetimeoffset NULL,
  [FaultedAt] datetimeoffset NULL,
  [Data] nvarchar(max) NULL,
  [LastExecutedActivityId] nvarchar(max) NULL,
  [DefinitionVersionId] nvarchar(450) NOT NULL
)


-- ----------------------------
-- Table structure for triggers
-- ----------------------------

CREATE TABLE [Elsa].[Triggers] (
  [Id] nvarchar(450) NOT NULL,
  [TenantId] nvarchar(450) NULL,
  [Hash] nvarchar(450) NOT NULL,
  [Model] nvarchar(max) NOT NULL,
  [ModelType] nvarchar(max) NOT NULL,
  [ActivityType] nvarchar(450) NOT NULL,
  [ActivityId] nvarchar(450) NOT NULL,
  [WorkflowDefinitionId] nvarchar(450) NOT NULL
)


-- ----------------------------
-- Table structure for workflowsettings
-- ----------------------------

CREATE TABLE [Elsa].[WorkflowSettings] (
  [Id] nvarchar(450) NOT NULL,
  [WorkflowBlueprintId] nvarchar(450) NULL,
  [Key] nvarchar(450) NULL,
  [Value] nvarchar(450) NULL
)


-- ----------------------------
-- Table structure for secrets
-- ----------------------------

CREATE TABLE [Elsa].[Secrets] (
  [Id] nvarchar(450) NOT NULL,
  [Type] nvarchar(max) NOT NULL,
  [Name] nvarchar(max) NOT NULL,
  [DisplayName] nvarchar(max) NULL,
  [Data] nvarchar(max) NULL
)


-- ----------------------------
-- Table structure for webhookdefinitions
-- ----------------------------

CREATE TABLE [Elsa].[WebhookDefinitions] (
  [Id] nvarchar(450) NOT NULL,
  [TenantId] nvarchar(450) NULL,
  [Name] nvarchar(450) NOT NULL,
  [Path] nvarchar(450) NOT NULL,
  [Description] nvarchar(450) NULL,
  [PayloadTypeName] nvarchar(450) NULL,
  [IsEnabled] bit NOT NULL
)


-- ----------------------------
-- Indexes structure for table bookmarks
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_Bookmark_ActivityId]
ON [Elsa].[Bookmarks] (
  [ActivityId] ASC
)

CREATE NONCLUSTERED INDEX [IX_Bookmark_ActivityType]
ON [Elsa].[Bookmarks] (
  [ActivityType] ASC
)

CREATE NONCLUSTERED INDEX [IX_Bookmark_ActivityType_TenantId_Hash]
ON [Elsa].[Bookmarks] (
  [ActivityType] ASC,
  [TenantId] ASC,
  [Hash] ASC
)

CREATE NONCLUSTERED INDEX [IX_Bookmark_CorrelationId]
ON [Elsa].[Bookmarks] (
  [CorrelationId] ASC
)

CREATE NONCLUSTERED INDEX [IX_Bookmark_Hash]
ON [Elsa].[Bookmarks] (
  [Hash] ASC
)

CREATE NONCLUSTERED INDEX [IX_Bookmark_Hash_CorrelationId_TenantId]
ON [Elsa].[Bookmarks] (
  [Hash] ASC,
  [CorrelationId] ASC,
  [TenantId] ASC
)

CREATE NONCLUSTERED INDEX [IX_Bookmark_TenantId]
ON [Elsa].[Bookmarks] (
  [TenantId] ASC
)

CREATE NONCLUSTERED INDEX [IX_Bookmark_WorkflowInstanceId]
ON [Elsa].[Bookmarks] (
  [WorkflowInstanceId] ASC
)


-- ----------------------------
-- Primary Key structure for table bookmarks
-- ----------------------------
ALTER TABLE [Elsa].[Bookmarks] ADD PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


-- ----------------------------
-- Indexes structure for table triggers
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_Trigger_ActivityId]
ON [Elsa].[Triggers] (
  [ActivityId] ASC
)

CREATE NONCLUSTERED INDEX [IX_Trigger_ActivityType]
ON [Elsa].[Triggers] (
  [ActivityType] ASC
)

CREATE NONCLUSTERED INDEX [IX_Trigger_ActivityType_TenantId_Hash]
ON [Elsa].[Triggers] (
  [ActivityType] ASC,
  [TenantId] ASC,
  [Hash] ASC
)

CREATE NONCLUSTERED INDEX [IX_Trigger_Hash]
ON [Elsa].[Triggers] (
  [Hash] ASC
)

CREATE NONCLUSTERED INDEX [IX_Trigger_Hash_TenantId]
ON [Elsa].[Triggers] (
  [Hash] ASC,
  [TenantId] ASC
)

CREATE NONCLUSTERED INDEX [IX_Trigger_TenantId]
ON [Elsa].[Triggers] (
  [TenantId] ASC
)

CREATE NONCLUSTERED INDEX [IX_Trigger_WorkflowDefinitionId]
ON [Elsa].[Triggers] (
  [WorkflowDefinitionId] ASC
)


-- ----------------------------
-- Primary Key structure for table triggers
-- ----------------------------
ALTER TABLE [Elsa].[Triggers] ADD PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


-- ----------------------------
-- Indexes structure for table workflowdefinitions
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [IX_WorkflowDefinition_DefinitionId_VersionId]
ON [Elsa].[WorkflowDefinitions] (
  [DefinitionId] ASC,
  [Version] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowDefinition_IsLatest]
ON [Elsa].[WorkflowDefinitions] (
  [IsLatest] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowDefinition_IsPublished]
ON [Elsa].[WorkflowDefinitions] (
  [IsPublished] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowDefinition_Name]
ON [Elsa].[WorkflowDefinitions] (
  [Name] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowDefinition_Tag]
ON [Elsa].[WorkflowDefinitions] (
  [Tag] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowDefinition_TenantId]
ON [Elsa].[WorkflowDefinitions] (
  [TenantId] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowDefinition_Version]
ON [Elsa].[WorkflowDefinitions] (
  [Version] ASC
)


-- ----------------------------
-- Primary Key structure for table workflowdefinitions
-- ----------------------------
ALTER TABLE [Elsa].[WorkflowDefinitions] ADD PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


-- ----------------------------
-- Indexes structure for table workflowexecutionlogrecords
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_WorkflowExecutionLogRecord_ActivityId]
ON [Elsa].[WorkflowExecutionLogRecords] (
  [ActivityId] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowExecutionLogRecord_ActivityType]
ON [Elsa].[WorkflowExecutionLogRecords] (
  [ActivityType] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowExecutionLogRecord_TenantId]
ON [Elsa].[WorkflowExecutionLogRecords] (
  [TenantId] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowExecutionLogRecord_Timestamp]
ON [Elsa].[WorkflowExecutionLogRecords] (
  [Timestamp] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowExecutionLogRecord_WorkflowInstanceId]
ON [Elsa].[WorkflowExecutionLogRecords] (
  [WorkflowInstanceId] ASC
)


-- ----------------------------
-- Primary Key structure for table workflowexecutionlogrecords
-- ----------------------------
ALTER TABLE [Elsa].[WorkflowExecutionLogRecords] ADD PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


-- ----------------------------
-- Indexes structure for table workflowinstances
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_WorkflowInstance_ContextId]
ON [Elsa].[WorkflowInstances] (
  [ContextId] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowInstance_ContextType]
ON [Elsa].[WorkflowInstances] (
  [ContextType] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowInstance_CorrelationId]
ON [Elsa].[WorkflowInstances] (
  [CorrelationId] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowInstance_CreatedAt]
ON [Elsa].[WorkflowInstances] (
  [CreatedAt] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowInstance_DefinitionId]
ON [Elsa].[WorkflowInstances] (
  [DefinitionId] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowInstance_FaultedAt]
ON [Elsa].[WorkflowInstances] (
  [FaultedAt] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowInstance_FinishedAt]
ON [Elsa].[WorkflowInstances] (
  [FinishedAt] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowInstance_LastExecutedAt]
ON [Elsa].[WorkflowInstances] (
  [LastExecutedAt] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowInstance_Name]
ON [Elsa].[WorkflowInstances] (
  [Name] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowInstance_TenantId]
ON [Elsa].[WorkflowInstances] (
  [TenantId] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowInstance_WorkflowStatus]
ON [Elsa].[WorkflowInstances] (
  [WorkflowStatus] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowInstance_WorkflowStatus_DefinitionId]
ON [Elsa].[WorkflowInstances] (
  [WorkflowStatus] ASC,
  [DefinitionId] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowInstance_WorkflowStatus_DefinitionId_Version]
ON [Elsa].[WorkflowInstances] (
  [WorkflowStatus] ASC,
  [DefinitionId] ASC,
  [Version] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowInstance_DefinitionVersionId]
ON [Elsa].[WorkflowInstances] (
  [DefinitionVersionId] ASC
)


-- ----------------------------
-- Primary Key structure for table workflowinstances
-- ----------------------------
ALTER TABLE [Elsa].[WorkflowInstances] ADD PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


-- ----------------------------
-- Indexes structure for table workflowsettings
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_WorkflowSetting_Key]
ON [Elsa].[WorkflowSettings] (
  [Key] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowSetting_Value]
ON [Elsa].[WorkflowSettings] (
  [Value] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowSetting_WorkflowBlueprintId]
ON [Elsa].[WorkflowSettings] (
  [WorkflowBlueprintId] ASC
)


-- ----------------------------
-- Primary Key structure for table workflowsettings
-- ----------------------------
ALTER TABLE [Elsa].[WorkflowSettings] ADD PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


-- ----------------------------
-- Indexes structure for table webhookdefinitions
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_WebhookDefinition_Description]
ON [Elsa].[WebhookDefinitions] (
  [Description] ASC
)

CREATE NONCLUSTERED INDEX [IX_WebhookDefinition_IsEnabled]
ON [Elsa].[WebhookDefinitions] (
  [IsEnabled] ASC
)

CREATE NONCLUSTERED INDEX [IX_WebhookDefinition_Name]
ON [Elsa].[WebhookDefinitions] (
  [Name] ASC
)

CREATE NONCLUSTERED INDEX [IX_WebhookDefinition_Path]
ON [Elsa].[WebhookDefinitions] (
  [Path] ASC
)

CREATE NONCLUSTERED INDEX [IX_WebhookDefinition_PayloadTypeName]
ON [Elsa].[WebhookDefinitions] (
  [PayloadTypeName] ASC
)

CREATE NONCLUSTERED INDEX [IX_WebhookDefinition_TenantId]
ON [Elsa].[WebhookDefinitions] (
  [TenantId] ASC
)

-- ----------------------------
-- Primary Key structure for table webhookdefinitions
-- ----------------------------
ALTER TABLE [Elsa].[WebhookDefinitions] ADD PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

-- ----------------------------
-- Primary Key structure for table secrets
-- ----------------------------
ALTER TABLE [Elsa].[Secrets] ADD PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


INSERT INTO [Elsa].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES 
(N'20210523093427_Initial', '5.0.10'),
(N'20210611200027_Update21', '5.0.10'),
(N'20210923112211_Update23', '5.0.10'),
(N'20211215100204_Update24', '5.0.10'),
(N'20220120170050_Update241', '5.0.10'),
(N'20220120204150_Update25', '5.0.10'),
(N'20220512203646_Update28', '5.0.10'),
(N'20210604065041_Initial', '5.0.10'),
(N'20210730112043_Initial', '5.0.10'),
(N'20210604065113_Initial', '5.0.10'),
(N'20230709045349_Initial', '5.0.10');
