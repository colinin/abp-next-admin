START TRANSACTION;

/*

	Description: 	IdentityServer 4.1 升级脚本
	Author:				colin.in@foxmail.com
	CreateTime:		2020-12-23 09:00
	Remarks:			升级需谨慎，数据无价，请提前备份，涉及到多个表的重建
	Reference:		https://docs.abp.io/en/abp/latest/Migration-Guides/Abp-4_0
*/

DROP PROCEDURE IF EXISTS `PROC_DROP_COLUMN`;
DELIMITER //
CREATE PROCEDURE PROC_DROP_COLUMN(IN `TABLE_NAME_ARGUMENT` VARCHAR(255), IN `COLUMN_NAME_ARGUMENT` VARCHAR(255))
BEGIN
    IF EXISTS(
        SELECT * FROM information_schema.columns
        WHERE 
            table_schema    = DATABASE()     AND
            table_name      = TABLE_NAME_ARGUMENT      AND
            column_name = COLUMN_NAME_ARGUMENT)
    THEN
        SET @query = CONCAT('ALTER TABLE ', TABLE_NAME_ARGUMENT, ' DROP COLUMN ', COLUMN_NAME_ARGUMENT, ';');
        PREPARE stmt FROM @query; 
        EXECUTE stmt; 
        DEALLOCATE PREPARE stmt; 
    END IF; 
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS `PROC_DROP_FOREIGN_KEY`;
DELIMITER //
CREATE PROCEDURE PROC_DROP_FOREIGN_KEY(IN `TABLE_NAME_ARGUMENT` VARCHAR(255), IN `COLUMN_NAME_ARGUMENT` VARCHAR(255))
BEGIN
    IF EXISTS(
        SELECT * FROM information_schema.table_constraints
        WHERE 
            table_schema    = DATABASE()     AND
            table_name      = TABLE_NAME_ARGUMENT      AND
            constraint_name = COLUMN_NAME_ARGUMENT AND
            constraint_type = 'FOREIGN KEY')
    THEN
        SET @query = CONCAT('ALTER TABLE ', TABLE_NAME_ARGUMENT, ' DROP FOREIGN KEY ', COLUMN_NAME_ARGUMENT, ';');
        PREPARE stmt FROM @query; 
        EXECUTE stmt; 
        DEALLOCATE PREPARE stmt; 
    END IF; 
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS `PROC_DROP_INDEX`;
DELIMITER //
CREATE PROCEDURE PROC_DROP_INDEX(IN `TABLE_NAME_ARGUMENT` VARCHAR(255), IN `COLUMN_NAME_ARGUMENT` VARCHAR(255))
BEGIN
    IF EXISTS(
        SELECT * FROM information_schema.statistics
        WHERE 
            table_schema    = DATABASE()     AND
            table_name      = TABLE_NAME_ARGUMENT      AND
            index_name = COLUMN_NAME_ARGUMENT)
    THEN
        SET @query = CONCAT('ALTER TABLE ', TABLE_NAME_ARGUMENT, ' DROP INDEX ', COLUMN_NAME_ARGUMENT, ';');
        PREPARE stmt FROM @query; 
        EXECUTE stmt; 
        DEALLOCATE PREPARE stmt; 
    END IF; 
END //
DELIMITER ;


DROP PROCEDURE IF EXISTS `POMELO_BEFORE_DROP_PRIMARY_KEY`;
DELIMITER //
CREATE PROCEDURE `POMELO_BEFORE_DROP_PRIMARY_KEY`(IN `SCHEMA_NAME_ARGUMENT` VARCHAR(255), IN `TABLE_NAME_ARGUMENT` VARCHAR(255))
BEGIN
	DECLARE HAS_AUTO_INCREMENT_ID TINYINT(1);
	DECLARE PRIMARY_KEY_COLUMN_NAME VARCHAR(255);
	DECLARE PRIMARY_KEY_TYPE VARCHAR(255);
	DECLARE SQL_EXP VARCHAR(1000);
	SELECT COUNT(*)
		INTO HAS_AUTO_INCREMENT_ID
		FROM `information_schema`.`COLUMNS`
		WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
			AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
			AND `Extra` = 'auto_increment'
			AND `COLUMN_KEY` = 'PRI'
			LIMIT 1;
	IF HAS_AUTO_INCREMENT_ID THEN
		SELECT `COLUMN_TYPE`
			INTO PRIMARY_KEY_TYPE
			FROM `information_schema`.`COLUMNS`
			WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
				AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
				AND `COLUMN_KEY` = 'PRI'
			LIMIT 1;
		SELECT `COLUMN_NAME`
			INTO PRIMARY_KEY_COLUMN_NAME
			FROM `information_schema`.`COLUMNS`
			WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
				AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
				AND `COLUMN_KEY` = 'PRI'
			LIMIT 1;
		SET SQL_EXP = CONCAT('ALTER TABLE `', (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA())), '`.`', TABLE_NAME_ARGUMENT, '` MODIFY COLUMN `', PRIMARY_KEY_COLUMN_NAME, '` ', PRIMARY_KEY_TYPE, ' NOT NULL;');
		SET @SQL_EXP = SQL_EXP;
		PREPARE SQL_EXP_EXECUTE FROM @SQL_EXP;
		EXECUTE SQL_EXP_EXECUTE;
		DEALLOCATE PREPARE SQL_EXP_EXECUTE;
	END IF;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS `POMELO_AFTER_ADD_PRIMARY_KEY`;
