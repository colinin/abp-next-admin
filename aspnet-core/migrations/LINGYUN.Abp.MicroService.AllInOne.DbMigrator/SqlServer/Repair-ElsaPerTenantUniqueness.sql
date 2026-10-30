-- Repairs the Elsa 3.9 SQL Server migrations that generate:
--   THROW 50001, CONCAT(...), 1;
-- SQL Server only accepts a literal or variable as the THROW message, so those
-- migrations always fail with "Incorrect syntax near 'CONCAT'".
--
-- The script is idempotent. Run it against the Elsa database before starting
-- the service when upgrading an existing SQL Server database.

SET XACT_ABORT ON;
BEGIN TRANSACTION;

/* -------------------------------------------------------------------------
 * 20260913133317_PerTenantLabelUniqueness
 * ---------------------------------------------------------------------- */

IF EXISTS (
    SELECT 1
    FROM [Elsa].[Labels]
    GROUP BY [TenantId], [NormalizedName]
    HAVING COUNT(*) > 1
)
BEGIN
    THROW 50001, N'Cannot create unique index IX_Label_TenantId_NormalizedName because duplicate (TenantId, NormalizedName) labels exist.', 1;
END;

IF COL_LENGTH('[Elsa].[Labels]', 'TenantId') IS NOT NULL
BEGIN
    ALTER TABLE [Elsa].[Labels] ALTER COLUMN [TenantId] nvarchar(400) NULL;
    ALTER TABLE [Elsa].[Labels] ALTER COLUMN [NormalizedName] nvarchar(400) NOT NULL;
END;

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE [name] = 'IX_Label_TenantId_NormalizedName'
      AND [object_id] = OBJECT_ID('[Elsa].[Labels]')
)
BEGIN
    CREATE UNIQUE INDEX [IX_Label_TenantId_NormalizedName]
        ON [Elsa].[Labels] ([TenantId], [NormalizedName]);
END;

IF NOT EXISTS (
    SELECT 1
    FROM [Elsa].[__EFMigrationsHistory]
    WHERE [MigrationId] = '20260913133317_PerTenantLabelUniqueness'
)
BEGIN
    INSERT INTO [Elsa].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('20260913133317_PerTenantLabelUniqueness', '10.0.11');
END;

/* -------------------------------------------------------------------------
 * 20260914120000_SecretDefaultTenantUniqueness
 * ---------------------------------------------------------------------- */

IF EXISTS (
    SELECT 1
    FROM [Elsa].[Secrets]
    GROUP BY [TenantId], [NormalizedName]
    HAVING COUNT(*) > 1
)
BEGIN
    THROW 50001, N'Cannot create unique index IX_Secret_TenantId_NormalizedName because duplicate (TenantId, NormalizedName) secrets exist.', 1;
END;

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE [name] = 'IX_Secret_TenantId_NormalizedName'
      AND [object_id] = OBJECT_ID('[Elsa].[Secrets]')
)
BEGIN
    CREATE UNIQUE INDEX [IX_Secret_TenantId_NormalizedName]
        ON [Elsa].[Secrets] ([TenantId], [NormalizedName]);
END;

IF NOT EXISTS (
    SELECT 1
    FROM [Elsa].[__EFMigrationsHistory]
    WHERE [MigrationId] = '20260914120000_SecretDefaultTenantUniqueness'
)
BEGIN
    INSERT INTO [Elsa].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('20260914120000_SecretDefaultTenantUniqueness', '10.0.11');
END;

COMMIT TRANSACTION;
