/*
 Navicat Premium Data Transfer

 Source Server         : 本机服务器
 Source Server Type    : MySQL
 Source Server Version : 80020
 Source Host           : localhost:3306
 Source Schema         : apigateway

 Target Server Type    : SQL Server
 Target Server Version : 12000000
 File Encoding         : 65001

 Date: 29/03/2021 21:22:42
*/


-- ----------------------------
-- Table structure for __efmigrationshistory
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[apigateway].[__efmigrationshistory]') AND type IN ('U'))
	DROP TABLE [apigateway].[__efmigrationshistory]
GO

CREATE TABLE [apigateway].[__efmigrationshistory] (
  [MigrationId] varchar(95) NOT NULL,
  [ProductVersion] varchar(32) NOT NULL
)
GO


-- ----------------------------
-- Records of __efmigrationshistory
-- ----------------------------
BEGIN TRANSACTION
GO

INSERT INTO [apigateway].[__efmigrationshistory] VALUES (N'20200513034946_Migration-ApiGateway-MySql', N'3.1.3'), (N'20200513111130_Rename-Router-To-RouteGroup', N'3.1.3'), (N'20200618090102_Modify-ReRoute-Index-Unique', N'3.1.4'), (N'20200908020925_Upgrade-abp-3.1.0', N'3.1.7')
GO

COMMIT
GO


-- ----------------------------
-- Table structure for appapigatewayaggregate
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[apigateway].[appapigatewayaggregate]') AND type IN ('U'))
	DROP TABLE [apigateway].[appapigatewayaggregate]
GO

CREATE TABLE [apigateway].[appapigatewayaggregate] (
  [Id] int NOT NULL,
  [ExtraProperties] nvarchar(max) NULL,
  [ConcurrencyStamp] nvarchar(40) NULL,
  [AppId] nvarchar(50) NOT NULL,
  [Name] nvarchar(max) NULL,
  [ReRouteId] bigint NOT NULL,
  [ReRouteKeys] nvarchar(1000) NULL,
  [UpstreamPathTemplate] nvarchar(1000) NULL,
  [UpstreamHost] nvarchar(1000) NULL,
  [ReRouteIsCaseSensitive] tinyint NOT NULL,
  [Aggregator] nvarchar(256) NULL,
  [Priority] int NULL,
  [UpstreamHttpMethod] nvarchar(500) NULL
)
GO


-- ----------------------------
-- Records of appapigatewayaggregate
-- ----------------------------
BEGIN TRANSACTION
GO

INSERT INTO [apigateway].[appapigatewayaggregate] VALUES (N'5', N'{}', N'90044e20fde546bab1bdd45999f8208a', N'TEST-APP', N'abp接口代理服务', N'1263083077348196352', N'platform-api-definition,backend-admin-api-definition,messages-api-definition,apigateway-api-definition,identity-server-api-definition,localization-api-definition,', N'/api/abp/api-definition', N'', N'1', N'AbpApiDefinitionAggregator', NULL, N''), (N'6', N'{}', N'870a2c5df9b34f8c9514aef0250fbb47', N'TEST-APP', N'abp框架配置', N'1263102116090970112', N'apigateway-configuration,platform-configuration,backend-admin-configuration,messages-configuration,identity-server-configuration,', N'/api/abp/application-configuration', N'', N'1', N'AbpApiDefinitionAggregator', NULL, N''), (N'8', N'{}', N'edc962f7e0844bb09cb0fb731f358b4b', N'TEST-APP', N'我的消息订阅', N'1322503807309881344', N'assignables-notifilers,my-subscribes,', N'/api/my-subscribes/assignables-notifilers', N'', N'1', N'AbpApiDefinitionAggregator', NULL, N''), (N'9', N'{}', N'c65227b8a0e143b2a0b6186ffde3dfc1', N'TEST-APP', N'全局设置', N'1329708867127799808', N'setting-global,wechat-setting-global,aliyun-setting-global,oss-management-global,', N'/api/setting-management/settings/by-global', N'', N'1', N'AbpApiDefinitionAggregator', NULL, N''), (N'10', N'{}', N'29fb4cbfcdfe41478b931df8c549992b', N'TEST-APP', N'当前租户设置', N'1329709265255329792', N'setting-current-tenant,wechat-setting-current-tenant,aliyun-setting-current-tenant,oss-management-current-tenant,', N'/api/setting-management/settings/by-current-tenant', N'', N'1', N'AbpApiDefinitionAggregator', NULL, N'')
GO

COMMIT
GO


-- ----------------------------
-- Table structure for appapigatewayaggregateconfig
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[apigateway].[appapigatewayaggregateconfig]') AND type IN ('U'))
	DROP TABLE [apigateway].[appapigatewayaggregateconfig]
GO

CREATE TABLE [apigateway].[appapigatewayaggregateconfig] (
  [Id] int NOT NULL,
  [ReRouteId] bigint NOT NULL,
  [ReRouteKey] nvarchar(256) NULL,
  [Parameter] nvarchar(1000) NULL,
  [JsonPath] nvarchar(256) NULL,
  [AggregateReRouteId] int NULL
)
GO


-- ----------------------------
-- Table structure for appapigatewayauthoptions
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[apigateway].[appapigatewayauthoptions]') AND type IN ('U'))
	DROP TABLE [apigateway].[appapigatewayauthoptions]
GO

CREATE TABLE [apigateway].[appapigatewayauthoptions] (
  [Id] int NOT NULL,
  [ReRouteId] bigint NOT NULL,
  [AuthenticationProviderKey] nvarchar(100) NULL,
  [AllowedScopes] nvarchar(200) NULL
)
GO


-- ----------------------------
-- Records of appapigatewayauthoptions
-- ----------------------------
BEGIN TRANSACTION
GO

INSERT INTO [apigateway].[appapigatewayauthoptions] VALUES (N'3', N'1261299170387169280', NULL, N''), (N'4', N'1261585859064872960', NULL, N''), (N'5', N'1261586605810368512', NULL, N''), (N'6', N'1261587558609436672', NULL, N''), (N'7', N'1261588213298348032', NULL, N''), (N'8', N'1261588367619375104', NULL, N''), (N'9', N'1261588628450557952', NULL, N''), (N'10', N'1261588881564221440', NULL, N''), (N'11', N'1261588983053795328', NULL, N''), (N'12', N'1261589139039961088', NULL, N''), (N'13', N'1261589197483393024', NULL, N''), (N'14', N'1261589278857084928', NULL, N''), (N'15', N'1261589420356124672', NULL, N''), (N'16', N'1261589960393736192', NULL, N''), (N'17', N'1261606600242085888', NULL, N''), (N'18', N'1261606689601732608', NULL, N''), (N'21', N'1262220447629058048', NULL, N''), (N'22', N'1262230734939758592', NULL, N''), (N'23', N'1262296916350869504', NULL, N''), (N'24', N'1262632376348594176', N'', N''), (N'25', N'1262632791869902848', N'', N''), (N'28', N'1262660336921235456', N'', N''), (N'29', N'1262660528277966848', N'', N''), (N'30', N'1262660706875625472', N'', N''), (N'31', N'1262660966393991168', N'', N''), (N'32', N'1262661109474283520', N'', N''), (N'33', N'1262663888804663296', N'', N''), (N'34', N'1262664024096133120', N'', N''), (N'35', N'1262664186252120064', N'', N''), (N'36', N'1262664357044178944', N'', N''), (N'37', N'1262664632928718848', N'', N''), (N'38', N'1262664751409418240', N'', N''), (N'39', N'1262664871274237952', N'', N''), (N'40', N'1262665026111164416', N'', N''), (N'41', N'1262665159905267712', N'', N''), (N'42', N'1262665329829105664', N'', N''), (N'43', N'1262665456471920640', N'', N''), (N'44', N'1262665628165754880', N'', N''), (N'45', N'1262666172682883072', N'', N''), (N'47', N'1262723402331885568', N'', N''), (N'48', N'1262935771746734080', N'', N''), (N'49', N'1262935906522304512', N'', N''), (N'52', N'1263074419073593344', N'', N''), (N'53', N'1263075249394790400', N'', N''), (N'54', N'1263075593499684864', N'', N''), (N'56', N'1263101898440146944', N'', N''), (N'57', N'1263303878648569856', N'', N''), (N'58', N'1263304204797648896', N'', N''), (N'59', N'1263304872891555840', N'', N''), (N'60', N'1263305106250047488', N'', N''), (N'61', N'1263305244594970624', N'', N''), (N'62', N'1263305430536855552', N'', N''), (N'63', N'1263639172959174656', N'', N''), (N'64', N'1264799968944640000', N'', N''), (N'65', N'1264800070161584128', N'', N''), (N'66', N'1267360794414161920', N'', N''), (N'67', N'1267383367629807616', N'', N''), (N'68', N'1267817055527632896', N'', N''), (N'69', N'1267817221286526976', N'', N''), (N'70', N'1268893687085518848', N'', N''), (N'94', N'1288657613998579712', N'', N''), (N'95', N'1288657941770854400', N'', N''), (N'96', N'1288658134067109888', N'', N''), (N'97', N'1288658305156964352', N'', N''), (N'98', N'1288658491216289792', N'', N''), (N'99', N'1288658638302142464', N'', N''), (N'100', N'1288658791784308736', N'', N''), (N'101', N'1290849478956199936', N'', N''), (N'102', N'1290849628051124224', N'', N''), (N'103', N'1290849798553776128', N'', N''), (N'105', N'1291259822512693248', N'', N''), (N'114', N'1293470838745821184', N'', N''), (N'115', N'1293471661785706496', N'', N''), (N'116', N'1293472678392721408', N'', N''), (N'117', N'1293472857510473728', N'', N''), (N'118', N'1299273336009359360', N'', N''), (N'119', N'1299273436282585088', N'', N''), (N'120', N'1299273618470567936', N'', N''), (N'121', N'1299273770182737920', N'', N''), (N'122', N'1299273978023084032', N'', N''), (N'123', N'1299274123225694208', N'', N''), (N'124', N'1299274222299348992', N'', N''), (N'125', N'1304238876758495232', N'', N''), (N'126', N'1304678610343383040', N'', N''), (N'127', N'1304679169305694208', N'', N''), (N'128', N'1310460417141817344', N'', N''), (N'129', N'1310502391475519488', N'', N''), (N'130', N'1310515546943569920', N'', N''), (N'131', N'1310515735292985344', N'', N''), (N'132', N'1316628769783480320', N'', N''), (N'133', N'1316628940663619584', N'', N''), (N'134', N'1316629112428756992', N'', N''), (N'135', N'1316652047017246720', N'', N''), (N'136', N'1316913899996737536', N'', N''), (N'137', N'1319200951383199744', N'', N''), (N'138', N'1319221929807024128', N'', N''), (N'139', N'1319554431134306304', N'', N''), (N'140', N'1319554550458060800', N'', N''), (N'141', N'1319554948434595840', N'', N''), (N'142', N'1319555067183730688', N'', N''), (N'143', N'1319555230765780992', N'', N''), (N'144', N'1319555333790470144', N'', N''), (N'145', N'1321001932510203904', N'', N''), (N'146', N'1321002059803136000', N'', N''), (N'147', N'1321002256440496128', N'', N''), (N'148', N'1321002350686507008', N'', N''), (N'149', N'1322190027988525056', N'', N''), (N'150', N'1322452079688458240', N'', N''), (N'151', N'1322452183929495552', N'', N''), (N'152', N'1322452308651319296', N'', N''), (N'153', N'1322452858176446464', N'', N''), (N'154', N'1322452989235863552', N'', N''), (N'155', N'1322453089655889920', N'', N''), (N'156', N'1329706860249804800', N'', N''), (N'157', N'1329707002411544576', N'', N''), (N'158', N'1329708512277098496', N'', N''), (N'159', N'1329708625917571072', N'', N''), (N'160', N'1335049839287357440', N'', N''), (N'161', N'1335050034221830144', N'', N''), (N'162', N'1335050145899368448', N'', N''), (N'163', N'1335050283434790912', N'', N''), (N'164', N'1335050381770248192', N'', N''), (N'165', N'1335050520941449216', N'', N''), (N'166', N'1335050615829188608', N'', N''), (N'167', N'1335111798720450560', N'', N''), (N'168', N'1335118541370314752', N'', N''), (N'169', N'1335118660417245184', N'', N''), (N'170', N'1335118782727344128', N'', N''), (N'171', N'1335118903200337920', N'', N''), (N'172', N'1336230645078921216', N'', N''), (N'173', N'1337314809113722880', N'', N''), (N'174', N'1337314938973569024', N'', N''), (N'175', N'1340961907637243904', N'', N''), (N'176', N'1341652247554379776', N'', N''), (N'177', N'1341652385555369984', N'', N''), (N'178', N'1342457939827552256', N'', N''), (N'179', N'1342458050112581632', N'', N''), (N'180', N'1363382062055915520', N'', N''), (N'181', N'1363382298501414912', N'', N''), (N'182', N'1368854800347848704', N'', N''), (N'183', N'1368855936576413696', N'', N''), (N'184', N'1368856295889854464', N'', N''), (N'185', N'1368856703572008960', N'', N''), (N'186', N'1368856819242524672', N'', N''), (N'187', N'1368856927887581184', N'', N''), (N'188', N'1368857128383700992', N'', N''), (N'189', N'1369560306297233408', N'', N''), (N'190', N'1369560450472239104', N'', N''), (N'191', N'1371705034307375104', N'', N''), (N'192', N'1376442078396276736', N'', N''), (N'193', N'1376442309850554368', N'', N''), (N'194', N'1376442440536678400', N'', N''), (N'195', N'1376442557943635968', N'', N''), (N'196', N'1376442689674141696', N'', N''), (N'197', N'1376442971032248320', N'', N''), (N'198', N'1376443123021242368', N'', N''), (N'199', N'1376443238851141632', N'', N''), (N'200', N'1376443392249421824', N'', N''), (N'201', N'1376443586777047040', N'', N''), (N'202', N'1376467826087682048', N'', N''), (N'203', N'1376467990894469120', N'', N''), (N'204', N'1376468110214029312', N'', N'')
GO

COMMIT
GO


-- ----------------------------
-- Table structure for appapigatewaybalanceroptions
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[apigateway].[appapigatewaybalanceroptions]') AND type IN ('U'))
	DROP TABLE [apigateway].[appapigatewaybalanceroptions]
GO

CREATE TABLE [apigateway].[appapigatewaybalanceroptions] (
  [Id] int NOT NULL,
  [ItemId] bigint NULL,
  [ReRouteId] bigint NULL,
  [Type] nvarchar(100) NULL,
  [Key] nvarchar(100) NULL,
  [Expiry] int NULL
)
GO


-- ----------------------------
-- Records of appapigatewaybalanceroptions
-- ----------------------------
BEGIN TRANSACTION
GO

INSERT INTO [apigateway].[appapigatewaybalanceroptions] VALUES (N'1', N'1260841964962947072', NULL, N'LeastConnection', NULL, NULL), (N'4', NULL, N'1261299170387169280', N'LeastConnection', NULL, N'60000'), (N'5', NULL, N'1261585859064872960', NULL, NULL, NULL), (N'6', NULL, N'1261586605810368512', NULL, NULL, NULL), (N'7', NULL, N'1261587558609436672', NULL, NULL, NULL), (N'8', NULL, N'1261588213298348032', NULL, NULL, NULL), (N'9', NULL, N'1261588367619375104', NULL, NULL, NULL), (N'10', NULL, N'1261588628450557952', NULL, NULL, NULL), (N'11', NULL, N'1261588881564221440', NULL, NULL, NULL), (N'12', NULL, N'1261588983053795328', NULL, NULL, NULL), (N'13', NULL, N'1261589139039961088', NULL, NULL, NULL), (N'14', NULL, N'1261589197483393024', NULL, NULL, NULL), (N'15', NULL, N'1261589278857084928', NULL, NULL, NULL), (N'16', NULL, N'1261589420356124672', NULL, NULL, NULL), (N'17', NULL, N'1261589960393736192', N'LeastConnection', NULL, N'60000'), (N'18', NULL, N'1261606600242085888', NULL, NULL, NULL), (N'19', NULL, N'1261606689601732608', NULL, NULL, NULL), (N'22', NULL, N'1262220447629058048', NULL, NULL, NULL), (N'23', NULL, N'1262230734939758592', NULL, NULL, NULL), (N'24', NULL, N'1262296916350869504', NULL, NULL, NULL), (N'25', NULL, N'1262632376348594176', N'', N'', N'0'), (N'26', NULL, N'1262632791869902848', N'', N'', N'0'), (N'29', NULL, N'1262660336921235456', N'', N'', N'0'), (N'30', NULL, N'1262660528277966848', N'', N'', N'0'), (N'31', NULL, N'1262660706875625472', N'', N'', N'0'), (N'32', NULL, N'1262660966393991168', N'', N'', N'0'), (N'33', NULL, N'1262661109474283520', N'', N'', N'0'), (N'34', NULL, N'1262663888804663296', N'', N'', N'0'), (N'35', NULL, N'1262664024096133120', N'', N'', N'0'), (N'36', NULL, N'1262664186252120064', N'', N'', N'0'), (N'37', NULL, N'1262664357044178944', N'', N'', N'0'), (N'38', NULL, N'1262664632928718848', N'', N'', N'0'), (N'39', NULL, N'1262664751409418240', N'', N'', N'0'), (N'40', NULL, N'1262664871274237952', N'', N'', N'0'), (N'41', NULL, N'1262665026111164416', N'', N'', N'0'), (N'42', NULL, N'1262665159905267712', N'', N'', N'0'), (N'43', NULL, N'1262665329829105664', N'', N'', N'0'), (N'44', NULL, N'1262665456471920640', N'', N'', N'0'), (N'45', NULL, N'1262665628165754880', N'', N'', N'0'), (N'46', NULL, N'1262666172682883072', N'', N'', N'0'), (N'48', NULL, N'1262723402331885568', N'', N'', N'0'), (N'49', NULL, N'1262935771746734080', N'', N'', N'0'), (N'50', NULL, N'1262935906522304512', N'', N'', N'0'), (N'53', NULL, N'1263074419073593344', N'', N'', N'0'), (N'54', NULL, N'1263075249394790400', N'', N'', N'0'), (N'55', NULL, N'1263075593499684864', N'', N'', N'0'), (N'57', NULL, N'1263101898440146944', N'', N'', N'0'), (N'58', NULL, N'1263303878648569856', N'', N'', N'0'), (N'59', NULL, N'1263304204797648896', N'', N'', N'0'), (N'60', NULL, N'1263304872891555840', N'', N'', N'0'), (N'61', NULL, N'1263305106250047488', N'', N'', N'0'), (N'62', NULL, N'1263305244594970624', N'', N'', N'0'), (N'63', NULL, N'1263305430536855552', N'', N'', N'0'), (N'64', NULL, N'1263639172959174656', N'', N'', N'0'), (N'65', NULL, N'1264799968944640000', N'', N'', N'0'), (N'66', NULL, N'1264800070161584128', N'', N'', N'0'), (N'68', NULL, N'1267360794414161920', N'', N'', N'0'), (N'69', NULL, N'1267383367629807616', N'', N'', N'0'), (N'70', NULL, N'1267817055527632896', N'', N'', N'0'), (N'71', NULL, N'1267817221286526976', N'', N'', N'0'), (N'72', NULL, N'1268893687085518848', N'', N'', N'0'), (N'97', NULL, N'1288657613998579712', N'LeastConnection', N'', N'60000'), (N'98', NULL, N'1288657941770854400', N'', N'', N'0'), (N'99', NULL, N'1288658134067109888', N'', N'', N'0'), (N'100', NULL, N'1288658305156964352', N'', N'', N'0'), (N'101', NULL, N'1288658491216289792', N'', N'', N'0'), (N'102', NULL, N'1288658638302142464', N'', N'', N'0'), (N'103', NULL, N'1288658791784308736', N'', N'', N'0'), (N'104', NULL, N'1290849478956199936', N'', N'', N'0'), (N'105', NULL, N'1290849628051124224', N'', N'', N'0'), (N'106', NULL, N'1290849798553776128', N'', N'', N'0'), (N'108', NULL, N'1291259822512693248', N'', N'', N'0'), (N'117', NULL, N'1293470838745821184', N'', N'', N'0'), (N'118', NULL, N'1293471661785706496', N'', N'', N'0'), (N'119', NULL, N'1293472678392721408', N'', N'', N'0'), (N'120', NULL, N'1293472857510473728', N'', N'', N'0'), (N'121', NULL, N'1299273336009359360', N'', N'', N'0'), (N'122', NULL, N'1299273436282585088', N'', N'', N'0'), (N'123', NULL, N'1299273618470567936', N'', N'', N'0'), (N'124', NULL, N'1299273770182737920', N'', N'', N'0'), (N'125', NULL, N'1299273978023084032', N'', N'', N'0'), (N'126', NULL, N'1299274123225694208', N'', N'', N'0'), (N'127', NULL, N'1299274222299348992', N'', N'', N'0'), (N'128', NULL, N'1304238876758495232', N'', N'', N'0'), (N'129', NULL, N'1304678610343383040', N'', N'', N'0'), (N'130', NULL, N'1304679169305694208', N'', N'', N'0'), (N'131', NULL, N'1310460417141817344', N'', N'', N'0'), (N'132', NULL, N'1310502391475519488', N'', N'', N'0'), (N'133', NULL, N'1310515546943569920', N'', N'', N'0'), (N'134', NULL, N'1310515735292985344', N'', N'', N'0'), (N'135', NULL, N'1316628769783480320', N'', N'', N'0'), (N'136', NULL, N'1316628940663619584', N'', N'', N'0'), (N'137', NULL, N'1316629112428756992', N'', N'', N'0'), (N'138', NULL, N'1316652047017246720', N'', N'', N'0'), (N'139', NULL, N'1316913899996737536', N'', N'', N'0'), (N'140', NULL, N'1319200951383199744', N'', N'', N'0'), (N'141', NULL, N'1319221929807024128', N'', N'', N'0'), (N'142', NULL, N'1319554431134306304', N'', N'', N'0'), (N'143', NULL, N'1319554550458060800', N'', N'', N'0'), (N'144', NULL, N'1319554948434595840', N'', N'', N'0'), (N'145', NULL, N'1319555067183730688', N'', N'', N'0'), (N'146', NULL, N'1319555230765780992', N'', N'', N'0'), (N'147', NULL, N'1319555333790470144', N'', N'', N'0'), (N'148', NULL, N'1321001932510203904', N'', N'', N'0'), (N'149', NULL, N'1321002059803136000', N'', N'', N'0'), (N'150', NULL, N'1321002256440496128', N'', N'', N'0'), (N'151', NULL, N'1321002350686507008', N'', N'', N'0'), (N'152', NULL, N'1322190027988525056', N'', N'', N'0'), (N'153', NULL, N'1322452079688458240', N'', N'', N'0'), (N'154', NULL, N'1322452183929495552', N'', N'', N'0'), (N'155', NULL, N'1322452308651319296', N'', N'', N'0'), (N'156', NULL, N'1322452858176446464', N'', N'', N'0'), (N'157', NULL, N'1322452989235863552', N'', N'', N'0'), (N'158', NULL, N'1322453089655889920', N'', N'', N'0'), (N'159', NULL, N'1329706860249804800', N'', N'', N'0'), (N'160', NULL, N'1329707002411544576', N'', N'', N'0'), (N'161', NULL, N'1329708512277098496', N'', N'', N'0'), (N'162', NULL, N'1329708625917571072', N'', N'', N'0'), (N'163', NULL, N'1335049839287357440', N'', N'', N'0'), (N'164', NULL, N'1335050034221830144', N'', N'', N'0'), (N'165', NULL, N'1335050145899368448', N'', N'', N'0'), (N'166', NULL, N'1335050283434790912', N'', N'', N'0'), (N'167', NULL, N'1335050381770248192', N'', N'', N'0'), (N'168', NULL, N'1335050520941449216', N'', N'', N'0'), (N'169', NULL, N'1335050615829188608', N'', N'', N'0'), (N'170', NULL, N'1335111798720450560', N'', N'', N'0'), (N'171', NULL, N'1335118541370314752', N'', N'', N'0'), (N'172', NULL, N'1335118660417245184', N'', N'', N'0'), (N'173', NULL, N'1335118782727344128', N'', N'', N'0'), (N'174', NULL, N'1335118903200337920', N'', N'', N'0'), (N'175', NULL, N'1336230645078921216', N'', N'', N'0'), (N'176', NULL, N'1337314809113722880', N'', N'', N'0'), (N'177', NULL, N'1337314938973569024', N'', N'', N'0'), (N'178', NULL, N'1340961907637243904', N'', N'', N'0'), (N'179', NULL, N'1341652247554379776', N'', N'', N'0'), (N'180', NULL, N'1341652385555369984', N'', N'', N'0'), (N'181', NULL, N'1342457939827552256', N'', N'', N'0'), (N'182', NULL, N'1342458050112581632', N'', N'', N'0'), (N'183', NULL, N'1363382062055915520', N'', N'', N'0'), (N'184', NULL, N'1363382298501414912', N'', N'', N'0'), (N'185', NULL, N'1368854800347848704', N'', N'', N'0'), (N'186', NULL, N'1368855936576413696', N'', N'', N'0'), (N'187', NULL, N'1368856295889854464', N'', N'', N'0'), (N'188', NULL, N'1368856703572008960', N'', N'', N'0'), (N'189', NULL, N'1368856819242524672', N'', N'', N'0'), (N'190', NULL, N'1368856927887581184', N'', N'', N'0'), (N'191', NULL, N'1368857128383700992', N'', N'', N'0'), (N'192', NULL, N'1369560306297233408', N'', N'', N'0'), (N'193', NULL, N'1369560450472239104', N'', N'', N'0'), (N'194', NULL, N'1371705034307375104', N'', N'', N'0'), (N'195', NULL, N'1376442078396276736', N'', N'', N'0'), (N'196', NULL, N'1376442309850554368', N'', N'', N'0'), (N'197', NULL, N'1376442440536678400', N'', N'', N'0'), (N'198', NULL, N'1376442557943635968', N'', N'', N'0'), (N'199', NULL, N'1376442689674141696', N'', N'', N'0'), (N'200', NULL, N'1376442971032248320', N'', N'', N'0'), (N'201', NULL, N'1376443123021242368', N'', N'', N'0'), (N'202', NULL, N'1376443238851141632', N'', N'', N'0'), (N'203', NULL, N'1376443392249421824', N'', N'', N'0'), (N'204', NULL, N'1376443586777047040', N'', N'', N'0'), (N'205', NULL, N'1376467826087682048', N'', N'', N'0'), (N'206', NULL, N'1376467990894469120', N'', N'', N'0'), (N'207', NULL, N'1376468110214029312', N'', N'', N'0')
GO

