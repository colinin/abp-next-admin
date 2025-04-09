
USE [${DataBase}];
-- ----------------------------
-- Table structure for __EFMigrationsHistory
-- ----------------------------
IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

-- ----------------------------
-- Table structure for bookmarks
-- ----------------------------

CREATE TABLE [dbo].[Bookmarks] (
  [Id] nvarchar(255) NOT NULL,
  [TenantId] nvarchar(255) NULL,
  [Hash] nvarchar(255) NOT NULL,
  [Model] nvarchar(max) NOT NULL,
  [ModelType] nvarchar(max) NOT NULL,
  [ActivityType] nvarchar(255) NOT NULL,
  [ActivityId] nvarchar(255) NOT NULL,
  [WorkflowInstanceId] nvarchar(255) NOT NULL,
  [CorrelationId] nvarchar(255) NOT NULL
)


-- ----------------------------
-- Table structure for triggers
-- ----------------------------

CREATE TABLE [dbo].[Triggers] (
  [Id] nvarchar(255) NOT NULL,
  [TenantId] nvarchar(255) NULL,
  [Hash] nvarchar(255) NOT NULL,
  [Model] nvarchar(max) NOT NULL,
  [ModelType] nvarchar(max) NOT NULL,
  [ActivityType] nvarchar(255) NOT NULL,
  [ActivityId] nvarchar(255) NOT NULL,
  [WorkflowDefinitionId] nvarchar(255) NOT NULL
)


-- ----------------------------
-- Table structure for workflowdefinitions
-- ----------------------------

CREATE TABLE [dbo].[WorkflowDefinitions] (
  [Id] nvarchar(255) NOT NULL,
  [DefinitionId] nvarchar(255) NOT NULL,
  [TenantId] nvarchar(255) NULL,
  [Name] nvarchar(255) NULL,
  [DisplayName] nvarchar(max) NULL,
  [Description] nvarchar(max) NULL,
  [Version] int NOT NULL,
  [IsSingleton] tinyint NOT NULL,
  [PersistenceBehavior] int NOT NULL,
  [DeleteCompletedInstances] tinyint NOT NULL,
  [IsPublished] tinyint NOT NULL,
  [IsLatest] tinyint NOT NULL,
  [Tag] nvarchar(255) NULL,
  [Data] nvarchar(max) NULL,
  [CreatedAt] datetime2 NOT NULL
)


-- ----------------------------
-- Table structure for workflowexecutionlogrecords
-- ----------------------------

CREATE TABLE [dbo].[WorkflowExecutionLogRecords] (
  [Id] nvarchar(255) NOT NULL,
  [TenantId] nvarchar(255) NULL,
  [WorkflowInstanceId] nvarchar(255) NOT NULL,
  [ActivityId] nvarchar(255) NOT NULL,
  [ActivityType] nvarchar(255) NOT NULL,
  [Timestamp] datetime2 NOT NULL,
  [EventName] nvarchar(max) NULL,
  [Message] nvarchar(max) NULL,
  [Source] nvarchar(max) NULL,
  [Data] nvarchar(max) NULL
)


-- ----------------------------
-- Table structure for workflowinstances
-- ----------------------------

CREATE TABLE [dbo].[WorkflowInstances] (
  [Id] nvarchar(255) NOT NULL,
  [DefinitionId] nvarchar(255) NOT NULL,
  [TenantId] nvarchar(255) NULL,
  [Version] int NOT NULL,
  [WorkflowStatus] int NOT NULL,
  [CorrelationId] nvarchar(255) NOT NULL,
  [ContextType] nvarchar(255) NULL,
  [ContextId] nvarchar(255) NULL,
  [Name] nvarchar(255) NULL,
  [CreatedAt] datetime2 NOT NULL,
  [LastExecutedAt] datetime2 NULL,
  [FinishedAt] datetime2 NULL,
  [CancelledAt] datetime2 NULL,
  [FaultedAt] datetime2 NULL,
  [Data] nvarchar(max) NULL,
  [LastExecutedActivityId] nvarchar(max) NULL,
  [DefinitionVersionId] nvarchar(255) NOT NULL
)


-- ----------------------------
-- Table structure for workflowsettings
-- ----------------------------

