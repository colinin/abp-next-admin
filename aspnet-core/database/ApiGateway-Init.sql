/*
 Navicat Premium Data Transfer

 Source Server         : 本机服务器
 Source Server Type    : MySQL
 Source Server Version : 80020
 Source Host           : localhost:3306
 Source Schema         : apigateway

 Target Server Type    : MySQL
 Target Server Version : 80020
 File Encoding         : 65001

 Date: 31/10/2020 11:28:59
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
INSERT INTO `__efmigrationshistory` VALUES ('20200908020925_Upgrade-abp-3.1.0', '3.1.7');

-- ----------------------------
-- Table structure for appapigatewayaggregate
-- ----------------------------
DROP TABLE IF EXISTS `appapigatewayaggregate`;
CREATE TABLE `appapigatewayaggregate`  (
  `Id` int(0) NOT NULL AUTO_INCREMENT,
  `ExtraProperties` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `ConcurrencyStamp` varchar(40) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
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
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of appapigatewayaggregate
-- ----------------------------
INSERT INTO `appapigatewayaggregate` VALUES (5, '{}', 'db569f15577848568bee9616d6b8b53c', 'TEST-APP', 'abp接口代理服务', 1263083077348196352, 'platform-api-definition,backend-admin-api-definition,messages-api-definition,apigateway-api-definition,identity-server-api-definition,', '/api/abp/api-definition', '', 1, 'AbpApiDefinitionAggregator', NULL, '');
INSERT INTO `appapigatewayaggregate` VALUES (6, '{}', '870a2c5df9b34f8c9514aef0250fbb47', 'TEST-APP', 'abp框架配置', 1263102116090970112, 'apigateway-configuration,platform-configuration,backend-admin-configuration,messages-configuration,identity-server-configuration,', '/api/abp/application-configuration', '', 1, 'AbpApiDefinitionAggregator', NULL, '');

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
) ENGINE = InnoDB CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

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
) ENGINE = InnoDB AUTO_INCREMENT = 149 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

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
INSERT INTO `appapigatewayauthoptions` VALUES (21, 1262220447629058048, NULL, '');
INSERT INTO `appapigatewayauthoptions` VALUES (22, 1262230734939758592, NULL, '');
INSERT INTO `appapigatewayauthoptions` VALUES (23, 1262296916350869504, NULL, '');
INSERT INTO `appapigatewayauthoptions` VALUES (24, 1262632376348594176, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (25, 1262632791869902848, '', '');
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
INSERT INTO `appapigatewayauthoptions` VALUES (94, 1288657613998579712, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (95, 1288657941770854400, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (96, 1288658134067109888, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (97, 1288658305156964352, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (98, 1288658491216289792, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (99, 1288658638302142464, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (100, 1288658791784308736, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (101, 1290849478956199936, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (102, 1290849628051124224, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (103, 1290849798553776128, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (105, 1291259822512693248, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (106, 1292620505149145088, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (107, 1292620665505775616, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (108, 1292620843398791168, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (109, 1292621027574874112, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (110, 1292621363161137152, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (111, 1292621494837116928, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (112, 1292621629260365824, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (113, 1292622526073864192, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (114, 1293470838745821184, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (115, 1293471661785706496, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (116, 1293472678392721408, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (117, 1293472857510473728, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (118, 1299273336009359360, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (119, 1299273436282585088, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (120, 1299273618470567936, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (121, 1299273770182737920, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (122, 1299273978023084032, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (123, 1299274123225694208, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (124, 1299274222299348992, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (125, 1304238876758495232, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (126, 1304678610343383040, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (127, 1304679169305694208, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (128, 1310460417141817344, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (129, 1310502391475519488, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (130, 1310515546943569920, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (131, 1310515735292985344, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (132, 1316628769783480320, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (133, 1316628940663619584, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (134, 1316629112428756992, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (135, 1316652047017246720, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (136, 1316913899996737536, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (137, 1319200951383199744, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (138, 1319221929807024128, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (139, 1319554431134306304, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (140, 1319554550458060800, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (141, 1319554948434595840, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (142, 1319555067183730688, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (143, 1319555230765780992, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (144, 1319555333790470144, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (145, 1321001932510203904, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (146, 1321002059803136000, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (147, 1321002256440496128, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (148, 1321002350686507008, '', '');
INSERT INTO `appapigatewayauthoptions` VALUES (149, 1322190027988525056, '', '');

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
) ENGINE = InnoDB AUTO_INCREMENT = 152 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

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
INSERT INTO `appapigatewaybalanceroptions` VALUES (22, NULL, 1262220447629058048, NULL, NULL, NULL);
INSERT INTO `appapigatewaybalanceroptions` VALUES (23, NULL, 1262230734939758592, NULL, NULL, NULL);
INSERT INTO `appapigatewaybalanceroptions` VALUES (24, NULL, 1262296916350869504, NULL, NULL, NULL);
INSERT INTO `appapigatewaybalanceroptions` VALUES (25, NULL, 1262632376348594176, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (26, NULL, 1262632791869902848, '', '', 0);
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
INSERT INTO `appapigatewaybalanceroptions` VALUES (68, NULL, 1267360794414161920, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (69, NULL, 1267383367629807616, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (70, NULL, 1267817055527632896, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (71, NULL, 1267817221286526976, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (72, NULL, 1268893687085518848, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (97, NULL, 1288657613998579712, 'LeastConnection', '', 60000);
INSERT INTO `appapigatewaybalanceroptions` VALUES (98, NULL, 1288657941770854400, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (99, NULL, 1288658134067109888, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (100, NULL, 1288658305156964352, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (101, NULL, 1288658491216289792, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (102, NULL, 1288658638302142464, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (103, NULL, 1288658791784308736, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (104, NULL, 1290849478956199936, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (105, NULL, 1290849628051124224, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (106, NULL, 1290849798553776128, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (108, NULL, 1291259822512693248, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (109, NULL, 1292620505149145088, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (110, NULL, 1292620665505775616, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (111, NULL, 1292620843398791168, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (112, NULL, 1292621027574874112, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (113, NULL, 1292621363161137152, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (114, NULL, 1292621494837116928, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (115, NULL, 1292621629260365824, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (116, NULL, 1292622526073864192, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (117, NULL, 1293470838745821184, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (118, NULL, 1293471661785706496, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (119, NULL, 1293472678392721408, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (120, NULL, 1293472857510473728, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (121, NULL, 1299273336009359360, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (122, NULL, 1299273436282585088, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (123, NULL, 1299273618470567936, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (124, NULL, 1299273770182737920, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (125, NULL, 1299273978023084032, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (126, NULL, 1299274123225694208, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (127, NULL, 1299274222299348992, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (128, NULL, 1304238876758495232, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (129, NULL, 1304678610343383040, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (130, NULL, 1304679169305694208, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (131, NULL, 1310460417141817344, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (132, NULL, 1310502391475519488, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (133, NULL, 1310515546943569920, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (134, NULL, 1310515735292985344, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (135, NULL, 1316628769783480320, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (136, NULL, 1316628940663619584, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (137, NULL, 1316629112428756992, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (138, NULL, 1316652047017246720, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (139, NULL, 1316913899996737536, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (140, NULL, 1319200951383199744, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (141, NULL, 1319221929807024128, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (142, NULL, 1319554431134306304, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (143, NULL, 1319554550458060800, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (144, NULL, 1319554948434595840, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (145, NULL, 1319555067183730688, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (146, NULL, 1319555230765780992, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (147, NULL, 1319555333790470144, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (148, NULL, 1321001932510203904, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (149, NULL, 1321002059803136000, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (150, NULL, 1321002256440496128, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (151, NULL, 1321002350686507008, '', '', 0);
INSERT INTO `appapigatewaybalanceroptions` VALUES (152, NULL, 1322190027988525056, '', '', 0);

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
) ENGINE = InnoDB AUTO_INCREMENT = 149 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

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
INSERT INTO `appapigatewaycacheoptions` VALUES (21, 1262220447629058048, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (22, 1262230734939758592, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (23, 1262296916350869504, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (24, 1262632376348594176, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (25, 1262632791869902848, NULL, NULL);
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
INSERT INTO `appapigatewaycacheoptions` VALUES (44, 1262665628165754880, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (45, 1262666172682883072, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (47, 1262723402331885568, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (48, 1262935771746734080, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (49, 1262935906522304512, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (52, 1263074419073593344, NULL, NULL);
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
INSERT INTO `appapigatewaycacheoptions` VALUES (64, 1264799968944640000, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (65, 1264800070161584128, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (66, 1267360794414161920, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (67, 1267383367629807616, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (68, 1267817055527632896, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (69, 1267817221286526976, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (70, 1268893687085518848, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (94, 1288657613998579712, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (95, 1288657941770854400, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (96, 1288658134067109888, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (97, 1288658305156964352, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (98, 1288658491216289792, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (99, 1288658638302142464, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (100, 1288658791784308736, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (101, 1290849478956199936, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (102, 1290849628051124224, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (103, 1290849798553776128, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (105, 1291259822512693248, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (106, 1292620505149145088, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (107, 1292620665505775616, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (108, 1292620843398791168, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (109, 1292621027574874112, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (110, 1292621363161137152, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (111, 1292621494837116928, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (112, 1292621629260365824, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (113, 1292622526073864192, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (114, 1293470838745821184, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (115, 1293471661785706496, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (116, 1293472678392721408, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (117, 1293472857510473728, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (118, 1299273336009359360, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (119, 1299273436282585088, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (120, 1299273618470567936, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (121, 1299273770182737920, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (122, 1299273978023084032, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (123, 1299274123225694208, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (124, 1299274222299348992, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (125, 1304238876758495232, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (126, 1304678610343383040, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (127, 1304679169305694208, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (128, 1310460417141817344, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (129, 1310502391475519488, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (130, 1310515546943569920, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (131, 1310515735292985344, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (132, 1316628769783480320, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (133, 1316628940663619584, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (134, 1316629112428756992, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (135, 1316652047017246720, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (136, 1316913899996737536, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (137, 1319200951383199744, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (138, 1319221929807024128, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (139, 1319554431134306304, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (140, 1319554550458060800, NULL, NULL);
INSERT INTO `appapigatewaycacheoptions` VALUES (141, 1319554948434595840, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (142, 1319555067183730688, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (143, 1319555230765780992, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (144, 1319555333790470144, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (145, 1321001932510203904, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (146, 1321002059803136000, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (147, 1321002256440496128, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (148, 1321002350686507008, 0, '');
INSERT INTO `appapigatewaycacheoptions` VALUES (149, 1322190027988525056, 0, '');

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
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of appapigatewaydiscovery
-- ----------------------------
INSERT INTO `appapigatewaydiscovery` VALUES (1, 1260841964962947072, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

-- ----------------------------
-- Table structure for appapigatewaydynamicreroute
-- ----------------------------
DROP TABLE IF EXISTS `appapigatewaydynamicreroute`;
CREATE TABLE `appapigatewaydynamicreroute`  (
  `Id` int(0) NOT NULL AUTO_INCREMENT,
  `ExtraProperties` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `ConcurrencyStamp` varchar(40) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
  `DynamicReRouteId` bigint(0) NOT NULL,
  `ServiceName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `DownstreamHttpVersion` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  `AppId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  UNIQUE INDEX `AK_AppApiGatewayDynamicReRoute_DynamicReRouteId`(`DynamicReRouteId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for appapigatewayglobalconfiguration
-- ----------------------------
DROP TABLE IF EXISTS `appapigatewayglobalconfiguration`;
CREATE TABLE `appapigatewayglobalconfiguration`  (
  `Id` int(0) NOT NULL AUTO_INCREMENT,
  `ExtraProperties` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `ConcurrencyStamp` varchar(40) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
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
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

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
) ENGINE = InnoDB AUTO_INCREMENT = 152 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

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
INSERT INTO `appapigatewayhttpoptions` VALUES (22, NULL, 1262220447629058048, NULL, 0, 0, 1, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (23, NULL, 1262230734939758592, NULL, 0, 0, 1, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (24, NULL, 1262296916350869504, NULL, 0, 0, 1, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (25, NULL, 1262632376348594176, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (26, NULL, 1262632791869902848, 0, 0, 0, 0, 0);
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
INSERT INTO `appapigatewayhttpoptions` VALUES (68, NULL, 1267360794414161920, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (69, NULL, 1267383367629807616, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (70, NULL, 1267817055527632896, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (71, NULL, 1267817221286526976, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (72, NULL, 1268893687085518848, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (97, NULL, 1288657613998579712, 1000, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (98, NULL, 1288657941770854400, 1000, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (99, NULL, 1288658134067109888, 1000, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (100, NULL, 1288658305156964352, 1000, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (101, NULL, 1288658491216289792, 1000, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (102, NULL, 1288658638302142464, 1000, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (103, NULL, 1288658791784308736, 1000, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (104, NULL, 1290849478956199936, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (105, NULL, 1290849628051124224, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (106, NULL, 1290849798553776128, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (108, NULL, 1291259822512693248, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (109, NULL, 1292620505149145088, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (110, NULL, 1292620665505775616, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (111, NULL, 1292620843398791168, 100, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (112, NULL, 1292621027574874112, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (113, NULL, 1292621363161137152, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (114, NULL, 1292621494837116928, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (115, NULL, 1292621629260365824, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (116, NULL, 1292622526073864192, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (117, NULL, 1293470838745821184, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (118, NULL, 1293471661785706496, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (119, NULL, 1293472678392721408, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (120, NULL, 1293472857510473728, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (121, NULL, 1299273336009359360, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (122, NULL, 1299273436282585088, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (123, NULL, 1299273618470567936, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (124, NULL, 1299273770182737920, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (125, NULL, 1299273978023084032, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (126, NULL, 1299274123225694208, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (127, NULL, 1299274222299348992, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (128, NULL, 1304238876758495232, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (129, NULL, 1304678610343383040, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (130, NULL, 1304679169305694208, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (131, NULL, 1310460417141817344, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (132, NULL, 1310502391475519488, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (133, NULL, 1310515546943569920, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (134, NULL, 1310515735292985344, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (135, NULL, 1316628769783480320, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (136, NULL, 1316628940663619584, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (137, NULL, 1316629112428756992, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (138, NULL, 1316652047017246720, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (139, NULL, 1316913899996737536, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (140, NULL, 1319200951383199744, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (141, NULL, 1319221929807024128, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (142, NULL, 1319554431134306304, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (143, NULL, 1319554550458060800, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (144, NULL, 1319554948434595840, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (145, NULL, 1319555067183730688, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (146, NULL, 1319555230765780992, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (147, NULL, 1319555333790470144, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (148, NULL, 1321001932510203904, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (149, NULL, 1321002059803136000, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (150, NULL, 1321002256440496128, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (151, NULL, 1321002350686507008, 0, 0, 0, 0, 0);
INSERT INTO `appapigatewayhttpoptions` VALUES (152, NULL, 1322190027988525056, 0, 0, 0, 0, 0);

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
) ENGINE = InnoDB AUTO_INCREMENT = 152 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

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
INSERT INTO `appapigatewayqosoptions` VALUES (22, NULL, 1262220447629058048, 60, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (23, NULL, 1262230734939758592, 60, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (24, NULL, 1262296916350869504, 60, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (25, NULL, 1262632376348594176, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (26, NULL, 1262632791869902848, 50, 60000, 30000);
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
INSERT INTO `appapigatewayqosoptions` VALUES (68, NULL, 1267360794414161920, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (69, NULL, 1267383367629807616, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (70, NULL, 1267817055527632896, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (71, NULL, 1267817221286526976, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (72, NULL, 1268893687085518848, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (97, NULL, 1288657613998579712, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (98, NULL, 1288657941770854400, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (99, NULL, 1288658134067109888, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (100, NULL, 1288658305156964352, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (101, NULL, 1288658491216289792, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (102, NULL, 1288658638302142464, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (103, NULL, 1288658791784308736, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (104, NULL, 1290849478956199936, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (105, NULL, 1290849628051124224, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (106, NULL, 1290849798553776128, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (108, NULL, 1291259822512693248, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (109, NULL, 1292620505149145088, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (110, NULL, 1292620665505775616, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (111, NULL, 1292620843398791168, 50, 60000, 1200000);
INSERT INTO `appapigatewayqosoptions` VALUES (112, NULL, 1292621027574874112, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (113, NULL, 1292621363161137152, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (114, NULL, 1292621494837116928, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (115, NULL, 1292621629260365824, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (116, NULL, 1292622526073864192, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (117, NULL, 1293470838745821184, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (118, NULL, 1293471661785706496, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (119, NULL, 1293472678392721408, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (120, NULL, 1293472857510473728, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (121, NULL, 1299273336009359360, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (122, NULL, 1299273436282585088, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (123, NULL, 1299273618470567936, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (124, NULL, 1299273770182737920, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (125, NULL, 1299273978023084032, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (126, NULL, 1299274123225694208, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (127, NULL, 1299274222299348992, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (128, NULL, 1304238876758495232, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (129, NULL, 1304678610343383040, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (130, NULL, 1304679169305694208, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (131, NULL, 1310460417141817344, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (132, NULL, 1310502391475519488, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (133, NULL, 1310515546943569920, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (134, NULL, 1310515735292985344, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (135, NULL, 1316628769783480320, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (136, NULL, 1316628940663619584, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (137, NULL, 1316629112428756992, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (138, NULL, 1316652047017246720, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (139, NULL, 1316913899996737536, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (140, NULL, 1319200951383199744, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (141, NULL, 1319221929807024128, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (142, NULL, 1319554431134306304, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (143, NULL, 1319554550458060800, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (144, NULL, 1319554948434595840, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (145, NULL, 1319555067183730688, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (146, NULL, 1319555230765780992, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (147, NULL, 1319555333790470144, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (148, NULL, 1321001932510203904, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (149, NULL, 1321002059803136000, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (150, NULL, 1321002256440496128, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (151, NULL, 1321002350686507008, 50, 60000, 30000);
INSERT INTO `appapigatewayqosoptions` VALUES (152, NULL, 1322190027988525056, 50, 60000, 30000);

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
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of appapigatewayratelimitoptions
-- ----------------------------
INSERT INTO `appapigatewayratelimitoptions` VALUES (1, 1260841964962947072, 'ClientId', '您的操作过快,请稍后再试!', 'ocelot', 1, 429);

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
) ENGINE = InnoDB AUTO_INCREMENT = 149 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

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
INSERT INTO `appapigatewayratelimitrule` VALUES (21, 1262220447629058048, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (22, 1262230734939758592, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (23, 1262296916350869504, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (24, 1262632376348594176, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (25, 1262632791869902848, NULL, '', 0, NULL, NULL, NULL);
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
INSERT INTO `appapigatewayratelimitrule` VALUES (94, 1288657613998579712, NULL, '', 1, '1m', 60, 200);
INSERT INTO `appapigatewayratelimitrule` VALUES (95, 1288657941770854400, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (96, 1288658134067109888, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (97, 1288658305156964352, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (98, 1288658491216289792, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (99, 1288658638302142464, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (100, 1288658791784308736, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (101, 1290849478956199936, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (102, 1290849628051124224, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (103, 1290849798553776128, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (105, 1291259822512693248, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (106, 1292620505149145088, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (107, 1292620665505775616, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (108, 1292620843398791168, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (109, 1292621027574874112, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (110, 1292621363161137152, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (111, 1292621494837116928, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (112, 1292621629260365824, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (113, 1292622526073864192, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (114, 1293470838745821184, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (115, 1293471661785706496, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (116, 1293472678392721408, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (117, 1293472857510473728, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (118, 1299273336009359360, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (119, 1299273436282585088, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (120, 1299273618470567936, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (121, 1299273770182737920, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (122, 1299273978023084032, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (123, 1299274123225694208, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (124, 1299274222299348992, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (125, 1304238876758495232, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (126, 1304678610343383040, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (127, 1304679169305694208, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (128, 1310460417141817344, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (129, 1310502391475519488, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (130, 1310515546943569920, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (131, 1310515735292985344, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (132, 1316628769783480320, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (133, 1316628940663619584, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (134, 1316629112428756992, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (135, 1316652047017246720, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (136, 1316913899996737536, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (137, 1319200951383199744, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (138, 1319221929807024128, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (139, 1319554431134306304, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (140, 1319554550458060800, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (141, 1319554948434595840, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (142, 1319555067183730688, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (143, 1319555230765780992, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (144, 1319555333790470144, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (145, 1321001932510203904, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (146, 1321002059803136000, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (147, 1321002256440496128, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (148, 1321002350686507008, NULL, '', 0, NULL, NULL, NULL);
INSERT INTO `appapigatewayratelimitrule` VALUES (149, 1322190027988525056, NULL, '', 0, NULL, NULL, NULL);

-- ----------------------------
-- Table structure for appapigatewayreroute
-- ----------------------------
DROP TABLE IF EXISTS `appapigatewayreroute`;
CREATE TABLE `appapigatewayreroute`  (
  `Id` int(0) NOT NULL AUTO_INCREMENT,
  `ExtraProperties` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `ConcurrencyStamp` varchar(40) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
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
) ENGINE = InnoDB AUTO_INCREMENT = 156 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of appapigatewayreroute
-- ----------------------------
INSERT INTO `appapigatewayreroute` VALUES (4, '{}', '84059fcecc91498b9beafac914865e2c', 1261299170387169280, '【后台管理】- 权限管理', '/api/permission-management/permissions', '', NULL, '/api/permission-management/permissions', 'GET,PUT,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30010,', '', NULL, NULL, NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (5, '{}', 'f2312eed73cc4d3cbefcd1816849fd74', 1261585859064872960, '【身份认证服务】- 客户端', '/api/identity-server/clients', '', NULL, '/api/identity-server/clients', 'POST,GET,', '', '', 'X-Forwarded-For:{RemoteIpAddress},', '', '', '', NULL, 1, '', NULL, 'HTTP', '127.0.0.1:30015,', '', NULL, NULL, NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (6, '{}', 'b764ec7b994147abb12974bfcee4a0a9', 1261586605810368512, '【身份认证服务】- 管理客户端', '/api/identity-server/clients/{Id}', '', NULL, '/api/identity-server/clients/{Id}', 'GET,DELETE,PUT,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30015,', '', NULL, NULL, NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (7, '{}', 'b4ce189320804dc6b87e602594e93d35', 1261587558609436672, '【服务网关管理】- 路由组管理', '/api/ApiGateway/RouteGroups', '', '', '/api/ApiGateway/RouteGroups', 'GET,POST,PUT,DELETE,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, '', NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (8, '{}', '8736fefa36da4b129f3fcf6aa095f2ce', 1261588213298348032, '【服务网关管理】- 查询单个路由组', '/api/ApiGateway/RouteGroups/By-AppId/{AppId}', '', '', '/api/ApiGateway/RouteGroups/By-AppId/{AppId}', 'GET,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, '', NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (9, '{}', 'befd14ad39e244bc9dea7e0c01e642ce', 1261588367619375104, '【服务网关管理】- 查询所有有效路由组', '/api/ApiGateway/RouteGroups/Actived', '', '', '/api/ApiGateway/RouteGroups/Actived', 'GET,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, '', NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (10, '{}', '0a95945d77144ce69addb0d1e8d37837', 1261588628450557952, '【服务网关管理】- 基础配置', '/api/ApiGateway/Globals', '', '', '/api/ApiGateway/Globals', 'GET,POST,PUT,DELETE,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, '', NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (11, '{}', '70ee7f919bf44b42b549c905316bfd75', 1261588881564221440, '【服务网关管理】- 查询单个基础配置', '/api/ApiGateway/Globals/By-AppId/{AppId}', '', '', '/api/ApiGateway/Globals/By-AppId/{AppId}', 'GET,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, '', NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (12, '{}', 'caf54542d561428a9123ebed88e4b2e9', 1261588983053795328, '【服务网关管理】- 路由配置', '/api/ApiGateway/Routes', '', '', '/api/ApiGateway/Routes', 'GET,POST,PUT,DELETE,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, '', NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (13, '{}', 'df6c48fdaab44a37842992ae61c59dc5', 1261589139039961088, '【服务网关管理】- 通过标识查询路由', '/api/ApiGateway/Routes/By-RouteId/{RouteId}', '', '', '/api/ApiGateway/Routes/By-RouteId/{RouteId}', 'GET,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, '', NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (14, '{}', 'aaeaedebd24a4011ad565b5559f84c5f', 1261589197483393024, '【服务网关管理】- 通过名称查询路由', '/api/ApiGateway/Routes/By-RouteName/{RouteName}', '', '', '/api/ApiGateway/Routes/By-RouteName/{RouteName}', 'GET,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, '', NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (15, '{}', '559c9f1b2b8c44caac86f7a643a16aaa', 1261589278857084928, '【服务网关管理】- 通过应用标识查询路由', '/api/ApiGateway/Routes/By-AppId/{AppId}', '', '', '/api/ApiGateway/Routes/By-AppId/{AppId}', 'GET,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, '', NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (16, '{}', '00d0a12f403a4a919c99c534bd76d0d0', 1261589420356124672, '【服务网关管理】- 清空应用标识下所有路由', '/api/ApiGateway/Routes/Clear', '', '', '/api/ApiGateway/Routes/Clear', 'DELETE,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, '', NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (17, '{}', '8c308f1386ad49c799cd281eb95170ac', 1261589960393736192, '【服务网关管理】- 通过应用标识查询动态路由', '/api/ApiGateway/DynamicRoutes/By-AppId/{AppId}', '', NULL, '/api/ApiGateway/DynamicRoutes/By-AppId/{AppId}', 'GET,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, NULL, NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (18, '{}', 'e659ebbf61534a978335cfeabdc0b375', 1261606600242085888, '【服务网关管理】- 通过应用标识查询聚合路由', '/api/ApiGateway/Aggregates/by-AppId/{AppId}', '', NULL, '/api/ApiGateway/Aggregates/by-AppId/{AppId}', 'GET,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, NULL, NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (19, '{}', 'd665e4491b81413385858601d9cf9a1d', 1261606689601732608, '【服务网关管理】- 聚合路由', '/api/ApiGateway/Aggregates', '', NULL, '/api/ApiGateway/Aggregates', 'GET,POST,PUT,DELETE,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30001,', '', NULL, NULL, NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (22, '{}', '2aad614e2c2a497593a4784ff639c3d9', 1262220447629058048, '【身份认证服务】- 克隆客户端', '/api/identity-server/clients/{id}/clone', '', NULL, '/api/identity-server/clients/{id}/clone', 'POST,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30015,', '', NULL, NULL, NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (23, '{}', '1504c5e4a7334298878339a305445b21', 1262230734939758592, '【身份认证服务】- 可用的Api资源', '/api/identity-server/clients/assignable-api-resources', '', NULL, '/api/identity-server/clients/assignable-api-resources', 'GET,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30015,', '', NULL, 'assignable-api-resources', NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (25, '{}', '53dd1751d9104940a966006a5e93d1fa', 1262296916350869504, '【身份认证服务】- 可用的身份资源', '/api/identity-server/clients/assignable-identity-resources', '', NULL, '/api/identity-server/clients/assignable-identity-resources', 'GET,', '', '', '', '', '', '', NULL, 1, NULL, NULL, 'HTTP', '127.0.0.1:30015,', '', NULL, 'assignable-identity-resources', NULL, 30000, 1, NULL, 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (26, '{}', '3fccd1318d0d47d9aef85542668829a6', 1262632376348594176, '【身份认证服务】- Api资源', '/api/identity-server/api-resources', '', '', '/api/identity-server/api-resources', 'GET,POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (27, '{}', 'de6bd0ddea6d4019b4855be5442fafdc', 1262632791869902848, '【身份认证服务】- 管理Api资源', '/api/identity-server/api-resources/{id}', '', '', '/api/identity-server/api-resources/{id}', 'GET,DELETE,PUT,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (30, '{}', '0f9875697b74420c9dc2eaf77099b210', 1262660336921235456, '【身份认证服务】- 用户登录', '/api/account/login', '', '', '/api/account/login', 'POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (31, '{}', 'a890c6ecc6a64c9fa313a0f6b5406e1c', 1262660528277966848, '【身份认证服务】- 用户登出', '/api/account/logout', '', '', '/api/account/logout', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (32, '{}', '88de580b6beb4d9d9d4367840ba1fcea', 1262660706875625472, '【身份认证服务】- 检查密码', '/api/account/checkPassword', '', '', '/api/account/checkPassword', 'POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (33, '{}', '78f3c1adc7a54696af37a419eda47c62', 1262660966393991168, '【身份认证服务】- 个人信息页', '/api/identity/my-profile', '', '', '/api/identity/my-profile', 'GET,PUT,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (34, '{}', '95b23aa5cebb40598a78c0761cfd0b26', 1262661109474283520, '【身份认证服务】- 修改密码', '/api/identity/my-profile/change-password', '', '', '/api/identity/my-profile/change-password', 'POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (35, '{}', '4828f7c2aff8485189f37aba5de62d60', 1262663888804663296, '【身份认证管理】- 角色管理', '/api/identity/roles', '', '', '/api/identity/roles', 'GET,POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (36, '{}', '0fddcd3b50a24c6795ec9034fdb44778', 1262664024096133120, '【身份认证服务】- 角色列表', '/api/identity/roles/all', '', '', '/api/identity/roles/all', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (37, '{}', '191e555219e845069dfd93793263a840', 1262664186252120064, '【身份认证服务】- 单个角色', '/api/identity/roles/{id}', '', '', '/api/identity/roles/{id}', 'GET,PUT,DELETE,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (38, '{}', 'c316858e82f74e6ca6e923d6b3a3fa76', 1262664357044178944, '【身份认证服务】- 用户注册', '/api/account/register', '', '', '/api/account/register', 'POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (39, '{}', 'db53b6f957914a10a6a97ba306b1f6ef', 1262664632928718848, '【身份认证服务】- 单个用户', '/api/identity/users/{id}', '', '', '/api/identity/users/{id}', 'GET,PUT,DELETE,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (40, '{}', '1833434b8ce34f8ab791e7e950f4c61f', 1262664751409418240, '【身份认证服务】- 用户管理', '/api/identity/users', '', '', '/api/identity/users', 'GET,POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (41, '{}', 'b3c963a1612144918bffaf272697498c', 1262664871274237952, '【身份认证服务】- 用户角色', '/api/identity/users/{id}/roles', '', '', '/api/identity/users/{id}/roles', 'GET,POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (42, '{}', '33dd757b79cb4f52994af13bfb4f6783', 1262665026111164416, '【身份认证服务】- 通过用户名查询用户', '/api/identity/users/by-username/{userName}', '', '', '/api/identity/users/by-username/{userName}', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (43, '{}', 'e46fd6cb3a104da3aadfe0149fe4de68', 1262665159905267712, '【身份认证服务】- 通过邮件查询用户', '/api/identity/users/by-email/{email}', '', '', '/api/identity/users/by-email/{email}', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (44, '{}', '8c8ec5ad6aaa4145981ee7ac876c36c9', 1262665329829105664, '【身份认证服务】- 通过标识查询用户', '/api/identity/users/lookup/{id}', '', '', '/api/identity/users/lookup/{id}', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (45, '{}', 'f5c0c8c02c0846fdbe5015cd86f3d81b', 1262665456471920640, '【身份认证服务】- 通过名称查询用户', '/api/identity/users/lookup/by-username/{userName}', '', '', '/api/identity/users/lookup/by-username/{userName}', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (46, '{}', 'ecf0ea4a3e3c4b2e8fa3621514d00c74', 1262665628165754880, '【基础服务】- 通过名称查询租户', '/api/abp/multi-tenancy/tenants/by-name/{name}', '', '', '/api/abp/multi-tenancy/tenants/by-name/{name}', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (47, '{}', '69132bc515b64005af4292ce0dee5626', 1262666172682883072, '【基础服务】- 通过标识查询租户', '/api/abp/multi-tenancy/tenants/by-id/{id}', '', '', '/api/abp/multi-tenancy/tenants/by-id/{id}', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (49, '{}', '35f48d7bc3694bbf95d64fe59aa631ac', 1262723402331885568, '【身份认证服务】- 已有的跨域资源', '/api/identity-server/clients/distinct-cors-origins', '', '', '/api/identity-server/clients/distinct-cors-origins', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', 'distinct-cors-origins', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (50, '{}', 'f333d028839d4fc2aafa8509e674d7dd', 1262935771746734080, '【身份认证服务】- 身份资源', '/api/identity-server/identity-resources', '', '', '/api/identity-server/identity-resources', 'GET,POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (51, '{}', 'dffd1bfaca5b4c1890221678f2b16cd5', 1262935906522304512, '【身份认证服务】- 身份资源管理', '/api/identity-server/identity-resources/{id}', '', '', '/api/identity-server/identity-resources/{id}', 'GET,PUT,DELETE,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (54, '{}', '7b847d8434bc4d1db07fa8961d90c14a', 1263074419073593344, '【服务网关管理】- 接口代理', '/api/abp/api-definition', '', '', '/api/abp/apigateway/api-definition', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30001,', '', '', 'apigateway-api-definition', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (55, '{}', 'ca2cedfa620045a9adef0be2f958c4bc', 1263075249394790400, '【服务网关管理】- 查询聚合路由', '/api/ApiGateway/Aggregates/{RouteId}', '', '', '/api/ApiGateway/Aggregates/{RouteId}', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30001,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (57, '{}', '98fbc99fc8644946ac0a72cc3dc5fd1f', 1263075593499684864, '【服务网关管理】- 聚合路由配置', '/api/ApiGateway/Aggregates/RouteConfig', '', '', '/api/ApiGateway/Aggregates/RouteConfig', 'POST,DELETE,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30001,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (59, '{}', 'c692b30c72d4424eb4740ac49f4e9373', 1263101898440146944, '【服务网关管理】- 框架配置', '/api/abp/application-configuration', '', '', '/api/abp/apigateway/application-configuration', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30001,', '', '', 'apigateway-configuration', 0, 120000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (60, '{}', '8409117162504f71aa66982f05c38a80', 1263303878648569856, '【平台服务】- 接口代理', '/api/abp/api-definition', '', '', '/api/abp/platform/api-definition', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30025,', '', '', 'platform-api-definition', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (61, '{}', '9f520820071b4e14bc94ab57989cea1f', 1263304204797648896, '【平台服务】- 框架配置', '/api/abp/application-configuration', '', '', '/api/abp/platform/application-configuration', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30025,', '', '', 'platform-configuration', 0, 120000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (62, '{}', '530ab314560f41678b40f48da9383d51', 1263304872891555840, '【后台管理】- 租户管理', '/api/tenant-management/tenants', '', '', '/api/tenant-management/tenants', 'GET,POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (63, '{}', '21334c6da4c349cc883c38c13de0e754', 1263305106250047488, '【后台管理】- 特定租户管理', '/api/tenant-management/tenants/{id}', '', '', '/api/tenant-management/tenants/{id}', 'GET,PUT,DELETE,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (64, '{}', 'cc8fdf1b2d0b414ebf2dc51a6dc78305', 1263305244594970624, '【后台管理】- 租户连接字符串', '/api/tenant-management/tenants/{id}/connection-string', '', '', '/api/tenant-management/tenants/{id}/concatenation', 'GET,PUT,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 2, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (65, '{}', 'aaf285ed10da4024ba561d5cf8c6322b', 1263305430536855552, '【后台管理】- 特定租户连接字符串', '/api/tenant-management/tenants/{id}/connection-string/{name}', '', '', '/api/tenant-management/tenants/{id}/concatenation/{name}', 'GET,DELETE,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 1, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (66, '{}', 'b6ec80673b354c239def3bdce31b4cc0', 1263639172959174656, '【后台管理】- 全局设置', '/api/setting-management/settings/by-global', '', '', '/api/setting-management/settings/by-global', 'GET,PUT,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (67, '{}', '755b4dce5c34444785fa3b647fef4131', 1264799968944640000, '【身份认证服务】- 验证手机号', '/api/account/phone/verify', '', '', '/api/account/phone/verify', 'POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (68, '{}', '535191c570ae453ab320012304d7a62c', 1264800070161584128, '【身份认证服务】- 手机号注册', '/api/account/phone/register', '', '', '/api/account/phone/register', 'POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (69, '{}', '723c9b111f9f4a1aa804118cdde193d3', 1267360794414161920, '【消息服务】- 通知', '/signalr-hubs/notifications/{everything}', '', '', '/signalr-hubs/notifications/{everything}', 'POST,GET,OPTIONS,PUT,DELETE,', '', '', '', '', '', '', '', 1, '', '', 'ws', '127.0.0.1:30020,', '', '', '', 1, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (70, '{}', 'f3aa2b42dd9f468aa5aae4ef64754427', 1267383367629807616, '【消息服务】- 通知0', '/signalr-hubs/notifications', '', '', '/signalr-hubs/notifications', 'GET,POST,PUT,DELETE,OPTIONS,', '', '', '', '', '', '', '', 1, '', '', 'ws', '127.0.0.1:30020,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (71, '{}', '0344947bb79b401baa2ef7b4e58297f6', 1267817055527632896, '【消息服务】- 聊天', '/signalr-hubs/messages', '', '', '/signalr-hubs/messages', 'GET,POST,PUT,DELETE,OPTIONS,', '', '', '', '', '', '', '', 1, '', '', 'ws', '127.0.0.1:30020,', '', '', '', 1, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (72, '{}', '6676b5e5f76d40739f9ccc3e371e2f18', 1267817221286526976, '【消息服务】- 聊天1', '/signalr-hubs/messages/{everything}', '', '', '/signalr-hubs/messages/{everything}', 'GET,POST,PUT,DELETE,OPTIONS,', '', '', '', '', '', '', '', 1, '', '', 'ws', '127.0.0.1:30020,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (73, '{}', 'cfb5f09a12bf495fbcaf2fa5d9123a40', 1268893687085518848, '【身份认证服务】- 重置密码', '/api/account/phone/reset-password', '', '', '/api/account/phone/reset-password', 'PUT,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 1, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (101, '{}', '997a4c27a433458aafed9b8aa252d957', 1288657613998579712, '【身份认证服务】- 组织机构列表', '/api/identity/organization-units', '', '', '/api/identity/organization-units', 'GET,POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (102, '{}', 'a2c6acc9882a425ab26bd3ad5a9c17c6', 1288657941770854400, '【身份认证服务】- 组织机构管理', '/api/identity/organization-units/{id}', '', '', '/api/identity/organization-units/{id}', 'GET,PUT,DELETE,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 1, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (103, '{}', '390acfb0e16943c6b61e731d47c282e9', 1288658134067109888, '【身份认证服务】- 组织机构移动', '/api/identity/organization-units/{id}/move', '', '', '/api/identity/organization-units/{id}/move', 'PUT,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (104, '{}', '3515e75becf9447492ad60466b27c397', 1288658305156964352, '【身份认证服务】- 查询组织机构子级', '/api/identity/organization-units/find-children', '', '', '/api/identity/organization-units/find-children', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (105, '{}', 'aab0a24d930f4f9687497e5ccaac2a31', 1288658491216289792, '【身份认证服务】- 查询组织机构最后一个子节点', '/api/identity/organization-units/last-children', '', '', '/api/identity/organization-units/last-children', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (106, '{}', '9b7c999b1c5140c497bc15914a815401', 1288658638302142464, '【身份认证服务】- 未加入组织机构角色', '/api/identity/organization-units/{id}/unadded-roles', '', '', '/api/identity/organization-units/{id}/unadded-roles', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (107, '{}', 'b0cdb3f6908e42bd934ca99a78f22c3f', 1288658791784308736, '【身份认证服务】- 未加入组织机构用户', '/api/identity/organization-units/{id}/unadded-users', '', '', '/api/identity/organization-units/{id}/unadded-users', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (108, '{}', '1982bc6ffd92445b9b3f47067378d1fc', 1290849478956199936, '【后台管理】- 当前租户设置', '/api/setting-management/settings/by-current-tenant', '', '', '/api/setting-management/settings/by-current-tenant', 'GET,PUT,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (109, '{}', 'a7df3a04805d4cc8a6e6b3823c6dd468', 1290849628051124224, '【后台管理】- 用户设置', '/api/setting-management/settings/by-user/{userId}', '', '', '/api/setting-management/settings/by-user/{userId}', 'GET,PUT,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (110, '{}', 'ef6e38a529a345fab67f6a627cf20635', 1290849798553776128, '【后台管理】- 当前用户设置', '/api/setting-management/settings/by-current-user', '', '', '/api/setting-management/settings/by-current-user', 'GET,PUT,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (112, '{}', '9844fed6507844f2ac64bd08649bd3a6', 1291259822512693248, '【身份认证服务】- 查询组织机构根节点', '/api/identity/organization-units/root-node', '', '', '/api/identity/organization-units/root-node', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (113, '{}', '24d8794cf8f943b4ac45d2bcccf7c128', 1292620505149145088, '【平台服务】- 文件系统', '/api/file-management/file-system', '', '', '/api/file-management/file-system', 'GET,PUT,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30025,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (114, '{}', '0acf6762d3af43efb655107e0039f5fc', 1292620665505775616, '【平台服务】- 文件系统 - 目录管理', '/api/file-management/file-system/folders', '', '', '/api/file-management/file-system/folders', 'POST,DELETE,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30025,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (115, '{}', '8b4363f70865419089b5f62ba35382df', 1292620843398791168, '【平台服务】- 文件系统 - 文件管理', '/api/file-management/file-system/files', '', '', '/api/file-management/file-system/files', 'GET,POST,DELETE,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30025,', '', '', '', 0, 1200000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (116, '{}', '7eb315567bbc470bbbfd26923c5d0aba', 1292621027574874112, '【平台服务】- 文件系统 - 复制目录', '/api/file-management/file-system/folders/copy', '', '', '/api/file-management/file-system/folders/copy', 'PUT,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30025,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (117, '{}', 'af5853680cff454fa66ff6022f18da23', 1292621363161137152, '【平台服务】- 文件系统 - 移动目录', '/api/file-management/file-system/folders/move', '', '', '/api/file-management/file-system/folders/move', 'PUT,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30025,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (118, '{}', '6daa6d8c8adb466899988fd8181c29a8', 1292621494837116928, '【平台服务】- 文件系统 - 复制文件', '/api/file-management/file-system/files/copy', '', '', '/api/file-management/file-system/files/copy', 'PUT,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30025,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (119, '{}', '9560caaa3bd9424984c44724aa54bfe9', 1292621629260365824, '【平台服务】- 文件系统 - 移动文件', '/api/file-management/file-system/files/move', '', '', '/api/file-management/file-system/files/move', 'PUT,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30025,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (120, '{}', 'fc2aaa6035484201b9014912930fb7cb', 1292622526073864192, '【平台服务】- 文件系统 - 详情页', '/api/file-management/file-system/profile', '', '', '/api/file-management/file-system/profile', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30025,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (121, '{}', 'c6c7b027000942dda8ba0d2e2d8cf705', 1293470838745821184, '【后台管理】- 框架配置', '/api/abp/application-configuration', '', '', '/api/abp/backend-admin/application-configuration', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', 'backend-admin-configuration', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (122, '{}', 'becd4342079d4399abda5b5ba3b46fdc', 1293471661785706496, '【消息服务】- 框架配置', '/api/abp/application-configuration', '', '', '/api/abp/messages/application-configuration', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30020,', '', '', 'messages-configuration', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (123, '{}', 'c828140cee3043c18ffc274f6461f0f2', 1293472678392721408, '【后台管理】- 接口代理', '/api/abp/api-definition', '', '', '/api/abp/backend-admin/api-definition', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', 'backend-admin-api-definition', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (124, '{}', 'e683cff8066d4c2899a17d0f618f1a0b', 1293472857510473728, '【消息服务】- 接口代理', '/api/abp/api-definition', '', '', '/api/abp/messages/api-definition', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30020,', '', '', 'messages-api-definition', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (125, '{}', '0e9c3bff5b58428eba97a5516140ba5e', 1299273336009359360, '【消息服务】- Hangfire仪表板 ', '/hangfire', '', '', '/hangfire', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30020,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (126, '{}', 'e906924ad3a947cf8e6956e2dd258192', 1299273436282585088, '【消息服务】- Hangfire仪表板 - 主页', '/hangfire/', '', '', '/hangfire/', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30020,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (127, '{}', 'e02f2049efbc4ee1ad6629bd0341ed2b', 1299273618470567936, '【消息服务】- Hangfire仪表板 - 状态', '/hangfire/stats', '', '', '/hangfire/stats', 'POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30020,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (128, '{}', 'f8d2b2f0f1d649c2a07eeef23d6adb0e', 1299273770182737920, '【消息服务】- Hangfire仪表板 - 作业管理', '/hangfire/jobs/{everything}', '', '', '/hangfire/jobs/{everything}', 'GET,POST,PUT,DELETE,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30020,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (129, '{}', '9785be7a29774b468e271b23009fe115', 1299273978023084032, '【消息服务】- Hangfire仪表板 - 重试', '/hangfire/retries', '', '', '/hangfire/retries', 'GET,POST,PUT,DELETE,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30020,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (130, '{}', '9c0c1cd196bb45c0bc03fafb7a1eb8f2', 1299274123225694208, '【消息服务】- Hangfire仪表板 - 周期性作业', '/hangfire/recurring', '', '', '/hangfire/recurring', 'GET,POST,DELETE,PUT,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30020,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (131, '{}', '243bafe828be463ea63a3e2b521f9923', 1299274222299348992, '【消息服务】- Hangfire仪表板 - 服务器列表', '/hangfire/servers', '', '', '/hangfire/servers', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30020,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (132, '{}', '57a8ac1b41bb434cad38fbde0e2ba2f0', 1304238876758495232, '【后台管理】- 管理功能', '/api/feature-management/features', '', '', '/api/feature-management/features', 'GET,PUT,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (133, '{}', '40a150f629b047f587c91a9436a699c0', 1304678610343383040, '【身份认证服务】- 接口代理', '/api/abp/api-definition', '', '', '/api/abp/identity-server/api-definition', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', 'identity-server-api-definition', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (134, '{}', '3a2d5d538fa44ac690402fc5c4e1a401', 1304679169305694208, '【身份认证服务】- 框架配置', '/api/abp/application-configuration', '', '', '/api/abp/identity-server/application-configuration', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', 'identity-server-configuration', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (135, '{}', '2ecfe7483bc94c28ad0769b654eb765d', 1310460417141817344, '【后台管理】- 审计日志列表', '/api/auditing/audit-log', '', '', '/api/auditing/audit-log', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (136, '{}', '744e340c0024462d88458b7ea9605b3c', 1310502391475519488, '【后台服务】- 安全日志列表', '/api/auditing/security-log', '', '', '/api/auditing/security-log', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (137, '{}', 'a99639f4172547c4ba9b4f4ca5cb4ab9', 1310515546943569920, '【后台服务】- 安全日志', '/api/auditing/security-log/{id}', '', '', '/api/auditing/security-log/{id}', 'DELETE,GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (138, '{}', 'c3ebc82d55f640fb9d70a911e97e4ec1', 1310515735292985344, '【后台管理】- 审计日志', '/api/auditing/audit-log/{id}', '', '', '/api/auditing/audit-log/{id}', 'DELETE,GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30010,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (139, '{}', '0379fcb3a9cd4b13b562b3b5b5c3eb7d', 1316628769783480320, '【身份认证服务】- 声明类型', '/api/identity/claim-types', '', '', '/api/identity/claim-types', 'GET,POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (140, '{}', 'de25c9a80d994f728b37eb483b2f5127', 1316628940663619584, '【身份认证服务】- 管理声明类型', '/api/identity/claim-types/{id}', '', '', '/api/identity/claim-types/{id}', 'GET,PUT,DELETE,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (141, '{}', '25c19106baff4cf3a877ae8cd690a1b5', 1316629112428756992, '【身份认证服务】- 查询在用的声明类型列表', '/api/identity/claim-types/actived-list', '', '', '/api/identity/claim-types/actived-list', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (142, '{}', 'd42f8b779cf344eeaa24df0ae37fb7cf', 1316652047017246720, '【身份认证服务】- 管理用户声明', '/api/identity/users/claims/{id}', '', '', '/api/identity/users/claims/{id}', 'POST,PUT,DELETE,GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (143, '{}', 'b86af44a34a14db4b482df8550f1bde1', 1316913899996737536, '【身份认证管理】- 管理角色声明', '/api/identity/roles/claims/{id}', '', '', '/api/identity/roles/claims/{id}', 'GET,POST,PUT,DELETE,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (144, '{}', '51a14bc295044de985ae014fbcc5bddf', 1319200951383199744, '【IdentityServer4】- 发现端点', '/.well-known/openid-configuration', '', '', '/.well-known/openid-configuration', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:44385,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (145, '{}', '9d859a444d774e93818237e53b6cc102', 1319221929807024128, '【身份认证服务】- 查询所有组织机构', '/api/identity/organization-units/all', '', '', '/api/identity/organization-units/all', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (146, '{}', '89f42175b24540caba2a1fded145acf8', 1319554431134306304, '【身份认证服务】- 管理组织机构用户', '/api/identity/organization-units/{id}/users', '', '', '/api/identity/organization-units/{id}/users', 'POST,GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (147, '{}', '126d82509cec43eda712e94737b01039', 1319554550458060800, '【身份认证服务】- 管理组织机构角色', '/api/identity/organization-units/{id}/roles', '', '', '/api/identity/organization-units/{id}/roles', 'POST,GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (148, '{}', '824f5eee6877489f96f1022e306c968a', 1319554948434595840, '【身份认证服务】- 管理角色组织机构', '/api/identity/roles/{id}/organization-units', '', '', '/api/identity/roles/{id}/organization-units', 'GET,PUT,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (149, '{}', 'fe1379d4a13f41afb6410f4c948871f3', 1319555067183730688, '【身份认证服务】- 删除角色组织机构', '/api/identity/roles/{id}/organization-units/{ouId}', '', '', '/api/identity/roles/{id}/organization-units/{ouId}', 'DELETE,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (150, '{}', '197a69bb723346aba3601bd61e7fa655', 1319555230765780992, '【身份认证服务】- 管理用户组织机构', '/api/identity/users/{id}/organization-units', '', '', '/api/identity/users/{id}/organization-units', 'GET,PUT,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (151, '{}', '1a7e6d0b1c95484f82a75a2ce6e6f453', 1319555333790470144, '【身份认证服务】- 删除用户组织机构', '/api/identity/users/{id}/organization-units/{ouId}', '', '', '/api/identity/users/{id}/organization-units/{ouId}', 'DELETE,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30015,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (152, '{}', 'ecfa9bbd19694097b33e691b653f2124', 1321001932510203904, '【消息服务】- 我的消息', '/api/im/chat/my-messages', '', '', '/api/im/chat/my-messages', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30020,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (153, '{}', '4867ad188ca54acb8b961d20297b6545', 1321002059803136000, '【消息服务】- 我的最近消息', '/api/im/chat/my-last-messages', '', '', '/api/im/chat/my-last-messages', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30020,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (154, '{}', '291ab802d7bc4af98fc15c509f98fa75', 1321002256440496128, '【消息服务】- 我的朋友', '/api/im/my-friends', '', '', '/api/im/my-friends', 'GET,POST,DELETE,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30020,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (155, '{}', '08438dabb1e849988e0c304f82b08a10', 1321002350686507008, '【消息服务】- 我的所有朋友', '/api/im/my-friends/all', '', '', '/api/im/my-friends/all', 'GET,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30020,', '', '', '', 0, 30000, 1, '', 'TEST-APP');
INSERT INTO `appapigatewayreroute` VALUES (156, '{}', 'cecf632785b7402299764698369c751f', 1322190027988525056, '【消息服务】- 发送好友请求', '/api/im/my-friends/add-request', '', '', '/api/im/my-friends/add-request', 'POST,', '', '', '', '', '', '', '', 1, '', '', 'HTTP', '127.0.0.1:30020,', '', '', '', 0, 30000, 1, '', 'TEST-APP');

-- ----------------------------
-- Table structure for appapigatewayroutegroup
-- ----------------------------
DROP TABLE IF EXISTS `appapigatewayroutegroup`;
CREATE TABLE `appapigatewayroutegroup`  (
  `Id` char(36) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `ExtraProperties` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `ConcurrencyStamp` varchar(40) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL,
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
INSERT INTO `appapigatewayroutegroup` VALUES ('08d7f735-a83b-49ab-8cee-5d602502bea8', '{}', '83cac848676f4b658d5c9f7d802a497a', '2020-05-13 20:03:32.524271', NULL, '2020-08-05 15:43:28.205288', 'bf289dbb-838e-a89b-c622-39f51dcc4f43', 0, NULL, NULL, 'abp后台管理', 'TEST-APP', 'abp后台管理', '127.0.0.1', 'abp后台管理项目网关', 1);

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
) ENGINE = InnoDB AUTO_INCREMENT = 149 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

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
INSERT INTO `appapigatewaysecurityoptions` VALUES (21, 1262220447629058048, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (22, 1262230734939758592, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (23, 1262296916350869504, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (24, 1262632376348594176, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (25, 1262632791869902848, '', '');
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
INSERT INTO `appapigatewaysecurityoptions` VALUES (94, 1288657613998579712, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (95, 1288657941770854400, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (96, 1288658134067109888, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (97, 1288658305156964352, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (98, 1288658491216289792, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (99, 1288658638302142464, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (100, 1288658791784308736, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (101, 1290849478956199936, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (102, 1290849628051124224, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (103, 1290849798553776128, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (105, 1291259822512693248, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (106, 1292620505149145088, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (107, 1292620665505775616, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (108, 1292620843398791168, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (109, 1292621027574874112, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (110, 1292621363161137152, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (111, 1292621494837116928, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (112, 1292621629260365824, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (113, 1292622526073864192, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (114, 1293470838745821184, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (115, 1293471661785706496, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (116, 1293472678392721408, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (117, 1293472857510473728, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (118, 1299273336009359360, '127.0.0.1', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (119, 1299273436282585088, '127.0.0.1', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (120, 1299273618470567936, '127.0.0.1', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (121, 1299273770182737920, '127.0.0.1', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (122, 1299273978023084032, '127.0.0.1', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (123, 1299274123225694208, '127.0.0.1', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (124, 1299274222299348992, '127.0.0.1', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (125, 1304238876758495232, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (126, 1304678610343383040, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (127, 1304679169305694208, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (128, 1310460417141817344, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (129, 1310502391475519488, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (130, 1310515546943569920, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (131, 1310515735292985344, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (132, 1316628769783480320, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (133, 1316628940663619584, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (134, 1316629112428756992, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (135, 1316652047017246720, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (136, 1316913899996737536, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (137, 1319200951383199744, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (138, 1319221929807024128, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (139, 1319554431134306304, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (140, 1319554550458060800, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (141, 1319554948434595840, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (142, 1319555067183730688, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (143, 1319555230765780992, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (144, 1319555333790470144, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (145, 1321001932510203904, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (146, 1321002059803136000, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (147, 1321002256440496128, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (148, 1321002350686507008, '', '');
INSERT INTO `appapigatewaysecurityoptions` VALUES (149, 1322190027988525056, '', '');

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
INSERT INTO `cap.published` VALUES (1322190029573971968, 'v1', 'LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData', '{\"Headers\":{\"cap-callback-name\":null,\"cap-msg-id\":\"1322190029573971968\",\"cap-msg-name\":\"LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData\",\"cap-msg-type\":\"Object\",\"cap-senttime\":\"2020/10/30 22:54:02 +08:00\",\"cap-corr-id\":\"1322190029573971968\",\"cap-corr-seq\":\"0\"},\"Value\":{\"DateTime\":\"2020-10-30T22:54:02.8070185+08:00\",\"AppId\":\"TEST-APP\",\"Method\":\"Create\",\"Object\":\"ReRoute\"}}', 0, '2020-10-30 22:54:03', '2020-10-31 22:54:03', 'Succeeded');

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