DELIMITER //
CREATE PROCEDURE `POMELO_AFTER_ADD_PRIMARY_KEY`(IN `SCHEMA_NAME_ARGUMENT` VARCHAR(255), IN `TABLE_NAME_ARGUMENT` VARCHAR(255), IN `COLUMN_NAME_ARGUMENT` VARCHAR(255))
BEGIN
	DECLARE HAS_AUTO_INCREMENT_ID INT(11);
	DECLARE PRIMARY_KEY_COLUMN_NAME VARCHAR(255);
	DECLARE PRIMARY_KEY_TYPE VARCHAR(255);
	DECLARE SQL_EXP VARCHAR(1000);
	SELECT COUNT(*)
		INTO HAS_AUTO_INCREMENT_ID
		FROM `information_schema`.`COLUMNS`
		WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
			AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
			AND `COLUMN_NAME` = COLUMN_NAME_ARGUMENT
			AND `COLUMN_TYPE` LIKE '%int%'
			AND `COLUMN_KEY` = 'PRI';
	IF HAS_AUTO_INCREMENT_ID THEN
		SELECT `COLUMN_TYPE`
			INTO PRIMARY_KEY_TYPE
			FROM `information_schema`.`COLUMNS`
			WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
				AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
				AND `COLUMN_NAME` = COLUMN_NAME_ARGUMENT
				AND `COLUMN_TYPE` LIKE '%int%'
				AND `COLUMN_KEY` = 'PRI';
		SELECT `COLUMN_NAME`
			INTO PRIMARY_KEY_COLUMN_NAME
			FROM `information_schema`.`COLUMNS`
			WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
				AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
				AND `COLUMN_NAME` = COLUMN_NAME_ARGUMENT
				AND `COLUMN_TYPE` LIKE '%int%'
				AND `COLUMN_KEY` = 'PRI';
		SET SQL_EXP = CONCAT('ALTER TABLE `', (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA())), '`.`', TABLE_NAME_ARGUMENT, '` MODIFY COLUMN `', PRIMARY_KEY_COLUMN_NAME, '` ', PRIMARY_KEY_TYPE, ' NOT NULL AUTO_INCREMENT;');
		SET @SQL_EXP = SQL_EXP;
		PREPARE SQL_EXP_EXECUTE FROM @SQL_EXP;
		EXECUTE SQL_EXP_EXECUTE;
		DEALLOCATE PREPARE SQL_EXP_EXECUTE;
	END IF;
END //
DELIMITER ;

ALTER TABLE `IdentityServerApiClaims` DROP FOREIGN KEY `FK_IdentityServerApiClaims_IdentityServerApiResources_ApiResour~`;
-- CALL PROC_DROP_FOREIGN_KEY('IdentityServerApiClaims', 'FK_IdentityServerApiClaims_IdentityServerApiResources_ApiResour~');

ALTER TABLE `IdentityServerApiScopes` DROP FOREIGN KEY `FK_IdentityServerApiScopes_IdentityServerApiResources_ApiResour~`;
-- CALL PROC_DROP_FOREIGN_KEY('IdentityServerApiScopes', 'FK_IdentityServerApiScopes_IdentityServerApiResources_ApiResour~');

DROP TABLE IF EXISTS `IdentityServerApiSecrets`;

DROP TABLE IF EXISTS `IdentityServerIdentityClaims`;

CALL PROC_DROP_INDEX('IdentityServerDeviceFlowCodes', 'IX_IdentityServerDeviceFlowCodes_UserCode');

ALTER TABLE `IdentityServerClientProperties` DROP FOREIGN KEY `FK_IdentityServerClientProperties_IdentityServerClients_ClientId`;
-- CALL PROC_DROP_FOREIGN_KEY('IdentityServerClientProperties', 'FK_IdentityServerClientProperties_IdentityServerClients_ClientId');

CALL POMELO_BEFORE_DROP_PRIMARY_KEY(NULL, 'IdentityServerClientProperties');
ALTER TABLE `IdentityServerClientProperties` DROP PRIMARY KEY;