CREATE TABLE [dbo].[WorkflowSettings] (
  [Id] nvarchar(255) NOT NULL,
  [WorkflowBlueprintId] nvarchar(255) NULL,
  [Key] nvarchar(255) NULL,
  [Value] nvarchar(255) NULL
)


-- ----------------------------
-- Indexes structure for table bookmarks
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_Bookmark_ActivityId]
ON [dbo].[Bookmarks] (
  [ActivityId] ASC
)

CREATE NONCLUSTERED INDEX [IX_Bookmark_ActivityType]
ON [dbo].[Bookmarks] (
  [ActivityType] ASC
)

CREATE NONCLUSTERED INDEX [IX_Bookmark_ActivityType_TenantId_Hash]
ON [dbo].[Bookmarks] (
  [ActivityType] ASC,
  [TenantId] ASC,
  [Hash] ASC
)

CREATE NONCLUSTERED INDEX [IX_Bookmark_CorrelationId]
ON [dbo].[Bookmarks] (
  [CorrelationId] ASC
)

CREATE NONCLUSTERED INDEX [IX_Bookmark_Hash]
ON [dbo].[Bookmarks] (
  [Hash] ASC
)

CREATE NONCLUSTERED INDEX [IX_Bookmark_Hash_CorrelationId_TenantId]
ON [dbo].[Bookmarks] (
  [Hash] ASC,
  [CorrelationId] ASC,
  [TenantId] ASC
)

CREATE NONCLUSTERED INDEX [IX_Bookmark_TenantId]
ON [dbo].[Bookmarks] (
  [TenantId] ASC
)

CREATE NONCLUSTERED INDEX [IX_Bookmark_WorkflowInstanceId]
ON [dbo].[Bookmarks] (
  [WorkflowInstanceId] ASC
)


-- ----------------------------
-- Primary Key structure for table bookmarks
-- ----------------------------
ALTER TABLE [dbo].[Bookmarks] ADD PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


-- ----------------------------
-- Indexes structure for table triggers
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_Trigger_ActivityId]
ON [dbo].[Triggers] (
  [ActivityId] ASC
)

CREATE NONCLUSTERED INDEX [IX_Trigger_ActivityType]
ON [dbo].[Triggers] (
  [ActivityType] ASC
)

CREATE NONCLUSTERED INDEX [IX_Trigger_ActivityType_TenantId_Hash]
ON [dbo].[Triggers] (
  [ActivityType] ASC,
  [TenantId] ASC,
  [Hash] ASC
)

CREATE NONCLUSTERED INDEX [IX_Trigger_Hash]
ON [dbo].[Triggers] (
  [Hash] ASC
)

CREATE NONCLUSTERED INDEX [IX_Trigger_Hash_TenantId]
ON [dbo].[Triggers] (
  [Hash] ASC,
  [TenantId] ASC
)

CREATE NONCLUSTERED INDEX [IX_Trigger_TenantId]
ON [dbo].[Triggers] (
  [TenantId] ASC
)

CREATE NONCLUSTERED INDEX [IX_Trigger_WorkflowDefinitionId]
ON [dbo].[Triggers] (
  [WorkflowDefinitionId] ASC
)


-- ----------------------------
-- Primary Key structure for table triggers
-- ----------------------------
ALTER TABLE [dbo].[Triggers] ADD PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


-- ----------------------------
-- Indexes structure for table workflowdefinitions
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [IX_WorkflowDefinition_DefinitionId_VersionId]
ON [dbo].[WorkflowDefinitions] (
  [DefinitionId] ASC,
  [Version] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowDefinition_IsLatest]
ON [dbo].[WorkflowDefinitions] (
  [IsLatest] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowDefinition_IsPublished]
ON [dbo].[WorkflowDefinitions] (
  [IsPublished] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowDefinition_Name]
ON [dbo].[WorkflowDefinitions] (
  [Name] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowDefinition_Tag]
ON [dbo].[WorkflowDefinitions] (
  [Tag] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowDefinition_TenantId]
ON [dbo].[WorkflowDefinitions] (
  [TenantId] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowDefinition_Version]
ON [dbo].[WorkflowDefinitions] (
  [Version] ASC
)