COMMIT
GO


-- ----------------------------
-- Table structure for appapigatewaycacheoptions
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[apigateway].[appapigatewaycacheoptions]') AND type IN ('U'))
	DROP TABLE [apigateway].[appapigatewaycacheoptions]
GO

CREATE TABLE [apigateway].[appapigatewaycacheoptions] (
  [Id] int NOT NULL,
  [ReRouteId] bigint NOT NULL,
  [TtlSeconds] int NULL,
  [Region] nvarchar(256) NULL
)
GO


-- ----------------------------
-- Records of appapigatewaycacheoptions
-- ----------------------------
BEGIN TRANSACTION
GO

INSERT INTO [apigateway].[appapigatewaycacheoptions] VALUES (N'3', N'1261299170387169280', NULL, NULL), (N'4', N'1261585859064872960', NULL, NULL), (N'5', N'1261586605810368512', NULL, NULL), (N'6', N'1261587558609436672', NULL, NULL), (N'7', N'1261588213298348032', NULL, NULL), (N'8', N'1261588367619375104', NULL, NULL), (N'9', N'1261588628450557952', NULL, NULL), (N'10', N'1261588881564221440', NULL, NULL), (N'11', N'1261588983053795328', NULL, NULL), (N'12', N'1261589139039961088', NULL, NULL), (N'13', N'1261589197483393024', NULL, NULL), (N'14', N'1261589278857084928', NULL, NULL), (N'15', N'1261589420356124672', NULL, NULL), (N'16', N'1261589960393736192', NULL, NULL), (N'17', N'1261606600242085888', NULL, NULL), (N'18', N'1261606689601732608', NULL, NULL), (N'21', N'1262220447629058048', NULL, NULL), (N'22', N'1262230734939758592', NULL, NULL), (N'23', N'1262296916350869504', NULL, NULL), (N'24', N'1262632376348594176', NULL, NULL), (N'25', N'1262632791869902848', NULL, NULL), (N'28', N'1262660336921235456', N'0', N''), (N'29', N'1262660528277966848', N'0', N''), (N'30', N'1262660706875625472', N'0', N''), (N'31', N'1262660966393991168', N'0', N''), (N'32', N'1262661109474283520', N'0', N''), (N'33', N'1262663888804663296', N'0', N''), (N'34', N'1262664024096133120', N'0', N''), (N'35', N'1262664186252120064', N'0', N''), (N'36', N'1262664357044178944', N'0', N''), (N'37', N'1262664632928718848', N'0', N''), (N'38', N'1262664751409418240', N'0', N''), (N'39', N'1262664871274237952', N'0', N''), (N'40', N'1262665026111164416', N'0', N''), (N'41', N'1262665159905267712', N'0', N''), (N'42', N'1262665329829105664', N'0', N''), (N'43', N'1262665456471920640', N'0', N''), (N'44', N'1262665628165754880', NULL, NULL), (N'45', N'1262666172682883072', NULL, NULL), (N'47', N'1262723402331885568', NULL, NULL), (N'48', N'1262935771746734080', NULL, NULL), (N'49', N'1262935906522304512', NULL, NULL), (N'52', N'1263074419073593344', NULL, NULL), (N'53', N'1263075249394790400', N'0', N''), (N'54', N'1263075593499684864', N'0', N''), (N'56', N'1263101898440146944', NULL, NULL), (N'57', N'1263303878648569856', NULL, NULL), (N'58', N'1263304204797648896', NULL, NULL), (N'59', N'1263304872891555840', NULL, NULL), (N'60', N'1263305106250047488', NULL, NULL), (N'61', N'1263305244594970624', NULL, NULL), (N'62', N'1263305430536855552', NULL, NULL), (N'63', N'1263639172959174656', NULL, NULL), (N'64', N'1264799968944640000', NULL, NULL), (N'65', N'1264800070161584128', NULL, NULL), (N'66', N'1267360794414161920', NULL, NULL), (N'67', N'1267383367629807616', NULL, NULL), (N'68', N'1267817055527632896', NULL, NULL), (N'69', N'1267817221286526976', NULL, NULL), (N'70', N'1268893687085518848', NULL, NULL), (N'94', N'1288657613998579712', NULL, NULL), (N'95', N'1288657941770854400', N'0', N''), (N'96', N'1288658134067109888', N'0', N''), (N'97', N'1288658305156964352', N'0', N''), (N'98', N'1288658491216289792', N'0', N''), (N'99', N'1288658638302142464', NULL, NULL), (N'100', N'1288658791784308736', NULL, NULL), (N'101', N'1290849478956199936', NULL, NULL), (N'102', N'1290849628051124224', NULL, NULL), (N'103', N'1290849798553776128', NULL, NULL), (N'105', N'1291259822512693248', N'0', N''), (N'114', N'1293470838745821184', N'0', N''), (N'115', N'1293471661785706496', N'0', N''), (N'116', N'1293472678392721408', N'0', N''), (N'117', N'1293472857510473728', N'0', N''), (N'118', N'1299273336009359360', NULL, NULL), (N'119', N'1299273436282585088', NULL, NULL), (N'120', N'1299273618470567936', NULL, NULL), (N'121', N'1299273770182737920', NULL, NULL), (N'122', N'1299273978023084032', NULL, NULL), (N'123', N'1299274123225694208', NULL, NULL), (N'124', N'1299274222299348992', NULL, NULL), (N'125', N'1304238876758495232', N'0', N''), (N'126', N'1304678610343383040', NULL, NULL), (N'127', N'1304679169305694208', NULL, NULL), (N'128', N'1310460417141817344', N'0', N''), (N'129', N'1310502391475519488', N'0', N''), (N'130', N'1310515546943569920', NULL, NULL), (N'131', N'1310515735292985344', NULL, NULL), (N'132', N'1316628769783480320', N'0', N''), (N'133', N'1316628940663619584', N'0', N''), (N'134', N'1316629112428756992', N'0', N''), (N'135', N'1316652047017246720', NULL, NULL), (N'136', N'1316913899996737536', N'0', N''), (N'137', N'1319200951383199744', N'0', N''), (N'138', N'1319221929807024128', N'0', N''), (N'139', N'1319554431134306304', NULL, NULL), (N'140', N'1319554550458060800', NULL, NULL), (N'141', N'1319554948434595840', N'0', N''), (N'142', N'1319555067183730688', N'0', N''), (N'143', N'1319555230765780992', N'0', N''), (N'144', N'1319555333790470144', N'0', N''), (N'145', N'1321001932510203904', N'0', N''), (N'146', N'1321002059803136000', N'0', N''), (N'147', N'1321002256440496128', N'0', N''), (N'148', N'1321002350686507008', N'0', N''), (N'149', N'1322190027988525056', N'0', N''), (N'150', N'1322452079688458240', NULL, NULL), (N'151', N'1322452183929495552', NULL, NULL), (N'152', N'1322452308651319296', NULL, NULL), (N'153', N'1322452858176446464', N'0', N''), (N'154', N'1322452989235863552', N'0', N''), (N'155', N'1322453089655889920', NULL, NULL), (N'156', N'1329706860249804800', N'0', N''), (N'157', N'1329707002411544576', N'0', N''), (N'158', N'1329708512277098496', N'0', N''), (N'159', N'1329708625917571072', N'0', N''), (N'160', N'1335049839287357440', NULL, NULL), (N'161', N'1335050034221830144', N'0', N''), (N'162', N'1335050145899368448', N'0', N''), (N'163', N'1335050283434790912', N'0', N''), (N'164', N'1335050381770248192', N'0', N''), (N'165', N'1335050520941449216', N'0', N''), (N'166', N'1335050615829188608', N'0', N''), (N'167', N'1335111798720450560', N'0', N''), (N'168', N'1335118541370314752', N'0', N''), (N'169', N'1335118660417245184', N'0', N''), (N'170', N'1335118782727344128', N'0', N''), (N'171', N'1335118903200337920', N'0', N''), (N'172', N'1336230645078921216', N'0', N''), (N'173', N'1337314809113722880', NULL, NULL), (N'174', N'1337314938973569024', N'0', N''), (N'175', N'1340961907637243904', N'0', N''), (N'176', N'1341652247554379776', N'0', N''), (N'177', N'1341652385555369984', N'0', N''), (N'178', N'1342457939827552256', N'0', N''), (N'179', N'1342458050112581632', N'0', N''), (N'180', N'1363382062055915520', NULL, NULL), (N'181', N'1363382298501414912', NULL, NULL), (N'182', N'1368854800347848704', NULL, NULL), (N'183', N'1368855936576413696', NULL, NULL), (N'184', N'1368856295889854464', NULL, NULL), (N'185', N'1368856703572008960', NULL, NULL), (N'186', N'1368856819242524672', NULL, NULL), (N'187', N'1368856927887581184', NULL, NULL), (N'188', N'1368857128383700992', N'0', N''), (N'189', N'1369560306297233408', N'0', N''), (N'190', N'1369560450472239104', N'0', N''), (N'191', N'1371705034307375104', N'0', N''), (N'192', N'1376442078396276736', NULL, NULL), (N'193', N'1376442309850554368', N'0', N''), (N'194', N'1376442440536678400', N'0', N''), (N'195', N'1376442557943635968', N'0', N''), (N'196', N'1376442689674141696', N'0', N''), (N'197', N'1376442971032248320', N'0', N''), (N'198', N'1376443123021242368', N'0', N''), (N'199', N'1376443238851141632', N'0', N''), (N'200', N'1376443392249421824', N'0', N''), (N'201', N'1376443586777047040', N'0', N''), (N'202', N'1376467826087682048', N'0', N''), (N'203', N'1376467990894469120', N'0', N''), (N'204', N'1376468110214029312', N'0', N'')
GO

COMMIT
GO


-- ----------------------------
-- Table structure for appapigatewaydiscovery
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[apigateway].[appapigatewaydiscovery]') AND type IN ('U'))
	DROP TABLE [apigateway].[appapigatewaydiscovery]
GO

CREATE TABLE [apigateway].[appapigatewaydiscovery] (
  [Id] int NOT NULL,
  [ItemId] bigint NOT NULL,
  [Host] nvarchar(50) NULL,
  [Port] int NULL,
  [Type] nvarchar(128) NULL,
  [Token] nvarchar(256) NULL,
  [ConfigurationKey] nvarchar(256) NULL,
  [PollingInterval] int NULL,
  [Namespace] nvarchar(128) NULL,
  [Scheme] nvarchar(50) NULL
)
GO


-- ----------------------------
-- Records of appapigatewaydiscovery
-- ----------------------------
BEGIN TRANSACTION
GO

