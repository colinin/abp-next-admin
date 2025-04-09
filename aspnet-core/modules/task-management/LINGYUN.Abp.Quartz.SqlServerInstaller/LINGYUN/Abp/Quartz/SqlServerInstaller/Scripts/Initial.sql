-- this script is for SQL Server and Azure SQL

-- This initializes the database to pristine for Quartz, by first removing any existing Quartz tables
-- and then recreating them from scratch.
-- Should you only require it to create the tables, set @DropDb to 0.

USE [${DataBase}];

DECLARE @DropDb BIT = 1; -- Set this to 0 to skip DROP statements, 1 to include them

IF @DropDb = 1
BEGIN
    -- drop indexes if they exist and rebuild if current ones
    DROP INDEX IF EXISTS [IDX_${TablePrefix}T_J] ON [dbo].[${TablePrefix}TRIGGERS];
    DROP INDEX IF EXISTS [IDX_${TablePrefix}T_JG] ON [dbo].[${TablePrefix}TRIGGERS];
    DROP INDEX IF EXISTS [IDX_${TablePrefix}T_C] ON [dbo].[${TablePrefix}TRIGGERS];
    DROP INDEX IF EXISTS [IDX_${TablePrefix}T_G] ON [dbo].[${TablePrefix}TRIGGERS];
    DROP INDEX IF EXISTS [IDX_${TablePrefix}T_G_J] ON [dbo].[${TablePrefix}TRIGGERS];
    DROP INDEX IF EXISTS [IDX_${TablePrefix}T_STATE] ON [dbo].[${TablePrefix}TRIGGERS];
    DROP INDEX IF EXISTS [IDX_${TablePrefix}T_N_STATE] ON [dbo].[${TablePrefix}TRIGGERS];
    DROP INDEX IF EXISTS [IDX_${TablePrefix}T_N_G_STATE] ON [dbo].[${TablePrefix}TRIGGERS];
    DROP INDEX IF EXISTS [IDX_${TablePrefix}T_NEXT_FIRE_TIME] ON [dbo].[${TablePrefix}TRIGGERS];
    DROP INDEX IF EXISTS [IDX_${TablePrefix}T_NFT_ST] ON [dbo].[${TablePrefix}TRIGGERS];
    DROP INDEX IF EXISTS [IDX_${TablePrefix}T_NFT_MISFIRE] ON [dbo].[${TablePrefix}TRIGGERS];
    DROP INDEX IF EXISTS [IDX_${TablePrefix}T_NFT_ST_MISFIRE] ON [dbo].[${TablePrefix}TRIGGERS];
    DROP INDEX IF EXISTS [IDX_${TablePrefix}T_NFT_ST_MISFIRE_GRP] ON [dbo].[${TablePrefix}TRIGGERS];
    DROP INDEX IF EXISTS [IDX_${TablePrefix}FT_TRIG_INST_NAME] ON [dbo].[${TablePrefix}FIRED_TRIGGERS];
    DROP INDEX IF EXISTS [IDX_${TablePrefix}FT_INST_JOB_REQ_RCVRY] ON [dbo].[${TablePrefix}FIRED_TRIGGERS];
    DROP INDEX IF EXISTS [IDX_${TablePrefix}FT_J_G] ON [dbo].[${TablePrefix}FIRED_TRIGGERS];
    DROP INDEX IF EXISTS [IDX_${TablePrefix}FT_JG] ON [dbo].[${TablePrefix}FIRED_TRIGGERS];
    DROP INDEX IF EXISTS [IDX_${TablePrefix}FT_T_G] ON [dbo].[${TablePrefix}FIRED_TRIGGERS];
    DROP INDEX IF EXISTS [IDX_${TablePrefix}FT_TG] ON [dbo].[${TablePrefix}FIRED_TRIGGERS];
    DROP INDEX IF EXISTS [IDX_${TablePrefix}FT_G_J] ON [dbo].[${TablePrefix}FIRED_TRIGGERS];
    DROP INDEX IF EXISTS [IDX_${TablePrefix}FT_G_T] ON [dbo].[${TablePrefix}FIRED_TRIGGERS];

    IF OBJECT_ID(N'[dbo].[FK_${TablePrefix}TRIGGERS_${TablePrefix}JOB_DETAILS]', N'F') IS NOT NULL
    ALTER TABLE [dbo].[${TablePrefix}TRIGGERS] DROP CONSTRAINT [FK_${TablePrefix}TRIGGERS_${TablePrefix}JOB_DETAILS];
    
    IF OBJECT_ID(N'[dbo].[FK_${TablePrefix}CRON_TRIGGERS_${TablePrefix}TRIGGERS]', N'F') IS NOT NULL
    ALTER TABLE [dbo].[${TablePrefix}CRON_TRIGGERS] DROP CONSTRAINT [FK_${TablePrefix}CRON_TRIGGERS_${TablePrefix}TRIGGERS];
    
    IF OBJECT_ID(N'[dbo].[FK_${TablePrefix}SIMPLE_TRIGGERS_${TablePrefix}TRIGGERS]', N'F') IS NOT NULL
    ALTER TABLE [dbo].[${TablePrefix}SIMPLE_TRIGGERS] DROP CONSTRAINT [FK_${TablePrefix}SIMPLE_TRIGGERS_${TablePrefix}TRIGGERS];
    
    IF OBJECT_ID(N'[dbo].[FK_${TablePrefix}SIMPROP_TRIGGERS_${TablePrefix}TRIGGERS]', N'F') IS NOT NULL
    ALTER TABLE [dbo].[${TablePrefix}SIMPROP_TRIGGERS] DROP CONSTRAINT [FK_${TablePrefix}SIMPROP_TRIGGERS_${TablePrefix}TRIGGERS];
    
    IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_${TablePrefix}JOB_LISTENERS_${TablePrefix}JOB_DETAILS]') AND parent_object_id = OBJECT_ID(N'[dbo].[${TablePrefix}JOB_LISTENERS]'))
    ALTER TABLE [dbo].[${TablePrefix}JOB_LISTENERS] DROP CONSTRAINT [FK_${TablePrefix}JOB_LISTENERS_${TablePrefix}JOB_DETAILS];
    
    IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_${TablePrefix}TRIGGER_LISTENERS_${TablePrefix}TRIGGERS]') AND parent_object_id = OBJECT_ID(N'[dbo].[${TablePrefix}TRIGGER_LISTENERS]'))
    ALTER TABLE [dbo].[${TablePrefix}TRIGGER_LISTENERS] DROP CONSTRAINT [FK_${TablePrefix}TRIGGER_LISTENERS_${TablePrefix}TRIGGERS];
    
    IF OBJECT_ID(N'[dbo].[${TablePrefix}CALENDARS]', N'U') IS NOT NULL
    DROP TABLE [dbo].[${TablePrefix}CALENDARS];
    
    IF OBJECT_ID(N'[dbo].[${TablePrefix}CRON_TRIGGERS]', N'U') IS NOT NULL
    DROP TABLE [dbo].[${TablePrefix}CRON_TRIGGERS];
    
    IF OBJECT_ID(N'[dbo].[${TablePrefix}BLOB_TRIGGERS]', N'U') IS NOT NULL
    DROP TABLE [dbo].[${TablePrefix}BLOB_TRIGGERS];
    
    IF OBJECT_ID(N'[dbo].[${TablePrefix}FIRED_TRIGGERS]', N'U') IS NOT NULL
    DROP TABLE [dbo].[${TablePrefix}FIRED_TRIGGERS];
    
    IF OBJECT_ID(N'[dbo].[${TablePrefix}PAUSED_TRIGGER_GRPS]', N'U') IS NOT NULL
    DROP TABLE [dbo].[${TablePrefix}PAUSED_TRIGGER_GRPS];
    
    IF  OBJECT_ID(N'[dbo].[${TablePrefix}JOB_LISTENERS]', N'U') IS NOT NULL
    DROP TABLE [dbo].[${TablePrefix}JOB_LISTENERS];
    
    IF OBJECT_ID(N'[dbo].[${TablePrefix}SCHEDULER_STATE]', N'U') IS NOT NULL
    DROP TABLE [dbo].[${TablePrefix}SCHEDULER_STATE];
    
    IF OBJECT_ID(N'[dbo].[${TablePrefix}LOCKS]', N'U') IS NOT NULL
    DROP TABLE [dbo].[${TablePrefix}LOCKS];

    IF OBJECT_ID(N'[dbo].[${TablePrefix}TRIGGER_LISTENERS]', N'U') IS NOT NULL
    DROP TABLE [dbo].[${TablePrefix}TRIGGER_LISTENERS];
    
    IF OBJECT_ID(N'[dbo].[${TablePrefix}JOB_DETAILS]', N'U') IS NOT NULL
    DROP TABLE [dbo].[${TablePrefix}JOB_DETAILS];
    
    IF OBJECT_ID(N'[dbo].[${TablePrefix}SIMPLE_TRIGGERS]', N'U') IS NOT NULL
    DROP TABLE [dbo].[${TablePrefix}SIMPLE_TRIGGERS];
    
    IF OBJECT_ID(N'[dbo].[${TablePrefix}SIMPROP_TRIGGERS]', N'U') IS NOT NULL
    DROP TABLE [dbo].[${TablePrefix}SIMPROP_TRIGGERS];
    
    IF OBJECT_ID(N'[dbo].[${TablePrefix}TRIGGERS]', N'U') IS NOT NULL
    DROP TABLE [dbo].[${TablePrefix}TRIGGERS];