-- ----------------------------
-- Primary Key structure for table workflowdefinitions
-- ----------------------------
ALTER TABLE [dbo].[WorkflowDefinitions] ADD PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


-- ----------------------------
-- Indexes structure for table workflowexecutionlogrecords
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_WorkflowExecutionLogRecord_ActivityId]
ON [dbo].[WorkflowExecutionLogRecords] (
  [ActivityId] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowExecutionLogRecord_ActivityType]
ON [dbo].[WorkflowExecutionLogRecords] (
  [ActivityType] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowExecutionLogRecord_TenantId]
ON [dbo].[WorkflowExecutionLogRecords] (
  [TenantId] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowExecutionLogRecord_Timestamp]
ON [dbo].[WorkflowExecutionLogRecords] (
  [Timestamp] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowExecutionLogRecord_WorkflowInstanceId]
ON [dbo].[WorkflowExecutionLogRecords] (
  [WorkflowInstanceId] ASC
)


-- ----------------------------
-- Primary Key structure for table workflowexecutionlogrecords
-- ----------------------------
ALTER TABLE [dbo].[WorkflowExecutionLogRecords] ADD PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


-- ----------------------------
-- Indexes structure for table workflowinstances
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_WorkflowInstance_ContextId]
ON [dbo].[WorkflowInstances] (
  [ContextId] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowInstance_ContextType]
ON [dbo].[WorkflowInstances] (
  [ContextType] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowInstance_CorrelationId]
ON [dbo].[WorkflowInstances] (
  [CorrelationId] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowInstance_CreatedAt]
ON [dbo].[WorkflowInstances] (
  [CreatedAt] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowInstance_DefinitionId]
ON [dbo].[WorkflowInstances] (
  [DefinitionId] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowInstance_FaultedAt]
ON [dbo].[WorkflowInstances] (
  [FaultedAt] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowInstance_FinishedAt]
ON [dbo].[WorkflowInstances] (
  [FinishedAt] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowInstance_LastExecutedAt]
ON [dbo].[WorkflowInstances] (
  [LastExecutedAt] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowInstance_Name]
ON [dbo].[WorkflowInstances] (
  [Name] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowInstance_TenantId]
ON [dbo].[WorkflowInstances] (
  [TenantId] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowInstance_WorkflowStatus]
ON [dbo].[WorkflowInstances] (
  [WorkflowStatus] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowInstance_WorkflowStatus_DefinitionId]
ON [dbo].[WorkflowInstances] (
  [WorkflowStatus] ASC,
  [DefinitionId] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowInstance_WorkflowStatus_DefinitionId_Version]
ON [dbo].[WorkflowInstances] (
  [WorkflowStatus] ASC,
  [DefinitionId] ASC,
  [Version] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowInstance_DefinitionVersionId]
ON [dbo].[WorkflowInstances] (
  [DefinitionVersionId] ASC
)


-- ----------------------------
-- Primary Key structure for table workflowinstances
-- ----------------------------
ALTER TABLE [dbo].[WorkflowInstances] ADD PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)


-- ----------------------------
-- Indexes structure for table workflowsettings
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_WorkflowSetting_Key]
ON [dbo].[WorkflowSettings] (
  [Key] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowSetting_Value]
ON [dbo].[WorkflowSettings] (
  [Value] ASC
)

CREATE NONCLUSTERED INDEX [IX_WorkflowSetting_WorkflowBlueprintId]
ON [dbo].[WorkflowSettings] (
  [WorkflowBlueprintId] ASC
)


-- ----------------------------
-- Primary Key structure for table workflowsettings
-- ----------------------------
ALTER TABLE [dbo].[WorkflowSettings] ADD PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES 
(N'20210523093427_Initial', '5.0.10'),
(N'20210611200027_Update21', '5.0.10'),
(N'20210923112211_Update23', '5.0.10'),
(N'20211215100204_Update24', '5.0.10'),
(N'20220120170050_Update241', '5.0.10'),
(N'20220120204150_Update25', '5.0.10'),
(N'20220512203646_Update28', '5.0.10'),
(N'20210604065041_Initial', '5.0.10'),
(N'20210730112043_Initial', '5.0.10');