INSERT INTO [apigateway].[appapigatewaydiscovery] VALUES (N'1', N'1260841964962947072', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO

COMMIT
GO


-- ----------------------------
-- Table structure for appapigatewaydynamicreroute
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[apigateway].[appapigatewaydynamicreroute]') AND type IN ('U'))
	DROP TABLE [apigateway].[appapigatewaydynamicreroute]
GO

CREATE TABLE [apigateway].[appapigatewaydynamicreroute] (
  [Id] int NOT NULL,
  [ExtraProperties] nvarchar(max) NULL,
  [ConcurrencyStamp] nvarchar(40) NULL,
  [DynamicReRouteId] bigint NOT NULL,
  [ServiceName] nvarchar(100) NOT NULL,
  [DownstreamHttpVersion] nvarchar(30) NULL,
  [AppId] nvarchar(50) NOT NULL
)
GO


-- ----------------------------
-- Table structure for appapigatewayglobalconfiguration
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[apigateway].[appapigatewayglobalconfiguration]') AND type IN ('U'))
	DROP TABLE [apigateway].[appapigatewayglobalconfiguration]
GO

CREATE TABLE [apigateway].[appapigatewayglobalconfiguration] (
  [Id] int NOT NULL,
  [ExtraProperties] nvarchar(max) NULL,
  [ConcurrencyStamp] nvarchar(40) NULL,
  [ItemId] bigint NOT NULL,
  [RequestIdKey] nvarchar(100) NULL,
  [BaseUrl] nvarchar(256) NOT NULL,
  [DownstreamScheme] nvarchar(100) NULL,
  [DownstreamHttpVersion] nvarchar(30) NULL,
  [IsDeleted] tinyint NOT NULL,
  [IsActive] tinyint NOT NULL,
  [AppId] nvarchar(50) NOT NULL
)
GO


-- ----------------------------
-- Records of appapigatewayglobalconfiguration
-- ----------------------------
BEGIN TRANSACTION
GO

INSERT INTO [apigateway].[appapigatewayglobalconfiguration] VALUES (N'1', N'{}', N'f7973118f2c2425c8cc96b59883b99aa', N'1260841964962947072', NULL, N'http://localhost:30000', N'HTTP', NULL, N'0', N'1', N'TEST-APP')
GO

COMMIT
GO


-- ----------------------------
-- Table structure for appapigatewayheaders
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[apigateway].[appapigatewayheaders]') AND type IN ('U'))
	DROP TABLE [apigateway].[appapigatewayheaders]
GO

CREATE TABLE [apigateway].[appapigatewayheaders] (
  [Id] int NOT NULL,
  [ReRouteId] bigint NOT NULL,
  [Key] nvarchar(50) NULL,
  [Value] nvarchar(256) NULL
)
GO


-- ----------------------------
-- Table structure for appapigatewayhostandport
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[apigateway].[appapigatewayhostandport]') AND type IN ('U'))
	DROP TABLE [apigateway].[appapigatewayhostandport]
GO

CREATE TABLE [apigateway].[appapigatewayhostandport] (
  [Id] int NOT NULL,
  [ReRouteId] bigint NOT NULL,
  [Host] nvarchar(50) NOT NULL,
  [Port] int NULL
)
GO


-- ----------------------------
-- Table structure for appapigatewayhttpoptions
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[apigateway].[appapigatewayhttpoptions]') AND type IN ('U'))
	DROP TABLE [apigateway].[appapigatewayhttpoptions]
GO

CREATE TABLE [apigateway].[appapigatewayhttpoptions] (
  [Id] int NOT NULL,
  [ItemId] bigint NULL,
  [ReRouteId] bigint NULL,
  [MaxConnectionsPerServer] int NULL,
  [AllowAutoRedirect] tinyint NOT NULL,
  [UseCookieContainer] tinyint NOT NULL,
  [UseTracing] tinyint NOT NULL,
  [UseProxy] tinyint NOT NULL
)
GO


-- ----------------------------
-- Records of appapigatewayhttpoptions
-- ----------------------------
BEGIN TRANSACTION
GO

INSERT INTO [apigateway].[appapigatewayhttpoptions] VALUES (N'1', N'1260841964962947072', NULL, NULL, N'0', N'0', N'1', N'0'), (N'4', NULL, N'1261299170387169280', N'1000', N'1', N'0', N'1', N'0'), (N'5', NULL, N'1261585859064872960', NULL, N'0', N'0', N'1', N'0'), (N'6', NULL, N'1261586605810368512', NULL, N'0', N'0', N'0', N'0'), (N'7', NULL, N'1261587558609436672', NULL, N'0', N'0', N'0', N'0'), (N'8', NULL, N'1261588213298348032', NULL, N'0', N'0', N'0', N'0'), (N'9', NULL, N'1261588367619375104', NULL, N'0', N'0', N'0', N'0'), (N'10', NULL, N'1261588628450557952', NULL, N'0', N'0', N'0', N'0'), (N'11', NULL, N'1261588881564221440', NULL, N'0', N'0', N'0', N'0'), (N'12', NULL, N'1261588983053795328', NULL, N'0', N'0', N'0', N'0'), (N'13', NULL, N'1261589139039961088', NULL, N'0', N'0', N'0', N'0'), (N'14', NULL, N'1261589197483393024', NULL, N'0', N'0', N'0', N'0'), (N'15', NULL, N'1261589278857084928', NULL, N'0', N'0', N'0', N'0'), (N'16', NULL, N'1261589420356124672', NULL, N'0', N'0', N'0', N'0'), (N'17', NULL, N'1261589960393736192', N'1000', N'1', N'0', N'1', N'0'), (N'18', NULL, N'1261606600242085888', NULL, N'0', N'0', N'0', N'0'), (N'19', NULL, N'1261606689601732608', NULL, N'0', N'0', N'0', N'0'), (N'22', NULL, N'1262220447629058048', NULL, N'0', N'0', N'1', N'0'), (N'23', NULL, N'1262230734939758592', NULL, N'0', N'0', N'1', N'0'), (N'24', NULL, N'1262296916350869504', NULL, N'0', N'0', N'1', N'0'), (N'25', NULL, N'1262632376348594176', N'0', N'0', N'0', N'0', N'0'), (N'26', NULL, N'1262632791869902848', N'0', N'0', N'0', N'0', N'0'), (N'29', NULL, N'1262660336921235456', N'0', N'0', N'0', N'0', N'0'), (N'30', NULL, N'1262660528277966848', N'0', N'0', N'0', N'0', N'0'), (N'31', NULL, N'1262660706875625472', N'0', N'0', N'0', N'0', N'0'), (N'32', NULL, N'1262660966393991168', N'0', N'0', N'0', N'0', N'0'), (N'33', NULL, N'1262661109474283520', N'0', N'0', N'0', N'0', N'0'), (N'34', NULL, N'1262663888804663296', N'0', N'0', N'0', N'0', N'0'), (N'35', NULL, N'1262664024096133120', N'0', N'0', N'0', N'0', N'0'), (N'36', NULL, N'1262664186252120064', N'0', N'0', N'0', N'0', N'0'), (N'37', NULL, N'1262664357044178944', N'0', N'0', N'0', N'0', N'0'), (N'38', NULL, N'1262664632928718848', N'0', N'0', N'0', N'0', N'0'), (N'39', NULL, N'1262664751409418240', N'0', N'0', N'0', N'0', N'0'), (N'40', NULL, N'1262664871274237952', N'0', N'0', N'0', N'0', N'0'), (N'41', NULL, N'1262665026111164416', N'0', N'0', N'0', N'0', N'0'), (N'42', NULL, N'1262665159905267712', N'0', N'0', N'0', N'0', N'0'), (N'43', NULL, N'1262665329829105664', N'0', N'0', N'0', N'0', N'0'), (N'44', NULL, N'1262665456471920640', N'0', N'0', N'0', N'0', N'0'), (N'45', NULL, N'1262665628165754880', N'0', N'0', N'0', N'0', N'0'), (N'46', NULL, N'1262666172682883072', N'0', N'0', N'0', N'0', N'0'), (N'48', NULL, N'1262723402331885568', N'0', N'0', N'0', N'0', N'0'), (N'49', NULL, N'1262935771746734080', N'0', N'0', N'0', N'0', N'0'), (N'50', NULL, N'1262935906522304512', N'0', N'0', N'0', N'0', N'0'), (N'53', NULL, N'1263074419073593344', N'0', N'0', N'0', N'0', N'0'), (N'54', NULL, N'1263075249394790400', N'0', N'0', N'0', N'0', N'0'), (N'55', NULL, N'1263075593499684864', N'0', N'0', N'0', N'0', N'0'), (N'57', NULL, N'1263101898440146944', N'0', N'0', N'0', N'0', N'0'), (N'58', NULL, N'1263303878648569856', N'0', N'0', N'0', N'0', N'0'), (N'59', NULL, N'1263304204797648896', N'0', N'0', N'0', N'0', N'0'), (N'60', NULL, N'1263304872891555840', N'0', N'0', N'0', N'0', N'0'), (N'61', NULL, N'1263305106250047488', N'0', N'0', N'0', N'0', N'0'), (N'62', NULL, N'1263305244594970624', N'0', N'0', N'0', N'0', N'0'), (N'63', NULL, N'1263305430536855552', N'0', N'0', N'0', N'0', N'0'), (N'64', NULL, N'1263639172959174656', N'0', N'0', N'0', N'0', N'0'), (N'65', NULL, N'1264799968944640000', N'0', N'0', N'0', N'0', N'0'), (N'66', NULL, N'1264800070161584128', N'0', N'0', N'0', N'0', N'0'), (N'68', NULL, N'1267360794414161920', N'0', N'0', N'0', N'0', N'0'), (N'69', NULL, N'1267383367629807616', N'0', N'0', N'0', N'0', N'0'), (N'70', NULL, N'1267817055527632896', N'0', N'0', N'0', N'0', N'0'), (N'71', NULL, N'1267817221286526976', N'0', N'0', N'0', N'0', N'0'), (N'72', NULL, N'1268893687085518848', N'0', N'0', N'0', N'0', N'0'), (N'97', NULL, N'1288657613998579712', N'1000', N'0', N'0', N'0', N'0'), (N'98', NULL, N'1288657941770854400', N'1000', N'0', N'0', N'0', N'0'), (N'99', NULL, N'1288658134067109888', N'1000', N'0', N'0', N'0', N'0'), (N'100', NULL, N'1288658305156964352', N'1000', N'0', N'0', N'0', N'0'), (N'101', NULL, N'1288658491216289792', N'1000', N'0', N'0', N'0', N'0'), (N'102', NULL, N'1288658638302142464', N'1000', N'0', N'0', N'0', N'0'), (N'103', NULL, N'1288658791784308736', N'1000', N'0', N'0', N'0', N'0'), (N'104', NULL, N'1290849478956199936', N'0', N'0', N'0', N'0', N'0'), (N'105', NULL, N'1290849628051124224', N'0', N'0', N'0', N'0', N'0'), (N'106', NULL, N'1290849798553776128', N'0', N'0', N'0', N'0', N'0'), (N'108', NULL, N'1291259822512693248', N'0', N'0', N'0', N'0', N'0'), (N'117', NULL, N'1293470838745821184', N'0', N'0', N'0', N'0', N'0'), (N'118', NULL, N'1293471661785706496', N'0', N'0', N'0', N'0', N'0'), (N'119', NULL, N'1293472678392721408', N'0', N'0', N'0', N'0', N'0'), (N'120', NULL, N'1293472857510473728', N'0', N'0', N'0', N'0', N'0'), (N'121', NULL, N'1299273336009359360', N'0', N'0', N'0', N'0', N'0'), (N'122', NULL, N'1299273436282585088', N'0', N'0', N'0', N'0', N'0'), (N'123', NULL, N'1299273618470567936', N'0', N'0', N'0', N'0', N'0'), (N'124', NULL, N'1299273770182737920', N'0', N'0', N'0', N'0', N'0'), (N'125', NULL, N'1299273978023084032', N'0', N'0', N'0', N'0', N'0'), (N'126', NULL, N'1299274123225694208', N'0', N'0', N'0', N'0', N'0'), (N'127', NULL, N'1299274222299348992', N'0', N'0', N'0', N'0', N'0'), (N'128', NULL, N'1304238876758495232', N'0', N'0', N'0', N'0', N'0'), (N'129', NULL, N'1304678610343383040', N'0', N'0', N'0', N'0', N'0'), (N'130', NULL, N'1304679169305694208', N'0', N'0', N'0', N'0', N'0'), (N'131', NULL, N'1310460417141817344', N'0', N'0', N'0', N'0', N'0'), (N'132', NULL, N'1310502391475519488', N'0', N'0', N'0', N'0', N'0'), (N'133', NULL, N'1310515546943569920', N'0', N'0', N'0', N'0', N'0'), (N'134', NULL, N'1310515735292985344', N'0', N'0', N'0', N'0', N'0'), (N'135', NULL, N'1316628769783480320', N'0', N'0', N'0', N'0', N'0'), (N'136', NULL, N'1316628940663619584', N'0', N'0', N'0', N'0', N'0'), (N'137', NULL, N'1316629112428756992', N'0', N'0', N'0', N'0', N'0'), (N'138', NULL, N'1316652047017246720', N'0', N'0', N'0', N'0', N'0'), (N'139', NULL, N'1316913899996737536', N'0', N'0', N'0', N'0', N'0'), (N'140', NULL, N'1319200951383199744', N'0', N'0', N'0', N'0', N'0'), (N'141', NULL, N'1319221929807024128', N'0', N'0', N'0', N'0', N'0'), (N'142', NULL, N'1319554431134306304', N'0', N'0', N'0', N'0', N'0'), (N'143', NULL, N'1319554550458060800', N'0', N'0', N'0', N'0', N'0'), (N'144', NULL, N'1319554948434595840', N'0', N'0', N'0', N'0', N'0'), (N'145', NULL, N'1319555067183730688', N'0', N'0', N'0', N'0', N'0'), (N'146', NULL, N'1319555230765780992', N'0', N'0', N'0', N'0', N'0'), (N'147', NULL, N'1319555333790470144', N'0', N'0', N'0', N'0', N'0'), (N'148', NULL, N'1321001932510203904', N'0', N'0', N'0', N'0', N'0'), (N'149', NULL, N'1321002059803136000', N'0', N'0', N'0', N'0', N'0'), (N'150', NULL, N'1321002256440496128', N'0', N'0', N'0', N'0', N'0'), (N'151', NULL, N'1321002350686507008', N'0', N'0', N'0', N'0', N'0'), (N'152', NULL, N'1322190027988525056', N'0', N'0', N'0', N'0', N'0'), (N'153', NULL, N'1322452079688458240', N'0', N'0', N'0', N'0', N'0'), (N'154', NULL, N'1322452183929495552', N'0', N'0', N'0', N'0', N'0'), (N'155', NULL, N'1322452308651319296', N'0', N'0', N'0', N'0', N'0'), (N'156', NULL, N'1322452858176446464', N'0', N'0', N'0', N'0', N'0'), (N'157', NULL, N'1322452989235863552', N'0', N'0', N'0', N'0', N'0'), (N'158', NULL, N'1322453089655889920', N'0', N'0', N'0', N'0', N'0'), (N'159', NULL, N'1329706860249804800', N'0', N'0', N'0', N'0', N'0'), (N'160', NULL, N'1329707002411544576', N'0', N'0', N'0', N'0', N'0'), (N'161', NULL, N'1329708512277098496', N'0', N'0', N'0', N'0', N'0'), (N'162', NULL, N'1329708625917571072', N'0', N'0', N'0', N'0', N'0'), (N'163', NULL, N'1335049839287357440', N'0', N'0', N'0', N'0', N'0'), (N'164', NULL, N'1335050034221830144', N'0', N'0', N'0', N'0', N'0'), (N'165', NULL, N'1335050145899368448', N'0', N'0', N'0', N'0', N'0'), (N'166', NULL, N'1335050283434790912', N'0', N'0', N'0', N'0', N'0'), (N'167', NULL, N'1335050381770248192', N'0', N'0', N'0', N'0', N'0'), (N'168', NULL, N'1335050520941449216', N'0', N'0', N'0', N'0', N'0'), (N'169', NULL, N'1335050615829188608', N'0', N'0', N'0', N'0', N'0'), (N'170', NULL, N'1335111798720450560', N'0', N'0', N'0', N'0', N'0'), (N'171', NULL, N'1335118541370314752', N'0', N'0', N'0', N'0', N'0'), (N'172', NULL, N'1335118660417245184', N'0', N'0', N'0', N'0', N'0'), (N'173', NULL, N'1335118782727344128', N'0', N'0', N'0', N'0', N'0'), (N'174', NULL, N'1335118903200337920', N'0', N'0', N'0', N'0', N'0'), (N'175', NULL, N'1336230645078921216', N'0', N'0', N'0', N'0', N'0'), (N'176', NULL, N'1337314809113722880', N'0', N'0', N'0', N'0', N'0'), (N'177', NULL, N'1337314938973569024', N'0', N'0', N'0', N'0', N'0'), (N'178', NULL, N'1340961907637243904', N'0', N'0', N'0', N'0', N'0'), (N'179', NULL, N'1341652247554379776', N'0', N'0', N'0', N'0', N'0'), (N'180', NULL, N'1341652385555369984', N'0', N'0', N'0', N'0', N'0'), (N'181', NULL, N'1342457939827552256', N'0', N'0', N'0', N'0', N'0'), (N'182', NULL, N'1342458050112581632', N'0', N'0', N'0', N'0', N'0'), (N'183', NULL, N'1363382062055915520', N'0', N'0', N'0', N'0', N'0'), (N'184', NULL, N'1363382298501414912', N'0', N'0', N'0', N'0', N'0'), (N'185', NULL, N'1368854800347848704', N'0', N'0', N'0', N'0', N'0'), (N'186', NULL, N'1368855936576413696', N'0', N'0', N'0', N'0', N'0'), (N'187', NULL, N'1368856295889854464', N'0', N'0', N'0', N'0', N'0'), (N'188', NULL, N'1368856703572008960', N'0', N'0', N'0', N'0', N'0'), (N'189', NULL, N'1368856819242524672', N'0', N'0', N'0', N'0', N'0'), (N'190', NULL, N'1368856927887581184', N'0', N'0', N'0', N'0', N'0'), (N'191', NULL, N'1368857128383700992', N'0', N'0', N'0', N'0', N'0'), (N'192', NULL, N'1369560306297233408', N'0', N'0', N'0', N'0', N'0'), (N'193', NULL, N'1369560450472239104', N'0', N'0', N'0', N'0', N'0'), (N'194', NULL, N'1371705034307375104', N'0', N'0', N'0', N'0', N'0'), (N'195', NULL, N'1376442078396276736', N'0', N'0', N'0', N'0', N'0'), (N'196', NULL, N'1376442309850554368', N'0', N'0', N'0', N'0', N'0'), (N'197', NULL, N'1376442440536678400', N'0', N'0', N'0', N'0', N'0'), (N'198', NULL, N'1376442557943635968', N'0', N'0', N'0', N'0', N'0'), (N'199', NULL, N'1376442689674141696', N'0', N'0', N'0', N'0', N'0'), (N'200', NULL, N'1376442971032248320', N'0', N'0', N'0', N'0', N'0'), (N'201', NULL, N'1376443123021242368', N'0', N'0', N'0', N'0', N'0'), (N'202', NULL, N'1376443238851141632', N'0', N'0', N'0', N'0', N'0'), (N'203', NULL, N'1376443392249421824', N'0', N'0', N'0', N'0', N'0'), (N'204', NULL, N'1376443586777047040', N'0', N'0', N'0', N'0', N'0'), (N'205', NULL, N'1376467826087682048', N'0', N'0', N'0', N'0', N'0'), (N'206', NULL, N'1376467990894469120', N'0', N'0', N'0', N'0', N'0'), (N'207', NULL, N'1376468110214029312', N'0', N'0', N'0', N'0', N'0')
GO

COMMIT
GO


-- ----------------------------
-- Table structure for appapigatewayqosoptions
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[apigateway].[appapigatewayqosoptions]') AND type IN ('U'))
	DROP TABLE [apigateway].[appapigatewayqosoptions]
GO

CREATE TABLE [apigateway].[appapigatewayqosoptions] (
  [Id] int NOT NULL,
  [ItemId] bigint NULL,
  [ReRouteId] bigint NULL,
  [ExceptionsAllowedBeforeBreaking] int NULL,
  [DurationOfBreak] int NULL,
  [TimeoutValue] int NULL
)
GO


-- ----------------------------
-- Records of appapigatewayqosoptions
-- ----------------------------
BEGIN TRANSACTION
GO

INSERT INTO [apigateway].[appapigatewayqosoptions] VALUES (N'1', N'1260841964962947072', NULL, N'60', N'60000', N'30000'), (N'4', NULL, N'1261299170387169280', N'60', N'60000', N'30000'), (N'5', NULL, N'1261585859064872960', N'60', N'60000', N'30000'), (N'6', NULL, N'1261586605810368512', N'60', N'60000', N'30000'), (N'7', NULL, N'1261587558609436672', N'60', N'60000', N'30000'), (N'8', NULL, N'1261588213298348032', N'60', N'60000', N'30000'), (N'9', NULL, N'1261588367619375104', N'60', N'60000', N'30000'), (N'10', NULL, N'1261588628450557952', N'60', N'60000', N'30000'), (N'11', NULL, N'1261588881564221440', N'60', N'60000', N'30000'), (N'12', NULL, N'1261588983053795328', N'60', N'60000', N'30000'), (N'13', NULL, N'1261589139039961088', N'60', N'60000', N'30000'), (N'14', NULL, N'1261589197483393024', N'60', N'60000', N'30000'), (N'15', NULL, N'1261589278857084928', N'60', N'60000', N'30000'), (N'16', NULL, N'1261589420356124672', N'60', N'60000', N'30000'), (N'17', NULL, N'1261589960393736192', N'60', N'60000', N'30000'), (N'18', NULL, N'1261606600242085888', NULL, NULL, NULL), (N'19', NULL, N'1261606689601732608', NULL, NULL, NULL), (N'22', NULL, N'1262220447629058048', N'60', N'60000', N'30000'), (N'23', NULL, N'1262230734939758592', N'60', N'60000', N'30000'), (N'24', NULL, N'1262296916350869504', N'60', N'60000', N'30000'), (N'25', NULL, N'1262632376348594176', N'50', N'60000', N'30000'), (N'26', NULL, N'1262632791869902848', N'50', N'60000', N'30000'), (N'29', NULL, N'1262660336921235456', N'50', N'60000', N'30000'), (N'30', NULL, N'1262660528277966848', N'50', N'60000', N'30000'), (N'31', NULL, N'1262660706875625472', N'50', N'60000', N'30000'), (N'32', NULL, N'1262660966393991168', N'50', N'60000', N'30000'), (N'33', NULL, N'1262661109474283520', N'50', N'60000', N'30000'), (N'34', NULL, N'1262663888804663296', N'50', N'60000', N'30000'), (N'35', NULL, N'1262664024096133120', N'50', N'60000', N'30000'), (N'36', NULL, N'1262664186252120064', N'50', N'60000', N'30000'), (N'37', NULL, N'1262664357044178944', N'50', N'60000', N'30000'), (N'38', NULL, N'1262664632928718848', N'50', N'60000', N'30000'), (N'39', NULL, N'1262664751409418240', N'50', N'60000', N'30000'), (N'40', NULL, N'1262664871274237952', N'50', N'60000', N'30000'), (N'41', NULL, N'1262665026111164416', N'50', N'60000', N'30000'), (N'42', NULL, N'1262665159905267712', N'50', N'60000', N'30000'), (N'43', NULL, N'1262665329829105664', N'50', N'60000', N'30000'), (N'44', NULL, N'1262665456471920640', N'50', N'60000', N'30000'), (N'45', NULL, N'1262665628165754880', N'50', N'60000', N'30000'), (N'46', NULL, N'1262666172682883072', N'50', N'60000', N'30000'), (N'48', NULL, N'1262723402331885568', N'50', N'60000', N'30000'), (N'49', NULL, N'1262935771746734080', N'50', N'60000', N'30000'), (N'50', NULL, N'1262935906522304512', N'50', N'60000', N'30000'), (N'53', NULL, N'1263074419073593344', N'50', N'60000', N'30000'), (N'54', NULL, N'1263075249394790400', N'50', N'60000', N'30000'), (N'55', NULL, N'1263075593499684864', N'50', N'60000', N'30000'), (N'57', NULL, N'1263101898440146944', N'50', N'60000', N'120000'), (N'58', NULL, N'1263303878648569856', N'50', N'60000', N'30000'), (N'59', NULL, N'1263304204797648896', N'50', N'60000', N'120000'), (N'60', NULL, N'1263304872891555840', N'50', N'60000', N'30000'), (N'61', NULL, N'1263305106250047488', N'50', N'60000', N'30000'), (N'62', NULL, N'1263305244594970624', N'50', N'60000', N'30000'), (N'63', NULL, N'1263305430536855552', N'50', N'60000', N'30000'), (N'64', NULL, N'1263639172959174656', N'50', N'60000', N'30000'), (N'65', NULL, N'1264799968944640000', N'50', N'60000', N'30000'), (N'66', NULL, N'1264800070161584128', N'50', N'60000', N'30000'), (N'68', NULL, N'1267360794414161920', N'50', N'60000', N'30000'), (N'69', NULL, N'1267383367629807616', N'50', N'60000', N'30000'), (N'70', NULL, N'1267817055527632896', N'50', N'60000', N'30000'), (N'71', NULL, N'1267817221286526976', N'50', N'60000', N'30000'), (N'72', NULL, N'1268893687085518848', N'50', N'60000', N'30000'), (N'97', NULL, N'1288657613998579712', N'50', N'60000', N'30000'), (N'98', NULL, N'1288657941770854400', N'50', N'60000', N'30000'), (N'99', NULL, N'1288658134067109888', N'50', N'60000', N'30000'), (N'100', NULL, N'1288658305156964352', N'50', N'60000', N'30000'), (N'101', NULL, N'1288658491216289792', N'50', N'60000', N'30000'), (N'102', NULL, N'1288658638302142464', N'50', N'60000', N'30000'), (N'103', NULL, N'1288658791784308736', N'50', N'60000', N'30000'), (N'104', NULL, N'1290849478956199936', N'50', N'60000', N'30000'), (N'105', NULL, N'1290849628051124224', N'50', N'60000', N'30000'), (N'106', NULL, N'1290849798553776128', N'50', N'60000', N'30000'), (N'108', NULL, N'1291259822512693248', N'50', N'60000', N'30000'), (N'117', NULL, N'1293470838745821184', N'50', N'60000', N'30000'), (N'118', NULL, N'1293471661785706496', N'50', N'60000', N'30000'), (N'119', NULL, N'1293472678392721408', N'50', N'60000', N'30000'), (N'120', NULL, N'1293472857510473728', N'50', N'60000', N'30000'), (N'121', NULL, N'1299273336009359360', N'50', N'60000', N'30000'), (N'122', NULL, N'1299273436282585088', N'50', N'60000', N'30000'), (N'123', NULL, N'1299273618470567936', N'50', N'60000', N'30000'), (N'124', NULL, N'1299273770182737920', N'50', N'60000', N'30000'), (N'125', NULL, N'1299273978023084032', N'50', N'60000', N'30000'), (N'126', NULL, N'1299274123225694208', N'50', N'60000', N'30000'), (N'127', NULL, N'1299274222299348992', N'50', N'60000', N'30000'), (N'128', NULL, N'1304238876758495232', N'50', N'60000', N'30000'), (N'129', NULL, N'1304678610343383040', N'50', N'60000', N'30000'), (N'130', NULL, N'1304679169305694208', N'50', N'60000', N'30000'), (N'131', NULL, N'1310460417141817344', N'50', N'60000', N'30000'), (N'132', NULL, N'1310502391475519488', N'50', N'60000', N'30000'), (N'133', NULL, N'1310515546943569920', N'50', N'60000', N'30000'), (N'134', NULL, N'1310515735292985344', N'50', N'60000', N'30000'), (N'135', NULL, N'1316628769783480320', N'50', N'60000', N'30000'), (N'136', NULL, N'1316628940663619584', N'50', N'60000', N'30000'), (N'137', NULL, N'1316629112428756992', N'50', N'60000', N'30000'), (N'138', NULL, N'1316652047017246720', N'50', N'60000', N'30000'), (N'139', NULL, N'1316913899996737536', N'50', N'60000', N'30000'), (N'140', NULL, N'1319200951383199744', N'50', N'60000', N'30000'), (N'141', NULL, N'1319221929807024128', N'50', N'60000', N'30000'), (N'142', NULL, N'1319554431134306304', N'50', N'60000', N'30000'), (N'143', NULL, N'1319554550458060800', N'50', N'60000', N'30000'), (N'144', NULL, N'1319554948434595840', N'50', N'60000', N'30000'), (N'145', NULL, N'1319555067183730688', N'50', N'60000', N'30000'), (N'146', NULL, N'1319555230765780992', N'50', N'60000', N'30000'), (N'147', NULL, N'1319555333790470144', N'50', N'60000', N'30000'), (N'148', NULL, N'1321001932510203904', N'50', N'60000', N'30000'), (N'149', NULL, N'1321002059803136000', N'50', N'60000', N'30000'), (N'150', NULL, N'1321002256440496128', N'50', N'60000', N'30000'), (N'151', NULL, N'1321002350686507008', N'50', N'60000', N'30000'), (N'152', NULL, N'1322190027988525056', N'50', N'60000', N'30000'), (N'153', NULL, N'1322452079688458240', N'50', N'60000', N'30000'), (N'154', NULL, N'1322452183929495552', N'50', N'60000', N'30000'), (N'155', NULL, N'1322452308651319296', N'50', N'60000', N'30000'), (N'156', NULL, N'1322452858176446464', N'50', N'60000', N'30000'), (N'157', NULL, N'1322452989235863552', N'50', N'60000', N'30000'), (N'158', NULL, N'1322453089655889920', N'50', N'60000', N'30000'), (N'159', NULL, N'1329706860249804800', N'50', N'60000', N'30000'), (N'160', NULL, N'1329707002411544576', N'50', N'60000', N'30000'), (N'161', NULL, N'1329708512277098496', N'50', N'60000', N'30000'), (N'162', NULL, N'1329708625917571072', N'50', N'60000', N'30000'), (N'163', NULL, N'1335049839287357440', N'50', N'60000', N'30000'), (N'164', NULL, N'1335050034221830144', N'50', N'60000', N'30000'), (N'165', NULL, N'1335050145899368448', N'50', N'60000', N'30000'), (N'166', NULL, N'1335050283434790912', N'50', N'60000', N'30000'), (N'167', NULL, N'1335050381770248192', N'50', N'60000', N'30000'), (N'168', NULL, N'1335050520941449216', N'50', N'60000', N'30000'), (N'169', NULL, N'1335050615829188608', N'50', N'60000', N'30000'), (N'170', NULL, N'1335111798720450560', N'50', N'60000', N'30000'), (N'171', NULL, N'1335118541370314752', N'50', N'60000', N'30000'), (N'172', NULL, N'1335118660417245184', N'50', N'60000', N'30000'), (N'173', NULL, N'1335118782727344128', N'50', N'60000', N'30000'), (N'174', NULL, N'1335118903200337920', N'50', N'60000', N'30000'), (N'175', NULL, N'1336230645078921216', N'50', N'60000', N'30000'), (N'176', NULL, N'1337314809113722880', N'50', N'60000', N'30000'), (N'177', NULL, N'1337314938973569024', N'50', N'60000', N'30000'), (N'178', NULL, N'1340961907637243904', N'50', N'60000', N'30000'), (N'179', NULL, N'1341652247554379776', N'50', N'60000', N'30000'), (N'180', NULL, N'1341652385555369984', N'50', N'60000', N'30000'), (N'181', NULL, N'1342457939827552256', N'50', N'60000', N'30000'), (N'182', NULL, N'1342458050112581632', N'50', N'60000', N'30000'), (N'183', NULL, N'1363382062055915520', N'50', N'60000', N'30000'), (N'184', NULL, N'1363382298501414912', N'50', N'60000', N'30000'), (N'185', NULL, N'1368854800347848704', N'50', N'60000', N'30000'), (N'186', NULL, N'1368855936576413696', N'50', N'60000', N'30000'), (N'187', NULL, N'1368856295889854464', N'50', N'60000', N'30000'), (N'188', NULL, N'1368856703572008960', N'50', N'60000', N'30000'), (N'189', NULL, N'1368856819242524672', N'50', N'60000', N'30000'), (N'190', NULL, N'1368856927887581184', N'50', N'60000', N'30000'), (N'191', NULL, N'1368857128383700992', N'50', N'60000', N'30000'), (N'192', NULL, N'1369560306297233408', N'50', N'60000', N'30000'), (N'193', NULL, N'1369560450472239104', N'50', N'60000', N'30000'), (N'194', NULL, N'1371705034307375104', N'50', N'60000', N'30000'), (N'195', NULL, N'1376442078396276736', N'50', N'60000', N'30000'), (N'196', NULL, N'1376442309850554368', N'50', N'60000', N'30000'), (N'197', NULL, N'1376442440536678400', N'50', N'60000', N'30000'), (N'198', NULL, N'1376442557943635968', N'50', N'60000', N'30000'), (N'199', NULL, N'1376442689674141696', N'50', N'60000', N'30000'), (N'200', NULL, N'1376442971032248320', N'50', N'60000', N'30000'), (N'201', NULL, N'1376443123021242368', N'50', N'60000', N'30000'), (N'202', NULL, N'1376443238851141632', N'50', N'60000', N'30000'), (N'203', NULL, N'1376443392249421824', N'50', N'60000', N'30000'), (N'204', NULL, N'1376443586777047040', N'50', N'60000', N'30000'), (N'205', NULL, N'1376467826087682048', N'50', N'60000', N'30000'), (N'206', NULL, N'1376467990894469120', N'50', N'60000', N'30000'), (N'207', NULL, N'1376468110214029312', N'50', N'60000', N'30000')
GO

COMMIT
GO


-- ----------------------------
-- Table structure for appapigatewayratelimitoptions
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[apigateway].[appapigatewayratelimitoptions]') AND type IN ('U'))
	DROP TABLE [apigateway].[appapigatewayratelimitoptions]
GO

CREATE TABLE [apigateway].[appapigatewayratelimitoptions] (
  [Id] int NOT NULL,
  [ItemId] bigint NOT NULL,
  [ClientIdHeader] nvarchar(50) NULL,
  [QuotaExceededMessage] nvarchar(256) NULL,
  [RateLimitCounterPrefix] nvarchar(50) NULL,
  [DisableRateLimitHeaders] tinyint NOT NULL,
  [HttpStatusCode] int NULL
)
GO


-- ----------------------------
-- Records of appapigatewayratelimitoptions
-- ----------------------------
BEGIN TRANSACTION
GO

INSERT INTO [apigateway].[appapigatewayratelimitoptions] VALUES (N'1', N'1260841964962947072', N'ClientId', N'您的操作过快,请稍后再试!', N'ocelot', N'1', N'429')
GO

COMMIT
GO


-- ----------------------------
-- Table structure for appapigatewayratelimitrule
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[apigateway].[appapigatewayratelimitrule]') AND type IN ('U'))
	DROP TABLE [apigateway].[appapigatewayratelimitrule]
GO

CREATE TABLE [apigateway].[appapigatewayratelimitrule] (
  [Id] int NOT NULL,
  [ReRouteId] bigint NULL,
  [DynamicReRouteId] bigint NULL,
  [ClientWhitelist] nvarchar(1000) NULL,
  [EnableRateLimiting] tinyint NOT NULL,
  [Period] nvarchar(50) NULL,
  [PeriodTimespan] float NULL,
  [Limit] bigint NULL
)
GO


-- ----------------------------
-- Records of appapigatewayratelimitrule
-- ----------------------------
BEGIN TRANSACTION
GO

INSERT INTO [apigateway].[appapigatewayratelimitrule] VALUES (N'3', N'1261299170387169280', NULL, N'', N'0', NULL, NULL, NULL), (N'4', N'1261585859064872960', NULL, N'', N'0', NULL, NULL, NULL), (N'5', N'1261586605810368512', NULL, N'', N'0', NULL, NULL, NULL), (N'6', N'1261587558609436672', NULL, N'', N'0', NULL, NULL, NULL), (N'7', N'1261588213298348032', NULL, N'', N'0', NULL, NULL, NULL), (N'8', N'1261588367619375104', NULL, N'', N'0', NULL, NULL, NULL), (N'9', N'1261588628450557952', NULL, N'', N'0', NULL, NULL, NULL), (N'10', N'1261588881564221440', NULL, N'', N'0', NULL, NULL, NULL), (N'11', N'1261588983053795328', NULL, N'', N'0', NULL, NULL, NULL), (N'12', N'1261589139039961088', NULL, N'', N'0', NULL, NULL, NULL), (N'13', N'1261589197483393024', NULL, N'', N'0', NULL, NULL, NULL), (N'14', N'1261589278857084928', NULL, N'', N'0', NULL, NULL, NULL), (N'15', N'1261589420356124672', NULL, N'', N'0', NULL, NULL, NULL), (N'16', N'1261589960393736192', NULL, N'', N'0', NULL, NULL, NULL), (N'17', N'1261606600242085888', NULL, N'', N'0', NULL, NULL, NULL), (N'18', N'1261606689601732608', NULL, N'', N'0', NULL, NULL, NULL), (N'21', N'1262220447629058048', NULL, N'', N'0', NULL, NULL, NULL), (N'22', N'1262230734939758592', NULL, N'', N'0', NULL, NULL, NULL), (N'23', N'1262296916350869504', NULL, N'', N'0', NULL, NULL, NULL), (N'24', N'1262632376348594176', NULL, N'', N'0', NULL, NULL, NULL), (N'25', N'1262632791869902848', NULL, N'', N'0', NULL, NULL, NULL), (N'28', N'1262660336921235456', NULL, N'', N'0', NULL, NULL, NULL), (N'29', N'1262660528277966848', NULL, N'', N'0', NULL, NULL, NULL), (N'30', N'1262660706875625472', NULL, N'', N'0', NULL, NULL, NULL), (N'31', N'1262660966393991168', NULL, N'', N'0', NULL, NULL, NULL), (N'32', N'1262661109474283520', NULL, N'', N'0', NULL, NULL, NULL), (N'33', N'1262663888804663296', NULL, N'', N'0', NULL, NULL, NULL), (N'34', N'1262664024096133120', NULL, N'', N'0', NULL, NULL, NULL), (N'35', N'1262664186252120064', NULL, N'', N'0', NULL, NULL, NULL), (N'36', N'1262664357044178944', NULL, N'', N'0', NULL, NULL, NULL), (N'37', N'1262664632928718848', NULL, N'', N'0', NULL, NULL, NULL), (N'38', N'1262664751409418240', NULL, N'', N'0', NULL, NULL, NULL), (N'39', N'1262664871274237952', NULL, N'', N'0', NULL, NULL, NULL), (N'40', N'1262665026111164416', NULL, N'', N'0', NULL, NULL, NULL), (N'41', N'1262665159905267712', NULL, N'', N'0', NULL, NULL, NULL), (N'42', N'1262665329829105664', NULL, N'', N'0', NULL, NULL, NULL), (N'43', N'1262665456471920640', NULL, N'', N'0', NULL, NULL, NULL), (N'44', N'1262665628165754880', NULL, N'', N'0', NULL, NULL, NULL), (N'45', N'1262666172682883072', NULL, N'', N'0', NULL, NULL, NULL), (N'47', N'1262723402331885568', NULL, N'', N'0', NULL, NULL, NULL), (N'48', N'1262935771746734080', NULL, N'', N'0', NULL, NULL, NULL), (N'49', N'1262935906522304512', NULL, N'', N'0', NULL, NULL, NULL), (N'52', N'1263074419073593344', NULL, N'', N'0', NULL, NULL, NULL), (N'53', N'1263075249394790400', NULL, N'', N'0', NULL, NULL, NULL), (N'54', N'1263075593499684864', NULL, N'', N'0', NULL, NULL, NULL), (N'56', N'1263101898440146944', NULL, N'', N'0', NULL, NULL, NULL), (N'57', N'1263303878648569856', NULL, N'', N'0', NULL, NULL, NULL), (N'58', N'1263304204797648896', NULL, N'', N'0', NULL, NULL, NULL), (N'59', N'1263304872891555840', NULL, N'', N'0', NULL, NULL, NULL), (N'60', N'1263305106250047488', NULL, N'', N'0', NULL, NULL, NULL), (N'61', N'1263305244594970624', NULL, N'', N'0', NULL, NULL, NULL), (N'62', N'1263305430536855552', NULL, N'', N'0', NULL, NULL, NULL), (N'63', N'1263639172959174656', NULL, N'', N'0', NULL, NULL, NULL), (N'64', N'1264799968944640000', NULL, N'', N'0', NULL, NULL, NULL), (N'65', N'1264800070161584128', NULL, N'', N'0', NULL, NULL, NULL), (N'66', N'1267360794414161920', NULL, N'', N'0', NULL, NULL, NULL), (N'67', N'1267383367629807616', NULL, N'', N'0', NULL, NULL, NULL), (N'68', N'1267817055527632896', NULL, N'', N'0', NULL, NULL, NULL), (N'69', N'1267817221286526976', NULL, N'', N'0', NULL, NULL, NULL), (N'70', N'1268893687085518848', NULL, N'', N'0', NULL, NULL, NULL), (N'94', N'1288657613998579712', NULL, N'', N'1', N'1m', N'60', N'200'), (N'95', N'1288657941770854400', NULL, N'', N'0', NULL, NULL, NULL), (N'96', N'1288658134067109888', NULL, N'', N'0', NULL, NULL, NULL), (N'97', N'1288658305156964352', NULL, N'', N'0', NULL, NULL, NULL), (N'98', N'1288658491216289792', NULL, N'', N'0', NULL, NULL, NULL), (N'99', N'1288658638302142464', NULL, N'', N'0', NULL, NULL, NULL), (N'100', N'1288658791784308736', NULL, N'', N'0', NULL, NULL, NULL), (N'101', N'1290849478956199936', NULL, N'', N'0', NULL, NULL, NULL), (N'102', N'1290849628051124224', NULL, N'', N'0', NULL, NULL, NULL), (N'103', N'1290849798553776128', NULL, N'', N'0', NULL, NULL, NULL), (N'105', N'1291259822512693248', NULL, N'', N'0', NULL, NULL, NULL), (N'114', N'1293470838745821184', NULL, N'', N'0', NULL, NULL, NULL), (N'115', N'1293471661785706496', NULL, N'', N'0', NULL, NULL, NULL), (N'116', N'1293472678392721408', NULL, N'', N'0', NULL, NULL, NULL), (N'117', N'1293472857510473728', NULL, N'', N'0', NULL, NULL, NULL), (N'118', N'1299273336009359360', NULL, N'', N'0', NULL, NULL, NULL), (N'119', N'1299273436282585088', NULL, N'', N'0', NULL, NULL, NULL), (N'120', N'1299273618470567936', NULL, N'', N'0', NULL, NULL, NULL), (N'121', N'1299273770182737920', NULL, N'', N'0', NULL, NULL, NULL), (N'122', N'1299273978023084032', NULL, N'', N'0', NULL, NULL, NULL), (N'123', N'1299274123225694208', NULL, N'', N'0', NULL, NULL, NULL), (N'124', N'1299274222299348992', NULL, N'', N'0', NULL, NULL, NULL), (N'125', N'1304238876758495232', NULL, N'', N'0', NULL, NULL, NULL), (N'126', N'1304678610343383040', NULL, N'', N'0', NULL, NULL, NULL), (N'127', N'1304679169305694208', NULL, N'', N'0', NULL, NULL, NULL), (N'128', N'1310460417141817344', NULL, N'', N'0', NULL, NULL, NULL), (N'129', N'1310502391475519488', NULL, N'', N'0', NULL, NULL, NULL), (N'130', N'1310515546943569920', NULL, N'', N'0', NULL, NULL, NULL), (N'131', N'1310515735292985344', NULL, N'', N'0', NULL, NULL, NULL), (N'132', N'1316628769783480320', NULL, N'', N'0', NULL, NULL, NULL), (N'133', N'1316628940663619584', NULL, N'', N'0', NULL, NULL, NULL), (N'134', N'1316629112428756992', NULL, N'', N'0', NULL, NULL, NULL), (N'135', N'1316652047017246720', NULL, N'', N'0', NULL, NULL, NULL), (N'136', N'1316913899996737536', NULL, N'', N'0', NULL, NULL, NULL), (N'137', N'1319200951383199744', NULL, N'', N'0', NULL, NULL, NULL), (N'138', N'1319221929807024128', NULL, N'', N'0', NULL, NULL, NULL), (N'139', N'1319554431134306304', NULL, N'', N'0', NULL, NULL, NULL), (N'140', N'1319554550458060800', NULL, N'', N'0', NULL, NULL, NULL), (N'141', N'1319554948434595840', NULL, N'', N'0', NULL, NULL, NULL), (N'142', N'1319555067183730688', NULL, N'', N'0', NULL, NULL, NULL), (N'143', N'1319555230765780992', NULL, N'', N'0', NULL, NULL, NULL), (N'144', N'1319555333790470144', NULL, N'', N'0', NULL, NULL, NULL), (N'145', N'1321001932510203904', NULL, N'', N'0', NULL, NULL, NULL), (N'146', N'1321002059803136000', NULL, N'', N'0', NULL, NULL, NULL), (N'147', N'1321002256440496128', NULL, N'', N'0', NULL, NULL, NULL), (N'148', N'1321002350686507008', NULL, N'', N'0', NULL, NULL, NULL), (N'149', N'1322190027988525056', NULL, N'', N'0', NULL, NULL, NULL), (N'150', N'1322452079688458240', NULL, N'', N'0', NULL, NULL, NULL), (N'151', N'1322452183929495552', NULL, N'', N'0', NULL, NULL, NULL), (N'152', N'1322452308651319296', NULL, N'', N'0', NULL, NULL, NULL), (N'153', N'1322452858176446464', NULL, N'', N'0', NULL, NULL, NULL), (N'154', N'1322452989235863552', NULL, N'', N'0', NULL, NULL, NULL), (N'155', N'1322453089655889920', NULL, N'', N'0', NULL, NULL, NULL), (N'156', N'1329706860249804800', NULL, N'', N'0', NULL, NULL, NULL), (N'157', N'1329707002411544576', NULL, N'', N'0', NULL, NULL, NULL), (N'158', N'1329708512277098496', NULL, N'', N'0', NULL, NULL, NULL), (N'159', N'1329708625917571072', NULL, N'', N'0', NULL, NULL, NULL), (N'160', N'1335049839287357440', NULL, N'', N'0', NULL, NULL, NULL), (N'161', N'1335050034221830144', NULL, N'', N'0', NULL, NULL, NULL), (N'162', N'1335050145899368448', NULL, N'', N'0', NULL, NULL, NULL), (N'163', N'1335050283434790912', NULL, N'', N'0', NULL, NULL, NULL), (N'164', N'1335050381770248192', NULL, N'', N'0', NULL, NULL, NULL), (N'165', N'1335050520941449216', NULL, N'', N'0', NULL, NULL, NULL), (N'166', N'1335050615829188608', NULL, N'', N'0', NULL, NULL, NULL), (N'167', N'1335111798720450560', NULL, N'', N'0', NULL, NULL, NULL), (N'168', N'1335118541370314752', NULL, N'', N'0', NULL, NULL, NULL), (N'169', N'1335118660417245184', NULL, N'', N'0', NULL, NULL, NULL), (N'170', N'1335118782727344128', NULL, N'', N'0', NULL, NULL, NULL), (N'171', N'1335118903200337920', NULL, N'', N'0', NULL, NULL, NULL), (N'172', N'1336230645078921216', NULL, N'', N'0', NULL, NULL, NULL), (N'173', N'1337314809113722880', NULL, N'', N'0', NULL, NULL, NULL), (N'174', N'1337314938973569024', NULL, N'', N'0', NULL, NULL, NULL), (N'175', N'1340961907637243904', NULL, N'', N'0', NULL, NULL, NULL), (N'176', N'1341652247554379776', NULL, N'', N'0', NULL, NULL, NULL), (N'177', N'1341652385555369984', NULL, N'', N'0', NULL, NULL, NULL), (N'178', N'1342457939827552256', NULL, N'', N'0', NULL, NULL, NULL), (N'179', N'1342458050112581632', NULL, N'', N'0', NULL, NULL, NULL), (N'180', N'1363382062055915520', NULL, N'', N'0', NULL, NULL, NULL), (N'181', N'1363382298501414912', NULL, N'', N'0', NULL, NULL, NULL), (N'182', N'1368854800347848704', NULL, N'', N'0', NULL, NULL, NULL), (N'183', N'1368855936576413696', NULL, N'', N'0', NULL, NULL, NULL), (N'184', N'1368856295889854464', NULL, N'', N'0', NULL, NULL, NULL), (N'185', N'1368856703572008960', NULL, N'', N'0', NULL, NULL, NULL), (N'186', N'1368856819242524672', NULL, N'', N'0', NULL, NULL, NULL), (N'187', N'1368856927887581184', NULL, N'', N'0', NULL, NULL, NULL), (N'188', N'1368857128383700992', NULL, N'', N'0', NULL, NULL, NULL), (N'189', N'1369560306297233408', NULL, N'', N'0', NULL, NULL, NULL), (N'190', N'1369560450472239104', NULL, N'', N'0', NULL, NULL, NULL), (N'191', N'1371705034307375104', NULL, N'', N'0', NULL, NULL, NULL), (N'192', N'1376442078396276736', NULL, N'', N'0', NULL, NULL, NULL), (N'193', N'1376442309850554368', NULL, N'', N'0', NULL, NULL, NULL), (N'194', N'1376442440536678400', NULL, N'', N'0', NULL, NULL, NULL), (N'195', N'1376442557943635968', NULL, N'', N'0', NULL, NULL, NULL), (N'196', N'1376442689674141696', NULL, N'', N'0', NULL, NULL, NULL), (N'197', N'1376442971032248320', NULL, N'', N'0', NULL, NULL, NULL), (N'198', N'1376443123021242368', NULL, N'', N'0', NULL, NULL, NULL), (N'199', N'1376443238851141632', NULL, N'', N'0', NULL, NULL, NULL), (N'200', N'1376443392249421824', NULL, N'', N'0', NULL, NULL, NULL), (N'201', N'1376443586777047040', NULL, N'', N'0', NULL, NULL, NULL), (N'202', N'1376467826087682048', NULL, N'', N'0', NULL, NULL, NULL), (N'203', N'1376467990894469120', NULL, N'', N'0', NULL, NULL, NULL), (N'204', N'1376468110214029312', NULL, N'', N'0', NULL, NULL, NULL)
GO

COMMIT
GO


-- ----------------------------
-- Table structure for appapigatewayreroute
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[apigateway].[appapigatewayreroute]') AND type IN ('U'))
	DROP TABLE [apigateway].[appapigatewayreroute]
GO

CREATE TABLE [apigateway].[appapigatewayreroute] (
  [Id] int NOT NULL,
  [ExtraProperties] nvarchar(max) NULL,
  [ConcurrencyStamp] nvarchar(40) NULL,
  [ReRouteId] bigint NOT NULL,
  [ReRouteName] nvarchar(50) NOT NULL,
  [DownstreamPathTemplate] nvarchar(100) NOT NULL,
  [ChangeDownstreamPathTemplate] nvarchar(1000) NULL,
  [DownstreamHttpMethod] nvarchar(100) NULL,
  [UpstreamPathTemplate] nvarchar(100) NOT NULL,
  [UpstreamHttpMethod] nvarchar(50) NOT NULL,
  [AddHeadersToRequest] nvarchar(1000) NULL,
  [UpstreamHeaderTransform] nvarchar(1000) NULL,
  [DownstreamHeaderTransform] nvarchar(1000) NULL,
  [AddClaimsToRequest] nvarchar(1000) NULL,
  [RouteClaimsRequirement] nvarchar(1000) NULL,
  [AddQueriesToRequest] nvarchar(1000) NULL,
  [RequestIdKey] nvarchar(100) NULL,
  [ReRouteIsCaseSensitive] tinyint NOT NULL,
  [ServiceName] nvarchar(100) NULL,
  [ServiceNamespace] nvarchar(100) NULL,
  [DownstreamScheme] nvarchar(100) NULL,
  [DownstreamHostAndPorts] nvarchar(1000) NULL,
  [DelegatingHandlers] nvarchar(1000) NULL,
  [UpstreamHost] nvarchar(100) NULL,
  [Key] nvarchar(100) NULL,
  [Priority] int NULL,
  [Timeout] int NULL,
  [DangerousAcceptAnyServerCertificateValidator] tinyint NOT NULL,
  [DownstreamHttpVersion] nvarchar(30) NULL,
  [AppId] nvarchar(50) NOT NULL
)
GO


-- ----------------------------
-- Records of appapigatewayreroute
-- ----------------------------
BEGIN TRANSACTION
GO

INSERT INTO [apigateway].[appapigatewayreroute] VALUES (N'4', N'{}', N'84059fcecc91498b9beafac914865e2c', N'1261299170387169280', N'【后台管理】- 权限管理', N'/api/permission-management/permissions', N'', NULL, N'/api/permission-management/permissions', N'GET,PUT,', N'', N'', N'', N'', N'', N'', NULL, N'1', NULL, NULL, N'HTTP', N'127.0.0.1:30010,', N'', NULL, NULL, NULL, N'30000', N'1', NULL, N'TEST-APP'), (N'5', N'{}', N'f2312eed73cc4d3cbefcd1816849fd74', N'1261585859064872960', N'【身份认证服务】- 客户端', N'/api/identity-server/clients', N'', NULL, N'/api/identity-server/clients', N'POST,GET,', N'', N'', N'X-Forwarded-For:{RemoteIpAddress},', N'', N'', N'', NULL, N'1', N'', NULL, N'HTTP', N'127.0.0.1:30015,', N'', NULL, NULL, NULL, N'30000', N'1', NULL, N'TEST-APP'), (N'6', N'{}', N'b764ec7b994147abb12974bfcee4a0a9', N'1261586605810368512', N'【身份认证服务】- 管理客户端', N'/api/identity-server/clients/{Id}', N'', NULL, N'/api/identity-server/clients/{Id}', N'GET,DELETE,PUT,', N'', N'', N'', N'', N'', N'', NULL, N'1', NULL, NULL, N'HTTP', N'127.0.0.1:30015,', N'', NULL, NULL, NULL, N'30000', N'1', NULL, N'TEST-APP'), (N'7', N'{}', N'b4ce189320804dc6b87e602594e93d35', N'1261587558609436672', N'【服务网关管理】- 路由组管理', N'/api/ApiGateway/RouteGroups', N'', N'', N'/api/ApiGateway/RouteGroups', N'GET,POST,PUT,DELETE,', N'', N'', N'', N'', N'', N'', NULL, N'1', NULL, NULL, N'HTTP', N'127.0.0.1:30001,', N'', NULL, N'', NULL, N'30000', N'1', NULL, N'TEST-APP'), (N'8', N'{}', N'8736fefa36da4b129f3fcf6aa095f2ce', N'1261588213298348032', N'【服务网关管理】- 查询单个路由组', N'/api/ApiGateway/RouteGroups/By-AppId/{AppId}', N'', N'', N'/api/ApiGateway/RouteGroups/By-AppId/{AppId}', N'GET,', N'', N'', N'', N'', N'', N'', NULL, N'1', NULL, NULL, N'HTTP', N'127.0.0.1:30001,', N'', NULL, N'', NULL, N'30000', N'1', NULL, N'TEST-APP'), (N'9', N'{}', N'befd14ad39e244bc9dea7e0c01e642ce', N'1261588367619375104', N'【服务网关管理】- 查询所有有效路由组', N'/api/ApiGateway/RouteGroups/Actived', N'', N'', N'/api/ApiGateway/RouteGroups/Actived', N'GET,', N'', N'', N'', N'', N'', N'', NULL, N'1', NULL, NULL, N'HTTP', N'127.0.0.1:30001,', N'', NULL, N'', NULL, N'30000', N'1', NULL, N'TEST-APP'), (N'10', N'{}', N'0a95945d77144ce69addb0d1e8d37837', N'1261588628450557952', N'【服务网关管理】- 基础配置', N'/api/ApiGateway/Globals', N'', N'', N'/api/ApiGateway/Globals', N'GET,POST,PUT,DELETE,', N'', N'', N'', N'', N'', N'', NULL, N'1', NULL, NULL, N'HTTP', N'127.0.0.1:30001,', N'', NULL, N'', NULL, N'30000', N'1', NULL, N'TEST-APP'), (N'11', N'{}', N'70ee7f919bf44b42b549c905316bfd75', N'1261588881564221440', N'【服务网关管理】- 查询单个基础配置', N'/api/ApiGateway/Globals/By-AppId/{AppId}', N'', N'', N'/api/ApiGateway/Globals/By-AppId/{AppId}', N'GET,', N'', N'', N'', N'', N'', N'', NULL, N'1', NULL, NULL, N'HTTP', N'127.0.0.1:30001,', N'', NULL, N'', NULL, N'30000', N'1', NULL, N'TEST-APP'), (N'12', N'{}', N'caf54542d561428a9123ebed88e4b2e9', N'1261588983053795328', N'【服务网关管理】- 路由配置', N'/api/ApiGateway/Routes', N'', N'', N'/api/ApiGateway/Routes', N'GET,POST,PUT,DELETE,', N'', N'', N'', N'', N'', N'', NULL, N'1', NULL, NULL, N'HTTP', N'127.0.0.1:30001,', N'', NULL, N'', NULL, N'30000', N'1', NULL, N'TEST-APP'), (N'13', N'{}', N'df6c48fdaab44a37842992ae61c59dc5', N'1261589139039961088', N'【服务网关管理】- 通过标识查询路由', N'/api/ApiGateway/Routes/By-RouteId/{RouteId}', N'', N'', N'/api/ApiGateway/Routes/By-RouteId/{RouteId}', N'GET,', N'', N'', N'', N'', N'', N'', NULL, N'1', NULL, NULL, N'HTTP', N'127.0.0.1:30001,', N'', NULL, N'', NULL, N'30000', N'1', NULL, N'TEST-APP'), (N'14', N'{}', N'aaeaedebd24a4011ad565b5559f84c5f', N'1261589197483393024', N'【服务网关管理】- 通过名称查询路由', N'/api/ApiGateway/Routes/By-RouteName/{RouteName}', N'', N'', N'/api/ApiGateway/Routes/By-RouteName/{RouteName}', N'GET,', N'', N'', N'', N'', N'', N'', NULL, N'1', NULL, NULL, N'HTTP', N'127.0.0.1:30001,', N'', NULL, N'', NULL, N'30000', N'1', NULL, N'TEST-APP'), (N'15', N'{}', N'559c9f1b2b8c44caac86f7a643a16aaa', N'1261589278857084928', N'【服务网关管理】- 通过应用标识查询路由', N'/api/ApiGateway/Routes/By-AppId/{AppId}', N'', N'', N'/api/ApiGateway/Routes/By-AppId/{AppId}', N'GET,', N'', N'', N'', N'', N'', N'', NULL, N'1', NULL, NULL, N'HTTP', N'127.0.0.1:30001,', N'', NULL, N'', NULL, N'30000', N'1', NULL, N'TEST-APP'), (N'16', N'{}', N'00d0a12f403a4a919c99c534bd76d0d0', N'1261589420356124672', N'【服务网关管理】- 清空应用标识下所有路由', N'/api/ApiGateway/Routes/Clear', N'', N'', N'/api/ApiGateway/Routes/Clear', N'DELETE,', N'', N'', N'', N'', N'', N'', NULL, N'1', NULL, NULL, N'HTTP', N'127.0.0.1:30001,', N'', NULL, N'', NULL, N'30000', N'1', NULL, N'TEST-APP'), (N'17', N'{}', N'8c308f1386ad49c799cd281eb95170ac', N'1261589960393736192', N'【服务网关管理】- 通过应用标识查询动态路由', N'/api/ApiGateway/DynamicRoutes/By-AppId/{AppId}', N'', NULL, N'/api/ApiGateway/DynamicRoutes/By-AppId/{AppId}', N'GET,', N'', N'', N'', N'', N'', N'', NULL, N'1', NULL, NULL, N'HTTP', N'127.0.0.1:30001,', N'', NULL, NULL, NULL, N'30000', N'1', NULL, N'TEST-APP'), (N'18', N'{}', N'e659ebbf61534a978335cfeabdc0b375', N'1261606600242085888', N'【服务网关管理】- 通过应用标识查询聚合路由', N'/api/ApiGateway/Aggregates/by-AppId/{AppId}', N'', NULL, N'/api/ApiGateway/Aggregates/by-AppId/{AppId}', N'GET,', N'', N'', N'', N'', N'', N'', NULL, N'1', NULL, NULL, N'HTTP', N'127.0.0.1:30001,', N'', NULL, NULL, NULL, N'30000', N'1', NULL, N'TEST-APP'), (N'19', N'{}', N'd665e4491b81413385858601d9cf9a1d', N'1261606689601732608', N'【服务网关管理】- 聚合路由', N'/api/ApiGateway/Aggregates', N'', NULL, N'/api/ApiGateway/Aggregates', N'GET,POST,PUT,DELETE,', N'', N'', N'', N'', N'', N'', NULL, N'1', NULL, NULL, N'HTTP', N'127.0.0.1:30001,', N'', NULL, NULL, NULL, N'30000', N'1', NULL, N'TEST-APP'), (N'22', N'{}', N'2aad614e2c2a497593a4784ff639c3d9', N'1262220447629058048', N'【身份认证服务】- 克隆客户端', N'/api/identity-server/clients/{id}/clone', N'', NULL, N'/api/identity-server/clients/{id}/clone', N'POST,', N'', N'', N'', N'', N'', N'', NULL, N'1', NULL, NULL, N'HTTP', N'127.0.0.1:30015,', N'', NULL, NULL, NULL, N'30000', N'1', NULL, N'TEST-APP'), (N'23', N'{}', N'1504c5e4a7334298878339a305445b21', N'1262230734939758592', N'【身份认证服务】- 可用的Api资源', N'/api/identity-server/clients/assignable-api-resources', N'', NULL, N'/api/identity-server/clients/assignable-api-resources', N'GET,', N'', N'', N'', N'', N'', N'', NULL, N'1', NULL, NULL, N'HTTP', N'127.0.0.1:30015,', N'', NULL, N'assignable-api-resources', NULL, N'30000', N'1', NULL, N'TEST-APP'), (N'25', N'{}', N'53dd1751d9104940a966006a5e93d1fa', N'1262296916350869504', N'【身份认证服务】- 可用的身份资源', N'/api/identity-server/clients/assignable-identity-resources', N'', NULL, N'/api/identity-server/clients/assignable-identity-resources', N'GET,', N'', N'', N'', N'', N'', N'', NULL, N'1', NULL, NULL, N'HTTP', N'127.0.0.1:30015,', N'', NULL, N'assignable-identity-resources', NULL, N'30000', N'1', NULL, N'TEST-APP'), (N'26', N'{}', N'3fccd1318d0d47d9aef85542668829a6', N'1262632376348594176', N'【身份认证服务】- Api资源', N'/api/identity-server/api-resources', N'', N'', N'/api/identity-server/api-resources', N'GET,POST,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'27', N'{}', N'de6bd0ddea6d4019b4855be5442fafdc', N'1262632791869902848', N'【身份认证服务】- 管理Api资源', N'/api/identity-server/api-resources/{id}', N'', N'', N'/api/identity-server/api-resources/{id}', N'GET,DELETE,PUT,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'30', N'{}', N'0f9875697b74420c9dc2eaf77099b210', N'1262660336921235456', N'【身份认证服务】- 用户登录', N'/api/account/login', N'', N'', N'/api/account/login', N'POST,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'31', N'{}', N'a890c6ecc6a64c9fa313a0f6b5406e1c', N'1262660528277966848', N'【身份认证服务】- 用户登出', N'/api/account/logout', N'', N'', N'/api/account/logout', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'32', N'{}', N'88de580b6beb4d9d9d4367840ba1fcea', N'1262660706875625472', N'【身份认证服务】- 检查密码', N'/api/account/checkPassword', N'', N'', N'/api/account/checkPassword', N'POST,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'33', N'{}', N'78f3c1adc7a54696af37a419eda47c62', N'1262660966393991168', N'【身份认证服务】- 个人信息页', N'/api/identity/my-profile', N'', N'', N'/api/identity/my-profile', N'GET,PUT,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'34', N'{}', N'95b23aa5cebb40598a78c0761cfd0b26', N'1262661109474283520', N'【身份认证服务】- 修改密码', N'/api/identity/my-profile/change-password', N'', N'', N'/api/identity/my-profile/change-password', N'POST,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'35', N'{}', N'4828f7c2aff8485189f37aba5de62d60', N'1262663888804663296', N'【身份认证管理】- 角色管理', N'/api/identity/roles', N'', N'', N'/api/identity/roles', N'GET,POST,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'36', N'{}', N'0fddcd3b50a24c6795ec9034fdb44778', N'1262664024096133120', N'【身份认证服务】- 角色列表', N'/api/identity/roles/all', N'', N'', N'/api/identity/roles/all', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'37', N'{}', N'191e555219e845069dfd93793263a840', N'1262664186252120064', N'【身份认证服务】- 单个角色', N'/api/identity/roles/{id}', N'', N'', N'/api/identity/roles/{id}', N'GET,PUT,DELETE,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'38', N'{}', N'c316858e82f74e6ca6e923d6b3a3fa76', N'1262664357044178944', N'【身份认证服务】- 用户注册', N'/api/account/register', N'', N'', N'/api/account/register', N'POST,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'39', N'{}', N'db53b6f957914a10a6a97ba306b1f6ef', N'1262664632928718848', N'【身份认证服务】- 单个用户', N'/api/identity/users/{id}', N'', N'', N'/api/identity/users/{id}', N'GET,PUT,DELETE,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'40', N'{}', N'1833434b8ce34f8ab791e7e950f4c61f', N'1262664751409418240', N'【身份认证服务】- 用户管理', N'/api/identity/users', N'', N'', N'/api/identity/users', N'GET,POST,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'41', N'{}', N'b3c963a1612144918bffaf272697498c', N'1262664871274237952', N'【身份认证服务】- 用户角色', N'/api/identity/users/{id}/roles', N'', N'', N'/api/identity/users/{id}/roles', N'GET,POST,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'42', N'{}', N'33dd757b79cb4f52994af13bfb4f6783', N'1262665026111164416', N'【身份认证服务】- 通过用户名查询用户', N'/api/identity/users/by-username/{userName}', N'', N'', N'/api/identity/users/by-username/{userName}', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'43', N'{}', N'e46fd6cb3a104da3aadfe0149fe4de68', N'1262665159905267712', N'【身份认证服务】- 通过邮件查询用户', N'/api/identity/users/by-email/{email}', N'', N'', N'/api/identity/users/by-email/{email}', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'44', N'{}', N'8c8ec5ad6aaa4145981ee7ac876c36c9', N'1262665329829105664', N'【身份认证服务】- 通过标识查询用户', N'/api/identity/users/lookup/{id}', N'', N'', N'/api/identity/users/lookup/{id}', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'45', N'{}', N'f5c0c8c02c0846fdbe5015cd86f3d81b', N'1262665456471920640', N'【身份认证服务】- 通过名称查询用户', N'/api/identity/users/lookup/by-username/{userName}', N'', N'', N'/api/identity/users/lookup/by-username/{userName}', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'46', N'{}', N'ecf0ea4a3e3c4b2e8fa3621514d00c74', N'1262665628165754880', N'【基础服务】- 通过名称查询租户', N'/api/abp/multi-tenancy/tenants/by-name/{name}', N'', N'', N'/api/abp/multi-tenancy/tenants/by-name/{name}', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30010,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'47', N'{}', N'69132bc515b64005af4292ce0dee5626', N'1262666172682883072', N'【基础服务】- 通过标识查询租户', N'/api/abp/multi-tenancy/tenants/by-id/{id}', N'', N'', N'/api/abp/multi-tenancy/tenants/by-id/{id}', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30010,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'49', N'{}', N'35f48d7bc3694bbf95d64fe59aa631ac', N'1262723402331885568', N'【身份认证服务】- 已有的跨域资源', N'/api/identity-server/clients/distinct-cors-origins', N'', N'', N'/api/identity-server/clients/distinct-cors-origins', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'distinct-cors-origins', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'50', N'{}', N'f333d028839d4fc2aafa8509e674d7dd', N'1262935771746734080', N'【身份认证服务】- 身份资源', N'/api/identity-server/identity-resources', N'', N'', N'/api/identity-server/identity-resources', N'GET,POST,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'51', N'{}', N'dffd1bfaca5b4c1890221678f2b16cd5', N'1262935906522304512', N'【身份认证服务】- 身份资源管理', N'/api/identity-server/identity-resources/{id}', N'', N'', N'/api/identity-server/identity-resources/{id}', N'GET,PUT,DELETE,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'54', N'{}', N'7b847d8434bc4d1db07fa8961d90c14a', N'1263074419073593344', N'【服务网关管理】- 接口代理', N'/api/abp/api-definition', N'', N'', N'/api/abp/apigateway/api-definition', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30001,', N'', N'', N'apigateway-api-definition', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'55', N'{}', N'ca2cedfa620045a9adef0be2f958c4bc', N'1263075249394790400', N'【服务网关管理】- 查询聚合路由', N'/api/ApiGateway/Aggregates/{RouteId}', N'', N'', N'/api/ApiGateway/Aggregates/{RouteId}', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30001,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'57', N'{}', N'98fbc99fc8644946ac0a72cc3dc5fd1f', N'1263075593499684864', N'【服务网关管理】- 聚合路由配置', N'/api/ApiGateway/Aggregates/RouteConfig', N'', N'', N'/api/ApiGateway/Aggregates/RouteConfig', N'POST,DELETE,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30001,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'59', N'{}', N'c692b30c72d4424eb4740ac49f4e9373', N'1263101898440146944', N'【服务网关管理】- 框架配置', N'/api/abp/application-configuration', N'', N'', N'/api/abp/apigateway/application-configuration', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30001,', N'', N'', N'apigateway-configuration', N'0', N'120000', N'1', N'', N'TEST-APP'), (N'60', N'{}', N'8409117162504f71aa66982f05c38a80', N'1263303878648569856', N'【平台服务】- 接口代理', N'/api/abp/api-definition', N'', N'', N'/api/abp/platform/api-definition', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30025,', N'', N'', N'platform-api-definition', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'61', N'{}', N'9f520820071b4e14bc94ab57989cea1f', N'1263304204797648896', N'【平台服务】- 框架配置', N'/api/abp/application-configuration', N'', N'', N'/api/abp/platform/application-configuration', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30025,', N'', N'', N'platform-configuration', N'0', N'120000', N'1', N'', N'TEST-APP'), (N'62', N'{}', N'530ab314560f41678b40f48da9383d51', N'1263304872891555840', N'【后台管理】- 租户管理', N'/api/tenant-management/tenants', N'', N'', N'/api/tenant-management/tenants', N'GET,POST,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30010,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'63', N'{}', N'21334c6da4c349cc883c38c13de0e754', N'1263305106250047488', N'【后台管理】- 特定租户管理', N'/api/tenant-management/tenants/{id}', N'', N'', N'/api/tenant-management/tenants/{id}', N'GET,PUT,DELETE,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30010,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'64', N'{}', N'cc8fdf1b2d0b414ebf2dc51a6dc78305', N'1263305244594970624', N'【后台管理】- 租户连接字符串', N'/api/tenant-management/tenants/{id}/connection-string', N'', N'', N'/api/tenant-management/tenants/{id}/connection-string', N'GET,PUT,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30010,', N'', N'', N'', N'2', N'30000', N'1', N'', N'TEST-APP'), (N'65', N'{}', N'aaf285ed10da4024ba561d5cf8c6322b', N'1263305430536855552', N'【后台管理】- 特定租户连接字符串', N'/api/tenant-management/tenants/{id}/connection-string/{name}', N'', N'', N'/api/tenant-management/tenants/{id}/connection-string/{name}', N'GET,DELETE,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30010,', N'', N'', N'', N'1', N'30000', N'1', N'', N'TEST-APP'), (N'66', N'{}', N'6a7da198f4c84d94969a437adc47642b', N'1263639172959174656', N'【后台管理】- 全局设置', N'/api/setting-management/settings/by-global', N'', N'', N'/api/setting-management/settings/by-global/app', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30010,', N'', N'', N'setting-global', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'67', N'{}', N'755b4dce5c34444785fa3b647fef4131', N'1264799968944640000', N'【身份认证服务】- 验证手机号', N'/api/account/phone/verify', N'', N'', N'/api/account/phone/verify', N'POST,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'68', N'{}', N'535191c570ae453ab320012304d7a62c', N'1264800070161584128', N'【身份认证服务】- 手机号注册', N'/api/account/phone/register', N'', N'', N'/api/account/phone/register', N'POST,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'69', N'{}', N'723c9b111f9f4a1aa804118cdde193d3', N'1267360794414161920', N'【消息服务】- 通知', N'/signalr-hubs/notifications/{everything}', N'', N'', N'/signalr-hubs/notifications/{everything}', N'POST,GET,OPTIONS,PUT,DELETE,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'ws', N'127.0.0.1:30020,', N'', N'', N'', N'1', N'30000', N'1', N'', N'TEST-APP'), (N'70', N'{}', N'f3aa2b42dd9f468aa5aae4ef64754427', N'1267383367629807616', N'【消息服务】- 通知0', N'/signalr-hubs/notifications', N'', N'', N'/signalr-hubs/notifications', N'GET,POST,PUT,DELETE,OPTIONS,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'ws', N'127.0.0.1:30020,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'71', N'{}', N'0344947bb79b401baa2ef7b4e58297f6', N'1267817055527632896', N'【消息服务】- 聊天', N'/signalr-hubs/messages', N'', N'', N'/signalr-hubs/messages', N'GET,POST,PUT,DELETE,OPTIONS,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'ws', N'127.0.0.1:30020,', N'', N'', N'', N'1', N'30000', N'1', N'', N'TEST-APP'), (N'72', N'{}', N'6676b5e5f76d40739f9ccc3e371e2f18', N'1267817221286526976', N'【消息服务】- 聊天1', N'/signalr-hubs/messages/{everything}', N'', N'', N'/signalr-hubs/messages/{everything}', N'GET,POST,PUT,DELETE,OPTIONS,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'ws', N'127.0.0.1:30020,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'73', N'{}', N'cfb5f09a12bf495fbcaf2fa5d9123a40', N'1268893687085518848', N'【身份认证服务】- 重置密码', N'/api/account/phone/reset-password', N'', N'', N'/api/account/phone/reset-password', N'PUT,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'1', N'30000', N'1', N'', N'TEST-APP'), (N'101', N'{}', N'997a4c27a433458aafed9b8aa252d957', N'1288657613998579712', N'【身份认证服务】- 组织机构列表', N'/api/identity/organization-units', N'', N'', N'/api/identity/organization-units', N'GET,POST,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'102', N'{}', N'a2c6acc9882a425ab26bd3ad5a9c17c6', N'1288657941770854400', N'【身份认证服务】- 组织机构管理', N'/api/identity/organization-units/{id}', N'', N'', N'/api/identity/organization-units/{id}', N'GET,PUT,DELETE,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'1', N'30000', N'1', N'', N'TEST-APP'), (N'103', N'{}', N'390acfb0e16943c6b61e731d47c282e9', N'1288658134067109888', N'【身份认证服务】- 组织机构移动', N'/api/identity/organization-units/{id}/move', N'', N'', N'/api/identity/organization-units/{id}/move', N'PUT,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'104', N'{}', N'3515e75becf9447492ad60466b27c397', N'1288658305156964352', N'【身份认证服务】- 查询组织机构子级', N'/api/identity/organization-units/find-children', N'', N'', N'/api/identity/organization-units/find-children', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'105', N'{}', N'aab0a24d930f4f9687497e5ccaac2a31', N'1288658491216289792', N'【身份认证服务】- 查询组织机构最后一个子节点', N'/api/identity/organization-units/last-children', N'', N'', N'/api/identity/organization-units/last-children', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'106', N'{}', N'9b7c999b1c5140c497bc15914a815401', N'1288658638302142464', N'【身份认证服务】- 未加入组织机构角色', N'/api/identity/organization-units/{id}/unadded-roles', N'', N'', N'/api/identity/organization-units/{id}/unadded-roles', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'107', N'{}', N'b0cdb3f6908e42bd934ca99a78f22c3f', N'1288658791784308736', N'【身份认证服务】- 未加入组织机构用户', N'/api/identity/organization-units/{id}/unadded-users', N'', N'', N'/api/identity/organization-units/{id}/unadded-users', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'108', N'{}', N'9362040d10a94fb991f60bc391efcb85', N'1290849478956199936', N'【后台管理】- 当前租户设置', N'/api/setting-management/settings/by-current-tenant', N'', N'', N'/api/setting-management/settings/by-current-tenant/app', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30010,', N'', N'', N'setting-current-tenant', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'109', N'{}', N'a7df3a04805d4cc8a6e6b3823c6dd468', N'1290849628051124224', N'【后台管理】- 用户设置', N'/api/setting-management/settings/by-user/{userId}', N'', N'', N'/api/setting-management/settings/by-user/{userId}', N'GET,PUT,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30010,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'110', N'{}', N'ef6e38a529a345fab67f6a627cf20635', N'1290849798553776128', N'【后台管理】- 当前用户设置', N'/api/setting-management/settings/by-current-user', N'', N'', N'/api/setting-management/settings/by-current-user', N'GET,PUT,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30010,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'112', N'{}', N'9844fed6507844f2ac64bd08649bd3a6', N'1291259822512693248', N'【身份认证服务】- 查询组织机构根节点', N'/api/identity/organization-units/root-node', N'', N'', N'/api/identity/organization-units/root-node', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'121', N'{}', N'c6c7b027000942dda8ba0d2e2d8cf705', N'1293470838745821184', N'【后台管理】- 框架配置', N'/api/abp/application-configuration', N'', N'', N'/api/abp/backend-admin/application-configuration', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30010,', N'', N'', N'backend-admin-configuration', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'122', N'{}', N'becd4342079d4399abda5b5ba3b46fdc', N'1293471661785706496', N'【消息服务】- 框架配置', N'/api/abp/application-configuration', N'', N'', N'/api/abp/messages/application-configuration', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30020,', N'', N'', N'messages-configuration', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'123', N'{}', N'c828140cee3043c18ffc274f6461f0f2', N'1293472678392721408', N'【后台管理】- 接口代理', N'/api/abp/api-definition', N'', N'', N'/api/abp/backend-admin/api-definition', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30010,', N'', N'', N'backend-admin-api-definition', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'124', N'{}', N'e683cff8066d4c2899a17d0f618f1a0b', N'1293472857510473728', N'【消息服务】- 接口代理', N'/api/abp/api-definition', N'', N'', N'/api/abp/messages/api-definition', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30020,', N'', N'', N'messages-api-definition', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'125', N'{}', N'0e9c3bff5b58428eba97a5516140ba5e', N'1299273336009359360', N'【消息服务】- Hangfire仪表板 ', N'/hangfire', N'', N'', N'/hangfire', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30020,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'126', N'{}', N'e906924ad3a947cf8e6956e2dd258192', N'1299273436282585088', N'【消息服务】- Hangfire仪表板 - 主页', N'/hangfire/', N'', N'', N'/hangfire/', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30020,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'127', N'{}', N'e02f2049efbc4ee1ad6629bd0341ed2b', N'1299273618470567936', N'【消息服务】- Hangfire仪表板 - 状态', N'/hangfire/stats', N'', N'', N'/hangfire/stats', N'POST,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30020,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'128', N'{}', N'f8d2b2f0f1d649c2a07eeef23d6adb0e', N'1299273770182737920', N'【消息服务】- Hangfire仪表板 - 作业管理', N'/hangfire/jobs/{everything}', N'', N'', N'/hangfire/jobs/{everything}', N'GET,POST,PUT,DELETE,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30020,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'129', N'{}', N'9785be7a29774b468e271b23009fe115', N'1299273978023084032', N'【消息服务】- Hangfire仪表板 - 重试', N'/hangfire/retries', N'', N'', N'/hangfire/retries', N'GET,POST,PUT,DELETE,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30020,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'130', N'{}', N'9c0c1cd196bb45c0bc03fafb7a1eb8f2', N'1299274123225694208', N'【消息服务】- Hangfire仪表板 - 周期性作业', N'/hangfire/recurring', N'', N'', N'/hangfire/recurring', N'GET,POST,DELETE,PUT,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30020,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'131', N'{}', N'243bafe828be463ea63a3e2b521f9923', N'1299274222299348992', N'【消息服务】- Hangfire仪表板 - 服务器列表', N'/hangfire/servers', N'', N'', N'/hangfire/servers', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30020,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'132', N'{}', N'57a8ac1b41bb434cad38fbde0e2ba2f0', N'1304238876758495232', N'【后台管理】- 管理功能', N'/api/feature-management/features', N'', N'', N'/api/feature-management/features', N'GET,PUT,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30010,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'133', N'{}', N'40a150f629b047f587c91a9436a699c0', N'1304678610343383040', N'【身份认证服务】- 接口代理', N'/api/abp/api-definition', N'', N'', N'/api/abp/identity-server/api-definition', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'identity-server-api-definition', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'134', N'{}', N'3a2d5d538fa44ac690402fc5c4e1a401', N'1304679169305694208', N'【身份认证服务】- 框架配置', N'/api/abp/application-configuration', N'', N'', N'/api/abp/identity-server/application-configuration', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'identity-server-configuration', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'135', N'{}', N'2ecfe7483bc94c28ad0769b654eb765d', N'1310460417141817344', N'【后台管理】- 审计日志列表', N'/api/auditing/audit-log', N'', N'', N'/api/auditing/audit-log', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30010,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'136', N'{}', N'744e340c0024462d88458b7ea9605b3c', N'1310502391475519488', N'【后台服务】- 安全日志列表', N'/api/auditing/security-log', N'', N'', N'/api/auditing/security-log', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30010,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'137', N'{}', N'a99639f4172547c4ba9b4f4ca5cb4ab9', N'1310515546943569920', N'【后台服务】- 安全日志', N'/api/auditing/security-log/{id}', N'', N'', N'/api/auditing/security-log/{id}', N'DELETE,GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30010,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'138', N'{}', N'c3ebc82d55f640fb9d70a911e97e4ec1', N'1310515735292985344', N'【后台管理】- 审计日志', N'/api/auditing/audit-log/{id}', N'', N'', N'/api/auditing/audit-log/{id}', N'DELETE,GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30010,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'139', N'{}', N'0379fcb3a9cd4b13b562b3b5b5c3eb7d', N'1316628769783480320', N'【身份认证服务】- 声明类型', N'/api/identity/claim-types', N'', N'', N'/api/identity/claim-types', N'GET,POST,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'140', N'{}', N'de25c9a80d994f728b37eb483b2f5127', N'1316628940663619584', N'【身份认证服务】- 管理声明类型', N'/api/identity/claim-types/{id}', N'', N'', N'/api/identity/claim-types/{id}', N'GET,PUT,DELETE,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'141', N'{}', N'25c19106baff4cf3a877ae8cd690a1b5', N'1316629112428756992', N'【身份认证服务】- 查询在用的声明类型列表', N'/api/identity/claim-types/actived-list', N'', N'', N'/api/identity/claim-types/actived-list', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'142', N'{}', N'7d3b941d8c4d4d3ebc05b6332308b992', N'1316652047017246720', N'【身份认证服务】- 管理用户声明', N'/api/identity/users/{id}/claims', N'', N'', N'/api/identity/users/{id}/claims', N'POST,PUT,DELETE,GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'143', N'{}', N'b86af44a34a14db4b482df8550f1bde1', N'1316913899996737536', N'【身份认证管理】- 管理角色声明', N'/api/identity/roles/claims/{id}', N'', N'', N'/api/identity/roles/claims/{id}', N'GET,POST,PUT,DELETE,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'144', N'{}', N'51a14bc295044de985ae014fbcc5bddf', N'1319200951383199744', N'【IdentityServer4】- 发现端点', N'/.well-known/openid-configuration', N'', N'', N'/.well-known/openid-configuration', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:44385,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'145', N'{}', N'9d859a444d774e93818237e53b6cc102', N'1319221929807024128', N'【身份认证服务】- 查询所有组织机构', N'/api/identity/organization-units/all', N'', N'', N'/api/identity/organization-units/all', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'146', N'{}', N'89f42175b24540caba2a1fded145acf8', N'1319554431134306304', N'【身份认证服务】- 管理组织机构用户', N'/api/identity/organization-units/{id}/users', N'', N'', N'/api/identity/organization-units/{id}/users', N'POST,GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'147', N'{}', N'126d82509cec43eda712e94737b01039', N'1319554550458060800', N'【身份认证服务】- 管理组织机构角色', N'/api/identity/organization-units/{id}/roles', N'', N'', N'/api/identity/organization-units/{id}/roles', N'POST,GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'148', N'{}', N'824f5eee6877489f96f1022e306c968a', N'1319554948434595840', N'【身份认证服务】- 管理角色组织机构', N'/api/identity/roles/{id}/organization-units', N'', N'', N'/api/identity/roles/{id}/organization-units', N'GET,PUT,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'149', N'{}', N'fe1379d4a13f41afb6410f4c948871f3', N'1319555067183730688', N'【身份认证服务】- 删除角色组织机构', N'/api/identity/roles/{id}/organization-units/{ouId}', N'', N'', N'/api/identity/roles/{id}/organization-units/{ouId}', N'DELETE,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'150', N'{}', N'197a69bb723346aba3601bd61e7fa655', N'1319555230765780992', N'【身份认证服务】- 管理用户组织机构', N'/api/identity/users/{id}/organization-units', N'', N'', N'/api/identity/users/{id}/organization-units', N'GET,PUT,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'151', N'{}', N'1a7e6d0b1c95484f82a75a2ce6e6f453', N'1319555333790470144', N'【身份认证服务】- 删除用户组织机构', N'/api/identity/users/{id}/organization-units/{ouId}', N'', N'', N'/api/identity/users/{id}/organization-units/{ouId}', N'DELETE,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'152', N'{}', N'ecfa9bbd19694097b33e691b653f2124', N'1321001932510203904', N'【消息服务】- 我的消息', N'/api/im/chat/my-messages', N'', N'', N'/api/im/chat/my-messages', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30020,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'153', N'{}', N'4867ad188ca54acb8b961d20297b6545', N'1321002059803136000', N'【消息服务】- 我的最近消息', N'/api/im/chat/my-last-messages', N'', N'', N'/api/im/chat/my-last-messages', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30020,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'154', N'{}', N'291ab802d7bc4af98fc15c509f98fa75', N'1321002256440496128', N'【消息服务】- 我的朋友', N'/api/im/my-friends', N'', N'', N'/api/im/my-friends', N'GET,POST,DELETE,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30020,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'155', N'{}', N'08438dabb1e849988e0c304f82b08a10', N'1321002350686507008', N'【消息服务】- 我的所有朋友', N'/api/im/my-friends/all', N'', N'', N'/api/im/my-friends/all', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30020,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'156', N'{}', N'cecf632785b7402299764698369c751f', N'1322190027988525056', N'【消息服务】- 发送好友请求', N'/api/im/my-friends/add-request', N'', N'', N'/api/im/my-friends/add-request', N'POST,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30020,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'157', N'{}', N'91b088ab3e164116b8c4a2b6109e37e9', N'1322452079688458240', N'【消息服务】- 我的订阅', N'/api/my-subscribes', N'', N'', N'/api/my-subscribes', N'GET,POST,DELETE,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30020,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'158', N'{}', N'b9f57600634b40c087ac0730926895e2', N'1322452183929495552', N'【消息服务】- 我的订阅列表', N'/api/my-subscribes/all', N'', N'', N'/api/my-subscribes/all', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30020,', N'', N'', N'my-subscribes', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'159', N'{}', N'8e6a0c6759df42e29b9abd4d833646a3', N'1322452308651319296', N'【消息服务】- 是否已订阅', N'/api/my-subscribes/is-subscribed/{Name}', N'', N'', N'/api/my-subscribes/is-subscribed/{Name}', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30020,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'160', N'{}', N'6b747d0acfee478ea64241b7ea519861', N'1322452858176446464', N'【消息服务】- 我的通知', N'/api/my-notifilers', N'', N'', N'/api/my-notifilers', N'GET,POST,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30020,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'161', N'{}', N'25a64613f37f44cf8efff8d0cf6cd0ca', N'1322452989235863552', N'【消息服务】- 管理我的通知', N'/api/my-notifilers/{id}', N'', N'', N'/api/my-notifilers/{id}', N'GET,DELETE,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30020,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'162', N'{}', N'ddc7f13aaa2741b28c2102fbff18d836', N'1322453089655889920', N'【消息服务】- 可用通知列表', N'/api/my-notifilers/assignables', N'', N'', N'/api/my-notifilers/assignables', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30020,', N'', N'', N'assignables-notifilers', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'163', N'{}', N'1d586107d7e240b3bfc14c8ca04baea8', N'1329706860249804800', N'【微信管理】- 微信公共配置', N'/api/setting-management/wechat/by-global', N'', N'', N'/api/setting-management/wechat/by-global', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30010,', N'', N'', N'wechat-setting-global', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'164', N'{}', N'bd9b34dd564f405bb3a063d6a719ce6e', N'1329707002411544576', N'【微信管理】- 微信租户配置', N'/api/setting-management/wechat/by-current-tenant', N'', N'', N'/api/setting-management/wechat/by-current-tenant', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30010,', N'', N'', N'wechat-setting-current-tenant', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'165', N'{}', N'e9847659244b47dc80101b1c3f4edb31', N'1329708512277098496', N'【后台管理】- 变更全局设置', N'/api/setting-management/settings/change-global', N'', N'', N'/api/setting-management/settings/change-global', N'PUT,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30010,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'166', N'{}', N'94c3692dea394b39ac0cc19f60fc1964', N'1329708625917571072', N'【后台管理】- 变更当前租户设置', N'/api/setting-management/settings/change-current-tenant', N'', N'', N'/api/setting-management/settings/change-current-tenant', N'PUT,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30010,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'167', N'{}', N'626efeddcc0a474aa5e472b6089ca6fe', N'1335049839287357440', N'【平台服务】- 菜单管理', N'/api/platform/menus', N'', N'', N'/api/platform/menus', N'GET,POST,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30025,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'168', N'{}', N'e4b4acf5f7dd4015828bdcd735424937', N'1335050034221830144', N'【平台服务】- 管理菜单', N'/api/platform/menus/{id}', N'', N'', N'/api/platform/menus/{id}', N'GET,PUT,DELETE,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30025,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'169', N'{}', N'bbdb31a7eaba4bf7bead6a7e1151872c', N'1335050145899368448', N'【平台服务】- 获取当前用户菜单', N'/api/platform/menus/by-current-user', N'', N'', N'/api/platform/menus/by-current-user', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30025,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'170', N'{}', N'56a6d15682ba489aa46296cd8235cf81', N'1335050283434790912', N'【平台服务】- 管理用户菜单', N'/api/platform/menus/by-user', N'', N'', N'/api/platform/menus/by-user', N'GET,PUT,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30025,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'171', N'{}', N'484dc97d5f4843fc85679c1faf1488bf', N'1335050381770248192', N'【平台服务】- 获取用户菜单', N'/api/platform/menus/by-user/{userId}/{platformType}', N'', N'', N'/api/platform/menus/by-user/{userId}/{platformType}', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30025,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'172', N'{}', N'63b34bbe4d3b41f397432aa12e73a40d', N'1335050520941449216', N'【平台服务】- 管理角色菜单', N'/api/platform/menus/by-role', N'', N'', N'/api/platform/menus/by-role', N'GET,PUT,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30025,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'173', N'{}', N'c858522741c941bf838aef7eba34b213', N'1335050615829188608', N'【平台服务】- 获取角色菜单', N'/api/platform/menus/by-role/{role}/{platformType}', N'', N'', N'/api/platform/menus/by-role/{role}/{platformType}', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30025,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'174', N'{}', N'da02cbd7a46746f288bb98d40af03a21', N'1335111798720450560', N'【平台服务】- 获取所有数据字典', N'/api/platform/datas/all', N'', N'', N'/api/platform/datas/all', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30025,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'175', N'{}', N'6c6096b6593c47398d59f73ab62e54a3', N'1335118541370314752', N'【平台服务】- 数据字典', N'/api/platform/datas', N'', N'', N'/api/platform/datas', N'GET,POST,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30025,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'176', N'{}', N'5efb1b49a1cc4d34a3e338b9e83c12c7', N'1335118660417245184', N'【平台服务】- 管理数据字典', N'/api/platform/datas/{id}', N'', N'', N'/api/platform/datas/{id}', N'GET,PUT,DELETE,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30025,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'177', N'{}', N'00bd2892c6574942bda538aa9d3513cd', N'1335118782727344128', N'【平台服务】- 增加数据字典项目', N'/api/platform/datas/{id}/items', N'', N'', N'/api/platform/datas/{id}/items', N'POST,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30025,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'178', N'{}', N'9fae5da5d4634eaab7a59d643a8248a7', N'1335118903200337920', N'【平台服务】- 管理数据字典项目', N'/api/platform/datas/{id}/items/{name}', N'', N'', N'/api/platform/datas/{id}/items/{name}', N'DELETE,PUT,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30025,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'179', N'{}', N'70d6120e560f41c3879ec4549636cdb8', N'1336230645078921216', N'【平台服务】- 获取所有菜单', N'/api/platform/menus/all', N'', N'', N'/api/platform/menus/all', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30025,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'180', N'{}', N'c67549cd6cd246f08d9f6ce93c906c21', N'1337314809113722880', N'【平台服务】- 布局', N'/api/platform/layouts', N'', N'', N'/api/platform/layouts', N'GET,POST,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30025,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'181', N'{}', N'bacca946a092434cb2ee5994c88f2c33', N'1337314938973569024', N'【平台服务】- 管理布局', N'/api/platform/layouts/{id}', N'', N'', N'/api/platform/layouts/{id}', N'GET,DELETE,PUT,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30025,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'182', N'{}', N'741fdd4cc05a4025a9073a2e1ab9d5ea', N'1340961907637243904', N'【平台服务】- 获取所有布局', N'/api/platform/layouts/all', N'', N'', N'/api/platform/layouts/all', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30025,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'183', N'{}', N'26a1ef4016704dab8c254f90bc26ecfe', N'1341652247554379776', N'【身份认证服务】- Api范围', N'/api/identity-server/api-scopes', N'', N'', N'/api/identity-server/api-scopes', N'GET,POST,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'184', N'{}', N'3bfc1fef7f5446638f91c9b6e2fb12db', N'1341652385555369984', N'【身份认证服务】- 管理Api范围', N'/api/identity-server/api-scopes/{id}', N'', N'', N'/api/identity-server/api-scopes/{id}', N'GET,PUT,DELETE,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'185', N'{}', N'3288c25dd61e491db95313ca72016918', N'1342457939827552256', N'【身份认证服务】- 持久授权', N'/api/identity-server/persisted-grants', N'', N'', N'/api/identity-server/persisted-grants', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'186', N'{}', N'23f94678093148f58ba842096c3a0e39', N'1342458050112581632', N'【身份认证服务】- 管理持久授权', N'/api/identity-server/persisted-grants/{id}', N'', N'', N'/api/identity-server/persisted-grants/{id}', N'GET,DELETE,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'187', N'{}', N'1f61a3e35b374277ae2c13c671bc5d30', N'1363382062055915520', N'【阿里云】- 阿里云公共配置', N'/api/setting-management/aliyun/by-global', N'', N'', N'/api/setting-management/aliyun/by-global', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30010,', N'', N'', N'aliyun-setting-global', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'188', N'{}', N'46001a60f3d54b85a8417f6af24066e5', N'1363382298501414912', N'【阿里云】- 阿里云租户配置', N'/api/setting-management/aliyun/by-current-tenant', N'', N'', N'/api/setting-management/aliyun/by-current-tenant', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30010,', N'', N'', N'aliyun-setting-current-tenant', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'189', N'{}', N'0d21bec8bcc044bf9000227663039ef4', N'1368854800347848704', N'【Oss对象存储】- 管理容器（Bucket）', N'/api/oss-management/containes/{name}', N'', N'', N'/api/oss-management/containes/{name}', N'POST,GET,DELETE,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30025,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'190', N'{}', N'ca33dc2834e4439d9ca7827fb31c70f8', N'1368855936576413696', N'【Oss对象存储】- 获取容器列表', N'/api/oss-management/containes', N'', N'', N'/api/oss-management/containes', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30025,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'191', N'{}', N'e00fb37d9757471fbd1e64dca92e5d7d', N'1368856295889854464', N'【Oss对象存储】- 获取对象列表', N'/api/oss-management/containes/objects', N'', N'', N'/api/oss-management/containes/objects', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30025,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'192', N'{}', N'cef75a4f7eac43789a3e246229fd5a1b', N'1368856703572008960', N'【Oss对象存储】- 管理Oss对象', N'/api/oss-management/objects', N'', N'', N'/api/oss-management/objects', N'GET,POST,DELETE,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30025,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'193', N'{}', N'ee0d5e6de17c481ca2c0ee5c156b1973', N'1368856819242524672', N'【Oss对象存储】- 上传Oss对象', N'/api/oss-management/objects/upload', N'', N'', N'/api/oss-management/objects/upload', N'POST,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30025,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'194', N'{}', N'6c1a0f98129d418cba55a52dbdd88937', N'1368856927887581184', N'【Oss对象存储】- 批量删除Oss对象', N'/api/oss-management/objects/bulk-delete', N'', N'', N'/api/oss-management/objects/bulk-delete', N'DELETE,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30025,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'195', N'{}', N'de285bf4b6114032a45d5611ea8e3101', N'1368857128383700992', N'【Oss对象存储】- 静态文件管理', N'/api/files/static/{everything}', N'', N'', N'/api/api/files/static/{everything}', N'POST,GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30025,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'196', N'{}', N'6287c30d31c147faae917cb6e52636ec', N'1369560306297233408', N'【Oss对象存储】- 获取公共配置', N'/api/setting-management/oss-management/by-global', N'', N'', N'/api/setting-management/oss-management/by-global', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30025,', N'', N'', N'oss-management-global', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'197', N'{}', N'1012bb902aba47ada9a374fc73ed2632', N'1369560450472239104', N'【Oss对象存储】- 获取租户配置', N'/api/setting-management/oss-management/by-current-tenant', N'', N'', N'/api/setting-management/oss-management/by-current-tenant', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30025,', N'', N'', N'oss-management-current-tenant', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'198', N'{}', N'3ff4788f62744b27bc30f7247fb76b08', N'1371705034307375104', N'【Oss对象存储】- 静态文件', N'/api/files/static/{everything}', N'', N'', N'/api/files/static/{everything}', N'GET,POST,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30025,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'199', N'{}', N'2203b1db040942a3bbe1ed86b57a8501', N'1376442078396276736', N'【本地化管理】- 接口代理', N'/api/abp/api-definition', N'', N'', N'/api/abp/localization/api-definition', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30030,', N'', N'', N'localization-api-definition', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'200', N'{}', N'67206f8a85d94666a0c707697e706742', N'1376442309850554368', N'【本地化管理】- 资源管理', N'/api/localization/resources', N'', N'', N'/api/localization/resources', N'POST,GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30030,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'201', N'{}', N'055929710e414c5e97fc9168a1c25182', N'1376442440536678400', N'【本地化管理】- 管理单个资源', N'/api/localization/resources/{id}', N'', N'', N'/api/localization/resources/{id}', N'GET,PUT,DELETE,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30030,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'202', N'{}', N'c314a94d515045a4a27763aa5d3631de', N'1376442557943635968', N'【本地化管理】- 获取所有资源', N'/api/localization/resources/all', N'', N'', N'/api/localization/resources/all', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30030,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'203', N'{}', N'67fb0a90132c4a02b7ff44f82bf9fd13', N'1376442689674141696', N'【本地化管理】- 语言管理', N'/api/localization/languages', N'', N'', N'/api/localization/languages', N'GET,POST,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30030,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'204', N'{}', N'c6fab01d2b37434886f07bb601c36599', N'1376442971032248320', N'【本地化管理】- 管理单个语言', N'/api/localization/languages/{id}', N'', N'', N'/api/localization/languages/{id}', N'GET,PUT,DELETE,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30030,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'205', N'{}', N'074c20491b294a068fbb1df7874d0d4d', N'1376443123021242368', N'【本地化管理】- 获取所有语言', N'/api/localization/languages/all', N'', N'', N'/api/localization/languages/all', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30030,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'206', N'{}', N'75e48367d8f14ef99541abdb86d30b96', N'1376443238851141632', N'【本地化管理】- 文档管理', N'/api/localization/texts', N'', N'', N'/api/localization/texts', N'GET,POST,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30030,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'207', N'{}', N'5f8d7d88a97f4ff7b3bdb6fb105d527e', N'1376443392249421824', N'【本地化管理】- 管理单个文档', N'/api/localization/texts/{id}', N'', N'', N'/api/localization/texts/{id}', N'GET,PUT,DELETE,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30030,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'208', N'{}', N'35f470892bf04571ab49efadb2cb1eb5', N'1376443586777047040', N'【本地化管理】- 通过文化名称查询文档', N'/api/localization/texts/by-culture-key', N'', N'', N'/api/localization/texts/by-culture-key', N'GET,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30030,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'209', N'{}', N'72073c75c2fa4e1db162c7a74004558f', N'1376467826087682048', N'【身份认证服务】- 发送登录验证码', N'/api/account/phone/send-signin-code', N'', N'', N'/api/account/phone/send-signin-code', N'POST,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'210', N'{}', N'e8e2f943dd56456f8f5dd957dd30e8b9', N'1376467990894469120', N'【身份认证服务】- 发送重置密码短信', N'/api/account/phone/send-password-reset-code', N'', N'', N'/api/account/phone/send-password-reset-code', N'POST,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP'), (N'211', N'{}', N'89fd6b8a830d457f9d1d43263c7074b4', N'1376468110214029312', N'【身份认证服务】- 发送注册短信', N'/api/account/phone/send-register-code', N'', N'', N'/api/account/phone/send-register-code', N'POST,', N'', N'', N'', N'', N'', N'', N'', N'1', N'', N'', N'HTTP', N'127.0.0.1:30015,', N'', N'', N'', N'0', N'30000', N'1', N'', N'TEST-APP')
GO

COMMIT
GO


-- ----------------------------
-- Table structure for appapigatewayroutegroup
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[apigateway].[appapigatewayroutegroup]') AND type IN ('U'))
	DROP TABLE [apigateway].[appapigatewayroutegroup]
GO

CREATE TABLE [apigateway].[appapigatewayroutegroup] (
  [Id] char(36) NOT NULL,
  [ExtraProperties] nvarchar(max) NULL,
  [ConcurrencyStamp] nvarchar(40) NULL,
  [CreationTime] datetime2(0) NOT NULL,
  [CreatorId] char(36) NULL,
  [LastModificationTime] datetime2(0) NULL,
  [LastModifierId] char(36) NULL,
  [IsDeleted] tinyint NOT NULL,
  [DeleterId] char(36) NULL,
  [DeletionTime] datetime2(0) NULL,
  [Name] nvarchar(50) NOT NULL,
  [AppId] nvarchar(50) NOT NULL,
  [AppName] nvarchar(100) NOT NULL,
  [AppIpAddress] nvarchar(256) NOT NULL,
  [Description] nvarchar(256) NULL,
  [IsActive] tinyint NOT NULL
)
GO


-- ----------------------------
-- Records of appapigatewayroutegroup
-- ----------------------------
BEGIN TRANSACTION
GO

INSERT INTO [apigateway].[appapigatewayroutegroup] VALUES (N'08d7f735-a83b-49ab-8cee-5d602502bea8', N'{}', N'83cac848676f4b658d5c9f7d802a497a', N'2020-05-13 20:03:32.524271', NULL, N'2020-08-05 15:43:28.205288', N'bf289dbb-838e-a89b-c622-39f51dcc4f43', N'0', NULL, NULL, N'abp后台管理', N'TEST-APP', N'abp后台管理', N'127.0.0.1', N'abp后台管理项目网关', N'1')
GO

COMMIT
GO


-- ----------------------------
-- Table structure for appapigatewaysecurityoptions
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[apigateway].[appapigatewaysecurityoptions]') AND type IN ('U'))
	DROP TABLE [apigateway].[appapigatewaysecurityoptions]
GO

CREATE TABLE [apigateway].[appapigatewaysecurityoptions] (
  [Id] int NOT NULL,
  [ReRouteId] bigint NOT NULL,
  [IPAllowedList] nvarchar(1000) NULL,
  [IPBlockedList] nvarchar(1000) NULL
)
GO


-- ----------------------------
-- Records of appapigatewaysecurityoptions
-- ----------------------------
BEGIN TRANSACTION
GO

INSERT INTO [apigateway].[appapigatewaysecurityoptions] VALUES (N'3', N'1261299170387169280', N'', N''), (N'4', N'1261585859064872960', N'', N''), (N'5', N'1261586605810368512', N'', N''), (N'6', N'1261587558609436672', N'', N''), (N'7', N'1261588213298348032', N'', N''), (N'8', N'1261588367619375104', N'', N''), (N'9', N'1261588628450557952', N'', N''), (N'10', N'1261588881564221440', N'', N''), (N'11', N'1261588983053795328', N'', N''), (N'12', N'1261589139039961088', N'', N''), (N'13', N'1261589197483393024', N'', N''), (N'14', N'1261589278857084928', N'', N''), (N'15', N'1261589420356124672', N'', N''), (N'16', N'1261589960393736192', N'', N''), (N'17', N'1261606600242085888', N'', N''), (N'18', N'1261606689601732608', N'', N''), (N'21', N'1262220447629058048', N'', N''), (N'22', N'1262230734939758592', N'', N''), (N'23', N'1262296916350869504', N'', N''), (N'24', N'1262632376348594176', N'', N''), (N'25', N'1262632791869902848', N'', N''), (N'28', N'1262660336921235456', N'', N''), (N'29', N'1262660528277966848', N'', N''), (N'30', N'1262660706875625472', N'', N''), (N'31', N'1262660966393991168', N'', N''), (N'32', N'1262661109474283520', N'', N''), (N'33', N'1262663888804663296', N'', N''), (N'34', N'1262664024096133120', N'', N''), (N'35', N'1262664186252120064', N'', N''), (N'36', N'1262664357044178944', N'', N''), (N'37', N'1262664632928718848', N'', N''), (N'38', N'1262664751409418240', N'', N''), (N'39', N'1262664871274237952', N'', N''), (N'40', N'1262665026111164416', N'', N''), (N'41', N'1262665159905267712', N'', N''), (N'42', N'1262665329829105664', N'', N''), (N'43', N'1262665456471920640', N'', N''), (N'44', N'1262665628165754880', N'', N''), (N'45', N'1262666172682883072', N'', N''), (N'47', N'1262723402331885568', N'', N''), (N'48', N'1262935771746734080', N'', N''), (N'49', N'1262935906522304512', N'', N''), (N'52', N'1263074419073593344', N'', N''), (N'53', N'1263075249394790400', N'', N''), (N'54', N'1263075593499684864', N'', N''), (N'56', N'1263101898440146944', N'', N''), (N'57', N'1263303878648569856', N'', N''), (N'58', N'1263304204797648896', N'', N''), (N'59', N'1263304872891555840', N'', N''), (N'60', N'1263305106250047488', N'', N''), (N'61', N'1263305244594970624', N'', N''), (N'62', N'1263305430536855552', N'', N''), (N'63', N'1263639172959174656', N'', N''), (N'64', N'1264799968944640000', N'', N''), (N'65', N'1264800070161584128', N'', N''), (N'66', N'1267360794414161920', N'', N''), (N'67', N'1267383367629807616', N'', N''), (N'68', N'1267817055527632896', N'', N''), (N'69', N'1267817221286526976', N'', N''), (N'70', N'1268893687085518848', N'', N''), (N'94', N'1288657613998579712', N'', N''), (N'95', N'1288657941770854400', N'', N''), (N'96', N'1288658134067109888', N'', N''), (N'97', N'1288658305156964352', N'', N''), (N'98', N'1288658491216289792', N'', N''), (N'99', N'1288658638302142464', N'', N''), (N'100', N'1288658791784308736', N'', N''), (N'101', N'1290849478956199936', N'', N''), (N'102', N'1290849628051124224', N'', N''), (N'103', N'1290849798553776128', N'', N''), (N'105', N'1291259822512693248', N'', N''), (N'114', N'1293470838745821184', N'', N''), (N'115', N'1293471661785706496', N'', N''), (N'116', N'1293472678392721408', N'', N''), (N'117', N'1293472857510473728', N'', N''), (N'118', N'1299273336009359360', N'127.0.0.1', N''), (N'119', N'1299273436282585088', N'127.0.0.1', N''), (N'120', N'1299273618470567936', N'127.0.0.1', N''), (N'121', N'1299273770182737920', N'127.0.0.1', N''), (N'122', N'1299273978023084032', N'127.0.0.1', N''), (N'123', N'1299274123225694208', N'127.0.0.1', N''), (N'124', N'1299274222299348992', N'127.0.0.1', N''), (N'125', N'1304238876758495232', N'', N''), (N'126', N'1304678610343383040', N'', N''), (N'127', N'1304679169305694208', N'', N''), (N'128', N'1310460417141817344', N'', N''), (N'129', N'1310502391475519488', N'', N''), (N'130', N'1310515546943569920', N'', N''), (N'131', N'1310515735292985344', N'', N''), (N'132', N'1316628769783480320', N'', N''), (N'133', N'1316628940663619584', N'', N''), (N'134', N'1316629112428756992', N'', N''), (N'135', N'1316652047017246720', N'', N''), (N'136', N'1316913899996737536', N'', N''), (N'137', N'1319200951383199744', N'', N''), (N'138', N'1319221929807024128', N'', N''), (N'139', N'1319554431134306304', N'', N''), (N'140', N'1319554550458060800', N'', N''), (N'141', N'1319554948434595840', N'', N''), (N'142', N'1319555067183730688', N'', N''), (N'143', N'1319555230765780992', N'', N''), (N'144', N'1319555333790470144', N'', N''), (N'145', N'1321001932510203904', N'', N''), (N'146', N'1321002059803136000', N'', N''), (N'147', N'1321002256440496128', N'', N''), (N'148', N'1321002350686507008', N'', N''), (N'149', N'1322190027988525056', N'', N''), (N'150', N'1322452079688458240', N'', N''), (N'151', N'1322452183929495552', N'', N''), (N'152', N'1322452308651319296', N'', N''), (N'153', N'1322452858176446464', N'', N''), (N'154', N'1322452989235863552', N'', N''), (N'155', N'1322453089655889920', N'', N''), (N'156', N'1329706860249804800', N'', N''), (N'157', N'1329707002411544576', N'', N''), (N'158', N'1329708512277098496', N'', N''), (N'159', N'1329708625917571072', N'', N''), (N'160', N'1335049839287357440', N'', N''), (N'161', N'1335050034221830144', N'', N''), (N'162', N'1335050145899368448', N'', N''), (N'163', N'1335050283434790912', N'', N''), (N'164', N'1335050381770248192', N'', N''), (N'165', N'1335050520941449216', N'', N''), (N'166', N'1335050615829188608', N'', N''), (N'167', N'1335111798720450560', N'', N''), (N'168', N'1335118541370314752', N'', N''), (N'169', N'1335118660417245184', N'', N''), (N'170', N'1335118782727344128', N'', N''), (N'171', N'1335118903200337920', N'', N''), (N'172', N'1336230645078921216', N'', N''), (N'173', N'1337314809113722880', N'', N''), (N'174', N'1337314938973569024', N'', N''), (N'175', N'1340961907637243904', N'', N''), (N'176', N'1341652247554379776', N'', N''), (N'177', N'1341652385555369984', N'', N''), (N'178', N'1342457939827552256', N'', N''), (N'179', N'1342458050112581632', N'', N''), (N'180', N'1363382062055915520', N'', N''), (N'181', N'1363382298501414912', N'', N''), (N'182', N'1368854800347848704', N'', N''), (N'183', N'1368855936576413696', N'', N''), (N'184', N'1368856295889854464', N'', N''), (N'185', N'1368856703572008960', N'', N''), (N'186', N'1368856819242524672', N'', N''), (N'187', N'1368856927887581184', N'', N''), (N'188', N'1368857128383700992', N'', N''), (N'189', N'1369560306297233408', N'', N''), (N'190', N'1369560450472239104', N'', N''), (N'191', N'1371705034307375104', N'', N''), (N'192', N'1376442078396276736', N'', N''), (N'193', N'1376442309850554368', N'', N''), (N'194', N'1376442440536678400', N'', N''), (N'195', N'1376442557943635968', N'', N''), (N'196', N'1376442689674141696', N'', N''), (N'197', N'1376442971032248320', N'', N''), (N'198', N'1376443123021242368', N'', N''), (N'199', N'1376443238851141632', N'', N''), (N'200', N'1376443392249421824', N'', N''), (N'201', N'1376443586777047040', N'', N''), (N'202', N'1376467826087682048', N'', N''), (N'203', N'1376467990894469120', N'', N''), (N'204', N'1376468110214029312', N'', N'')
GO

COMMIT
GO


-- ----------------------------
-- Table structure for cap.published
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[apigateway].[cap.published]') AND type IN ('U'))
	DROP TABLE [apigateway].[cap.published]
GO

CREATE TABLE [apigateway].[cap.published] (
  [Id] bigint NOT NULL,
  [Version] nvarchar(20) NULL,
  [Name] nvarchar(200) NOT NULL,
  [Content] nvarchar(max) NULL,
  [Retries] int NULL,
  [Added] datetime2(0) NOT NULL,
  [ExpiresAt] datetime2(0) NULL,
  [StatusName] nvarchar(40) NOT NULL
)
GO


-- ----------------------------
-- Records of cap.published
-- ----------------------------
BEGIN TRANSACTION
GO

INSERT INTO [apigateway].[cap.published] VALUES (N'1376442080048832512', N'v1', N'LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData', N'{"Headers":{"cap-abp-user-id":"bf289dbb-838e-a89b-c622-39f51dcc4f43","cap-abp-client-id":"vue-admin-element","cap-abp-tenant-id":"","cap-msg-id":"1376442080048832512","cap-corr-id":"1376442080048832512","cap-corr-seq":"0","cap-msg-name":"LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData","cap-msg-type":"Object","cap-senttime":"2021/3/29 下午3:52:19 +08:00"},"Value":{"DateTime":"2021-03-29T15:52:19.593051+08:00","AppId":"TEST-APP","Method":"Create","Object":"ReRoute"}}', N'0', N'2021-03-29 15:52:20', N'2021-03-30 15:52:20', N'Succeeded'), (N'1376442309972189184', N'v1', N'LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData', N'{"Headers":{"cap-abp-user-id":"bf289dbb-838e-a89b-c622-39f51dcc4f43","cap-abp-client-id":"vue-admin-element","cap-abp-tenant-id":"","cap-msg-id":"1376442309972189184","cap-corr-id":"1376442309972189184","cap-corr-seq":"0","cap-msg-name":"LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData","cap-msg-type":"Object","cap-senttime":"2021/3/29 下午3:53:14 +08:00"},"Value":{"DateTime":"2021-03-29T15:53:14.4154232+08:00","AppId":"TEST-APP","Method":"Create","Object":"ReRoute"}}', N'0', N'2021-03-29 15:53:14', N'2021-03-30 15:53:14', N'Succeeded'), (N'1376442440738004992', N'v1', N'LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData', N'{"Headers":{"cap-abp-user-id":"bf289dbb-838e-a89b-c622-39f51dcc4f43","cap-abp-client-id":"vue-admin-element","cap-abp-tenant-id":"","cap-msg-id":"1376442440738004992","cap-corr-id":"1376442440738004992","cap-corr-seq":"0","cap-msg-name":"LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData","cap-msg-type":"Object","cap-senttime":"2021/3/29 下午3:53:45 +08:00"},"Value":{"DateTime":"2021-03-29T15:53:45.5924626+08:00","AppId":"TEST-APP","Method":"Create","Object":"ReRoute"}}', N'0', N'2021-03-29 15:53:46', N'2021-03-30 15:53:46', N'Succeeded'), (N'1376442558023327744', N'v1', N'LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData', N'{"Headers":{"cap-abp-user-id":"bf289dbb-838e-a89b-c622-39f51dcc4f43","cap-abp-client-id":"vue-admin-element","cap-abp-tenant-id":"","cap-msg-id":"1376442558023327744","cap-corr-id":"1376442558023327744","cap-corr-seq":"0","cap-msg-name":"LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData","cap-msg-type":"Object","cap-senttime":"2021/3/29 下午3:54:13 +08:00"},"Value":{"DateTime":"2021-03-29T15:54:13.5549538+08:00","AppId":"TEST-APP","Method":"Create","Object":"ReRoute"}}', N'0', N'2021-03-29 15:54:14', N'2021-03-30 15:54:14', N'Succeeded'), (N'1376442689783193600', N'v1', N'LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData', N'{"Headers":{"cap-abp-user-id":"bf289dbb-838e-a89b-c622-39f51dcc4f43","cap-abp-client-id":"vue-admin-element","cap-abp-tenant-id":"","cap-msg-id":"1376442689783193600","cap-corr-id":"1376442689783193600","cap-corr-seq":"0","cap-msg-name":"LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData","cap-msg-type":"Object","cap-senttime":"2021/3/29 下午3:54:44 +08:00"},"Value":{"DateTime":"2021-03-29T15:54:44.9636224+08:00","AppId":"TEST-APP","Method":"Create","Object":"ReRoute"}}', N'0', N'2021-03-29 15:54:45', N'2021-03-30 15:54:45', N'Succeeded'), (N'1376442971116134400', N'v1', N'LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData', N'{"Headers":{"cap-abp-user-id":"bf289dbb-838e-a89b-c622-39f51dcc4f43","cap-abp-client-id":"vue-admin-element","cap-abp-tenant-id":"","cap-msg-id":"1376442971116134400","cap-corr-id":"1376442971116134400","cap-corr-seq":"0","cap-msg-name":"LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData","cap-msg-type":"Object","cap-senttime":"2021/3/29 下午3:55:52 +08:00"},"Value":{"DateTime":"2021-03-29T15:55:52.0442879+08:00","AppId":"TEST-APP","Method":"Create","Object":"ReRoute"}}', N'0', N'2021-03-29 15:55:52', N'2021-03-30 15:55:52', N'Succeeded'), (N'1376443123109322752', N'v1', N'LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData', N'{"Headers":{"cap-abp-user-id":"bf289dbb-838e-a89b-c622-39f51dcc4f43","cap-abp-client-id":"vue-admin-element","cap-abp-tenant-id":"","cap-msg-id":"1376443123109322752","cap-corr-id":"1376443123109322752","cap-corr-seq":"0","cap-msg-name":"LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData","cap-msg-type":"Object","cap-senttime":"2021/3/29 下午3:56:28 +08:00"},"Value":{"DateTime":"2021-03-29T15:56:28.2817305+08:00","AppId":"TEST-APP","Method":"Create","Object":"ReRoute"}}', N'0', N'2021-03-29 15:56:28', N'2021-03-30 15:56:28', N'Succeeded'), (N'1376443238922444800', N'v1', N'LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData', N'{"Headers":{"cap-abp-user-id":"bf289dbb-838e-a89b-c622-39f51dcc4f43","cap-abp-client-id":"vue-admin-element","cap-abp-tenant-id":"","cap-msg-id":"1376443238922444800","cap-corr-id":"1376443238922444800","cap-corr-seq":"0","cap-msg-name":"LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData","cap-msg-type":"Object","cap-senttime":"2021/3/29 下午3:56:55 +08:00"},"Value":{"DateTime":"2021-03-29T15:56:55.8946549+08:00","AppId":"TEST-APP","Method":"Create","Object":"ReRoute"}}', N'0', N'2021-03-29 15:56:56', N'2021-03-30 15:56:56', N'Succeeded'), (N'1376443392333307904', N'v1', N'LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData', N'{"Headers":{"cap-abp-user-id":"bf289dbb-838e-a89b-c622-39f51dcc4f43","cap-abp-client-id":"vue-admin-element","cap-abp-tenant-id":"","cap-msg-id":"1376443392333307904","cap-corr-id":"1376443392333307904","cap-corr-seq":"0","cap-msg-name":"LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData","cap-msg-type":"Object","cap-senttime":"2021/3/29 下午3:57:32 +08:00"},"Value":{"DateTime":"2021-03-29T15:57:32.4706372+08:00","AppId":"TEST-APP","Method":"Create","Object":"ReRoute"}}', N'0', N'2021-03-29 15:57:32', N'2021-03-30 15:57:32', N'Succeeded'), (N'1376443586886098944', N'v1', N'LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData', N'{"Headers":{"cap-abp-user-id":"bf289dbb-838e-a89b-c622-39f51dcc4f43","cap-abp-client-id":"vue-admin-element","cap-abp-tenant-id":"","cap-msg-id":"1376443586886098944","cap-corr-id":"1376443586886098944","cap-corr-seq":"0","cap-msg-name":"LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData","cap-msg-type":"Object","cap-senttime":"2021/3/29 下午3:58:18 +08:00"},"Value":{"DateTime":"2021-03-29T15:58:18.8481239+08:00","AppId":"TEST-APP","Method":"Create","Object":"ReRoute"}}', N'0', N'2021-03-29 15:58:19', N'2021-03-30 15:58:19', N'Succeeded'), (N'1376467826179956736', N'v1', N'LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData', N'{"Headers":{"cap-abp-user-id":"bf289dbb-838e-a89b-c622-39f51dcc4f43","cap-abp-client-id":"vue-admin-element","cap-abp-tenant-id":"","cap-msg-id":"1376467826179956736","cap-corr-id":"1376467826179956736","cap-corr-seq":"0","cap-msg-name":"LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData","cap-msg-type":"Object","cap-senttime":"2021/3/29 下午5:34:37 +08:00"},"Value":{"DateTime":"2021-03-29T17:34:37.9529723+08:00","AppId":"TEST-APP","Method":"Create","Object":"ReRoute"}}', N'0', N'2021-03-29 17:34:38', N'2021-03-30 17:34:38', N'Succeeded'), (N'1376467990969966592', N'v1', N'LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData', N'{"Headers":{"cap-abp-user-id":"bf289dbb-838e-a89b-c622-39f51dcc4f43","cap-abp-client-id":"vue-admin-element","cap-abp-tenant-id":"","cap-msg-id":"1376467990969966592","cap-corr-id":"1376467990969966592","cap-corr-seq":"0","cap-msg-name":"LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData","cap-msg-type":"Object","cap-senttime":"2021/3/29 下午5:35:17 +08:00"},"Value":{"DateTime":"2021-03-29T17:35:17.2420976+08:00","AppId":"TEST-APP","Method":"Create","Object":"ReRoute"}}', N'0', N'2021-03-29 17:35:17', N'2021-03-30 17:35:17', N'Succeeded'), (N'1376468110281138176', N'v1', N'LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData', N'{"Headers":{"cap-abp-user-id":"bf289dbb-838e-a89b-c622-39f51dcc4f43","cap-abp-client-id":"vue-admin-element","cap-abp-tenant-id":"","cap-msg-id":"1376468110281138176","cap-corr-id":"1376468110281138176","cap-corr-seq":"0","cap-msg-name":"LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData","cap-msg-type":"Object","cap-senttime":"2021/3/29 下午5:35:45 +08:00"},"Value":{"DateTime":"2021-03-29T17:35:45.6880229+08:00","AppId":"TEST-APP","Method":"Create","Object":"ReRoute"}}', N'0', N'2021-03-29 17:35:46', N'2021-03-30 17:35:46', N'Succeeded'), (N'1376516577204494336', N'v1', N'LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData', N'{"Headers":{"cap-abp-user-id":"bf289dbb-838e-a89b-c622-39f51dcc4f43","cap-abp-client-id":"vue-admin-element","cap-abp-tenant-id":"","cap-msg-id":"1376516577204494336","cap-corr-id":"1376516577204494336","cap-corr-seq":"0","cap-msg-name":"LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData","cap-msg-type":"Object","cap-senttime":"2021/3/29 下午8:48:21 +08:00"},"Value":{"DateTime":"2021-03-29T20:48:21.103384+08:00","AppId":"TEST-APP","Method":"Modify","Object":"ReRoute"}}', N'0', N'2021-03-29 20:48:21', N'2021-03-30 20:48:21', N'Succeeded'), (N'1376516622045798400', N'v1', N'LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData', N'{"Headers":{"cap-abp-user-id":"bf289dbb-838e-a89b-c622-39f51dcc4f43","cap-abp-client-id":"vue-admin-element","cap-abp-tenant-id":"","cap-msg-id":"1376516622045798400","cap-corr-id":"1376516622045798400","cap-corr-seq":"0","cap-msg-name":"LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData","cap-msg-type":"Object","cap-senttime":"2021/3/29 下午8:48:31 +08:00"},"Value":{"DateTime":"2021-03-29T20:48:31.7941785+08:00","AppId":"TEST-APP","Method":"Update","Object":"AggregateRoute"}}', N'0', N'2021-03-29 20:48:32', N'2021-03-30 20:48:32', N'Succeeded'), (N'1376516635597594624', N'v1', N'LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData', N'{"Headers":{"cap-abp-user-id":"bf289dbb-838e-a89b-c622-39f51dcc4f43","cap-abp-client-id":"vue-admin-element","cap-abp-tenant-id":"","cap-msg-id":"1376516635597594624","cap-corr-id":"1376516635597594624","cap-corr-seq":"0","cap-msg-name":"LINGYUN.ApiGateway.EventBus.ApigatewayConfigChangeEventData","cap-msg-type":"Object","cap-senttime":"2021/3/29 下午8:48:35 +08:00"},"Value":{"DateTime":"2021-03-29T20:48:35.0253124+08:00","AppId":"TEST-APP","Method":"Update","Object":"AggregateRoute"}}', N'0', N'2021-03-29 20:48:35', N'2021-03-30 20:48:35', N'Succeeded')
GO

COMMIT
GO


-- ----------------------------
-- Table structure for cap.received
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[apigateway].[cap.received]') AND type IN ('U'))
	DROP TABLE [apigateway].[cap.received]
GO

CREATE TABLE [apigateway].[cap.received] (
  [Id] bigint NOT NULL,
  [Version] nvarchar(20) NULL,
  [Name] nvarchar(400) NOT NULL,
  [Group] nvarchar(200) NULL,
  [Content] nvarchar(max) NULL,
  [Retries] int NULL,
  [Added] datetime2(0) NOT NULL,
  [ExpiresAt] datetime2(0) NULL,
  [StatusName] nvarchar(50) NOT NULL
)
GO


-- ----------------------------
-- Primary Key structure for table __efmigrationshistory
-- ----------------------------
ALTER TABLE [apigateway].[__efmigrationshistory] ADD PRIMARY KEY CLUSTERED ([MigrationId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


-- ----------------------------
-- Primary Key structure for table appapigatewayaggregate
-- ----------------------------
ALTER TABLE [apigateway].[appapigatewayaggregate] ADD PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


-- ----------------------------
-- Indexes structure for table appapigatewayaggregateconfig
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_AppApiGatewayAggregateConfig_AggregateReRouteId]
ON [apigateway].[appapigatewayaggregateconfig] (
  [AggregateReRouteId]
)
GO


-- ----------------------------
-- Primary Key structure for table appapigatewayaggregateconfig
-- ----------------------------
ALTER TABLE [apigateway].[appapigatewayaggregateconfig] ADD PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


-- ----------------------------
-- Indexes structure for table appapigatewayauthoptions
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [IX_AppApiGatewayAuthOptions_ReRouteId]
ON [apigateway].[appapigatewayauthoptions] (
  [ReRouteId]
)
GO


-- ----------------------------
-- Primary Key structure for table appapigatewayauthoptions
-- ----------------------------
ALTER TABLE [apigateway].[appapigatewayauthoptions] ADD PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


-- ----------------------------
-- Indexes structure for table appapigatewaybalanceroptions
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [IX_AppApiGatewayBalancerOptions_ItemId]
ON [apigateway].[appapigatewaybalanceroptions] (
  [ItemId]
)
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_AppApiGatewayBalancerOptions_ReRouteId]
ON [apigateway].[appapigatewaybalanceroptions] (
  [ReRouteId]
)
GO


-- ----------------------------
-- Primary Key structure for table appapigatewaybalanceroptions
-- ----------------------------
ALTER TABLE [apigateway].[appapigatewaybalanceroptions] ADD PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


-- ----------------------------
-- Indexes structure for table appapigatewaycacheoptions
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [IX_AppApiGatewayCacheOptions_ReRouteId]
ON [apigateway].[appapigatewaycacheoptions] (
  [ReRouteId]
)
GO


-- ----------------------------
-- Primary Key structure for table appapigatewaycacheoptions
-- ----------------------------
ALTER TABLE [apigateway].[appapigatewaycacheoptions] ADD PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


-- ----------------------------
-- Indexes structure for table appapigatewaydiscovery
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [IX_AppApiGatewayDiscovery_ItemId]
ON [apigateway].[appapigatewaydiscovery] (
  [ItemId]
)
GO


-- ----------------------------
-- Primary Key structure for table appapigatewaydiscovery
-- ----------------------------
ALTER TABLE [apigateway].[appapigatewaydiscovery] ADD PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


-- ----------------------------
-- Indexes structure for table appapigatewaydynamicreroute
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [AK_AppApiGatewayDynamicReRoute_DynamicReRouteId]
ON [apigateway].[appapigatewaydynamicreroute] (
  [DynamicReRouteId]
)
GO


-- ----------------------------
-- Primary Key structure for table appapigatewaydynamicreroute
-- ----------------------------
ALTER TABLE [apigateway].[appapigatewaydynamicreroute] ADD PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


-- ----------------------------
-- Indexes structure for table appapigatewayglobalconfiguration
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [AK_AppApiGatewayGlobalConfiguration_ItemId]
ON [apigateway].[appapigatewayglobalconfiguration] (
  [ItemId]
)
GO


-- ----------------------------
-- Primary Key structure for table appapigatewayglobalconfiguration
-- ----------------------------
ALTER TABLE [apigateway].[appapigatewayglobalconfiguration] ADD PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


-- ----------------------------
-- Primary Key structure for table appapigatewayheaders
-- ----------------------------
ALTER TABLE [apigateway].[appapigatewayheaders] ADD PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


-- ----------------------------
-- Primary Key structure for table appapigatewayhostandport
-- ----------------------------
ALTER TABLE [apigateway].[appapigatewayhostandport] ADD PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


-- ----------------------------
-- Indexes structure for table appapigatewayhttpoptions
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [IX_AppApiGatewayHttpOptions_ItemId]
ON [apigateway].[appapigatewayhttpoptions] (
  [ItemId]
)
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_AppApiGatewayHttpOptions_ReRouteId]
ON [apigateway].[appapigatewayhttpoptions] (
  [ReRouteId]
)
GO


-- ----------------------------
-- Primary Key structure for table appapigatewayhttpoptions
-- ----------------------------
ALTER TABLE [apigateway].[appapigatewayhttpoptions] ADD PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


-- ----------------------------
-- Indexes structure for table appapigatewayqosoptions
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [IX_AppApiGatewayQoSOptions_ItemId]
ON [apigateway].[appapigatewayqosoptions] (
  [ItemId]
)
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_AppApiGatewayQoSOptions_ReRouteId]
ON [apigateway].[appapigatewayqosoptions] (
  [ReRouteId]
)
GO