END 

CREATE TABLE [dbo].[${TablePrefix}CALENDARS] (
  [SCHED_NAME] nvarchar(120) NOT NULL,
  [CALENDAR_NAME] nvarchar(200) NOT NULL,
  [CALENDAR] varbinary(max) NOT NULL
);

CREATE TABLE [dbo].[${TablePrefix}CRON_TRIGGERS] (
  [SCHED_NAME] nvarchar(120) NOT NULL,
  [TRIGGER_NAME] nvarchar(150) NOT NULL,
  [TRIGGER_GROUP] nvarchar(150) NOT NULL,
  [CRON_EXPRESSION] nvarchar(120) NOT NULL,
  [TIME_ZONE_ID] nvarchar(80) 
);

CREATE TABLE [dbo].[${TablePrefix}FIRED_TRIGGERS] (
  [SCHED_NAME] nvarchar(120) NOT NULL,
  [ENTRY_ID] nvarchar(140) NOT NULL,
  [TRIGGER_NAME] nvarchar(150) NOT NULL,
  [TRIGGER_GROUP] nvarchar(150) NOT NULL,
  [INSTANCE_NAME] nvarchar(200) NOT NULL,
  [FIRED_TIME] bigint NOT NULL,
  [SCHED_TIME] bigint NOT NULL,
  [PRIORITY] int NOT NULL,
  [STATE] nvarchar(16) NOT NULL,
  [JOB_NAME] nvarchar(150) NULL,
  [JOB_GROUP] nvarchar(150) NULL,
  [IS_NONCONCURRENT] bit NULL,
  [REQUESTS_RECOVERY] bit NULL 
);

