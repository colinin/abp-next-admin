/*
 Navicat MySQL Data Transfer

 Source Server         : 本机服务器
 Source Server Type    : MySQL
 Source Server Version : 80020
 Source Host           : localhost:3306
 Source Schema         : apigateway

 Target Server Type    : MySQL
 Target Server Version : 80020
 File Encoding         : 65001

 Date: 30/07/2020 16:54:44
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for __efmigrationshistory
-- ----------------------------
DROP TABLE IF EXISTS `__efmigrationshistory`;
CREATE TABLE `__efmigrationshistory`  (
  `MigrationId` varchar(95) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  PRIMARY KEY (`MigrationId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of __efmigrationshistory
-- ----------------------------
INSERT INTO `__efmigrationshistory` VALUES ('20200513034946_Migration-ApiGateway-MySql', '3.1.3');
INSERT INTO `__efmigrationshistory` VALUES ('20200513111130_Rename-Router-To-RouteGroup', '3.1.3');
INSERT INTO `__efmigrationshistory` VALUES ('20200618090102_Modify-ReRoute-Index-Unique', '3.1.4');

-- ----------------------------
-- Table structure for appapigatewayaggregate
-- ----------------------------
DROP TABLE IF EXISTS `appapigatewayaggregate`;
CREATE TABLE `appapigatewayaggregate`  (
  `Id` int(0) NOT NULL AUTO_INCREMENT,
  `ExtraProperties` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `AppId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `ReRouteId` bigint(0) NOT NULL,
  `ReRouteKeys` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `UpstreamPathTemplate` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `UpstreamHost` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ReRouteIsCaseSensitive` tinyint(1) NOT NULL DEFAULT 0,
  `Aggregator` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Priority` int(0) NULL DEFAULT NULL,
  `UpstreamHttpMethod` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 7 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of appapigatewayaggregate
-- ----------------------------
INSERT INTO `appapigatewayaggregate` VALUES (5, '{}', '324ada7e1f824c31ac113a3bf63dd725', 'TEST-APP', 'abp接口代理服务', 1263083077348196352, 'apigateway-definition,platform-definition,', '/api/abp/api-definition', '', 1, 'AbpApiDefinitionAggregator', NULL, '');
INSERT INTO `appapigatewayaggregate` VALUES (6, '{}', 'ac00ab19b3fd4a988cec490ca3a9ec22', 'TEST-APP', 'abp框架配置', 1263102116090970112, 'apigateway-configuration,platform-configuration,', '/api/abp/application-configuration', '', 1, 'AbpApiDefinitionAggregator', NULL, '');

-- ----------------------------
-- Table structure for appapigatewayaggregateconfig
-- ----------------------------
DROP TABLE IF EXISTS `appapigatewayaggregateconfig`;
CREATE TABLE `appapigatewayaggregateconfig`  (
  `Id` int(0) NOT NULL AUTO_INCREMENT,
  `ReRouteId` bigint(0) NOT NULL,
  `ReRouteKey` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Parameter` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `JsonPath` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `AggregateReRouteId` int(0) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IX_AppApiGatewayAggregateConfig_AggregateReRouteId`(`AggregateReRouteId`) USING BTREE,
  CONSTRAINT `FK_AppApiGatewayAggregateConfig_AppApiGatewayAggregate_Aggregat~` FOREIGN KEY (`AggregateReRouteId`) REFERENCES `appapigatewayaggregate` (`Id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for appapigatewayauthoptions
-- ----------------------------
DROP TABLE IF EXISTS `appapigatewayauthoptions`;
CREATE TABLE `appapigatewayauthoptions`  (
  `Id` int(0) NOT NULL AUTO_INCREMENT,
  `ReRouteId` bigint(0) NOT NULL,
  `AuthenticationProviderKey` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `AllowedScopes` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `IX_AppApiGatewayAuthOptions_ReRouteId`(`ReRouteId`) USING BTREE,
  CONSTRAINT `FK_AppApiGatewayAuthOptions_AppApiGatewayReRoute_ReRouteId` FOREIGN KEY (`ReRouteId`) REFERENCES `appapigatewayreroute` (`ReRouteId`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 101 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of appapigatewayauthoptions
-- ----------------------------
INSERT INTO `appapigatewayauthoptions` VALUES (3, 1261299170387169280, NULL, '');
INSERT INTO `appapigatewayauthoptions` VALUES (4, 1261585859064872960, NULL, '');
INSERT INTO `appapigatewayauthoptions` VALUES (5, 1261586605810368512, NULL, '');
INSERT INTO `appapigatewayauthoptions` VALUES (6, 1261587558609436672, NULL, '');
INSERT INTO `appapigatewayauthoptions` VALUES (7, 1261588213298348032, NULL, '');
INSERT INTO `appapigatewayauthoptions` VALUES (8, 1261588367619375104, NULL, '');
INSERT INTO `appapigatewayauthoptions` VALUES (9, 1261588628450557952, NULL, '');
INSERT INTO `appapigatewayauthoptions` VALUES (10, 1261588881564221440, NULL, '');
INSERT INTO `appapigatewayauthoptions` VALUES (11, 1261588983053795328, NULL, '');
INSERT INTO `appapigatewayauthoptions` VALUES (12, 1261589139039961088, NULL, '');
INSERT INTO `appapigatewayauthoptions` VALUES (13, 1261589197483393024, NULL, '');
INSERT INTO `appapigatewayauthoptions` VALUES (14, 1261589278857084928, NULL, '');
INSERT INTO `appapigatewayauthoptions` VALUES (15, 1261589420356124672, NULL, '');
INSERT INTO `appapigatewayauthoptions` VALUES (16, 1261589960393736192, NULL, '');
INSERT INTO `appapigatewayauthoptions` VALUES (17, 1261606600242085888, NULL, '');
INSERT INTO `appapigatewayauthoptions` VALUES (18, 1261606689601732608, NULL, '');
INSERT INTO `appapigatewayauthoptions` VALUES (19, 1261681880965038080, NULL, '');
INSERT INTO `appapigatewayauthoptions` VALUES (20, 1261682144920977408, NULL, '');
INSERT INTO `appapigatewayauthoptions` VALUES (21, 1262220447629058048, NULL, '');
INSERT INTO `appapigatewayauthoptions` VALUES (22, 1262230734939758592, NULL, '');
INSERT INTO `appapigatewayauthoptions` VALUES (23, 1262296916350869504, NULL, '');
INSERT INTO `appapigatewayauthoptions` VALUES (24, 1262632376348594176, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (25, 1262632791869902848, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (26, 1262632904575045632, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (27, 1262632976616411136, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (28, 1262660336921235456, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (29, 1262660528277966848, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (30, 1262660706875625472, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (31, 1262660966393991168, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (32, 1262661109474283520, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (33, 1262663888804663296, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (34, 1262664024096133120, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (35, 1262664186252120064, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (36, 1262664357044178944, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (37, 1262664632928718848, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (38, 1262664751409418240, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (39, 1262664871274237952, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (40, 1262665026111164416, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (41, 1262665159905267712, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (42, 1262665329829105664, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (43, 1262665456471920640, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (44, 1262665628165754880, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (45, 1262666172682883072, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (47, 1262723402331885568, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (48, 1262935771746734080, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (49, 1262935906522304512, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (50, 1262936009924481024, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (52, 1263074419073593344, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (53, 1263075249394790400, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (54, 1263075593499684864, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (56, 1263101898440146944, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (57, 1263303878648569856, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (58, 1263304204797648896, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (59, 1263304872891555840, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (60, 1263305106250047488, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (61, 1263305244594970624, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (62, 1263305430536855552, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (63, 1263639172959174656, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (64, 1264799968944640000, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (65, 1264800070161584128, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (66, 1267360794414161920, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (67, 1267383367629807616, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (68, 1267817055527632896, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (69, 1267817221286526976, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (70, 1268893687085518848, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (71, 1273527659565547520, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (72, 1273542336509079552, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (73, 1273542755520049152, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (74, 1273543111322857472, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (75, 1273543850526994432, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (76, 1273544135009857536, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (77, 1273544377432240128, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (78, 1273544549834911744, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (79, 1273545100509278208, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (80, 1273545462146363392, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (81, 1273545765801390080, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (82, 1273877904709361664, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (83, 1273878097483767808, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (84, 1273878425839050752, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (85, 1273879014778052608, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (86, 1273879265417076736, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (87, 1273879533101752320, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (88, 1274294160109314048, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (89, 1274524600855441408, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (90, 1274543888438525952, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (91, 1285579388652576768, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (92, 1285580096881778688, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (93, 1285582774663864320, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (94, 1288657613998579712, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (95, 1288657941770854400, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (96, 1288658134067109888, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (97, 1288658305156964352, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (98, 1288658491216289792, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (99, 1288658638302142464, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (100, 1288658791784308736, '', '');

-- ----------------------------
-- Table structure for appapigatewaybalanceroptions
-- ----------------------------
DROP TABLE IF EXISTS `appapigatewaybalanceroptions`;
CREATE TABLE `appapigatewaybalanceroptions`  (
  `Id` int(0) NOT NULL AUTO_INCREMENT,
  `ItemId` bigint(0) NULL DEFAULT NULL,
  `ReRouteId` bigint(0) NULL DEFAULT NULL,
  `Type` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Key` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Expiry` int(0) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `IX_AppApiGatewayBalancerOptions_ItemId`(`ItemId`) USING BTREE,
  UNIQUE INDEX `IX_AppApiGatewayBalancerOptions_ReRouteId`(`ReRouteId`) USING BTREE,
  CONSTRAINT `FK_AppApiGatewayBalancerOptions_AppApiGatewayGlobalConfiguratio~` FOREIGN KEY (`ItemId`) REFERENCES `appapigatewayglobalconfiguration` (`ItemId`) ON DELETE CASCADE ON UPDATE RESTRICT,
  CONSTRAINT `FK_AppApiGatewayBalancerOptions_AppApiGatewayReRoute_ReRouteId` FOREIGN KEY (`ReRouteId`) REFERENCES `appapigatewayreroute` (`ReRouteId`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 104 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of appapigatewaybalanceroptions
-- ----------------------------
INSERT INTO `appapigatewaybalanceroptions` VALUES (1, 1260841964962947072, NULL, 'LeastConnection', NULL, NULL);
INSERT INTO `appapigatewaybalanceroptions` VALUES (4, NULL, 1261299170387169280, 'LeastConnection', NULL, 60000);
INSERT INTO `appapigatewaybalanceroptions` VALUES (5, NULL, 1261585859064872960, NULL, NULL, NULL);
INSERT INTO `appapigatewaybalanceroptions` VALUES (6, NULL, 1261586605810368512, NULL, NULL, NULL);
INSERT INTO `appapigatewaybalanceroptions` VALUES (7, NULL, 1261587558609436672, NULL, NULL, NULL);
INSERT INTO `appapigatewaybalanceroptions` VALUES (8, NULL, 1261588213298348032, NULL, NULL, NULL);
INSERT INTO `appapigatewaybalanceroptions` VALUES (9, NULL, 1261588367619375104, NULL, NULL, NULL);
INSERT INTO `appapigatewaybalanceroptions` VALUES (10, NULL, 1261588628450557952, NULL, NULL, NULL);
INSERT INTO `appapigatewaybalanceroptions` VALUES (11, NULL, 1261588881564221440, NULL, NULL, NULL);
INSERT INTO `appapigatewaybalanceroptions` VALUES (12, NULL, 1261588983053795328, NULL, NULL, NULL);
INSERT INTO `appapigatewaybalanceroptions` VALUES (13, NULL, 1261589139039961088, NULL, NULL, NULL);
INSERT INTO `appapigatewaybalanceroptions` VALUES (14, NULL, 1261589197483393024, NULL, NULL, NULL);
INSERT INTO `appapigatewaybalanceroptions` VALUES (15, NULL, 1261589278857084928, NULL, NULL, NULL);
INSERT INTO `appapigatewaybalanceroptions` VALUES (16, NULL, 1261589420356124672, NULL, NULL, NULL);
INSERT INTO `appapigatewaybalanceroptions` VALUES (17, NULL, 1261589960393736192, 'LeastConnection', NULL, 60000);
INSERT INTO `appapigatewaybalanceroptions` VALUES (18, NULL, 1261606600242085888, NULL, NULL, NULL);
INSERT INTO `appapigatewaybalanceroptions` VALUES (19, NULL, 1261606689601732608, NULL, NULL, NULL);
INSERT INTO `appapigatewaybalanceroptions` VALUES (20, NULL, 1261681880965038080, NULL, NULL, NULL);
INSERT INTO `appapigatewaybalanceroptions` VALUES (21, NULL, 1261682144920977408, NULL, NULL, NULL);
INSERT INTO `appapigatewaybalanceroptions` VALUES (22, NULL, 1262220447629058048, NULL, NULL, NULL);
INSERT INTO `appapigatewaybalanceroptions` VALUES (23, NULL, 1262230734939758592, NULL, NULL, NULL);
INSERT INTO `appapigatewaybalanceroptions` VALUES (24, NULL, 1262296916350869504, NULL, NULL, NULL);
INSERT INTO `appapigatewaybalanceroptions` VALUES (25, NULL, 1262632376348594176, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (26, NULL, 1262632791869902848, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (27, NULL, 1262632904575045632, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (28, NULL, 1262632976616411136, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (29, NULL, 1262660336921235456, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (30, NULL, 1262660528277966848, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (31, NULL, 1262660706875625472, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (32, NULL, 1262660966393991168, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (33, NULL, 1262661109474283520, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (34, NULL, 1262663888804663296, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (35, NULL, 1262664024096133120, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (36, NULL, 1262664186252120064, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (37, NULL, 1262664357044178944, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (38, NULL, 1262664632928718848, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (39, NULL, 1262664751409418240, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (40, NULL, 1262664871274237952, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (41, NULL, 1262665026111164416, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (42, NULL, 1262665159905267712, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (43, NULL, 1262665329829105664, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (44, NULL, 1262665456471920640, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (45, NULL, 1262665628165754880, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (46, NULL, 1262666172682883072, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (48, NULL, 1262723402331885568, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (49, NULL, 1262935771746734080, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (50, NULL, 1262935906522304512, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (51, NULL, 1262936009924481024, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (53, NULL, 1263074419073593344, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (54, NULL, 1263075249394790400, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (55, NULL, 1263075593499684864, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (57, NULL, 1263101898440146944, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (58, NULL, 1263303878648569856, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (59, NULL, 1263304204797648896, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (60, NULL, 1263304872891555840, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (61, NULL, 1263305106250047488, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (62, NULL, 1263305244594970624, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (63, NULL, 1263305430536855552, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (64, NULL, 1263639172959174656, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (65, NULL, 1264799968944640000, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (66, NULL, 1264800070161584128, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (67, 1265168245423443968, NULL, 'LeastConnection', '', 60000);
INSERT INTO `appapigatewaybalanceroptions` VALUES (68, NULL, 1267360794414161920, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (69, NULL, 1267383367629807616, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (70, NULL, 1267817055527632896, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (71, NULL, 1267817221286526976, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (72, NULL, 1268893687085518848, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (73, 1273519675519270912, NULL, 'RoundRobin', '', 60000);
INSERT INTO `appapigatewaybalanceroptions` VALUES (74, NULL, 1273527659565547520, 'LeastConnection', '', 60000);
INSERT INTO `appapigatewaybalanceroptions` VALUES (75, NULL, 1273542336509079552, 'LeastConnection', '', 60000);
INSERT INTO `appapigatewaybalanceroptions` VALUES (76, NULL, 1273542755520049152, 'LeastConnection', '', 60000);
INSERT INTO `appapigatewaybalanceroptions` VALUES (77, NULL, 1273543111322857472, 'LeastConnection', '', 60000);
INSERT INTO `appapigatewaybalanceroptions` VALUES (78, NULL, 1273543850526994432, 'LeastConnection', '', 60000);
INSERT INTO `appapigatewaybalanceroptions` VALUES (79, NULL, 1273544135009857536, 'LeastConnection', '', 60000);
INSERT INTO `appapigatewaybalanceroptions` VALUES (80, NULL, 1273544377432240128, 'LeastConnection', '', 60000);
INSERT INTO `appapigatewaybalanceroptions` VALUES (81, NULL, 1273544549834911744, 'LeastConnection', '', 60000);
INSERT INTO `appapigatewaybalanceroptions` VALUES (82, NULL, 1273545100509278208, 'LeastConnection', '', 60000);
INSERT INTO `appapigatewaybalanceroptions` VALUES (83, NULL, 1273545462146363392, 'LeastConnection', '', 60000);
INSERT INTO `appapigatewaybalanceroptions` VALUES (84, NULL, 1273545765801390080, 'LeastConnection', '', 60000);
INSERT INTO `appapigatewaybalanceroptions` VALUES (85, NULL, 1273877904709361664, 'LeastConnection', '', 60000);
INSERT INTO `appapigatewaybalanceroptions` VALUES (86, NULL, 1273878097483767808, 'LeastConnection', '', 60000);
INSERT INTO `appapigatewaybalanceroptions` VALUES (87, NULL, 1273878425839050752, 'LeastConnection', '', 60000);
INSERT INTO `appapigatewaybalanceroptions` VALUES (88, NULL, 1273879014778052608, 'LeastConnection', '', 60000);
INSERT INTO `appapigatewaybalanceroptions` VALUES (89, NULL, 1273879265417076736, 'LeastConnection', '', 60000);
INSERT INTO `appapigatewaybalanceroptions` VALUES (90, NULL, 1273879533101752320, 'LeastConnection', '', 6000);
INSERT INTO `appapigatewaybalanceroptions` VALUES (91, NULL, 1274294160109314048, 'LeastConnection', '', 60000);
INSERT INTO `appapigatewaybalanceroptions` VALUES (92, NULL, 1274524600855441408, 'LeastConnection', '', 60000);
INSERT INTO `appapigatewaybalanceroptions` VALUES (93, NULL, 1274543888438525952, 'LeastConnection', '', 60000);
INSERT INTO `appapigatewaybalanceroptions` VALUES (94, NULL, 1285579388652576768, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (95, NULL, 1285580096881778688, 'LeastConnection', '', 60000);
INSERT INTO `appapigatewaybalanceroptions` VALUES (96, NULL, 1285582774663864320, 'LeastConnection', '', 60000);
INSERT INTO `appapigatewaybalanceroptions` VALUES (97, NULL, 1288657613998579712, 'LeastConnection', '', 60000);
INSERT INTO `appapigatewaybalanceroptions` VALUES (98, NULL, 1288657941770854400, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (99, NULL, 1288658134067109888, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (100, NULL, 1288658305156964352, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (101, NULL, 1288658491216289792, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (102, NULL, 1288658638302142464, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (103, NULL, 1288658791784308736, '', '', 0);

-- ----------------------------
-- Table structure for appapigatewaycacheoptions
-- ----------------------------
DROP TABLE IF EXISTS `appapigatewaycacheoptions`;
CREATE TABLE `appapigatewaycacheoptions`  (
  `Id` int(0) NOT NULL AUTO_INCREMENT,
  `ReRouteId` bigint(0) NOT NULL,
  `TtlSeconds` int(0) NULL DEFAULT NULL,
  `Region` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `IX_AppApiGatewayCacheOptions_ReRouteId`(`ReRouteId`) USING BTREE,
  CONSTRAINT `FK_AppApiGatewayCacheOptions_AppApiGatewayReRoute_ReRouteId` FOREIGN KEY (`ReRouteId`) REFERENCES `appapigatewayreroute` (`ReRouteId`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 101 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of appapigatewaycacheoptions
-- ----------------------------
INSERT INTO `appapigatewaycacheoptions` VALUES (3, 1261299170387169280, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (4, 1261585859064872960, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (5, 1261586605810368512, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (6, 1261587558609436672, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (7, 1261588213298348032, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (8, 1261588367619375104, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (9, 1261588628450557952, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (10, 1261588881564221440, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (11, 1261588983053795328, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (12, 1261589139039961088, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (13, 1261589197483393024, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (14, 1261589278857084928, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (15, 1261589420356124672, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (16, 1261589960393736192, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (17, 1261606600242085888, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (18, 1261606689601732608, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (19, 1261681880965038080, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (20, 1261682144920977408, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (21, 1262220447629058048, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (22, 1262230734939758592, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (23, 1262296916350869504, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (24, 1262632376348594176, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (25, 1262632791869902848, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (26, 1262632904575045632, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (27, 1262632976616411136, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (28, 1262660336921235456, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (29, 1262660528277966848, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (30, 1262660706875625472, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (31, 1262660966393991168, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (32, 1262661109474283520, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (33, 1262663888804663296, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (34, 1262664024096133120, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (35, 1262664186252120064, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (36, 1262664357044178944, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (37, 1262664632928718848, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (38, 1262664751409418240, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (39, 1262664871274237952, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (40, 1262665026111164416, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (41, 1262665159905267712, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (42, 1262665329829105664, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (43, 1262665456471920640, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (44, 1262665628165754880, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (45, 1262666172682883072, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (47, 1262723402331885568, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (48, 1262935771746734080, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (49, 1262935906522304512, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (50, 1262936009924481024, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (52, 1263074419073593344, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (53, 1263075249394790400, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (54, 1263075593499684864, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (56, 1263101898440146944, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (57, 1263303878648569856, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (58, 1263304204797648896, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (59, 1263304872891555840, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (60, 1263305106250047488, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (61, 1263305244594970624, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (62, 1263305430536855552, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (63, 1263639172959174656, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (64, 1264799968944640000, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (65, 1264800070161584128, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (66, 1267360794414161920, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (67, 1267383367629807616, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (68, 1267817055527632896, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (69, 1267817221286526976, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (70, 1268893687085518848, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (71, 1273527659565547520, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (72, 1273542336509079552, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (73, 1273542755520049152, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (74, 1273543111322857472, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (75, 1273543850526994432, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (76, 1273544135009857536, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (77, 1273544377432240128, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (78, 1273544549834911744, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (79, 1273545100509278208, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (80, 1273545462146363392, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (81, 1273545765801390080, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (82, 1273877904709361664, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (83, 1273878097483767808, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (84, 1273878425839050752, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (85, 1273879014778052608, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (86, 1273879265417076736, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (87, 1273879533101752320, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (88, 1274294160109314048, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (89, 1274524600855441408, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (90, 1274543888438525952, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (91, 1285579388652576768, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (92, 1285580096881778688, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (93, 1285582774663864320, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (94, 1288657613998579712, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (95, 1288657941770854400, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (96, 1288658134067109888, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (97, 1288658305156964352, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (98, 1288658491216289792, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (99, 1288658638302142464, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (100, 1288658791784308736, 0, '');

-- ----------------------------
-- Table structure for appapigatewaydiscovery
-- ----------------------------
DROP TABLE IF EXISTS `appapigatewaydiscovery`;
CREATE TABLE `appapigatewaydiscovery`  (
  `Id` int(0) NOT NULL AUTO_INCREMENT,
  `ItemId` bigint(0) NOT NULL,
  `Host` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Port` int(0) NULL DEFAULT NULL,
  `Type` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Token` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `ConfigurationKey` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `PollingInterval` int(0) NULL DEFAULT NULL,
  `Namespace` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Scheme` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `IX_AppApiGatewayDiscovery_ItemId`(`ItemId`) USING BTREE,
  CONSTRAINT `FK_AppApiGatewayDiscovery_AppApiGatewayGlobalConfiguration_Item~` FOREIGN KEY (`ItemId`) REFERENCES `appapigatewayglobalconfiguration` (`ItemId`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of appapigatewaydiscovery
-- ----------------------------
INSERT INTO `appapigatewaydiscovery` VALUES (1, 1260841964962947072, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `appapigatewaydiscovery` VALUES (2, 1265168245423443968, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);
INSERT INTO `appapigatewaydiscovery` VALUES (3, 1273519675519270912, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

-- ----------------------------
-- Table structure for appapigatewaydynamicreroute
-- ----------------------------
DROP TABLE IF EXISTS `appapigatewaydynamicreroute`;
CREATE TABLE `appapigatewaydynamicreroute`  (
  `Id` int(0) NOT NULL AUTO_INCREMENT,
  `ExtraProperties` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `DynamicReRouteId` bigint(0) NOT NULL,
  `ServiceName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `DownstreamHttpVersion` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `AppId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `AK_AppApiGatewayDynamicReRoute_DynamicReRouteId`(`DynamicReRouteId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for appapigatewayglobalconfiguration
-- ----------------------------
DROP TABLE IF EXISTS `appapigatewayglobalconfiguration`;
CREATE TABLE `appapigatewayglobalconfiguration`  (
  `Id` int(0) NOT NULL AUTO_INCREMENT,
  `ExtraProperties` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `ItemId` bigint(0) NOT NULL,
  `RequestIdKey` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `BaseUrl` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `DownstreamScheme` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `DownstreamHttpVersion` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT 0,
  `IsActive` tinyint(1) NOT NULL,
  `AppId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `AK_AppApiGatewayGlobalConfiguration_ItemId`(`ItemId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of appapigatewayglobalconfiguration
-- ----------------------------
INSERT INTO `appapigatewayglobalconfiguration` VALUES (1, '{}', 'f7973118f2c2425c8cc96b59883b99aa', 1260841964962947072, NULL, 'http://localhost:30000', 'HTTP', NULL, 0, 1, 'TEST-APP');

-- ----------------------------
-- Table structure for appapigatewayheaders
-- ----------------------------
DROP TABLE IF EXISTS `appapigatewayheaders`;
CREATE TABLE `appapigatewayheaders`  (
  `Id` int(0) NOT NULL AUTO_INCREMENT,
  `ReRouteId` bigint(0) NOT NULL,
  `Key` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `Value` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for appapigatewayhostandport
-- ----------------------------
DROP TABLE IF EXISTS `appapigatewayhostandport`;
CREATE TABLE `appapigatewayhostandport`  (
  `Id` int(0) NOT NULL AUTO_INCREMENT,
  `ReRouteId` bigint(0) NOT NULL,
  `Host` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Port` int(0) NULL DEFAULT 0,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for appapigatewayhttpoptions
-- ----------------------------
DROP TABLE IF EXISTS `appapigatewayhttpoptions`;
CREATE TABLE `appapigatewayhttpoptions`  (
  `Id` int(0) NOT NULL AUTO_INCREMENT,
  `ItemId` bigint(0) NULL DEFAULT NULL,
  `ReRouteId` bigint(0) NULL DEFAULT NULL,
  `MaxConnectionsPerServer` int(0) NULL DEFAULT NULL,
  `AllowAutoRedirect` tinyint(1) NOT NULL,
  `UseCookieContainer` tinyint(1) NOT NULL,
  `UseTracing` tinyint(1) NOT NULL,
  `UseProxy` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `IX_AppApiGatewayHttpOptions_ItemId`(`ItemId`) USING BTREE,
  UNIQUE INDEX `IX_AppApiGatewayHttpOptions_ReRouteId`(`ReRouteId`) USING BTREE,
  CONSTRAINT `FK_AppApiGatewayHttpOptions_AppApiGatewayGlobalConfiguration_It~` FOREIGN KEY (`ItemId`) REFERENCES `appapigatewayglobalconfiguration` (`ItemId`) ON DELETE CASCADE ON UPDATE RESTRICT,
  CONSTRAINT `FK_AppApiGatewayHttpOptions_AppApiGatewayReRoute_ReRouteId` FOREIGN KEY (`ReRouteId`) REFERENCES `appapigatewayreroute` (`ReRouteId`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 104 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of appapigatewayhttpoptions
-- ----------------------------
INSERT INTO `appapigatewayhttpoptions` VALUES (1, 1260841964962947072, NULL, NULL, 0, 0, 1, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (4, NULL, 1261299170387169280, 1000, 1, 0, 1, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (5, NULL, 1261585859064872960, NULL, 0, 0, 1, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (6, NULL, 1261586605810368512, NULL, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (7, NULL, 1261587558609436672, NULL, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (8, NULL, 1261588213298348032, NULL, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (9, NULL, 1261588367619375104, NULL, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (10, NULL, 1261588628450557952, NULL, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (11, NULL, 1261588881564221440, NULL, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (12, NULL, 1261588983053795328, NULL, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (13, NULL, 1261589139039961088, NULL, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (14, NULL, 1261589197483393024, NULL, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (15, NULL, 1261589278857084928, NULL, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (16, NULL, 1261589420356124672, NULL, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (17, NULL, 1261589960393736192, 1000, 1, 0, 1, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (18, NULL, 1261606600242085888, NULL, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (19, NULL, 1261606689601732608, NULL, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (20, NULL, 1261681880965038080, 100, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (21, NULL, 1261682144920977408, 100, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (22, NULL, 1262220447629058048, NULL, 0, 0, 1, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (23, NULL, 1262230734939758592, NULL, 0, 0, 1, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (24, NULL, 1262296916350869504, NULL, 0, 0, 1, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (25, NULL, 1262632376348594176, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (26, NULL, 1262632791869902848, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (27, NULL, 1262632904575045632, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (28, NULL, 1262632976616411136, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (29, NULL, 1262660336921235456, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (30, NULL, 1262660528277966848, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (31, NULL, 1262660706875625472, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (32, NULL, 1262660966393991168, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (33, NULL, 1262661109474283520, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (34, NULL, 1262663888804663296, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (35, NULL, 1262664024096133120, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (36, NULL, 1262664186252120064, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (37, NULL, 1262664357044178944, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (38, NULL, 1262664632928718848, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (39, NULL, 1262664751409418240, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (40, NULL, 1262664871274237952, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (41, NULL, 1262665026111164416, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (42, NULL, 1262665159905267712, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (43, NULL, 1262665329829105664, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (44, NULL, 1262665456471920640, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (45, NULL, 1262665628165754880, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (46, NULL, 1262666172682883072, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (48, NULL, 1262723402331885568, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (49, NULL, 1262935771746734080, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (50, NULL, 1262935906522304512, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (51, NULL, 1262936009924481024, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (53, NULL, 1263074419073593344, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (54, NULL, 1263075249394790400, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (55, NULL, 1263075593499684864, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (57, NULL, 1263101898440146944, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (58, NULL, 1263303878648569856, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (59, NULL, 1263304204797648896, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (60, NULL, 1263304872891555840, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (61, NULL, 1263305106250047488, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (62, NULL, 1263305244594970624, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (63, NULL, 1263305430536855552, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (64, NULL, 1263639172959174656, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (65, NULL, 1264799968944640000, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (66, NULL, 1264800070161584128, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (67, 1265168245423443968, NULL, 1000, 0, 0, 1, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (68, NULL, 1267360794414161920, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (69, NULL, 1267383367629807616, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (70, NULL, 1267817055527632896, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (71, NULL, 1267817221286526976, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (72, NULL, 1268893687085518848, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (73, 1273519675519270912, NULL, 1000, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (74, NULL, 1273527659565547520, 1000, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (75, NULL, 1273542336509079552, 1000, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (76, NULL, 1273542755520049152, 1000, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (77, NULL, 1273543111322857472, 1000, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (78, NULL, 1273543850526994432, 1000, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (79, NULL, 1273544135009857536, 1000, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (80, NULL, 1273544377432240128, 1000, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (81, NULL, 1273544549834911744, 1000, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (82, NULL, 1273545100509278208, 10000, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (83, NULL, 1273545462146363392, 1000, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (84, NULL, 1273545765801390080, 1000, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (85, NULL, 1273877904709361664, 1000, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (86, NULL, 1273878097483767808, 1000, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (87, NULL, 1273878425839050752, 1000, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (88, NULL, 1273879014778052608, 1000, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (89, NULL, 1273879265417076736, 1000, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (90, NULL, 1273879533101752320, 1000, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (91, NULL, 1274294160109314048, 1000, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (92, NULL, 1274524600855441408, 1000, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (93, NULL, 1274543888438525952, 1000, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (94, NULL, 1285579388652576768, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (95, NULL, 1285580096881778688, 1000, 0, 0, 1, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (96, NULL, 1285582774663864320, 1000, 0, 0, 1, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (97, NULL, 1288657613998579712, 1000, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (98, NULL, 1288657941770854400, 1000, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (99, NULL, 1288658134067109888, 1000, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (100, NULL, 1288658305156964352, 1000, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (101, NULL, 1288658491216289792, 1000, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (102, NULL, 1288658638302142464, 1000, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (103, NULL, 1288658791784308736, 1000, 0, 0, 0, 0);

-- ----------------------------
-- Table structure for appapigatewayqosoptions
-- ----------------------------
DROP TABLE IF EXISTS `appapigatewayqosoptions`;
CREATE TABLE `appapigatewayqosoptions`  (
  `Id` int(0) NOT NULL AUTO_INCREMENT,
  `ItemId` bigint(0) NULL DEFAULT NULL,
  `ReRouteId` bigint(0) NULL DEFAULT NULL,
  `ExceptionsAllowedBeforeBreaking` int(0) NULL DEFAULT NULL,
  `DurationOfBreak` int(0) NULL DEFAULT NULL,
  `TimeoutValue` int(0) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `IX_AppApiGatewayQoSOptions_ItemId`(`ItemId`) USING BTREE,
  UNIQUE INDEX `IX_AppApiGatewayQoSOptions_ReRouteId`(`ReRouteId`) USING BTREE,
  CONSTRAINT `FK_AppApiGatewayQoSOptions_AppApiGatewayGlobalConfiguration_Ite~` FOREIGN KEY (`ItemId`) REFERENCES `appapigatewayglobalconfiguration` (`ItemId`) ON DELETE CASCADE ON UPDATE RESTRICT,
  CONSTRAINT `FK_AppApiGatewayQoSOptions_AppApiGatewayReRoute_ReRouteId` FOREIGN KEY (`ReRouteId`) REFERENCES `appapigatewayreroute` (`ReRouteId`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 104 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of appapigatewayqosoptions
-- ----------------------------
INSERT INTO `appapigatewayqosoptions` VALUES (1, 1260841964962947072, NULL, 60, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (4, NULL, 1261299170387169280, 60, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (5, NULL, 1261585859064872960, 60, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (6, NULL, 1261586605810368512, 60, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (7, NULL, 1261587558609436672, 60, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (8, NULL, 1261588213298348032, 60, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (9, NULL, 1261588367619375104, 60, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (10, NULL, 1261588628450557952, 60, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (11, NULL, 1261588881564221440, 60, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (12, NULL, 1261588983053795328, 60, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (13, NULL, 1261589139039961088, 60, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (14, NULL, 1261589197483393024, 60, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (15, NULL, 1261589278857084928, 60, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (16, NULL, 1261589420356124672, 60, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (17, NULL, 1261589960393736192, 60, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (18, NULL, 1261606600242085888, NULL, NULL, NULL);
INSERT INTO `appapigatewayqosoptions` VALUES (19, NULL, 1261606689601732608, NULL, NULL, NULL);
INSERT INTO `appapigatewayqosoptions` VALUES (20, NULL, 1261681880965038080, 60, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (21, NULL, 1261682144920977408, 60, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (22, NULL, 1262220447629058048, 60, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (23, NULL, 1262230734939758592, 60, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (24, NULL, 1262296916350869504, 60, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (25, NULL, 1262632376348594176, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (26, NULL, 1262632791869902848, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (27, NULL, 1262632904575045632, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (28, NULL, 1262632976616411136, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (29, NULL, 1262660336921235456, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (30, NULL, 1262660528277966848, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (31, NULL, 1262660706875625472, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (32, NULL, 1262660966393991168, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (33, NULL, 1262661109474283520, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (34, NULL, 1262663888804663296, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (35, NULL, 1262664024096133120, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (36, NULL, 1262664186252120064, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (37, NULL, 1262664357044178944, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (38, NULL, 1262664632928718848, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (39, NULL, 1262664751409418240, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (40, NULL, 1262664871274237952, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (41, NULL, 1262665026111164416, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (42, NULL, 1262665159905267712, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (43, NULL, 1262665329829105664, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (44, NULL, 1262665456471920640, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (45, NULL, 1262665628165754880, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (46, NULL, 1262666172682883072, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (48, NULL, 1262723402331885568, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (49, NULL, 1262935771746734080, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (50, NULL, 1262935906522304512, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (51, NULL, 1262936009924481024, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (53, NULL, 1263074419073593344, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (54, NULL, 1263075249394790400, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (55, NULL, 1263075593499684864, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (57, NULL, 1263101898440146944, 50, 60000, 120000);
INSERT INTO `appapigatewayqosoptions` VALUES (58, NULL, 1263303878648569856, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (59, NULL, 1263304204797648896, 50, 60000, 120000);
INSERT INTO `appapigatewayqosoptions` VALUES (60, NULL, 1263304872891555840, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (61, NULL, 1263305106250047488, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (62, NULL, 1263305244594970624, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (63, NULL, 1263305430536855552, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (64, NULL, 1263639172959174656, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (65, NULL, 1264799968944640000, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (66, NULL, 1264800070161584128, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (67, 1265168245423443968, NULL, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (68, NULL, 1267360794414161920, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (69, NULL, 1267383367629807616, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (70, NULL, 1267817055527632896, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (71, NULL, 1267817221286526976, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (72, NULL, 1268893687085518848, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (73, 1273519675519270912, NULL, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (74, NULL, 1273527659565547520, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (75, NULL, 1273542336509079552, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (76, NULL, 1273542755520049152, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (77, NULL, 1273543111322857472, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (78, NULL, 1273543850526994432, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (79, NULL, 1273544135009857536, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (80, NULL, 1273544377432240128, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (81, NULL, 1273544549834911744, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (82, NULL, 1273545100509278208, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (83, NULL, 1273545462146363392, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (84, NULL, 1273545765801390080, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (85, NULL, 1273877904709361664, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (86, NULL, 1273878097483767808, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (87, NULL, 1273878425839050752, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (88, NULL, 1273879014778052608, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (89, NULL, 1273879265417076736, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (90, NULL, 1273879533101752320, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (91, NULL, 1274294160109314048, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (92, NULL, 1274524600855441408, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (93, NULL, 1274543888438525952, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (94, NULL, 1285579388652576768, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (95, NULL, 1285580096881778688, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (96, NULL, 1285582774663864320, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (97, NULL, 1288657613998579712, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (98, NULL, 1288657941770854400, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (99, NULL, 1288658134067109888, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (100, NULL, 1288658305156964352, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (101, NULL, 1288658491216289792, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (102, NULL, 1288658638302142464, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (103, NULL, 1288658791784308736, 50, 60000, 30000);

-- ----------------------------
-- Table structure for appapigatewayratelimitoptions
-- ----------------------------
DROP TABLE IF EXISTS `appapigatewayratelimitoptions`;
CREATE TABLE `appapigatewayratelimitoptions`  (
  `Id` int(0) NOT NULL AUTO_INCREMENT,
  `ItemId` bigint(0) NOT NULL,
  `ClientIdHeader` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT 'ClientId',
  `QuotaExceededMessage` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `RateLimitCounterPrefix` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT 'ocelot',
  `DisableRateLimitHeaders` tinyint(1) NOT NULL,
  `HttpStatusCode` int(0) NULL DEFAULT 429,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `IX_AppApiGatewayRateLimitOptions_ItemId`(`ItemId`) USING BTREE,
  CONSTRAINT `FK_AppApiGatewayRateLimitOptions_AppApiGatewayGlobalConfigurati~` FOREIGN KEY (`ItemId`) REFERENCES `appapigatewayglobalconfiguration` (`ItemId`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of appapigatewayratelimitoptions
-- ----------------------------
INSERT INTO `appapigatewayratelimitoptions` VALUES (1, 1260841964962947072, 'ClientId', '您的操作过快,请稍后再试!', 'ocelot', 1, 429);
INSERT INTO `appapigatewayratelimitoptions` VALUES (2, 1265168245423443968, 'ClientId', '{\n  \"error\": {\n    \"code\": \"429\",\n    \"message\": \"对不起,在处理您的请求期间出现了一个服务器内部错误!\",\n    \"details\": \"您的操作过快,请稍后再试!\",\n    \"validationErrors\": []\n  }\n}', 'ocelot', 1, 429);
INSERT INTO `appapigatewayratelimitoptions` VALUES (3, 1273519675519270912, 'ClientId', NULL, 'ocelot', 1, 429);

-- ----------------------------
-- Table structure for appapigatewayratelimitrule
-- ----------------------------
DROP TABLE IF EXISTS `appapigatewayratelimitrule`;
CREATE TABLE `appapigatewayratelimitrule`  (
  `Id` int(0) NOT NULL AUTO_INCREMENT,
  `ReRouteId` bigint(0) NULL DEFAULT NULL,
  `DynamicReRouteId` bigint(0) NULL DEFAULT NULL,
  `ClientWhitelist` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `EnableRateLimiting` tinyint(1) NOT NULL,
  `Period` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `PeriodTimespan` double NULL DEFAULT NULL,
  `Limit` bigint(0) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `IX_AppApiGatewayRateLimitRule_DynamicReRouteId`(`DynamicReRouteId`) USING BTREE,
  UNIQUE INDEX `IX_AppApiGatewayRateLimitRule_ReRouteId`(`ReRouteId`) USING BTREE,
  CONSTRAINT `FK_AppApiGatewayRateLimitRule_AppApiGatewayDynamicReRoute_Dynam~` FOREIGN KEY (`DynamicReRouteId`) REFERENCES `appapigatewaydynamicreroute` (`DynamicReRouteId`) ON DELETE CASCADE ON UPDATE RESTRICT,
  CONSTRAINT `FK_AppApiGatewayRateLimitRule_AppApiGatewayReRoute_ReRouteId` FOREIGN KEY (`ReRouteId`) REFERENCES `appapigatewayreroute` (`ReRouteId`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 101 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of appapigatewayratelimitrule
-- ----------------------------
INSERT INTO `appapigatewayratelimitrule` VALUES (3, 1261299170387169280, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (4, 1261585859064872960, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (5, 1261586605810368512, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (6, 1261587558609436672, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (7, 1261588213298348032, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (8, 1261588367619375104, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (9, 1261588628450557952, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (10, 1261588881564221440, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (11, 1261588983053795328, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (12, 1261589139039961088, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (13, 1261589197483393024, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (14, 1261589278857084928, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (15, 1261589420356124672, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (16, 1261589960393736192, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (17, 1261606600242085888, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (18, 1261606689601732608, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (19, 1261681880965038080, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (20, 1261682144920977408, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (21, 1262220447629058048, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (22, 1262230734939758592, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (23, 1262296916350869504, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (24, 1262632376348594176, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (25, 1262632791869902848, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (26, 1262632904575045632, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (27, 1262632976616411136, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (28, 1262660336921235456, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (29, 1262660528277966848, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (30, 1262660706875625472, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (31, 1262660966393991168, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (32, 1262661109474283520, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (33, 1262663888804663296, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (34, 1262664024096133120, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (35, 1262664186252120064, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (36, 1262664357044178944, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (37, 1262664632928718848, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (38, 1262664751409418240, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (39, 1262664871274237952, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (40, 1262665026111164416, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (41, 1262665159905267712, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (42, 1262665329829105664, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (43, 1262665456471920640, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (44, 1262665628165754880, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (45, 1262666172682883072, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (47, 1262723402331885568, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (48, 1262935771746734080, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (49, 1262935906522304512, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (50, 1262936009924481024, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (52, 1263074419073593344, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (53, 1263075249394790400, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (54, 1263075593499684864, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (56, 1263101898440146944, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (57, 1263303878648569856, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (58, 1263304204797648896, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (59, 1263304872891555840, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (60, 1263305106250047488, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (61, 1263305244594970624, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (62, 1263305430536855552, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (63, 1263639172959174656, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (64, 1264799968944640000, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (65, 1264800070161584128, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (66, 1267360794414161920, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (67, 1267383367629807616, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (68, 1267817055527632896, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (69, 1267817221286526976, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (70, 1268893687085518848, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (71, 1273527659565547520, NULL, 'apigateway-host-client,apigateway-admin-client', 1, '1m', 60, 200);
INSERT INTO `appapigatewayratelimitrule` VALUES (72, 1273542336509079552, NULL, '', 1, '1m', 60, 200);
INSERT INTO `appapigatewayratelimitrule` VALUES (73, 1273542755520049152, NULL, '', 1, '1m', 60, 200);
INSERT INTO `appapigatewayratelimitrule` VALUES (74, 1273543111322857472, NULL, '', 1, '1m', 60, 200);
INSERT INTO `appapigatewayratelimitrule` VALUES (75, 1273543850526994432, NULL, '', 1, '1m', 60, 200);
INSERT INTO `appapigatewayratelimitrule` VALUES (76, 1273544135009857536, NULL, '', 1, '1m', 60, 200);
INSERT INTO `appapigatewayratelimitrule` VALUES (77, 1273544377432240128, NULL, '', 1, '1m', 60, 200);
INSERT INTO `appapigatewayratelimitrule` VALUES (78, 1273544549834911744, NULL, '', 1, '1m', 60, 200);
INSERT INTO `appapigatewayratelimitrule` VALUES (79, 1273545100509278208, NULL, '', 1, '1m', 60, 200);
INSERT INTO `appapigatewayratelimitrule` VALUES (80, 1273545462146363392, NULL, '', 1, '1m', 60, 200);
INSERT INTO `appapigatewayratelimitrule` VALUES (81, 1273545765801390080, NULL, '', 1, '1m', 60, 200);
INSERT INTO `appapigatewayratelimitrule` VALUES (82, 1273877904709361664, NULL, '', 1, '1m', 60, 200);
INSERT INTO `appapigatewayratelimitrule` VALUES (83, 1273878097483767808, NULL, '', 1, '1m', 60, 200);
INSERT INTO `appapigatewayratelimitrule` VALUES (84, 1273878425839050752, NULL, '', 1, '1m', 60, 200);
INSERT INTO `appapigatewayratelimitrule` VALUES (85, 1273879014778052608, NULL, '', 1, '1m', 60, 200);
INSERT INTO `appapigatewayratelimitrule` VALUES (86, 1273879265417076736, NULL, '', 1, '1m', 60, 200);
INSERT INTO `appapigatewayratelimitrule` VALUES (87, 1273879533101752320, NULL, '', 1, '1m', 60, 200);
INSERT INTO `appapigatewayratelimitrule` VALUES (88, 1274294160109314048, NULL, '', 1, '1m', 60, 200);
INSERT INTO `appapigatewayratelimitrule` VALUES (89, 1274524600855441408, NULL, '', 1, '1m', 60, 200);
INSERT INTO `appapigatewayratelimitrule` VALUES (90, 1274543888438525952, NULL, '', 1, '1m', 60, 200);
INSERT INTO `appapigatewayratelimitrule` VALUES (91, 1285579388652576768, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (92, 1285580096881778688, NULL, '', 1, '1m', 60, 200);
INSERT INTO `appapigatewayratelimitrule` VALUES (93, 1285582774663864320, NULL, '', 1, '1m', 60, 200);
INSERT INTO `appapigatewayratelimitrule` VALUES (94, 1288657613998579712, NULL, '', 1, '1m', 60, 200);
INSERT INTO `appapigatewayratelimitrule` VALUES (95, 1288657941770854400, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (96, 1288658134067109888, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (97, 1288658305156964352, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (98, 1288658491216289792, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (99, 1288658638302142464, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (100, 1288658791784308736, NULL, '', 0, NULL, NULL, NULL);

-- ----------------------------
-- Table structure for appapigatewayreroute
-- ----------------------------
DROP TABLE IF EXISTS `appapigatewayreroute`;
CREATE TABLE `appapigatewayreroute`  (
  `Id` int(0) NOT NULL AUTO_INCREMENT,
  `ExtraProperties` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `ReRouteId` bigint(0) NOT NULL,
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
  `Priority` int(0) NULL DEFAULT NULL,
  `Timeout` int(0) NULL DEFAULT NULL,
  `DangerousAcceptAnyServerCertificateValidator` tinyint(1) NOT NULL,
  `DownstreamHttpVersion` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `AppId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `AK_AppApiGatewayReRoute_ReRouteId`(`ReRouteId`) USING BTREE,
  UNIQUE INDEX `IX_AppApiGatewayReRoute_AppId_DownstreamPathTemplate_UpstreamPa~`(`AppId`, `DownstreamPathTemplate`, `UpstreamPathTemplate`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 108 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of appapigatewayreroute
-- ----------------------------
INSERT INTO `appapigatewayreroute` VALUES (4, '{}', 'e4e1ea5c65ed4ab3baa912593372b51e', 1261299170387169280, '平台服务-权限管理', '/api/permission-management/permissions', '', NULL, '/api/abp/permissions', 'GET,PUT,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30010,', '', NULL, NULL, NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (5, '{}', 'd56f8c4547ec46219f5e7d2ec59e8bb3', 1261585859064872960, '身份认证服务-客户端管理', '/api/IdentityServer/Clients', '', NULL, '/api/IdentityServer/Clients', 'POST,GET,PUT,', '', '', 'X-Forwarded-For:{RemoteIpAddress},', '', '', '', NULL, 1, '', NULL, 'HTTP', '127.0.0.1:30010,', '', NULL, NULL, NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (6, '{}', '4d7c360bc40342c1b1cf16181e4ac2bb', 1261586605810368512, '身份认证服务-查询客户端', '/api/IdentityServer/Clients/{Id}', '', NULL, '/api/IdentityServer/Clients/{Id}', 'GET,DELETE,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30010,', '', NULL, NULL, NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (7, '{}', 'c222dcff22284b3e9ce9c424853705cd', 1261587558609436672, '服务网关管理-路由组管理', '/api/ApiGateway/RouteGroups', '', '', '/api/ApiGateway/RouteGroups', 'GET,POST,PUT,DELETE,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, '', NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (8, '{}', '4828b753ef5a45b1a8c7903b7977006f', 1261588213298348032, '服务网关管理-查询单个路由组', '/api/ApiGateway/RouteGroups/By-AppId/{AppId}', '', '', '/api/ApiGateway/RouteGroups/By-AppId/{AppId}', 'GET,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, '', NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (9, '{}', '7813111dea414ea6a48c498726c82e06', 1261588367619375104, '服务网关管理-查询所有有效路由组', '/api/ApiGateway/RouteGroups/Actived', '', '', '/api/ApiGateway/RouteGroups/Actived', 'GET,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, '', NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (10, '{}', 'a91c7f67e7cd4d9ca61b7d657213eaaf', 1261588628450557952, '服务网关管理-基础配置', '/api/ApiGateway/Globals', '', '', '/api/ApiGateway/Globals', 'GET,POST,PUT,DELETE,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, '', NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (11, '{}', 'fc3401e7942f4e63870fdd512372bf7d', 1261588881564221440, '服务网关管理-查询单个基础配置', '/api/ApiGateway/Globals/By-AppId/{AppId}', '', '', '/api/ApiGateway/Globals/By-AppId/{AppId}', 'GET,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, '', NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (12, '{}', '1298ef932a4845c7856ab0344e15daa9', 1261588983053795328, '服务网关管理-路由配置', '/api/ApiGateway/Routes', '', '', '/api/ApiGateway/Routes', 'GET,POST,PUT,DELETE,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, '', NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (13, '{}', 'd857a7a270cc44b38dfbd3d9dc7e62e6', 1261589139039961088, '服务网关管理-通过标识查询路由', '/api/ApiGateway/Routes/By-RouteId/{RouteId}', '', '', '/api/ApiGateway/Routes/By-RouteId/{RouteId}', 'GET,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, '', NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (14, '{}', 'aecd3c83218e490e8a48214dacacc88c', 1261589197483393024, '服务网关管理-通过名称查询路由', '/api/ApiGateway/Routes/By-RouteName/{RouteName}', '', '', '/api/ApiGateway/Routes/By-RouteName/{RouteName}', 'GET,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, '', NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (15, '{}', '73b62418ba424fa590113b58edf23e49', 1261589278857084928, '服务网关管理-通过应用标识查询路由', '/api/ApiGateway/Routes/By-AppId/{AppId}', '', '', '/api/ApiGateway/Routes/By-AppId/{AppId}', 'GET,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, '', NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (16, '{}', 'd096d74fb1d34619ba391dbd19a010ef', 1261589420356124672, '服务网关管理-清空应用标识下所有路由', '/api/ApiGateway/Routes/Clear', '', '', '/api/ApiGateway/Routes/Clear', 'DELETE,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, '', NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (17, '{}', '07fb9bc9e4604a24895dd60015856bc7', 1261589960393736192, '服务网关管理-通过应用标识查询动态路由', '/api/ApiGateway/DynamicRoutes/By-AppId/{AppId}', '', NULL, '/api/ApiGateway/DynamicRoutes/By-AppId/{AppId}', 'GET,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, NULL, NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (18, '{}', 'e659ebbf61534a978335cfeabdc0b375', 1261606600242085888, '服务网关管理-通过应用标识查询聚合路由', '/api/ApiGateway/Aggregates/by-AppId/{AppId}', '', NULL, '/api/ApiGateway/Aggregates/by-AppId/{AppId}', 'GET,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, NULL, NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (19, '{}', 'dd47edd7b03c42bc8666f4bded7cb991', 1261606689601732608, '服务网关管理-聚合路由', '/api/ApiGateway/Aggregates', '', NULL, '/api/ApiGateway/Aggregates', 'GET,POST,PUT,DELETE,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, NULL, NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (20, '{}', 'e9b65f34b85e4a52829204d1f687ec21', 1261681880965038080, '任务管理-任务列表', '/Task/Schedule/all', '', NULL, '/Task/Schedule/all', 'GET,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '10.21.15.14:40060,', '', NULL, NULL, NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (21, '{}', '6b9a3d81ed454ac0af2e3ccb4be7bd17', 1261682144920977408, '任务管理-任务日志', '/Task/Schedule/logs', '', NULL, '/Task/Schedule/logs', 'GET,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '10.21.15.14:40060,', '', NULL, NULL, NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (22, '{}', '5be66f8be176412a8e75aadd109b544a', 1262220447629058048, '身份认证服务-客户端密钥', '/api/IdentityServer/Clients/Secrets', '', NULL, '/api/IdentityServer/Clients/Secrets', 'PUT,POST,DELETE,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30010,', '', NULL, NULL, NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (23, '{}', '1cb56a15863f464e9b79db0d18acf4ba', 1262230734939758592, '身份认证服务-客户端声明', '/api/IdentityServer/Clients/Claims', '', NULL, '/api/IdentityServer/Clients/Claims', 'PUT,POST,DELETE,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30010,', '', NULL, NULL, NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (25, '{}', 'b6009df9b18c44b3aa2b77f27b0c2abb', 1262296916350869504, '身份认证服务-客户端属性', '/api/IdentityServer/Clients/Properties', '', NULL, '/api/IdentityServer/Clients/Properties', 'PUT,POST,DELETE,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30010,', '', NULL, NULL, NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (26, '{}', '401a7a8e8a2f41c599b1b87b55249a32', 1262632376348594176, '身份认证服务-Api资源管理', '/api/IdentityServer/ApiResources', '', '', '/api/IdentityServer/ApiResources', 'GET,POST,PUT,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (27, '{}', 'c662a4c29e654f9da6eb73ee456b533a', 1262632791869902848, '身份认证服务-单个Api资源', '/api/IdentityServer/ApiResources/{Id}', '', '', '/api/IdentityServer/ApiResources/{Id}', 'GET,DELETE,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (28, '{}', 'a0c2bdfdd0de4832b63d6dc3696c9c26', 1262632904575045632, '身份认证服务-Api资源密钥', '/api/IdentityServer/ApiResources/Secrets', '', '', '/api/IdentityServer/ApiResources/Secrets', 'DELETE,POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (29, '{}', 'fb0e7ca974f243ce9f4034b39bdda326', 1262632976616411136, '身份认证服务-Api资源作用域', '/api/IdentityServer/ApiResources/Scopes', '', '', '/api/IdentityServer/ApiResources/Scopes', 'DELETE,POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (30, '{}', '0f9875697b74420c9dc2eaf77099b210', 1262660336921235456, '身份认证服务-用户登录', '/api/account/login', '', '', '/api/account/login', 'POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (31, '{}', 'a890c6ecc6a64c9fa313a0f6b5406e1c', 1262660528277966848, '身份认证服务-用户登出', '/api/account/logout', '', '', '/api/account/logout', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (32, '{}', '88de580b6beb4d9d9d4367840ba1fcea', 1262660706875625472, '身份认证服务-检查密码', '/api/account/checkPassword', '', '', '/api/account/checkPassword', 'POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (33, '{}', '78f3c1adc7a54696af37a419eda47c62', 1262660966393991168, '身份认证服务-个人信息页', '/api/identity/my-profile', '', '', '/api/identity/my-profile', 'GET,PUT,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (34, '{}', '95b23aa5cebb40598a78c0761cfd0b26', 1262661109474283520, '身份认证服务-修改密码', '/api/identity/my-profile/change-password', '', '', '/api/identity/my-profile/change-password', 'POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (35, '{}', '4828f7c2aff8485189f37aba5de62d60', 1262663888804663296, '身份认证管理-角色管理', '/api/identity/roles', '', '', '/api/identity/roles', 'GET,POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (36, '{}', '0fddcd3b50a24c6795ec9034fdb44778', 1262664024096133120, '身份认证服务-角色列表', '/api/identity/roles/all', '', '', '/api/identity/roles/all', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (37, '{}', '191e555219e845069dfd93793263a840', 1262664186252120064, '身份认证服务-单个角色', '/api/identity/roles/{id}', '', '', '/api/identity/roles/{id}', 'GET,PUT,DELETE,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (38, '{}', 'c316858e82f74e6ca6e923d6b3a3fa76', 1262664357044178944, '身份认证服务-用户注册', '/api/account/register', '', '', '/api/account/register', 'POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (39, '{}', 'db53b6f957914a10a6a97ba306b1f6ef', 1262664632928718848, '身份认证服务-单个用户', '/api/identity/users/{id}', '', '', '/api/identity/users/{id}', 'GET,PUT,DELETE,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (40, '{}', '1833434b8ce34f8ab791e7e950f4c61f', 1262664751409418240, '身份认证服务-用户管理', '/api/identity/users', '', '', '/api/identity/users', 'GET,POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (41, '{}', 'b3c963a1612144918bffaf272697498c', 1262664871274237952, '身份认证服务-用户角色', '/api/identity/users/{id}/roles', '', '', '/api/identity/users/{id}/roles', 'GET,POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (42, '{}', '33dd757b79cb4f52994af13bfb4f6783', 1262665026111164416, '身份认证服务-通过用户名查询用户', '/api/identity/users/by-username/{userName}', '', '', '/api/identity/users/by-username/{userName}', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (43, '{}', 'e46fd6cb3a104da3aadfe0149fe4de68', 1262665159905267712, '身份认证服务-通过邮件查询用户', '/api/identity/users/by-email/{email}', '', '', '/api/identity/users/by-email/{email}', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (44, '{}', '8c8ec5ad6aaa4145981ee7ac876c36c9', 1262665329829105664, '身份认证服务-通过标识查询用户', '/api/identity/users/lookup/{id}', '', '', '/api/identity/users/lookup/{id}', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (45, '{}', 'f5c0c8c02c0846fdbe5015cd86f3d81b', 1262665456471920640, '身份认证服务-通过名称查询用户', '/api/identity/users/lookup/by-username/{userName}', '', '', '/api/identity/users/lookup/by-username/{userName}', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (46, '{}', '4de0c9e0437f40ce81628de956af9c5e', 1262665628165754880, '身份认证服务-通过名称查询租户', '/api/abp/multi-tenancy/tenants/by-name/{name}', '', '', '/api/abp/multi-tenancy/tenants/by-name/{name}', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (47, '{}', 'fa60a7253b2f4c80b9afad4e82ba6ba8', 1262666172682883072, '身份认证服务-通过标识查询租户', '/api/abp/multi-tenancy/tenants/by-id/{id}', '', '', '/api/abp/multi-tenancy/tenants/by-id/{id}', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (49, '{}', '4114a097b9e04a9e90458edf02ef41c7', 1262723402331885568, '身份认证服务-克隆客户端', '/api/IdentityServer/Clients/Clone', '', '', '/api/IdentityServer/Clients/Clone', 'POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (50, '{}', '2a9cc98d3ed0462d98c4bf0e946f410d', 1262935771746734080, '身份认证服务-身份资源管理', '/api/IdentityServer/IdentityResources', '', '', '/api/IdentityServer/IdentityResources', 'GET,POST,PUT,DELETE,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (51, '{}', '4d36b0ea5b92473ea5de3e91ff155830', 1262935906522304512, '身份认证服务-查询身份资源', '/api/IdentityServer/IdentityResources/{Id}', '', '', '/api/IdentityServer/IdentityResources/{Id}', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (52, '{}', '387df08693e54e91ac055a2324d4c10e', 1262936009924481024, '身份认证服务-身份资源属性', '/api/IdentityServer/IdentityResources/Properties', '', '', '/api/IdentityServer/IdentityResources/Properties', 'POST,DELETE,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (54, '{}', '22950f5be9cd434b8452a9da11cfc034', 1263074419073593344, '服务网关管理-abp代理接口', '/api/abp/api-definition', '', '', '/api/abp/apigateway/api-definition', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30001,', '', '', 'apigateway-definition', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (55, '{}', 'ca2cedfa620045a9adef0be2f958c4bc', 1263075249394790400, '服务网关管理-查询聚合路由', '/api/ApiGateway/Aggregates/{RouteId}', '', '', '/api/ApiGateway/Aggregates/{RouteId}', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30001,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (57, '{}', '98fbc99fc8644946ac0a72cc3dc5fd1f', 1263075593499684864, '服务网关管理-聚合路由配置', '/api/ApiGateway/Aggregates/RouteConfig', '', '', '/api/ApiGateway/Aggregates/RouteConfig', 'POST,DELETE,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30001,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (59, '{}', 'c692b30c72d4424eb4740ac49f4e9373', 1263101898440146944, '服务网关管理-框架配置', '/api/abp/application-configuration', '', '', '/api/abp/apigateway/application-configuration', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30001,', '', '', 'apigateway-configuration', 0, 120000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (60, '{}', 'd6b379ce336c4a33bc8d8ff2f99fad83', 1263303878648569856, '平台服务-abp代理接口', '/api/abp/api-definition', '', '', '/api/abp/platform/api-definition', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', 'platform-definition', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (61, '{}', 'bbfd4597517947808e5ea66883fbed18', 1263304204797648896, '平台服务-框架配置', '/api/abp/application-configuration', '', '', '/api/abp/platform/application-configuration', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', 'platform-configuration', 0, 120000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (62, '{}', '9810656b884947e3897e776b47208352', 1263304872891555840, '平台服务-租户管理', '/api/multi-tenancy/tenants', '', '', '/api/multi-tenancy/tenants', 'GET,POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (63, '{}', '12254ca25e15420faa694f62148dd694', 1263305106250047488, '平台服务-特定租户管理', '/api/multi-tenancy/tenants/{id}', '', '', '/api/multi-tenancy/tenants/{id}', 'GET,PUT,DELETE,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (64, '{}', '27761205c6344bfebbafbc077781ab76', 1263305244594970624, '平台服务-租户连接字符串', '/api/multi-tenancy/tenants/{id}/connection-string', '', '', '/api/multi-tenancy/tenants/{id}/concatenation', 'GET,PUT,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 2, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (65, '{}', 'af470c53a25340fd9248fd0309ad41ef', 1263305430536855552, '平台服务-特定租户连接字符串', '/api/multi-tenancy/tenants/{id}/connection-string/{name}', '', '', '/api/multi-tenancy/tenants/{id}/concatenation/{name}', 'GET,DELETE,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 1, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (66, '{}', 'a7a61cb210484794a5ad380938630058', 1263639172959174656, '平台服务-配置管理', '/api/abp/setting', '', '', '/api/abp/setting', 'GET,PUT,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (67, '{}', '858056ce80b8420084b60d62ef4aa25c', 1264799968944640000, '平台服务-验证手机号', '/api/account/phone/verify', '', '', '/api/account/phone/verify', 'POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (68, '{}', 'd460979de403436e840de179767ed770', 1264800070161584128, '平台服务-手机号注册', '/api/account/phone/register', '', '', '/api/account/phone/register', 'POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (69, '{}', 'd2329516ff954ab68f29c69da8db725a', 1267360794414161920, '即时通讯-通知', '/signalr-hubs/notifications/{everything}', '', '', '/signalr-hubs/notifications/{everything}', 'POST,GET,OPTIONS,PUT,DELETE,', '', '', '', '', '', '', '', 1, '', '', 'ws', '127.0.0.1:30020,', '', '', '', 1, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (70, '{}', 'e83c654c4e4e4ef5bac6ced23305f843', 1267383367629807616, '即时通讯-通知0', '/signalr-hubs/notifications', '', '', '/signalr-hubs/notifications', 'GET,POST,PUT,DELETE,OPTIONS,', '', '', '', '', '', '', '', 1, '', '', 'ws', '127.0.0.1:30020,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (71, '{}', '01b7a7e9975a41809fd636df95a57cde', 1267817055527632896, '即时通讯-聊天', '/signalr-hubs/message', '', '', '/signalr-hubs/message', 'GET,POST,PUT,DELETE,OPTIONS,', '', '', '', '', '', '', '', 1, '', '', 'ws', '127.0.0.1:30020,', '', '', '', 1, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (72, '{}', 'dbc40249beb74d01906e435f132a79d7', 1267817221286526976, '即时通讯-聊天1', '/signalr-hubs/message/{everything}', '', '', '/signalr-hubs/message/{everything}', 'GET,POST,PUT,DELETE,OPTIONS,', '', '', '', '', '', '', '', 1, '', '', 'ws', '127.0.0.1:30020,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (73, '{}', 'cfb5f09a12bf495fbcaf2fa5d9123a40', 1268893687085518848, '身份认证服务-重置密码', '/api/account/phone/reset-password', '', '', '/api/account/phone/reset-password', 'PUT,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 1, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (97, '{}', 'f9e4d8f541b34ea7a22fa159b8cbcf6a', 1285579388652576768, '【SingalR】- 本地打印服务', '/document/print-single/{everything}', '', '', '/signalr-hubs/document/print-single/{everything}', 'GET,POST,PUT,DELETE,OPTIONS,', '', '', '', '', '', '', '', 1, '', '', 'ws', '127.0.0.1:36390,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (99, '{}', '4105470ee52345448e75b64983cd50cd', 1285580096881778688, '【SingalR】- 本地打印多个文档', '/document/print-multiple/{everything}', '', '', '/signalr-hubs/document/print-multiple/{everything}', 'GET,PUT,DELETE,OPTIONS,POST,', '', '', '', '', '', '', '', 1, '', '', 'ws', '127.0.0.1:36390,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (100, '{}', 'e4dfea0fab254653b3b4424f9cdfec9f', 1285582774663864320, '【SingalR】- 本地打印多个文档-0', '/document/print-multiple', '', '', '/signalr-hubs/document/print-multiple', 'GET,PUT,POST,DELETE,OPTIONS,', '', '', '', '', '', '', '', 1, '', '', 'ws', '127.0.0.1:36390,', '', '', '', 1, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (101, '{}', '997a4c27a433458aafed9b8aa252d957', 1288657613998579712, '【身份认证服务】- 组织机构列表', '/api/identity/organization-units', '', '', '/api/identity/organization-units', 'GET,POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (102, '{}', 'a2c6acc9882a425ab26bd3ad5a9c17c6', 1288657941770854400, '【身份认证服务】- 组织机构管理', '/api/identity/organization-units/{id}', '', '', '/api/identity/organization-units/{id}', 'GET,PUT,DELETE,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 1, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (103, '{}', '390acfb0e16943c6b61e731d47c282e9', 1288658134067109888, '【身份认证服务】- 组织机构移动', '/api/identity/organization-units/{id}/move', '', '', '/api/identity/organization-units/{id}/move', 'PUT,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (104, '{}', '3515e75becf9447492ad60466b27c397', 1288658305156964352, '【身份认证服务】- 查询组织机构子级', '/api/identity/organization-units/find-children', '', '', '/api/identity/organization-units/find-children', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (105, '{}', 'aab0a24d930f4f9687497e5ccaac2a31', 1288658491216289792, '【身份认证服务】- 查询组织机构最后一个子节点', '/api/identity/organization-units/last-children', '', '', '/api/identity/organization-units/last-children', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (106, '{}', 'edf67e297c6d494baf3ea66465418faf', 1288658638302142464, '【身份认证服务】- 组织机构角色管理', '/api/identity/organization-units/management-roles', '', '', '/api/identity/organization-units/management-roles', 'GET,POST,DELETE,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (107, '{}', '21bcb13e71c648a98861ce9b6fb3e7b0', 1288658791784308736, '【身份认证服务】- 组织机构用户管理', '/api/identity/organization-units/management-users', '', '', '/api/identity/organization-units/management-users', 'GET,POST,DELETE,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');

-- ----------------------------
-- Table structure for appapigatewayroutegroup
-- ----------------------------
DROP TABLE IF EXISTS `appapigatewayroutegroup`;
CREATE TABLE `appapigatewayroutegroup`  (
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
-- Records of appapigatewayroutegroup
-- ----------------------------
INSERT INTO `appapigatewayroutegroup` VALUES ('08d7f735-a83b-49ab-8cee-5d602502bea8', '{}', '8484f6852d3b4252a0b6bf278f9afd48', '2020-05-13 20:03:32.524271', NULL, '2020-05-13 21:34:30.407974', NULL, 0, NULL, NULL, '测试组', 'TEST-APP', '测试网关分组', '127.0.0.1', '测试网关分组', 1);

-- ----------------------------
-- Table structure for appapigatewaysecurityoptions
-- ----------------------------
DROP TABLE IF EXISTS `appapigatewaysecurityoptions`;
CREATE TABLE `appapigatewaysecurityoptions`  (
  `Id` int(0) NOT NULL AUTO_INCREMENT,
  `ReRouteId` bigint(0) NOT NULL,
  `IPAllowedList` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `IPBlockedList` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `IX_AppApiGatewaySecurityOptions_ReRouteId`(`ReRouteId`) USING BTREE,
  CONSTRAINT `FK_AppApiGatewaySecurityOptions_AppApiGatewayReRoute_ReRouteId` FOREIGN KEY (`ReRouteId`) REFERENCES `appapigatewayreroute` (`ReRouteId`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 101 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of appapigatewaysecurityoptions
-- ----------------------------
INSERT INTO `appapigatewaysecurityoptions` VALUES (3, 1261299170387169280, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (4, 1261585859064872960, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (5, 1261586605810368512, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (6, 1261587558609436672, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (7, 1261588213298348032, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (8, 1261588367619375104, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (9, 1261588628450557952, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (10, 1261588881564221440, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (11, 1261588983053795328, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (12, 1261589139039961088, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (13, 1261589197483393024, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (14, 1261589278857084928, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (15, 1261589420356124672, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (16, 1261589960393736192, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (17, 1261606600242085888, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (18, 1261606689601732608, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (19, 1261681880965038080, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (20, 1261682144920977408, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (21, 1262220447629058048, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (22, 1262230734939758592, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (23, 1262296916350869504, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (24, 1262632376348594176, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (25, 1262632791869902848, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (26, 1262632904575045632, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (27, 1262632976616411136, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (28, 1262660336921235456, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (29, 1262660528277966848, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (30, 1262660706875625472, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (31, 1262660966393991168, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (32, 1262661109474283520, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (33, 1262663888804663296, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (34, 1262664024096133120, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (35, 1262664186252120064, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (36, 1262664357044178944, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (37, 1262664632928718848, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (38, 1262664751409418240, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (39, 1262664871274237952, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (40, 1262665026111164416, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (41, 1262665159905267712, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (42, 1262665329829105664, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (43, 1262665456471920640, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (44, 1262665628165754880, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (45, 1262666172682883072, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (47, 1262723402331885568, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (48, 1262935771746734080, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (49, 1262935906522304512, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (50, 1262936009924481024, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (52, 1263074419073593344, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (53, 1263075249394790400, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (54, 1263075593499684864, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (56, 1263101898440146944, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (57, 1263303878648569856, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (58, 1263304204797648896, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (59, 1263304872891555840, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (60, 1263305106250047488, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (61, 1263305244594970624, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (62, 1263305430536855552, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (63, 1263639172959174656, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (64, 1264799968944640000, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (65, 1264800070161584128, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (66, 1267360794414161920, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (67, 1267383367629807616, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (68, 1267817055527632896, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (69, 1267817221286526976, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (70, 1268893687085518848, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (71, 1273527659565547520, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (72, 1273542336509079552, '127.0.0.1', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (73, 1273542755520049152, '127.0.0.1', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (74, 1273543111322857472, '127.0.0.1', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (75, 1273543850526994432, '127.0.0.1', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (76, 1273544135009857536, '127.0.0.1', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (77, 1273544377432240128, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (78, 1273544549834911744, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (79, 1273545100509278208, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (80, 1273545462146363392, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (81, 1273545765801390080, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (82, 1273877904709361664, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (83, 1273878097483767808, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (84, 1273878425839050752, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (85, 1273879014778052608, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (86, 1273879265417076736, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (87, 1273879533101752320, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (88, 1274294160109314048, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (89, 1274524600855441408, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (90, 1274543888438525952, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (91, 1285579388652576768, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (92, 1285580096881778688, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (93, 1285582774663864320, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (94, 1288657613998579712, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (95, 1288657941770854400, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (96, 1288658134067109888, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (97, 1288658305156964352, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (98, 1288658491216289792, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (99, 1288658638302142464, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (100, 1288658791784308736, '', '');

-- ----------------------------
-- Table structure for cap.published
-- ----------------------------
DROP TABLE IF EXISTS `cap.published`;
CREATE TABLE `cap.published`  (
  `Id` bigint(0) NOT NULL,
  `Version` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `Name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Content` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `Retries` int(0) NULL DEFAULT NULL,
  `Added` datetime(0) NOT NULL,
  `ExpiresAt` datetime(0) NULL DEFAULT NULL,
  `StatusName` varchar(40) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IX_ExpiresAt`(`ExpiresAt`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of cap.published
-- ----------------------------
INSERT INTO `cap.published` VALUES (1288657615026184192, 'v1', 'LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData', '{\"Headers\":{\"cap-callback-name\":null,\"cap-msg-id\":\"1288657615026184192\",\"cap-msg-name\":\"LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData\",\"cap-msg-type\":\"Object\",\"cap-senttime\":\"2020/7/30 10:08:12 +08:00\",\"cap-corr-id\":\"1288657615026184192\",\"cap-corr-seq\":\"0\"},\"Value\":{\"DateTime\":\"2020-07-30T10:08:12.1765932+08:00\",\"AppId\":\"TEST-APP\",\"Method\":\"Create\",\"Object\":\"ReRoute\"}}', 0, '2020-07-30 10:08:12', '2020-07-31 10:08:12', 'Succeeded');
INSERT INTO `cap.published` VALUES (1288657799890132992, 'v1', 'LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData', '{\"Headers\":{\"cap-callback-name\":null,\"cap-msg-id\":\"1288657799890132992\",\"cap-msg-name\":\"LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData\",\"cap-msg-type\":\"Object\",\"cap-senttime\":\"2020/7/30 10:08:56 +08:00\",\"cap-corr-id\":\"1288657799890132992\",\"cap-corr-seq\":\"0\"},\"Value\":{\"DateTime\":\"2020-07-30T10:08:56.2542+08:00\",\"AppId\":\"TEST-APP\",\"Method\":\"Modify\",\"Object\":\"ReRoute\"}}', 0, '2020-07-30 10:08:56', '2020-07-31 10:08:56', 'Succeeded');
INSERT INTO `cap.published` VALUES (1288657941858934784, 'v1', 'LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData', '{\"Headers\":{\"cap-callback-name\":null,\"cap-msg-id\":\"1288657941858934784\",\"cap-msg-name\":\"LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData\",\"cap-msg-type\":\"Object\",\"cap-senttime\":\"2020/7/30 10:09:30 +08:00\",\"cap-corr-id\":\"1288657941858934784\",\"cap-corr-seq\":\"0\"},\"Value\":{\"DateTime\":\"2020-07-30T10:09:30.1028+08:00\",\"AppId\":\"TEST-APP\",\"Method\":\"Create\",\"Object\":\"ReRoute\"}}', 0, '2020-07-30 10:09:30', '2020-07-31 10:09:30', 'Succeeded');
INSERT INTO `cap.published` VALUES (1288658134121635840, 'v1', 'LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData', '{\"Headers\":{\"cap-callback-name\":null,\"cap-msg-id\":\"1288658134121635840\",\"cap-msg-name\":\"LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData\",\"cap-msg-type\":\"Object\",\"cap-senttime\":\"2020/7/30 10:10:15 +08:00\",\"cap-corr-id\":\"1288658134121635840\",\"cap-corr-seq\":\"0\"},\"Value\":{\"DateTime\":\"2020-07-30T10:10:15.9418485+08:00\",\"AppId\":\"TEST-APP\",\"Method\":\"Create\",\"Object\":\"ReRoute\"}}', 0, '2020-07-30 10:10:16', '2020-07-31 10:10:16', 'Succeeded');
INSERT INTO `cap.published` VALUES (1288658305198907392, 'v1', 'LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData', '{\"Headers\":{\"cap-callback-name\":null,\"cap-msg-id\":\"1288658305198907392\",\"cap-msg-name\":\"LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData\",\"cap-msg-type\":\"Object\",\"cap-senttime\":\"2020/7/30 10:10:56 +08:00\",\"cap-corr-id\":\"1288658305198907392\",\"cap-corr-seq\":\"0\"},\"Value\":{\"DateTime\":\"2020-07-30T10:10:56.7297211+08:00\",\"AppId\":\"TEST-APP\",\"Method\":\"Create\",\"Object\":\"ReRoute\"}}', 0, '2020-07-30 10:10:57', '2020-07-31 10:10:57', 'Succeeded');
INSERT INTO `cap.published` VALUES (1288658491258232832, 'v1', 'LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData', '{\"Headers\":{\"cap-callback-name\":null,\"cap-msg-id\":\"1288658491258232832\",\"cap-msg-name\":\"LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData\",\"cap-msg-type\":\"Object\",\"cap-senttime\":\"2020/7/30 10:11:41 +08:00\",\"cap-corr-id\":\"1288658491258232832\",\"cap-corr-seq\":\"0\"},\"Value\":{\"DateTime\":\"2020-07-30T10:11:41.0897204+08:00\",\"AppId\":\"TEST-APP\",\"Method\":\"Create\",\"Object\":\"ReRoute\"}}', 0, '2020-07-30 10:11:41', '2020-07-31 10:11:41', 'Succeeded');
INSERT INTO `cap.published` VALUES (1288658638331502592, 'v1', 'LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData', '{\"Headers\":{\"cap-callback-name\":null,\"cap-msg-id\":\"1288658638331502592\",\"cap-msg-name\":\"LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData\",\"cap-msg-type\":\"Object\",\"cap-senttime\":\"2020/7/30 10:12:16 +08:00\",\"cap-corr-id\":\"1288658638331502592\",\"cap-corr-seq\":\"0\"},\"Value\":{\"DateTime\":\"2020-07-30T10:12:16.1548814+08:00\",\"AppId\":\"TEST-APP\",\"Method\":\"Create\",\"Object\":\"ReRoute\"}}', 0, '2020-07-30 10:12:16', '2020-07-31 10:12:16', 'Succeeded');
INSERT INTO `cap.published` VALUES (1288658791822057472, 'v1', 'LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData', '{\"Headers\":{\"cap-callback-name\":null,\"cap-msg-id\":\"1288658791822057472\",\"cap-msg-name\":\"LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData\",\"cap-msg-type\":\"Object\",\"cap-senttime\":\"2020/7/30 10:12:52 +08:00\",\"cap-corr-id\":\"1288658791822057472\",\"cap-corr-seq\":\"0\"},\"Value\":{\"DateTime\":\"2020-07-30T10:12:52.7491793+08:00\",\"AppId\":\"TEST-APP\",\"Method\":\"Create\",\"Object\":\"ReRoute\"}}', 0, '2020-07-30 10:12:53', '2020-07-31 10:12:53', 'Succeeded');

-- ----------------------------
-- Table structure for cap.received
-- ----------------------------
DROP TABLE IF EXISTS `cap.received`;
CREATE TABLE `cap.received`  (
  `Id` bigint(0) NOT NULL,
  `Version` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `Name` varchar(400) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Group` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `Content` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
  `Retries` int(0) NULL DEFAULT NULL,
  `Added` datetime(0) NOT NULL,
  `ExpiresAt` datetime(0) NULL DEFAULT NULL,
  `StatusName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IX_ExpiresAt`(`ExpiresAt`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

SET FOREIGN_KEY_CHECKS = 1;