-- ----------------------------
-- Primary Key structure for table appapigatewayqosoptions
-- ----------------------------
ALTER TABLE [apigateway].[appapigatewayqosoptions] ADD PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


-- ----------------------------
-- Indexes structure for table appapigatewayratelimitoptions
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [IX_AppApiGatewayRateLimitOptions_ItemId]
ON [apigateway].[appapigatewayratelimitoptions] (
  [ItemId]
)
GO


-- ----------------------------
-- Primary Key structure for table appapigatewayratelimitoptions
-- ----------------------------
ALTER TABLE [apigateway].[appapigatewayratelimitoptions] ADD PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


-- ----------------------------
-- Indexes structure for table appapigatewayratelimitrule
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [IX_AppApiGatewayRateLimitRule_DynamicReRouteId]
ON [apigateway].[appapigatewayratelimitrule] (
  [DynamicReRouteId]
)
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_AppApiGatewayRateLimitRule_ReRouteId]
ON [apigateway].[appapigatewayratelimitrule] (
  [ReRouteId]
)
GO


-- ----------------------------
-- Primary Key structure for table appapigatewayratelimitrule
-- ----------------------------
ALTER TABLE [apigateway].[appapigatewayratelimitrule] ADD PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


-- ----------------------------
-- Indexes structure for table appapigatewayreroute
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [AK_AppApiGatewayReRoute_ReRouteId]
ON [apigateway].[appapigatewayreroute] (
  [ReRouteId]
)
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_AppApiGatewayReRoute_AppId_DownstreamPathTemplate_UpstreamPa~]
ON [apigateway].[appapigatewayreroute] (
  [AppId],
  [DownstreamPathTemplate],
  [UpstreamPathTemplate]
)
GO