CREATE TABLE [dbo].[${TablePrefix}PAUSED_TRIGGER_GRPS] (
  [SCHED_NAME] nvarchar(120) NOT NULL,
  [TRIGGER_GROUP] nvarchar(150) NOT NULL 
);

CREATE TABLE [dbo].[${TablePrefix}SCHEDULER_STATE] (
  [SCHED_NAME] nvarchar(120) NOT NULL,
  [INSTANCE_NAME] nvarchar(200) NOT NULL,
  [LAST_CHECKIN_TIME] bigint NOT NULL,
  [CHECKIN_INTERVAL] bigint NOT NULL
);

CREATE TABLE [dbo].[${TablePrefix}LOCKS] (
  [SCHED_NAME] nvarchar(120) NOT NULL,
  [LOCK_NAME] nvarchar(40) NOT NULL 
);

CREATE TABLE [dbo].[${TablePrefix}JOB_DETAILS] (
  [SCHED_NAME] nvarchar(120) NOT NULL,
  [JOB_NAME] nvarchar(150) NOT NULL,
  [JOB_GROUP] nvarchar(150) NOT NULL,
  [DESCRIPTION] nvarchar(250) NULL,
  [JOB_CLASS_NAME] nvarchar(512) NOT NULL,
  [IS_DURABLE] bit NOT NULL,
  [IS_NONCONCURRENT] bit NOT NULL,
  [IS_UPDATE_DATA] bit NOT NULL,
  [REQUESTS_RECOVERY] bit NOT NULL,
  [JOB_DATA] varbinary(max) NULL
);

