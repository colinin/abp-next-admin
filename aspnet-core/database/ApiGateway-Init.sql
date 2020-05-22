/*
 Navicat MySQL Data Transfer

 Source Server         : 
 Source Server Type    : MySQL
 Source Server Version : 
 Source Host           : 
 Source Schema         : ApiGateway

 Target Server Type    : MySQL
 Target Server Version : 50730
 File Encoding         : 65001

 Date: 22/05/2020 15:22:18
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for AppApiGatewayAggregate
-- ----------------------------
DROP TABLE IF EXISTS `AppApiGatewayAggregate`;
CREATE TABLE `AppApiGatewayAggregate`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ExtraProperties` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `AppId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `ReRouteId` bigint(20) NOT NULL,
  `ReRouteKeys` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `UpstreamPathTemplate` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `UpstreamHost` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ReRouteIsCaseSensitive` tinyint(1) NOT NULL DEFAULT 0,
  `Aggregator` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Priority` int(11) NULL DEFAULT NULL,
  `UpstreamHttpMethod` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 7 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of AppApiGatewayAggregate
-- ----------------------------
INSERT INTO `AppApiGatewayAggregate` VALUES (5, '{}', '324ada7e1f824c31ac113a3bf63dd725', 'TEST-APP', 'abp接口代理服务', 1263083077348196352, 'apigateway-definition,platform-definition,', '/api/abp/api-definition', '', 1, 'AbpApiDefinitionAggregator', NULL, '');
INSERT INTO `AppApiGatewayAggregate` VALUES (6, '{}', 'ac00ab19b3fd4a988cec490ca3a9ec22', 'TEST-APP', 'abp框架配置', 1263102116090970112, 'apigateway-configuration,platform-configuration,', '/api/abp/application-configuration', '', 1, 'AbpApiDefinitionAggregator', NULL, '');

-- ----------------------------
-- Table structure for AppApiGatewayAggregateConfig
-- ----------------------------
DROP TABLE IF EXISTS `AppApiGatewayAggregateConfig`;
CREATE TABLE `AppApiGatewayAggregateConfig`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ReRouteId` bigint(20) NOT NULL,
  `ReRouteKey` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Parameter` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `JsonPath` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `AggregateReRouteId` int(11) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IX_AppApiGatewayAggregateConfig_AggregateReRouteId`(`AggregateReRouteId`) USING BTREE,
  CONSTRAINT `FK_AppApiGatewayAggregateConfig_AppApiGatewayAggregate_Aggregat~` FOREIGN KEY (`AggregateReRouteId`) REFERENCES `AppApiGatewayAggregate` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for AppApiGatewayAuthOptions
-- ----------------------------
DROP TABLE IF EXISTS `AppApiGatewayAuthOptions`;
CREATE TABLE `AppApiGatewayAuthOptions`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ReRouteId` bigint(20) NOT NULL,
  `AuthenticationProviderKey` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `AllowedScopes` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `IX_AppApiGatewayAuthOptions_ReRouteId`(`ReRouteId`) USING BTREE,
  CONSTRAINT `FK_AppApiGatewayAuthOptions_AppApiGatewayReRoute_ReRouteId` FOREIGN KEY (`ReRouteId`) REFERENCES `AppApiGatewayReRoute` (`ReRouteId`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 64 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of AppApiGatewayAuthOptions
-- ----------------------------
INSERT INTO `AppApiGatewayAuthOptions` VALUES (3, 1261299170387169280, NULL, '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (4, 1261585859064872960, NULL, '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (5, 1261586605810368512, NULL, '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (6, 1261587558609436672, NULL, '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (7, 1261588213298348032, NULL, '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (8, 1261588367619375104, NULL, '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (9, 1261588628450557952, NULL, '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (10, 1261588881564221440, NULL, '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (11, 1261588983053795328, NULL, '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (12, 1261589139039961088, NULL, '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (13, 1261589197483393024, NULL, '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (14, 1261589278857084928, NULL, '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (15, 1261589420356124672, NULL, '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (16, 1261589960393736192, NULL, '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (17, 1261606600242085888, NULL, '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (18, 1261606689601732608, NULL, '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (19, 1261681880965038080, NULL, '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (20, 1261682144920977408, NULL, '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (21, 1262220447629058048, NULL, '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (22, 1262230734939758592, NULL, '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (23, 1262296916350869504, NULL, '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (24, 1262632376348594176, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (25, 1262632791869902848, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (26, 1262632904575045632, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (27, 1262632976616411136, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (28, 1262660336921235456, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (29, 1262660528277966848, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (30, 1262660706875625472, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (31, 1262660966393991168, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (32, 1262661109474283520, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (33, 1262663888804663296, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (34, 1262664024096133120, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (35, 1262664186252120064, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (36, 1262664357044178944, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (37, 1262664632928718848, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (38, 1262664751409418240, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (39, 1262664871274237952, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (40, 1262665026111164416, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (41, 1262665159905267712, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (42, 1262665329829105664, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (43, 1262665456471920640, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (44, 1262665628165754880, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (45, 1262666172682883072, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (47, 1262723402331885568, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (48, 1262935771746734080, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (49, 1262935906522304512, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (50, 1262936009924481024, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (52, 1263074419073593344, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (53, 1263075249394790400, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (54, 1263075593499684864, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (56, 1263101898440146944, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (57, 1263303878648569856, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (58, 1263304204797648896, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (59, 1263304872891555840, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (60, 1263305106250047488, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (61, 1263305244594970624, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (62, 1263305430536855552, '', '');
INSERT INTO `AppApiGatewayAuthOptions` VALUES (63, 1263639172959174656, '', '');

-- ----------------------------
-- Table structure for AppApiGatewayBalancerOptions
-- ----------------------------
DROP TABLE IF EXISTS `AppApiGatewayBalancerOptions`;
CREATE TABLE `AppApiGatewayBalancerOptions`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ItemId` bigint(20) NULL DEFAULT NULL,
  `ReRouteId` bigint(20) NULL DEFAULT NULL,
  `Type` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Key` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Expiry` int(11) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `IX_AppApiGatewayBalancerOptions_ItemId`(`ItemId`) USING BTREE,
  UNIQUE INDEX `IX_AppApiGatewayBalancerOptions_ReRouteId`(`ReRouteId`) USING BTREE,
  CONSTRAINT `FK_AppApiGatewayBalancerOptions_AppApiGatewayGlobalConfiguratio~` FOREIGN KEY (`ItemId`) REFERENCES `AppApiGatewayGlobalConfiguration` (`ItemId`) ON DELETE CASCADE ON UPDATE RESTRICT,
  CONSTRAINT `FK_AppApiGatewayBalancerOptions_AppApiGatewayReRoute_ReRouteId` FOREIGN KEY (`ReRouteId`) REFERENCES `AppApiGatewayReRoute` (`ReRouteId`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 65 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of AppApiGatewayBalancerOptions
-- ----------------------------
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (1, 1260841964962947072, NULL, 'LeastConnection', NULL, NULL);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (4, NULL, 1261299170387169280, 'LeastConnection', NULL, 60000);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (5, NULL, 1261585859064872960, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (6, NULL, 1261586605810368512, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (7, NULL, 1261587558609436672, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (8, NULL, 1261588213298348032, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (9, NULL, 1261588367619375104, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (10, NULL, 1261588628450557952, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (11, NULL, 1261588881564221440, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (12, NULL, 1261588983053795328, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (13, NULL, 1261589139039961088, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (14, NULL, 1261589197483393024, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (15, NULL, 1261589278857084928, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (16, NULL, 1261589420356124672, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (17, NULL, 1261589960393736192, 'LeastConnection', NULL, 60000);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (18, NULL, 1261606600242085888, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (19, NULL, 1261606689601732608, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (20, NULL, 1261681880965038080, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (21, NULL, 1261682144920977408, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (22, NULL, 1262220447629058048, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (23, NULL, 1262230734939758592, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (24, NULL, 1262296916350869504, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (25, NULL, 1262632376348594176, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (26, NULL, 1262632791869902848, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (27, NULL, 1262632904575045632, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (28, NULL, 1262632976616411136, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (29, NULL, 1262660336921235456, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (30, NULL, 1262660528277966848, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (31, NULL, 1262660706875625472, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (32, NULL, 1262660966393991168, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (33, NULL, 1262661109474283520, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (34, NULL, 1262663888804663296, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (35, NULL, 1262664024096133120, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (36, NULL, 1262664186252120064, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (37, NULL, 1262664357044178944, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (38, NULL, 1262664632928718848, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (39, NULL, 1262664751409418240, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (40, NULL, 1262664871274237952, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (41, NULL, 1262665026111164416, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (42, NULL, 1262665159905267712, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (43, NULL, 1262665329829105664, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (44, NULL, 1262665456471920640, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (45, NULL, 1262665628165754880, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (46, NULL, 1262666172682883072, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (48, NULL, 1262723402331885568, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (49, NULL, 1262935771746734080, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (50, NULL, 1262935906522304512, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (51, NULL, 1262936009924481024, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (53, NULL, 1263074419073593344, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (54, NULL, 1263075249394790400, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (55, NULL, 1263075593499684864, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (57, NULL, 1263101898440146944, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (58, NULL, 1263303878648569856, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (59, NULL, 1263304204797648896, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (60, NULL, 1263304872891555840, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (61, NULL, 1263305106250047488, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (62, NULL, 1263305244594970624, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (63, NULL, 1263305430536855552, '', '', 0);
INSERT INTO `AppApiGatewayBalancerOptions` VALUES (64, NULL, 1263639172959174656, '', '', 0);

-- ----------------------------
-- Table structure for AppApiGatewayCacheOptions
-- ----------------------------
DROP TABLE IF EXISTS `AppApiGatewayCacheOptions`;
CREATE TABLE `AppApiGatewayCacheOptions`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ReRouteId` bigint(20) NOT NULL,
  `TtlSeconds` int(11) NULL DEFAULT NULL,
  `Region` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `IX_AppApiGatewayCacheOptions_ReRouteId`(`ReRouteId`) USING BTREE,
  CONSTRAINT `FK_AppApiGatewayCacheOptions_AppApiGatewayReRoute_ReRouteId` FOREIGN KEY (`ReRouteId`) REFERENCES `AppApiGatewayReRoute` (`ReRouteId`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 64 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of AppApiGatewayCacheOptions
-- ----------------------------
INSERT INTO `AppApiGatewayCacheOptions` VALUES (3, 1261299170387169280, NULL, NULL);
INSERT INTO `AppApiGatewayCacheOptions` VALUES (4, 1261585859064872960, NULL, NULL);
INSERT INTO `AppApiGatewayCacheOptions` VALUES (5, 1261586605810368512, NULL, NULL);
INSERT INTO `AppApiGatewayCacheOptions` VALUES (6, 1261587558609436672, NULL, NULL);
INSERT INTO `AppApiGatewayCacheOptions` VALUES (7, 1261588213298348032, NULL, NULL);
INSERT INTO `AppApiGatewayCacheOptions` VALUES (8, 1261588367619375104, NULL, NULL);
INSERT INTO `AppApiGatewayCacheOptions` VALUES (9, 1261588628450557952, NULL, NULL);
INSERT INTO `AppApiGatewayCacheOptions` VALUES (10, 1261588881564221440, NULL, NULL);
INSERT INTO `AppApiGatewayCacheOptions` VALUES (11, 1261588983053795328, NULL, NULL);
INSERT INTO `AppApiGatewayCacheOptions` VALUES (12, 1261589139039961088, NULL, NULL);
INSERT INTO `AppApiGatewayCacheOptions` VALUES (13, 1261589197483393024, NULL, NULL);
INSERT INTO `AppApiGatewayCacheOptions` VALUES (14, 1261589278857084928, NULL, NULL);
INSERT INTO `AppApiGatewayCacheOptions` VALUES (15, 1261589420356124672, NULL, NULL);
INSERT INTO `AppApiGatewayCacheOptions` VALUES (16, 1261589960393736192, NULL, NULL);
INSERT INTO `AppApiGatewayCacheOptions` VALUES (17, 1261606600242085888, NULL, NULL);
INSERT INTO `AppApiGatewayCacheOptions` VALUES (18, 1261606689601732608, NULL, NULL);
INSERT INTO `AppApiGatewayCacheOptions` VALUES (19, 1261681880965038080, NULL, NULL);
INSERT INTO `AppApiGatewayCacheOptions` VALUES (20, 1261682144920977408, NULL, NULL);
INSERT INTO `AppApiGatewayCacheOptions` VALUES (21, 1262220447629058048, NULL, NULL);
INSERT INTO `AppApiGatewayCacheOptions` VALUES (22, 1262230734939758592, NULL, NULL);
INSERT INTO `AppApiGatewayCacheOptions` VALUES (23, 1262296916350869504, NULL, NULL);
INSERT INTO `AppApiGatewayCacheOptions` VALUES (24, 1262632376348594176, 0, '');
INSERT INTO `AppApiGatewayCacheOptions` VALUES (25, 1262632791869902848, NULL, NULL);
INSERT INTO `AppApiGatewayCacheOptions` VALUES (26, 1262632904575045632, NULL, NULL);
INSERT INTO `AppApiGatewayCacheOptions` VALUES (27, 1262632976616411136, NULL, NULL);
INSERT INTO `AppApiGatewayCacheOptions` VALUES (28, 1262660336921235456, 0, '');
INSERT INTO `AppApiGatewayCacheOptions` VALUES (29, 1262660528277966848, 0, '');
INSERT INTO `AppApiGatewayCacheOptions` VALUES (30, 1262660706875625472, 0, '');
INSERT INTO `AppApiGatewayCacheOptions` VALUES (31, 1262660966393991168, 0, '');
INSERT INTO `AppApiGatewayCacheOptions` VALUES (32, 1262661109474283520, 0, '');
INSERT INTO `AppApiGatewayCacheOptions` VALUES (33, 1262663888804663296, 0, '');
INSERT INTO `AppApiGatewayCacheOptions` VALUES (34, 1262664024096133120, 0, '');
INSERT INTO `AppApiGatewayCacheOptions` VALUES (35, 1262664186252120064, 0, '');
INSERT INTO `AppApiGatewayCacheOptions` VALUES (36, 1262664357044178944, 0, '');
INSERT INTO `AppApiGatewayCacheOptions` VALUES (37, 1262664632928718848, 0, '');
INSERT INTO `AppApiGatewayCacheOptions` VALUES (38, 1262664751409418240, 0, '');
INSERT INTO `AppApiGatewayCacheOptions` VALUES (39, 1262664871274237952, 0, '');
INSERT INTO `AppApiGatewayCacheOptions` VALUES (40, 1262665026111164416, 0, '');
INSERT INTO `AppApiGatewayCacheOptions` VALUES (41, 1262665159905267712, 0, '');
INSERT INTO `AppApiGatewayCacheOptions` VALUES (42, 1262665329829105664, 0, '');
INSERT INTO `AppApiGatewayCacheOptions` VALUES (43, 1262665456471920640, 0, '');
INSERT INTO `AppApiGatewayCacheOptions` VALUES (44, 1262665628165754880, 0, '');
INSERT INTO `AppApiGatewayCacheOptions` VALUES (45, 1262666172682883072, 0, '');
INSERT INTO `AppApiGatewayCacheOptions` VALUES (47, 1262723402331885568, 0, '');
INSERT INTO `AppApiGatewayCacheOptions` VALUES (48, 1262935771746734080, 0, '');
INSERT INTO `AppApiGatewayCacheOptions` VALUES (49, 1262935906522304512, 0, '');
INSERT INTO `AppApiGatewayCacheOptions` VALUES (50, 1262936009924481024, 0, '');
INSERT INTO `AppApiGatewayCacheOptions` VALUES (52, 1263074419073593344, 0, '');
INSERT INTO `AppApiGatewayCacheOptions` VALUES (53, 1263075249394790400, 0, '');
INSERT INTO `AppApiGatewayCacheOptions` VALUES (54, 1263075593499684864, 0, '');
INSERT INTO `AppApiGatewayCacheOptions` VALUES (56, 1263101898440146944, 0, '');
INSERT INTO `AppApiGatewayCacheOptions` VALUES (57, 1263303878648569856, NULL, NULL);
INSERT INTO `AppApiGatewayCacheOptions` VALUES (58, 1263304204797648896, NULL, NULL);
INSERT INTO `AppApiGatewayCacheOptions` VALUES (59, 1263304872891555840, NULL, NULL);
INSERT INTO `AppApiGatewayCacheOptions` VALUES (60, 1263305106250047488, NULL, NULL);
INSERT INTO `AppApiGatewayCacheOptions` VALUES (61, 1263305244594970624, NULL, NULL);
INSERT INTO `AppApiGatewayCacheOptions` VALUES (62, 1263305430536855552, NULL, NULL);
INSERT INTO `AppApiGatewayCacheOptions` VALUES (63, 1263639172959174656, NULL, NULL);

-- ----------------------------
-- Table structure for AppApiGatewayDiscovery
-- ----------------------------
DROP TABLE IF EXISTS `AppApiGatewayDiscovery`;
CREATE TABLE `AppApiGatewayDiscovery`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ItemId` bigint(20) NOT NULL,
  `Host` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Port` int(11) NULL DEFAULT NULL,
  `Type` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Token` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ConfigurationKey` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `PollingInterval` int(11) NULL DEFAULT NULL,
  `Namespace` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Scheme` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `IX_AppApiGatewayDiscovery_ItemId`(`ItemId`) USING BTREE,
  CONSTRAINT `FK_AppApiGatewayDiscovery_AppApiGatewayGlobalConfiguration_Item~` FOREIGN KEY (`ItemId`) REFERENCES `AppApiGatewayGlobalConfiguration` (`ItemId`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of AppApiGatewayDiscovery
-- ----------------------------
INSERT INTO `AppApiGatewayDiscovery` VALUES (1, 1260841964962947072, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

-- ----------------------------
-- Table structure for AppApiGatewayDynamicReRoute
-- ----------------------------
DROP TABLE IF EXISTS `AppApiGatewayDynamicReRoute`;
CREATE TABLE `AppApiGatewayDynamicReRoute`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ExtraProperties` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `DynamicReRouteId` bigint(20) NOT NULL,
  `ServiceName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `DownstreamHttpVersion` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `AppId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `AK_AppApiGatewayDynamicReRoute_DynamicReRouteId`(`DynamicReRouteId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for AppApiGatewayGlobalConfiguration
-- ----------------------------
DROP TABLE IF EXISTS `AppApiGatewayGlobalConfiguration`;
CREATE TABLE `AppApiGatewayGlobalConfiguration`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ExtraProperties` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `ItemId` bigint(20) NOT NULL,
  `RequestIdKey` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `BaseUrl` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `DownstreamScheme` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `DownstreamHttpVersion` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT 0,
  `IsActive` tinyint(1) NOT NULL,
  `AppId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `AK_AppApiGatewayGlobalConfiguration_ItemId`(`ItemId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of AppApiGatewayGlobalConfiguration
-- ----------------------------
INSERT INTO `AppApiGatewayGlobalConfiguration` VALUES (1, '{}', 'f7973118f2c2425c8cc96b59883b99aa', 1260841964962947072, NULL, 'http://localhost:30000', 'HTTP', NULL, 0, 1, 'TEST-APP');

-- ----------------------------
-- Table structure for AppApiGatewayHeaders
-- ----------------------------
DROP TABLE IF EXISTS `AppApiGatewayHeaders`;
CREATE TABLE `AppApiGatewayHeaders`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ReRouteId` bigint(20) NOT NULL,
  `Key` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Value` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for AppApiGatewayHostAndPort
-- ----------------------------
DROP TABLE IF EXISTS `AppApiGatewayHostAndPort`;
CREATE TABLE `AppApiGatewayHostAndPort`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ReRouteId` bigint(20) NOT NULL,
  `Host` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Port` int(11) NULL DEFAULT 0,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for AppApiGatewayHttpOptions
-- ----------------------------
DROP TABLE IF EXISTS `AppApiGatewayHttpOptions`;
CREATE TABLE `AppApiGatewayHttpOptions`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ItemId` bigint(20) NULL DEFAULT NULL,
  `ReRouteId` bigint(20) NULL DEFAULT NULL,
  `MaxConnectionsPerServer` int(11) NULL DEFAULT NULL,
  `AllowAutoRedirect` tinyint(1) NOT NULL,
  `UseCookieContainer` tinyint(1) NOT NULL,
  `UseTracing` tinyint(1) NOT NULL,
  `UseProxy` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `IX_AppApiGatewayHttpOptions_ItemId`(`ItemId`) USING BTREE,
  UNIQUE INDEX `IX_AppApiGatewayHttpOptions_ReRouteId`(`ReRouteId`) USING BTREE,
  CONSTRAINT `FK_AppApiGatewayHttpOptions_AppApiGatewayGlobalConfiguration_It~` FOREIGN KEY (`ItemId`) REFERENCES `AppApiGatewayGlobalConfiguration` (`ItemId`) ON DELETE CASCADE ON UPDATE RESTRICT,
  CONSTRAINT `FK_AppApiGatewayHttpOptions_AppApiGatewayReRoute_ReRouteId` FOREIGN KEY (`ReRouteId`) REFERENCES `AppApiGatewayReRoute` (`ReRouteId`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 65 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of AppApiGatewayHttpOptions
-- ----------------------------
INSERT INTO `AppApiGatewayHttpOptions` VALUES (1, 1260841964962947072, NULL, NULL, 0, 0, 1, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (4, NULL, 1261299170387169280, 1000, 1, 0, 1, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (5, NULL, 1261585859064872960, NULL, 0, 0, 1, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (6, NULL, 1261586605810368512, NULL, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (7, NULL, 1261587558609436672, NULL, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (8, NULL, 1261588213298348032, NULL, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (9, NULL, 1261588367619375104, NULL, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (10, NULL, 1261588628450557952, NULL, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (11, NULL, 1261588881564221440, NULL, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (12, NULL, 1261588983053795328, NULL, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (13, NULL, 1261589139039961088, NULL, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (14, NULL, 1261589197483393024, NULL, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (15, NULL, 1261589278857084928, NULL, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (16, NULL, 1261589420356124672, NULL, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (17, NULL, 1261589960393736192, 1000, 1, 0, 1, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (18, NULL, 1261606600242085888, NULL, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (19, NULL, 1261606689601732608, NULL, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (20, NULL, 1261681880965038080, 100, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (21, NULL, 1261682144920977408, 100, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (22, NULL, 1262220447629058048, NULL, 0, 0, 1, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (23, NULL, 1262230734939758592, NULL, 0, 0, 1, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (24, NULL, 1262296916350869504, NULL, 0, 0, 1, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (25, NULL, 1262632376348594176, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (26, NULL, 1262632791869902848, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (27, NULL, 1262632904575045632, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (28, NULL, 1262632976616411136, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (29, NULL, 1262660336921235456, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (30, NULL, 1262660528277966848, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (31, NULL, 1262660706875625472, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (32, NULL, 1262660966393991168, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (33, NULL, 1262661109474283520, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (34, NULL, 1262663888804663296, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (35, NULL, 1262664024096133120, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (36, NULL, 1262664186252120064, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (37, NULL, 1262664357044178944, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (38, NULL, 1262664632928718848, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (39, NULL, 1262664751409418240, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (40, NULL, 1262664871274237952, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (41, NULL, 1262665026111164416, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (42, NULL, 1262665159905267712, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (43, NULL, 1262665329829105664, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (44, NULL, 1262665456471920640, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (45, NULL, 1262665628165754880, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (46, NULL, 1262666172682883072, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (48, NULL, 1262723402331885568, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (49, NULL, 1262935771746734080, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (50, NULL, 1262935906522304512, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (51, NULL, 1262936009924481024, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (53, NULL, 1263074419073593344, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (54, NULL, 1263075249394790400, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (55, NULL, 1263075593499684864, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (57, NULL, 1263101898440146944, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (58, NULL, 1263303878648569856, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (59, NULL, 1263304204797648896, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (60, NULL, 1263304872891555840, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (61, NULL, 1263305106250047488, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (62, NULL, 1263305244594970624, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (63, NULL, 1263305430536855552, 0, 0, 0, 0, 0);
INSERT INTO `AppApiGatewayHttpOptions` VALUES (64, NULL, 1263639172959174656, 0, 0, 0, 0, 0);

-- ----------------------------
-- Table structure for AppApiGatewayQoSOptions
-- ----------------------------
DROP TABLE IF EXISTS `AppApiGatewayQoSOptions`;
CREATE TABLE `AppApiGatewayQoSOptions`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ItemId` bigint(20) NULL DEFAULT NULL,
  `ReRouteId` bigint(20) NULL DEFAULT NULL,
  `ExceptionsAllowedBeforeBreaking` int(11) NULL DEFAULT NULL,
  `DurationOfBreak` int(11) NULL DEFAULT NULL,
  `TimeoutValue` int(11) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `IX_AppApiGatewayQoSOptions_ItemId`(`ItemId`) USING BTREE,
  UNIQUE INDEX `IX_AppApiGatewayQoSOptions_ReRouteId`(`ReRouteId`) USING BTREE,
  CONSTRAINT `FK_AppApiGatewayQoSOptions_AppApiGatewayGlobalConfiguration_Ite~` FOREIGN KEY (`ItemId`) REFERENCES `AppApiGatewayGlobalConfiguration` (`ItemId`) ON DELETE CASCADE ON UPDATE RESTRICT,
  CONSTRAINT `FK_AppApiGatewayQoSOptions_AppApiGatewayReRoute_ReRouteId` FOREIGN KEY (`ReRouteId`) REFERENCES `AppApiGatewayReRoute` (`ReRouteId`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 65 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of AppApiGatewayQoSOptions
-- ----------------------------
INSERT INTO `AppApiGatewayQoSOptions` VALUES (1, 1260841964962947072, NULL, 60, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (4, NULL, 1261299170387169280, 60, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (5, NULL, 1261585859064872960, 60, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (6, NULL, 1261586605810368512, 60, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (7, NULL, 1261587558609436672, 60, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (8, NULL, 1261588213298348032, 60, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (9, NULL, 1261588367619375104, 60, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (10, NULL, 1261588628450557952, 60, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (11, NULL, 1261588881564221440, 60, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (12, NULL, 1261588983053795328, 60, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (13, NULL, 1261589139039961088, 60, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (14, NULL, 1261589197483393024, 60, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (15, NULL, 1261589278857084928, 60, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (16, NULL, 1261589420356124672, 60, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (17, NULL, 1261589960393736192, 60, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (18, NULL, 1261606600242085888, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (19, NULL, 1261606689601732608, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (20, NULL, 1261681880965038080, 60, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (21, NULL, 1261682144920977408, 60, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (22, NULL, 1262220447629058048, 60, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (23, NULL, 1262230734939758592, 60, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (24, NULL, 1262296916350869504, 60, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (25, NULL, 1262632376348594176, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (26, NULL, 1262632791869902848, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (27, NULL, 1262632904575045632, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (28, NULL, 1262632976616411136, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (29, NULL, 1262660336921235456, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (30, NULL, 1262660528277966848, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (31, NULL, 1262660706875625472, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (32, NULL, 1262660966393991168, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (33, NULL, 1262661109474283520, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (34, NULL, 1262663888804663296, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (35, NULL, 1262664024096133120, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (36, NULL, 1262664186252120064, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (37, NULL, 1262664357044178944, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (38, NULL, 1262664632928718848, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (39, NULL, 1262664751409418240, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (40, NULL, 1262664871274237952, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (41, NULL, 1262665026111164416, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (42, NULL, 1262665159905267712, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (43, NULL, 1262665329829105664, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (44, NULL, 1262665456471920640, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (45, NULL, 1262665628165754880, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (46, NULL, 1262666172682883072, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (48, NULL, 1262723402331885568, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (49, NULL, 1262935771746734080, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (50, NULL, 1262935906522304512, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (51, NULL, 1262936009924481024, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (53, NULL, 1263074419073593344, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (54, NULL, 1263075249394790400, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (55, NULL, 1263075593499684864, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (57, NULL, 1263101898440146944, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (58, NULL, 1263303878648569856, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (59, NULL, 1263304204797648896, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (60, NULL, 1263304872891555840, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (61, NULL, 1263305106250047488, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (62, NULL, 1263305244594970624, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (63, NULL, 1263305430536855552, 50, 60000, 30000);
INSERT INTO `AppApiGatewayQoSOptions` VALUES (64, NULL, 1263639172959174656, 50, 60000, 30000);

-- ----------------------------
-- Table structure for AppApiGatewayRateLimitOptions
-- ----------------------------
DROP TABLE IF EXISTS `AppApiGatewayRateLimitOptions`;
CREATE TABLE `AppApiGatewayRateLimitOptions`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ItemId` bigint(20) NOT NULL,
  `ClientIdHeader` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT 'ClientId',
  `QuotaExceededMessage` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `RateLimitCounterPrefix` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT 'ocelot',
  `DisableRateLimitHeaders` tinyint(1) NOT NULL,
  `HttpStatusCode` int(11) NULL DEFAULT 429,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `IX_AppApiGatewayRateLimitOptions_ItemId`(`ItemId`) USING BTREE,
  CONSTRAINT `FK_AppApiGatewayRateLimitOptions_AppApiGatewayGlobalConfigurati~` FOREIGN KEY (`ItemId`) REFERENCES `AppApiGatewayGlobalConfiguration` (`ItemId`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of AppApiGatewayRateLimitOptions
-- ----------------------------
INSERT INTO `AppApiGatewayRateLimitOptions` VALUES (1, 1260841964962947072, 'ClientId', '您的操作过快,请稍后再试!', 'ocelot', 1, 429);

-- ----------------------------
-- Table structure for AppApiGatewayRateLimitRule
-- ----------------------------
DROP TABLE IF EXISTS `AppApiGatewayRateLimitRule`;
CREATE TABLE `AppApiGatewayRateLimitRule`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ReRouteId` bigint(20) NULL DEFAULT NULL,
  `DynamicReRouteId` bigint(20) NULL DEFAULT NULL,
  `ClientWhitelist` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `EnableRateLimiting` tinyint(1) NOT NULL,
  `Period` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `PeriodTimespan` double NULL DEFAULT NULL,
  `Limit` bigint(20) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `IX_AppApiGatewayRateLimitRule_DynamicReRouteId`(`DynamicReRouteId`) USING BTREE,
  UNIQUE INDEX `IX_AppApiGatewayRateLimitRule_ReRouteId`(`ReRouteId`) USING BTREE,
  CONSTRAINT `FK_AppApiGatewayRateLimitRule_AppApiGatewayDynamicReRoute_Dynam~` FOREIGN KEY (`DynamicReRouteId`) REFERENCES `AppApiGatewayDynamicReRoute` (`DynamicReRouteId`) ON DELETE CASCADE ON UPDATE RESTRICT,
  CONSTRAINT `FK_AppApiGatewayRateLimitRule_AppApiGatewayReRoute_ReRouteId` FOREIGN KEY (`ReRouteId`) REFERENCES `AppApiGatewayReRoute` (`ReRouteId`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 64 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of AppApiGatewayRateLimitRule
-- ----------------------------
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (3, 1261299170387169280, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (4, 1261585859064872960, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (5, 1261586605810368512, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (6, 1261587558609436672, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (7, 1261588213298348032, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (8, 1261588367619375104, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (9, 1261588628450557952, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (10, 1261588881564221440, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (11, 1261588983053795328, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (12, 1261589139039961088, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (13, 1261589197483393024, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (14, 1261589278857084928, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (15, 1261589420356124672, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (16, 1261589960393736192, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (17, 1261606600242085888, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (18, 1261606689601732608, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (19, 1261681880965038080, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (20, 1261682144920977408, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (21, 1262220447629058048, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (22, 1262230734939758592, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (23, 1262296916350869504, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (24, 1262632376348594176, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (25, 1262632791869902848, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (26, 1262632904575045632, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (27, 1262632976616411136, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (28, 1262660336921235456, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (29, 1262660528277966848, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (30, 1262660706875625472, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (31, 1262660966393991168, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (32, 1262661109474283520, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (33, 1262663888804663296, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (34, 1262664024096133120, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (35, 1262664186252120064, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (36, 1262664357044178944, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (37, 1262664632928718848, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (38, 1262664751409418240, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (39, 1262664871274237952, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (40, 1262665026111164416, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (41, 1262665159905267712, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (42, 1262665329829105664, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (43, 1262665456471920640, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (44, 1262665628165754880, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (45, 1262666172682883072, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (47, 1262723402331885568, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (48, 1262935771746734080, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (49, 1262935906522304512, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (50, 1262936009924481024, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (52, 1263074419073593344, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (53, 1263075249394790400, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (54, 1263075593499684864, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (56, 1263101898440146944, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (57, 1263303878648569856, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (58, 1263304204797648896, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (59, 1263304872891555840, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (60, 1263305106250047488, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (61, 1263305244594970624, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (62, 1263305430536855552, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `AppApiGatewayRateLimitRule` VALUES (63, 1263639172959174656, NULL, '', 0, NULL, NULL, NULL);

-- ----------------------------
-- Table structure for AppApiGatewayReRoute
-- ----------------------------
DROP TABLE IF EXISTS `AppApiGatewayReRoute`;
CREATE TABLE `AppApiGatewayReRoute`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ExtraProperties` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `ReRouteId` bigint(20) NOT NULL,
  `ReRouteName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `DownstreamPathTemplate` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `ChangeDownstreamPathTemplate` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `DownstreamHttpMethod` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `UpstreamPathTemplate` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `UpstreamHttpMethod` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `AddHeadersToRequest` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `UpstreamHeaderTransform` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `DownstreamHeaderTransform` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `AddClaimsToRequest` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `RouteClaimsRequirement` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `AddQueriesToRequest` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `RequestIdKey` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ReRouteIsCaseSensitive` tinyint(1) NOT NULL,
  `ServiceName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ServiceNamespace` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `DownstreamScheme` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `DownstreamHostAndPorts` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `DelegatingHandlers` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `UpstreamHost` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Key` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Priority` int(11) NULL DEFAULT NULL,
  `Timeout` int(11) NULL DEFAULT NULL,
  `DangerousAcceptAnyServerCertificateValidator` tinyint(1) NOT NULL,
  `DownstreamHttpVersion` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `AppId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `AK_AppApiGatewayReRoute_ReRouteId`(`ReRouteId`) USING BTREE,
  UNIQUE INDEX `IX_AppApiGatewayReRoute_DownstreamPathTemplate_UpstreamPathTemp~`(`DownstreamPathTemplate`, `UpstreamPathTemplate`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 67 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of AppApiGatewayReRoute
-- ----------------------------
INSERT INTO `AppApiGatewayReRoute` VALUES (4, '{}', '430fb64b9c3949088aa32bddd86497d0', 1261299170387169280, '平台服务-权限管理', '/api/abp/permissions', '', NULL, '/api/abp/permissions', 'GET,PUT,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30010,', '', NULL, NULL, NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (5, '{}', '302b4871e4ba49efb39189944761d251', 1261585859064872960, '身份认证服务-客户端管理', '/api/IdentityServer/Clients', '', NULL, '/api/IdentityServer/Clients', 'POST,GET,PUT,', '', '', '', '', '', '', NULL, 1, '', NULL, 'HTTP', '127.0.0.1:30010,', '', NULL, NULL, NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (6, '{}', '4d7c360bc40342c1b1cf16181e4ac2bb', 1261586605810368512, '身份认证服务-查询客户端', '/api/IdentityServer/Clients/{Id}', '', NULL, '/api/IdentityServer/Clients/{Id}', 'GET,DELETE,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30010,', '', NULL, NULL, NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (7, '{}', 'c222dcff22284b3e9ce9c424853705cd', 1261587558609436672, '服务网关管理-路由组管理', '/api/ApiGateway/RouteGroups', '', '', '/api/ApiGateway/RouteGroups', 'GET,POST,PUT,DELETE,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, '', NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (8, '{}', '4828b753ef5a45b1a8c7903b7977006f', 1261588213298348032, '服务网关管理-查询单个路由组', '/api/ApiGateway/RouteGroups/By-AppId/{AppId}', '', '', '/api/ApiGateway/RouteGroups/By-AppId/{AppId}', 'GET,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, '', NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (9, '{}', '7813111dea414ea6a48c498726c82e06', 1261588367619375104, '服务网关管理-查询所有有效路由组', '/api/ApiGateway/RouteGroups/Actived', '', '', '/api/ApiGateway/RouteGroups/Actived', 'GET,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, '', NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (10, '{}', 'a91c7f67e7cd4d9ca61b7d657213eaaf', 1261588628450557952, '服务网关管理-基础配置', '/api/ApiGateway/Globals', '', '', '/api/ApiGateway/Globals', 'GET,POST,PUT,DELETE,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, '', NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (11, '{}', 'fc3401e7942f4e63870fdd512372bf7d', 1261588881564221440, '服务网关管理-查询单个基础配置', '/api/ApiGateway/Globals/By-AppId/{AppId}', '', '', '/api/ApiGateway/Globals/By-AppId/{AppId}', 'GET,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, '', NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (12, '{}', '1298ef932a4845c7856ab0344e15daa9', 1261588983053795328, '服务网关管理-路由配置', '/api/ApiGateway/Routes', '', '', '/api/ApiGateway/Routes', 'GET,POST,PUT,DELETE,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, '', NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (13, '{}', 'd857a7a270cc44b38dfbd3d9dc7e62e6', 1261589139039961088, '服务网关管理-通过标识查询路由', '/api/ApiGateway/Routes/By-RouteId/{RouteId}', '', '', '/api/ApiGateway/Routes/By-RouteId/{RouteId}', 'GET,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, '', NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (14, '{}', 'aecd3c83218e490e8a48214dacacc88c', 1261589197483393024, '服务网关管理-通过名称查询路由', '/api/ApiGateway/Routes/By-RouteName/{RouteName}', '', '', '/api/ApiGateway/Routes/By-RouteName/{RouteName}', 'GET,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, '', NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (15, '{}', '73b62418ba424fa590113b58edf23e49', 1261589278857084928, '服务网关管理-通过应用标识查询路由', '/api/ApiGateway/Routes/By-AppId/{AppId}', '', '', '/api/ApiGateway/Routes/By-AppId/{AppId}', 'GET,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, '', NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (16, '{}', 'd096d74fb1d34619ba391dbd19a010ef', 1261589420356124672, '服务网关管理-清空应用标识下所有路由', '/api/ApiGateway/Routes/Clear', '', '', '/api/ApiGateway/Routes/Clear', 'DELETE,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, '', NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (17, '{}', '07fb9bc9e4604a24895dd60015856bc7', 1261589960393736192, '服务网关管理-通过应用标识查询动态路由', '/api/ApiGateway/DynamicRoutes/By-AppId/{AppId}', '', NULL, '/api/ApiGateway/DynamicRoutes/By-AppId/{AppId}', 'GET,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, NULL, NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (18, '{}', 'e659ebbf61534a978335cfeabdc0b375', 1261606600242085888, '服务网关管理-通过应用标识查询聚合路由', '/api/ApiGateway/Aggregates/by-AppId/{AppId}', '', NULL, '/api/ApiGateway/Aggregates/by-AppId/{AppId}', 'GET,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, NULL, NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (19, '{}', 'dd47edd7b03c42bc8666f4bded7cb991', 1261606689601732608, '服务网关管理-聚合路由', '/api/ApiGateway/Aggregates', '', NULL, '/api/ApiGateway/Aggregates', 'GET,POST,PUT,DELETE,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, NULL, NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (22, '{}', '5be66f8be176412a8e75aadd109b544a', 1262220447629058048, '身份认证服务-客户端密钥', '/api/IdentityServer/Clients/Secrets', '', NULL, '/api/IdentityServer/Clients/Secrets', 'PUT,POST,DELETE,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30010,', '', NULL, NULL, NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (23, '{}', '1cb56a15863f464e9b79db0d18acf4ba', 1262230734939758592, '身份认证服务-客户端声明', '/api/IdentityServer/Clients/Claims', '', NULL, '/api/IdentityServer/Clients/Claims', 'PUT,POST,DELETE,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30010,', '', NULL, NULL, NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (25, '{}', 'b6009df9b18c44b3aa2b77f27b0c2abb', 1262296916350869504, '身份认证服务-客户端属性', '/api/IdentityServer/Clients/Properties', '', NULL, '/api/IdentityServer/Clients/Properties', 'PUT,POST,DELETE,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30010,', '', NULL, NULL, NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (26, '{}', '401a7a8e8a2f41c599b1b87b55249a32', 1262632376348594176, '身份认证服务-Api资源管理', '/api/IdentityServer/ApiResources', '', '', '/api/IdentityServer/ApiResources', 'GET,POST,PUT,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (27, '{}', 'c662a4c29e654f9da6eb73ee456b533a', 1262632791869902848, '身份认证服务-单个Api资源', '/api/IdentityServer/ApiResources/{Id}', '', '', '/api/IdentityServer/ApiResources/{Id}', 'GET,DELETE,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (28, '{}', 'a0c2bdfdd0de4832b63d6dc3696c9c26', 1262632904575045632, '身份认证服务-Api资源密钥', '/api/IdentityServer/ApiResources/Secrets', '', '', '/api/IdentityServer/ApiResources/Secrets', 'DELETE,POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (29, '{}', 'fb0e7ca974f243ce9f4034b39bdda326', 1262632976616411136, '身份认证服务-Api资源作用域', '/api/IdentityServer/ApiResources/Scopes', '', '', '/api/IdentityServer/ApiResources/Scopes', 'DELETE,POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (30, '{}', '0f9875697b74420c9dc2eaf77099b210', 1262660336921235456, '身份认证服务-用户登录', '/api/account/login', '', '', '/api/account/login', 'POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (31, '{}', 'a890c6ecc6a64c9fa313a0f6b5406e1c', 1262660528277966848, '身份认证服务-用户登出', '/api/account/logout', '', '', '/api/account/logout', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (32, '{}', '88de580b6beb4d9d9d4367840ba1fcea', 1262660706875625472, '身份认证服务-检查密码', '/api/account/checkPassword', '', '', '/api/account/checkPassword', 'POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (33, '{}', '78f3c1adc7a54696af37a419eda47c62', 1262660966393991168, '身份认证服务-个人信息页', '/api/identity/my-profile', '', '', '/api/identity/my-profile', 'GET,PUT,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (34, '{}', '95b23aa5cebb40598a78c0761cfd0b26', 1262661109474283520, '身份认证服务-修改密码', '/api/identity/my-profile/change-password', '', '', '/api/identity/my-profile/change-password', 'POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (35, '{}', '4828f7c2aff8485189f37aba5de62d60', 1262663888804663296, '身份认证管理-角色管理', '/api/identity/roles', '', '', '/api/identity/roles', 'GET,POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (36, '{}', '0fddcd3b50a24c6795ec9034fdb44778', 1262664024096133120, '身份认证服务-角色列表', '/api/identity/roles/all', '', '', '/api/identity/roles/all', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (37, '{}', '191e555219e845069dfd93793263a840', 1262664186252120064, '身份认证服务-单个角色', '/api/identity/roles/{id}', '', '', '/api/identity/roles/{id}', 'GET,PUT,DELETE,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (38, '{}', 'c316858e82f74e6ca6e923d6b3a3fa76', 1262664357044178944, '身份认证服务-用户注册', '/api/account/register', '', '', '/api/account/register', 'POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (39, '{}', 'db53b6f957914a10a6a97ba306b1f6ef', 1262664632928718848, '身份认证服务-单个用户', '/api/identity/users/{id}', '', '', '/api/identity/users/{id}', 'GET,PUT,DELETE,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (40, '{}', '1833434b8ce34f8ab791e7e950f4c61f', 1262664751409418240, '身份认证服务-用户管理', '/api/identity/users', '', '', '/api/identity/users', 'GET,POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (41, '{}', 'b3c963a1612144918bffaf272697498c', 1262664871274237952, '身份认证服务-用户角色', '/api/identity/users/{id}/roles', '', '', '/api/identity/users/{id}/roles', 'GET,POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (42, '{}', '33dd757b79cb4f52994af13bfb4f6783', 1262665026111164416, '身份认证服务-通过用户名查询用户', '/api/identity/users/by-username/{userName}', '', '', '/api/identity/users/by-username/{userName}', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (43, '{}', 'e46fd6cb3a104da3aadfe0149fe4de68', 1262665159905267712, '身份认证服务-通过邮件查询用户', '/api/identity/users/by-email/{email}', '', '', '/api/identity/users/by-email/{email}', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (44, '{}', '8c8ec5ad6aaa4145981ee7ac876c36c9', 1262665329829105664, '身份认证服务-通过标识查询用户', '/api/identity/users/lookup/{id}', '', '', '/api/identity/users/lookup/{id}', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (45, '{}', 'f5c0c8c02c0846fdbe5015cd86f3d81b', 1262665456471920640, '身份认证服务-通过名称查询用户', '/api/identity/users/lookup/by-username/{userName}', '', '', '/api/identity/users/lookup/by-username/{userName}', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (46, '{}', '4de0c9e0437f40ce81628de956af9c5e', 1262665628165754880, '身份认证服务-通过名称查询租户', '/api/abp/multi-tenancy/tenants/by-name/{name}', '', '', '/api/abp/multi-tenancy/tenants/by-name/{name}', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (47, '{}', 'fa60a7253b2f4c80b9afad4e82ba6ba8', 1262666172682883072, '身份认证服务-通过标识查询租户', '/api/abp/multi-tenancy/tenants/by-id/{id}', '', '', '/api/abp/multi-tenancy/tenants/by-id/{id}', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (49, '{}', '4114a097b9e04a9e90458edf02ef41c7', 1262723402331885568, '身份认证服务-克隆客户端', '/api/IdentityServer/Clients/Clone', '', '', '/api/IdentityServer/Clients/Clone', 'POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (50, '{}', '2a9cc98d3ed0462d98c4bf0e946f410d', 1262935771746734080, '身份认证服务-身份资源管理', '/api/IdentityServer/IdentityResources', '', '', '/api/IdentityServer/IdentityResources', 'GET,POST,PUT,DELETE,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (51, '{}', '4d36b0ea5b92473ea5de3e91ff155830', 1262935906522304512, '身份认证服务-查询身份资源', '/api/IdentityServer/IdentityResources/{Id}', '', '', '/api/IdentityServer/IdentityResources/{Id}', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (52, '{}', '387df08693e54e91ac055a2324d4c10e', 1262936009924481024, '身份认证服务-身份资源属性', '/api/IdentityServer/IdentityResources/Properties', '', '', '/api/IdentityServer/IdentityResources/Properties', 'POST,DELETE,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (54, '{}', '22950f5be9cd434b8452a9da11cfc034', 1263074419073593344, '服务网关管理-abp代理接口', '/api/abp/api-definition', '', '', '/api/abp/apigateway/api-definition', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30001,', '', '', 'apigateway-definition', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (55, '{}', 'ca2cedfa620045a9adef0be2f958c4bc', 1263075249394790400, '服务网关管理-查询聚合路由', '/api/ApiGateway/Aggregates/{RouteId}', '', '', '/api/ApiGateway/Aggregates/{RouteId}', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30001,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (57, '{}', '98fbc99fc8644946ac0a72cc3dc5fd1f', 1263075593499684864, '服务网关管理-聚合路由配置', '/api/ApiGateway/Aggregates/RouteConfig', '', '', '/api/ApiGateway/Aggregates/RouteConfig', 'POST,DELETE,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30001,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (59, '{}', 'e298803438ef445b9e0efb249261019f', 1263101898440146944, '服务网关管理-框架配置', '/api/abp/application-configuration', '', '', '/api/abp/apigateway/application-configuration', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30001,', '', '', 'apigateway-configuration', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (60, '{}', 'd6b379ce336c4a33bc8d8ff2f99fad83', 1263303878648569856, '平台服务-abp代理接口', '/api/abp/api-definition', '', '', '/api/abp/platform/api-definition', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', 'platform-definition', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (61, '{}', 'fa47587dbce74fb59d3d58528f2ddce6', 1263304204797648896, '平台服务-框架配置', '/api/abp/application-configuration', '', '', '/api/abp/platform/application-configuration', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', 'platform-configuration', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (62, '{}', '9810656b884947e3897e776b47208352', 1263304872891555840, '平台服务-租户管理', '/api/multi-tenancy/tenants', '', '', '/api/multi-tenancy/tenants', 'GET,POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (63, '{}', '12254ca25e15420faa694f62148dd694', 1263305106250047488, '平台服务-特定租户管理', '/api/multi-tenancy/tenants/{id}', '', '', '/api/multi-tenancy/tenants/{id}', 'GET,PUT,DELETE,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (64, '{}', '27761205c6344bfebbafbc077781ab76', 1263305244594970624, '平台服务-租户连接字符串', '/api/multi-tenancy/tenants/{id}/connection-string', '', '', '/api/multi-tenancy/tenants/{id}/concatenation', 'GET,PUT,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 2, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (65, '{}', 'af470c53a25340fd9248fd0309ad41ef', 1263305430536855552, '平台服务-特定租户连接字符串', '/api/multi-tenancy/tenants/{id}/connection-string/{name}', '', '', '/api/multi-tenancy/tenants/{id}/concatenation/{name}', 'GET,DELETE,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 1, 30000, 1, '', 'TEST-APP');
INSERT INTO `AppApiGatewayReRoute` VALUES (66, '{}', 'a7a61cb210484794a5ad380938630058', 1263639172959174656, '平台服务-配置管理', '/api/abp/setting', '', '', '/api/abp/setting', 'GET,PUT,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');

-- ----------------------------
-- Table structure for AppApiGatewayRouteGroup
-- ----------------------------
DROP TABLE IF EXISTS `AppApiGatewayRouteGroup`;
CREATE TABLE `AppApiGatewayRouteGroup`  (
  `Id` char(36) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `ExtraProperties` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorId` char(36) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL,
  `LastModificationTime` datetime(6) NULL DEFAULT NULL,
  `LastModifierId` char(36) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `DeleterId` char(36) CHARACTER SET latin1 COLLATE latin1_swedish_ci NULL DEFAULT NULL,
  `DeletionTime` datetime(6) NULL DEFAULT NULL,
  `Name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `AppId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `AppName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `AppIpAddress` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Description` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `IsActive` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IX_AppApiGatewayRouteGroup_AppId_AppName_AppIpAddress`(`AppId`, `AppName`, `AppIpAddress`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of AppApiGatewayRouteGroup
-- ----------------------------
INSERT INTO `AppApiGatewayRouteGroup` VALUES ('08d7f735-a83b-49ab-8cee-5d602502bea8', '{}', '8484f6852d3b4252a0b6bf278f9afd48', '2020-05-13 20:03:32.524271', NULL, '2020-05-13 21:34:30.407974', NULL, 0, NULL, NULL, '测试组', 'TEST-APP', '测试网关分组', '127.0.0.1', '测试网关分组', 1);

-- ----------------------------
-- Table structure for AppApiGatewaySecurityOptions
-- ----------------------------
DROP TABLE IF EXISTS `AppApiGatewaySecurityOptions`;
CREATE TABLE `AppApiGatewaySecurityOptions`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ReRouteId` bigint(20) NOT NULL,
  `IPAllowedList` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `IPBlockedList` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `IX_AppApiGatewaySecurityOptions_ReRouteId`(`ReRouteId`) USING BTREE,
  CONSTRAINT `FK_AppApiGatewaySecurityOptions_AppApiGatewayReRoute_ReRouteId` FOREIGN KEY (`ReRouteId`) REFERENCES `AppApiGatewayReRoute` (`ReRouteId`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 64 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of AppApiGatewaySecurityOptions
-- ----------------------------
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (3, 1261299170387169280, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (4, 1261585859064872960, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (5, 1261586605810368512, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (6, 1261587558609436672, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (7, 1261588213298348032, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (8, 1261588367619375104, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (9, 1261588628450557952, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (10, 1261588881564221440, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (11, 1261588983053795328, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (12, 1261589139039961088, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (13, 1261589197483393024, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (14, 1261589278857084928, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (15, 1261589420356124672, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (16, 1261589960393736192, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (17, 1261606600242085888, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (18, 1261606689601732608, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (19, 1261681880965038080, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (20, 1261682144920977408, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (21, 1262220447629058048, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (22, 1262230734939758592, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (23, 1262296916350869504, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (24, 1262632376348594176, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (25, 1262632791869902848, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (26, 1262632904575045632, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (27, 1262632976616411136, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (28, 1262660336921235456, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (29, 1262660528277966848, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (30, 1262660706875625472, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (31, 1262660966393991168, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (32, 1262661109474283520, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (33, 1262663888804663296, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (34, 1262664024096133120, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (35, 1262664186252120064, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (36, 1262664357044178944, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (37, 1262664632928718848, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (38, 1262664751409418240, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (39, 1262664871274237952, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (40, 1262665026111164416, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (41, 1262665159905267712, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (42, 1262665329829105664, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (43, 1262665456471920640, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (44, 1262665628165754880, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (45, 1262666172682883072, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (47, 1262723402331885568, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (48, 1262935771746734080, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (49, 1262935906522304512, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (50, 1262936009924481024, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (52, 1263074419073593344, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (53, 1263075249394790400, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (54, 1263075593499684864, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (56, 1263101898440146944, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (57, 1263303878648569856, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (58, 1263304204797648896, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (59, 1263304872891555840, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (60, 1263305106250047488, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (61, 1263305244594970624, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (62, 1263305430536855552, '', '');
INSERT INTO `AppApiGatewaySecurityOptions` VALUES (63, 1263639172959174656, '', '');

-- ----------------------------
-- Table structure for __EFMigrationsHistory
-- ----------------------------
DROP TABLE IF EXISTS `__EFMigrationsHistory`;
CREATE TABLE `__EFMigrationsHistory`  (
  `MigrationId` varchar(95) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  PRIMARY KEY (`MigrationId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of __EFMigrationsHistory
-- ----------------------------
INSERT INTO `__EFMigrationsHistory` VALUES ('20200513034946_Migration-ApiGateway-MySql', '3.1.3');
INSERT INTO `__EFMigrationsHistory` VALUES ('20200513111130_Rename-Router-To-RouteGroup', '3.1.3');

-- ----------------------------
-- Table structure for cap.published
-- ----------------------------
DROP TABLE IF EXISTS `cap.published`;
CREATE TABLE `cap.published`  (
  `Id` bigint(20) NOT NULL,
  `Version` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Content` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `Retries` int(11) NULL DEFAULT NULL,
  `Added` datetime(0) NOT NULL,
  `ExpiresAt` datetime(0) NULL DEFAULT NULL,
  `StatusName` varchar(40) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IX_ExpiresAt`(`ExpiresAt`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for cap.received
-- ----------------------------
DROP TABLE IF EXISTS `cap.received`;
CREATE TABLE `cap.received`  (
  `Id` bigint(20) NOT NULL,
  `Version` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Name` varchar(400) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Group` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Content` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `Retries` int(11) NULL DEFAULT NULL,
  `Added` datetime(0) NOT NULL,
  `ExpiresAt` datetime(0) NULL DEFAULT NULL,
  `StatusName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IX_ExpiresAt`(`ExpiresAt`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

SET FOREIGN_KEY_CHECKS = 1;