ALTER TABLE `IdentityServerClientProperties` ADD CONSTRAINT `FK_IdentityServerClientProperties_IdentityServerClients_ClientId` FOREIGN KEY (`ClientId`) REFERENCES `IdentityServerClients` (`Id`) ON DELETE CASCADE;


CALL PROC_DROP_COLUMN('IdentityServerIdentityResources', 'Properties');

CALL PROC_DROP_COLUMN('IdentityServerApiResources', 'Properties');

ALTER TABLE `IdentityServerApiClaims` RENAME `IdentityServerApiResourceClaims`;

ALTER TABLE `IdentityServerApiScopes` CHANGE COLUMN `ApiResourceId` `Id` CHAR(36);

ALTER TABLE `IdentityServerApiScopeClaims` CHANGE COLUMN `ApiResourceId` `ApiScopeId` CHAR(36);

-- 将要删除此表的所有数据，请注意
TRUNCATE TABLE `IdentityServerApiScopeClaims`;
-- 删除列之前需要重建外键约束
ALTER TABLE `IdentityServerApiScopeClaims` DROP FOREIGN KEY `FK_IdentityServerApiScopeClaims_IdentityServerApiScopes_ApiReso~`;
ALTER TABLE `IdentityServerApiScopeClaims` ADD CONSTRAINT `FK_IdentityServerApiScopeClaims_IdentityServerApiScopes_ApiReso~` FOREIGN KEY (`ApiScopeId`) REFERENCES `IdentityServerApiScopes` (`Id`) ON DELETE CASCADE;
CALL PROC_DROP_COLUMN('IdentityServerApiScopeClaims', 'Name');

ALTER TABLE `IdentityServerPersistedGrants` ADD `ConsumedTime` datetime(6) NULL;

ALTER TABLE `IdentityServerPersistedGrants` ADD `Description` varchar(200) CHARACTER SET utf8mb4 NULL;

ALTER TABLE `IdentityServerPersistedGrants` ADD `SessionId` varchar(100) CHARACTER SET utf8mb4 NULL;

ALTER TABLE `IdentityServerDeviceFlowCodes` ADD `Description` varchar(200) CHARACTER SET utf8mb4 NULL;

ALTER TABLE `IdentityServerDeviceFlowCodes` ADD `SessionId` varchar(100) CHARACTER SET utf8mb4 NULL;

ALTER TABLE `IdentityServerClients` ADD `AllowedIdentityTokenSigningAlgorithms` varchar(100) CHARACTER SET utf8mb4 NULL;

ALTER TABLE `IdentityServerClients` ADD `RequireRequestObject` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `IdentityServerClientProperties` MODIFY COLUMN `Value` varchar(300) CHARACTER SET utf8mb4 NOT NULL;

ALTER TABLE `IdentityServerApiScopes` ADD `ConcurrencyStamp` varchar(40) CHARACTER SET utf8mb4 NULL;

ALTER TABLE `IdentityServerApiScopes` ADD `CreationTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00';

ALTER TABLE `IdentityServerApiScopes` ADD `CreatorId` char(36) NULL;

ALTER TABLE `IdentityServerApiScopes` ADD `DeleterId` char(36) NULL;

ALTER TABLE `IdentityServerApiScopes` ADD `DeletionTime` datetime(6) NULL;

ALTER TABLE `IdentityServerApiScopes` ADD `Enabled` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `IdentityServerApiScopes` ADD `ExtraProperties` longtext CHARACTER SET utf8mb4 NULL;

ALTER TABLE `IdentityServerApiScopes` ADD `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `IdentityServerApiScopes` ADD `LastModificationTime` datetime(6) NULL;

ALTER TABLE `IdentityServerApiScopes` ADD `LastModifierId` char(36) NULL;

ALTER TABLE `IdentityServerApiResources` ADD `AllowedAccessTokenSigningAlgorithms` varchar(100) CHARACTER SET utf8mb4 NULL;

ALTER TABLE `IdentityServerApiResources` ADD `ShowInDiscoveryDocument` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `IdentityServerClientProperties` ADD CONSTRAINT `PK_IdentityServerClientProperties` PRIMARY KEY (`ClientId`, `Key`, `Value`);

CREATE TABLE `AbpLinkUsers` (
    `Id` char(36) NOT NULL,
    `SourceUserId` char(36) NOT NULL,
    `SourceTenantId` char(36) NULL,
    `TargetUserId` char(36) NOT NULL,
    `TargetTenantId` char(36) NULL,
    CONSTRAINT `PK_AbpLinkUsers` PRIMARY KEY (`Id`)
);