CREATE TABLE [dbo].[${TablePrefix}SIMPLE_TRIGGERS] (
  [SCHED_NAME] nvarchar(120) NOT NULL,
  [TRIGGER_NAME] nvarchar(150) NOT NULL,
  [TRIGGER_GROUP] nvarchar(150) NOT NULL,
  [REPEAT_COUNT] int NOT NULL,
  [REPEAT_INTERVAL] bigint NOT NULL,
  [TIMES_TRIGGERED] int NOT NULL
);

CREATE TABLE [dbo].[${TablePrefix}SIMPROP_TRIGGERS] (
  [SCHED_NAME] nvarchar(120) NOT NULL,
  [TRIGGER_NAME] nvarchar(150) NOT NULL,
  [TRIGGER_GROUP] nvarchar(150) NOT NULL,
  [STR_PROP_1] nvarchar(512) NULL,
  [STR_PROP_2] nvarchar(512) NULL,
  [STR_PROP_3] nvarchar(512) NULL,
  [INT_PROP_1] int NULL,
  [INT_PROP_2] int NULL,
  [LONG_PROP_1] bigint NULL,
  [LONG_PROP_2] bigint NULL,
  [DEC_PROP_1] numeric(13,4) NULL,
  [DEC_PROP_2] numeric(13,4) NULL,
  [BOOL_PROP_1] bit NULL,
  [BOOL_PROP_2] bit NULL,
  [TIME_ZONE_ID] nvarchar(80) NULL 
);

CREATE TABLE [dbo].[${TablePrefix}BLOB_TRIGGERS] (
  [SCHED_NAME] nvarchar(120) NOT NULL,
  [TRIGGER_NAME] nvarchar(150) NOT NULL,
  [TRIGGER_GROUP] nvarchar(150) NOT NULL,
  [BLOB_DATA] varbinary(max) NULL
);

CREATE TABLE [dbo].[${TablePrefix}TRIGGERS] (
  [SCHED_NAME] nvarchar(120) NOT NULL,
  [TRIGGER_NAME] nvarchar(150) NOT NULL,
  [TRIGGER_GROUP] nvarchar(150) NOT NULL,
  [JOB_NAME] nvarchar(150) NOT NULL,
  [JOB_GROUP] nvarchar(150) NOT NULL,
  [DESCRIPTION] nvarchar(250) NULL,
  [NEXT_FIRE_TIME] bigint NULL,
  [PREV_FIRE_TIME] bigint NULL,
  [PRIORITY] int NULL,
  [TRIGGER_STATE] nvarchar(16) NOT NULL,
  [TRIGGER_TYPE] nvarchar(8) NOT NULL,
  [START_TIME] bigint NOT NULL,
  [END_TIME] bigint NULL,
  [CALENDAR_NAME] nvarchar(200) NULL,
  [MISFIRE_INSTR] int NULL,
  [JOB_DATA] varbinary(max) NULL
);

ALTER TABLE [dbo].[${TablePrefix}CALENDARS] WITH NOCHECK ADD
  CONSTRAINT [PK_${TablePrefix}CALENDARS] PRIMARY KEY  CLUSTERED
  (
    [SCHED_NAME],
    [CALENDAR_NAME]
  );

ALTER TABLE [dbo].[${TablePrefix}CRON_TRIGGERS] WITH NOCHECK ADD
  CONSTRAINT [PK_${TablePrefix}CRON_TRIGGERS] PRIMARY KEY  CLUSTERED
  (
    [SCHED_NAME],
    [TRIGGER_NAME],
    [TRIGGER_GROUP]
  );