-- ----------------------------
-- Primary Key structure for table appapigatewayreroute
-- ----------------------------
ALTER TABLE [apigateway].[appapigatewayreroute] ADD PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


-- ----------------------------
-- Indexes structure for table appapigatewayroutegroup
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_AppApiGatewayRouteGroup_AppId_AppName_AppIpAddress]
ON [apigateway].[appapigatewayroutegroup] (
  [AppId],
  [AppName],
  [AppIpAddress]
)
GO


-- ----------------------------
-- Primary Key structure for table appapigatewayroutegroup
-- ----------------------------
ALTER TABLE [apigateway].[appapigatewayroutegroup] ADD PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


-- ----------------------------
-- Indexes structure for table appapigatewaysecurityoptions
-- ----------------------------
CREATE UNIQUE NONCLUSTERED INDEX [IX_AppApiGatewaySecurityOptions_ReRouteId]
ON [apigateway].[appapigatewaysecurityoptions] (
  [ReRouteId]
)
GO


-- ----------------------------
-- Primary Key structure for table appapigatewaysecurityoptions
-- ----------------------------
ALTER TABLE [apigateway].[appapigatewaysecurityoptions] ADD PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


-- ----------------------------
-- Indexes structure for table cap.published
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_ExpiresAt]
ON [apigateway].[cap.published] (
  [ExpiresAt]
)
GO