CREATE TABLE `IdentityServerApiResourceProperties` (
    `ApiResourceId` char(36) NOT NULL,
    `Key` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
    `Value` varchar(300) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_IdentityServerApiResourceProperties` PRIMARY KEY (`ApiResourceId`, `Key`, `Value`),
    CONSTRAINT `FK_IdentityServerApiResourceProperties_IdentityServerApiResourc~` FOREIGN KEY (`ApiResourceId`) REFERENCES `IdentityServerApiResources` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `IdentityServerApiResourceScopes` (
    `ApiResourceId` char(36) NOT NULL,
    `Scope` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_IdentityServerApiResourceScopes` PRIMARY KEY (`ApiResourceId`, `Scope`),
    CONSTRAINT `FK_IdentityServerApiResourceScopes_IdentityServerApiResources_A~` FOREIGN KEY (`ApiResourceId`) REFERENCES `IdentityServerApiResources` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `IdentityServerApiResourceSecrets` (
    `Type` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
    `Value` varchar(300) CHARACTER SET utf8mb4 NOT NULL,
    `ApiResourceId` char(36) NOT NULL,
    `Description` varchar(1000) CHARACTER SET utf8mb4 NULL,
    `Expiration` datetime(6) NULL,
    CONSTRAINT `PK_IdentityServerApiResourceSecrets` PRIMARY KEY (`ApiResourceId`, `Type`, `Value`),
    CONSTRAINT `FK_IdentityServerApiResourceSecrets_IdentityServerApiResources_~` FOREIGN KEY (`ApiResourceId`) REFERENCES `IdentityServerApiResources` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `IdentityServerApiScopeProperties` (
    `ApiScopeId` char(36) NOT NULL,
    `Key` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
    `Value` varchar(300) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_IdentityServerApiScopeProperties` PRIMARY KEY (`ApiScopeId`, `Key`, `Value`),
    CONSTRAINT `FK_IdentityServerApiScopeProperties_IdentityServerApiScopes_Api~` FOREIGN KEY (`ApiScopeId`) REFERENCES `IdentityServerApiScopes` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `IdentityServerIdentityResourceClaims` (
    `Type` varchar(200) CHARACTER SET utf8mb4 NOT NULL,
    `IdentityResourceId` char(36) NOT NULL,
    CONSTRAINT `PK_IdentityServerIdentityResourceClaims` PRIMARY KEY (`IdentityResourceId`, `Type`),
    CONSTRAINT `FK_IdentityServerIdentityResourceClaims_IdentityServerIdentityR~` FOREIGN KEY (`IdentityResourceId`) REFERENCES `IdentityServerIdentityResources` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `IdentityServerIdentityResourceProperties` (
    `IdentityResourceId` char(36) NOT NULL,
    `Key` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
    `Value` varchar(300) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_IdentityServerIdentityResourceProperties` PRIMARY KEY (`IdentityResourceId`, `Key`, `Value`),
    CONSTRAINT `FK_IdentityServerIdentityResourceProperties_IdentityServerIdent~` FOREIGN KEY (`IdentityResourceId`) REFERENCES `IdentityServerIdentityResources` (`Id`) ON DELETE CASCADE
);

CREATE INDEX `IX_IdentityServerPersistedGrants_SubjectId_SessionId_Type` ON `IdentityServerPersistedGrants` (`SubjectId`, `SessionId`, `Type`);

CREATE INDEX `IX_IdentityServerDeviceFlowCodes_UserCode` ON `IdentityServerDeviceFlowCodes` (`UserCode`);

CREATE UNIQUE INDEX `IX_AbpLinkUsers_SourceUserId_SourceTenantId_TargetUserId_Target~` ON `AbpLinkUsers` (`SourceUserId`, `SourceTenantId`, `TargetUserId`, `TargetTenantId`);

ALTER TABLE `IdentityServerApiResourceClaims` ADD CONSTRAINT `FK_IdentityServerApiResourceClaims_IdentityServerApiResources_A~` FOREIGN KEY (`ApiResourceId`) REFERENCES `IdentityServerApiResources` (`Id`) ON DELETE CASCADE;

ALTER TABLE `IdentityServerApiScopeClaims` ADD CONSTRAINT `FK_IdentityServerApiScopeClaims_IdentityServerApiScopes_ApiScop~` FOREIGN KEY (`ApiScopeId`) REFERENCES `IdentityServerApiScopes` (`Id`) ON DELETE CASCADE;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20201222101851_Upgrade-Abp-4.0.0', '5.0.0');

DROP PROCEDURE `POMELO_BEFORE_DROP_PRIMARY_KEY`;

DROP PROCEDURE `POMELO_AFTER_ADD_PRIMARY_KEY`;

DROP PROCEDURE `PROC_DROP_FOREIGN_KEY`;

DROP PROCEDURE `PROC_DROP_INDEX`;

DROP PROCEDURE `PROC_DROP_COLUMN`;

COMMIT;