ALTER TABLE [dbo].[${TablePrefix}FIRED_TRIGGERS] WITH NOCHECK ADD
  CONSTRAINT [PK_${TablePrefix}FIRED_TRIGGERS] PRIMARY KEY  CLUSTERED
  (
    [SCHED_NAME],
    [ENTRY_ID]
  );

ALTER TABLE [dbo].[${TablePrefix}PAUSED_TRIGGER_GRPS] WITH NOCHECK ADD
  CONSTRAINT [PK_${TablePrefix}PAUSED_TRIGGER_GRPS] PRIMARY KEY  CLUSTERED
  (
    [SCHED_NAME],
    [TRIGGER_GROUP]
  );

ALTER TABLE [dbo].[${TablePrefix}SCHEDULER_STATE] WITH NOCHECK ADD
  CONSTRAINT [PK_${TablePrefix}SCHEDULER_STATE] PRIMARY KEY  CLUSTERED
  (
    [SCHED_NAME],
    [INSTANCE_NAME]
  );

ALTER TABLE [dbo].[${TablePrefix}LOCKS] WITH NOCHECK ADD
  CONSTRAINT [PK_${TablePrefix}LOCKS] PRIMARY KEY  CLUSTERED
  (
    [SCHED_NAME],
    [LOCK_NAME]
  );

ALTER TABLE [dbo].[${TablePrefix}JOB_DETAILS] WITH NOCHECK ADD
  CONSTRAINT [PK_${TablePrefix}JOB_DETAILS] PRIMARY KEY  CLUSTERED
  (
    [SCHED_NAME],
    [JOB_NAME],
    [JOB_GROUP]
  );

ALTER TABLE [dbo].[${TablePrefix}SIMPLE_TRIGGERS] WITH NOCHECK ADD
  CONSTRAINT [PK_${TablePrefix}SIMPLE_TRIGGERS] PRIMARY KEY  CLUSTERED
  (
    [SCHED_NAME],
    [TRIGGER_NAME],
    [TRIGGER_GROUP]
  );

ALTER TABLE [dbo].[${TablePrefix}SIMPROP_TRIGGERS] WITH NOCHECK ADD
  CONSTRAINT [PK_${TablePrefix}SIMPROP_TRIGGERS] PRIMARY KEY  CLUSTERED
  (
    [SCHED_NAME],
    [TRIGGER_NAME],
    [TRIGGER_GROUP]
  );

ALTER TABLE [dbo].[${TablePrefix}TRIGGERS] WITH NOCHECK ADD
  CONSTRAINT [PK_${TablePrefix}TRIGGERS] PRIMARY KEY  CLUSTERED
  (
    [SCHED_NAME],
    [TRIGGER_NAME],
    [TRIGGER_GROUP]
  );

ALTER TABLE [dbo].[${TablePrefix}BLOB_TRIGGERS] WITH NOCHECK ADD
  CONSTRAINT [PK_${TablePrefix}BLOB_TRIGGERS] PRIMARY KEY  CLUSTERED
  (
    [SCHED_NAME],
    [TRIGGER_NAME],
    [TRIGGER_GROUP]
  );

ALTER TABLE [dbo].[${TablePrefix}CRON_TRIGGERS] ADD
  CONSTRAINT [FK_${TablePrefix}CRON_TRIGGERS_${TablePrefix}TRIGGERS] FOREIGN KEY
  (
    [SCHED_NAME],
    [TRIGGER_NAME],
    [TRIGGER_GROUP]
  ) REFERENCES [dbo].[${TablePrefix}TRIGGERS] (
    [SCHED_NAME],
    [TRIGGER_NAME],
    [TRIGGER_GROUP]
  ) ON DELETE CASCADE;

ALTER TABLE [dbo].[${TablePrefix}SIMPLE_TRIGGERS] ADD
  CONSTRAINT [FK_${TablePrefix}SIMPLE_TRIGGERS_${TablePrefix}TRIGGERS] FOREIGN KEY
  (
    [SCHED_NAME],
    [TRIGGER_NAME],
    [TRIGGER_GROUP]
  ) REFERENCES [dbo].[${TablePrefix}TRIGGERS] (
    [SCHED_NAME],
    [TRIGGER_NAME],
    [TRIGGER_GROUP]
  ) ON DELETE CASCADE;