-- ----------------------------
-- Primary Key structure for table cap.published
-- ----------------------------
ALTER TABLE [apigateway].[cap.published] ADD PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


-- ----------------------------
-- Indexes structure for table cap.received
-- ----------------------------
CREATE NONCLUSTERED INDEX [IX_ExpiresAt]
ON [apigateway].[cap.received] (
  [ExpiresAt]
)
GO


-- ----------------------------
-- Primary Key structure for table cap.received
-- ----------------------------
ALTER TABLE [apigateway].[cap.received] ADD PRIMARY KEY CLUSTERED ([Id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


-- ----------------------------
-- Foreign Keys structure for table appapigatewayaggregateconfig
-- ----------------------------
ALTER TABLE [apigateway].[appapigatewayaggregateconfig] ADD CONSTRAINT [FK_AppApiGatewayAggregateConfig_AppApiGatewayAggregate_Aggregat~] FOREIGN KEY ([AggregateReRouteId]) REFERENCES [apigateway].[appapigatewayaggregate] ([Id])
GO


-- ----------------------------
-- Foreign Keys structure for table appapigatewayauthoptions
-- ----------------------------
ALTER TABLE [apigateway].[appapigatewayauthoptions] ADD CONSTRAINT [FK_AppApiGatewayAuthOptions_AppApiGatewayReRoute_ReRouteId] FOREIGN KEY ([ReRouteId]) REFERENCES [apigateway].[appapigatewayreroute] ([ReRouteId])
GO


-- ----------------------------
-- Foreign Keys structure for table appapigatewaybalanceroptions
-- ----------------------------
ALTER TABLE [apigateway].[appapigatewaybalanceroptions] ADD CONSTRAINT [FK_AppApiGatewayBalancerOptions_AppApiGatewayGlobalConfiguratio~] FOREIGN KEY ([ItemId]) REFERENCES [apigateway].[appapigatewayglobalconfiguration] ([ItemId])
GO

ALTER TABLE [apigateway].[appapigatewaybalanceroptions] ADD CONSTRAINT [FK_AppApiGatewayBalancerOptions_AppApiGatewayReRoute_ReRouteId] FOREIGN KEY ([ReRouteId]) REFERENCES [apigateway].[appapigatewayreroute] ([ReRouteId])
GO


-- ----------------------------
-- Foreign Keys structure for table appapigatewaycacheoptions
-- ----------------------------
ALTER TABLE [apigateway].[appapigatewaycacheoptions] ADD CONSTRAINT [FK_AppApiGatewayCacheOptions_AppApiGatewayReRoute_ReRouteId] FOREIGN KEY ([ReRouteId]) REFERENCES [apigateway].[appapigatewayreroute] ([ReRouteId])
GO


-- ----------------------------
-- Foreign Keys structure for table appapigatewaydiscovery
-- ----------------------------
ALTER TABLE [apigateway].[appapigatewaydiscovery] ADD CONSTRAINT [FK_AppApiGatewayDiscovery_AppApiGatewayGlobalConfiguration_Item~] FOREIGN KEY ([ItemId]) REFERENCES [apigateway].[appapigatewayglobalconfiguration] ([ItemId])
GO


-- ----------------------------
-- Foreign Keys structure for table appapigatewayhttpoptions
-- ----------------------------
ALTER TABLE [apigateway].[appapigatewayhttpoptions] ADD CONSTRAINT [FK_AppApiGatewayHttpOptions_AppApiGatewayGlobalConfiguration_It~] FOREIGN KEY ([ItemId]) REFERENCES [apigateway].[appapigatewayglobalconfiguration] ([ItemId])
GO

ALTER TABLE [apigateway].[appapigatewayhttpoptions] ADD CONSTRAINT [FK_AppApiGatewayHttpOptions_AppApiGatewayReRoute_ReRouteId] FOREIGN KEY ([ReRouteId]) REFERENCES [apigateway].[appapigatewayreroute] ([ReRouteId])
GO


-- ----------------------------
-- Foreign Keys structure for table appapigatewayqosoptions
-- ----------------------------
ALTER TABLE [apigateway].[appapigatewayqosoptions] ADD CONSTRAINT [FK_AppApiGatewayQoSOptions_AppApiGatewayGlobalConfiguration_Ite~] FOREIGN KEY ([ItemId]) REFERENCES [apigateway].[appapigatewayglobalconfiguration] ([ItemId])
GO

ALTER TABLE [apigateway].[appapigatewayqosoptions] ADD CONSTRAINT [FK_AppApiGatewayQoSOptions_AppApiGatewayReRoute_ReRouteId] FOREIGN KEY ([ReRouteId]) REFERENCES [apigateway].[appapigatewayreroute] ([ReRouteId])
GO


-- ----------------------------
-- Foreign Keys structure for table appapigatewayratelimitoptions
-- ----------------------------
ALTER TABLE [apigateway].[appapigatewayratelimitoptions] ADD CONSTRAINT [FK_AppApiGatewayRateLimitOptions_AppApiGatewayGlobalConfigurati~] FOREIGN KEY ([ItemId]) REFERENCES [apigateway].[appapigatewayglobalconfiguration] ([ItemId])
GO


-- ----------------------------
-- Foreign Keys structure for table appapigatewayratelimitrule
-- ----------------------------
ALTER TABLE [apigateway].[appapigatewayratelimitrule] ADD CONSTRAINT [FK_AppApiGatewayRateLimitRule_AppApiGatewayDynamicReRoute_Dynam~] FOREIGN KEY ([DynamicReRouteId]) REFERENCES [apigateway].[appapigatewaydynamicreroute] ([DynamicReRouteId])
GO

ALTER TABLE [apigateway].[appapigatewayratelimitrule] ADD CONSTRAINT [FK_AppApiGatewayRateLimitRule_AppApiGatewayReRoute_ReRouteId] FOREIGN KEY ([ReRouteId]) REFERENCES [apigateway].[appapigatewayreroute] ([ReRouteId])
GO


-- ----------------------------
-- Foreign Keys structure for table appapigatewaysecurityoptions
-- ----------------------------
ALTER TABLE [apigateway].[appapigatewaysecurityoptions] ADD CONSTRAINT [FK_AppApiGatewaySecurityOptions_AppApiGatewayReRoute_ReRouteId] FOREIGN KEY ([ReRouteId]) REFERENCES [apigateway].[appapigatewayreroute] ([ReRouteId])
GO