ALTER TABLE [dbo].[${TablePrefix}SIMPROP_TRIGGERS] ADD
  CONSTRAINT [FK_${TablePrefix}SIMPROP_TRIGGERS_${TablePrefix}TRIGGERS] FOREIGN KEY
  (
	[SCHED_NAME],
    [TRIGGER_NAME],
    [TRIGGER_GROUP]
  ) REFERENCES [dbo].[${TablePrefix}TRIGGERS] (
    [SCHED_NAME],
    [TRIGGER_NAME],
    [TRIGGER_GROUP]
  ) ON DELETE CASCADE;

ALTER TABLE [dbo].[${TablePrefix}TRIGGERS] ADD
  CONSTRAINT [FK_${TablePrefix}TRIGGERS_${TablePrefix}JOB_DETAILS] FOREIGN KEY
  (
    [SCHED_NAME],
    [JOB_NAME],
    [JOB_GROUP]
  ) REFERENCES [dbo].[${TablePrefix}JOB_DETAILS] (
    [SCHED_NAME],
    [JOB_NAME],
    [JOB_GROUP]
  );

-- Create indexes
CREATE INDEX [IDX_${TablePrefix}T_G_J]                 ON [dbo].[${TablePrefix}TRIGGERS](SCHED_NAME, JOB_GROUP, JOB_NAME);
CREATE INDEX [IDX_${TablePrefix}T_C]                   ON [dbo].[${TablePrefix}TRIGGERS](SCHED_NAME, CALENDAR_NAME);

CREATE INDEX [IDX_${TablePrefix}T_N_G_STATE]           ON [dbo].[${TablePrefix}TRIGGERS](SCHED_NAME, TRIGGER_GROUP, TRIGGER_STATE);
CREATE INDEX [IDX_${TablePrefix}T_STATE]               ON [dbo].[${TablePrefix}TRIGGERS](SCHED_NAME, TRIGGER_STATE);
CREATE INDEX [IDX_${TablePrefix}T_N_STATE]             ON [dbo].[${TablePrefix}TRIGGERS](SCHED_NAME, TRIGGER_NAME, TRIGGER_GROUP, TRIGGER_STATE);
CREATE INDEX [IDX_${TablePrefix}T_NEXT_FIRE_TIME]      ON [dbo].[${TablePrefix}TRIGGERS](SCHED_NAME, NEXT_FIRE_TIME);
CREATE INDEX [IDX_${TablePrefix}T_NFT_ST]              ON [dbo].[${TablePrefix}TRIGGERS](SCHED_NAME, TRIGGER_STATE, NEXT_FIRE_TIME);
CREATE INDEX [IDX_${TablePrefix}T_NFT_ST_MISFIRE]      ON [dbo].[${TablePrefix}TRIGGERS](SCHED_NAME, MISFIRE_INSTR, NEXT_FIRE_TIME, TRIGGER_STATE);
CREATE INDEX [IDX_${TablePrefix}T_NFT_ST_MISFIRE_GRP]  ON [dbo].[${TablePrefix}TRIGGERS](SCHED_NAME, MISFIRE_INSTR, NEXT_FIRE_TIME, TRIGGER_GROUP, TRIGGER_STATE);

CREATE INDEX [IDX_${TablePrefix}FT_INST_JOB_REQ_RCVRY] ON [dbo].[${TablePrefix}FIRED_TRIGGERS](SCHED_NAME, INSTANCE_NAME, REQUESTS_RECOVERY);
CREATE INDEX [IDX_${TablePrefix}FT_G_J]                ON [dbo].[${TablePrefix}FIRED_TRIGGERS](SCHED_NAME, JOB_GROUP, JOB_NAME);
CREATE INDEX [IDX_${TablePrefix}FT_G_T]                ON [dbo].[${TablePrefix}FIRED_TRIGGERS](SCHED_NAME, TRIGGER_GROUP, TRIGGER_NAME);