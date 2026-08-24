# 10.4.0 - 10.6.0

## 模块变更

1. [功能管理模块]   功能分组/定义列表从仓储接口直接查询,避免受缓存影响.[PR #1511](https://github.com/colinin/abp-next-admin/pull/1511)   
2. [功能管理模块]   重构FeatureDefinitionManager,实现合并内置功能与动态功能定义.[PR #1559](https://github.com/colinin/abp-next-admin/pull/1559)  
3. [权限管理模块]   权限分组/定义列表从仓储接口直接查询,避免受缓存影响.[PR #1511](https://github.com/colinin/abp-next-admin/pull/1511)  
4. [权限管理模块]   重构PermissionDefinitionManager,实现合并内置权限与动态权限定义.[PR #1559](https://github.com/colinin/abp-next-admin/pull/1559)  
5. [权限管理模块]   修复MySql资源权限唯一索引重复.[PR #1526](https://github.com/colinin/abp-next-admin/pull/1526)  
6. [通知管理模块]   通知分组/定义列表从仓储接口直接查询,避免受缓存影响.[PR #1511](https://github.com/colinin/abp-next-admin/pull/1511)  
7. [通知管理模块]   重构NotificationDefinitionManager,增加合并、忽略、覆盖三种策略.[PR #1559](https://github.com/colinin/abp-next-admin/pull/1559)    
8. [通知管理模块]   修复微信小程序通知内容序列化无效.[PR #1530](https://github.com/colinin/abp-next-admin/pull/1530)    
9. [设置管理模块]   设置定义列表从仓储接口直接查询,避免受缓存影响.[PR #1511](https://github.com/colinin/abp-next-admin/pull/1511)  
10. [设置管理模块]   重构SettingDefinitionManager,实现合并内置设置与动态设置定义.[PR #1559](https://github.com/colinin/abp-next-admin/pull/1559)  
11. [Webhook管理模块]   Webhook分组/定义列表从仓储接口直接查询,避免受缓存影响.[PR #1511](https://github.com/colinin/abp-next-admin/pull/1511)  
12. [Webhook管理模块]   重构WebhookDefinitionManager,增加合并、忽略、覆盖三种策略.[PR #1559](https://github.com/colinin/abp-next-admin/pull/1559)    
13. [AI管理模块]   重构WorkspaceDefinitionManager,增加合并、忽略、覆盖三种策略.[PR #1516](https://github.com/colinin/abp-next-admin/pull/1516)   
14. [AI管理模块]   重构AIToolDefinitionManager,增加合并、忽略、覆盖三种策略.[PR #1516](https://github.com/colinin/abp-next-admin/pull/1516)   
15. [OpenIddict模块]   重写MySql仓储Token批量清理删除接口,修复运行时错误.[PR #1512](https://github.com/colinin/abp-next-admin/pull/1512)  
16. [本地化管理模块]   资源/语言/本地化文本列表从仓储接口直接查询,避免受缓存影响.[PR #1517](https://github.com/colinin/abp-next-admin/pull/1517)  
17. [OpenIddict模块]   增加模拟用户登录实现.[PR #1525](https://github.com/colinin/abp-next-admin/pull/1525)    
18. [OpenIddict模块]   增加模拟租户用户登录实现.[PR #1525](https://github.com/colinin/abp-next-admin/pull/1525)    
19. [反向代理模块]   用户配置文件重写反向代理服务器/地址白名单配置.[PR #1532](https://github.com/colinin/abp-next-admin/pull/1532)    
20. [身份认证模块]   用户密码变更后使用户会话失效(自行修改密码时当前会话不受影响).[PR #1543](https://github.com/colinin/abp-next-admin/pull/1543)    
21. [多租户UI模块]   优化租户选择弹窗组件,已定义平台租户关系时获取平台企业列表作为选择租户.[PR #1538](https://github.com/colinin/abp-next-admin/pull/1538)    
22. [Elasticsearch模块]   增加表达式树翻译ES查询功能.[PR #1552](https://github.com/colinin/abp-next-admin/pull/1552)    
23. [系统日志模块]   重构系统日志列表查询为通过ES表达式树查询.[PR #1558](https://github.com/colinin/abp-next-admin/pull/1558)    
24. [审计日志模块]   重构审计日志列表查询为通过ES表达式树查询.[PR #1558](https://github.com/colinin/abp-next-admin/pull/1558)    

## 依赖项变更  

| 库名称                                       | 原版本    | 现版本 |
| -------------------------------------------- | --------- | ------ |
| AlibabaCloud.SDK.Dingtalk					   |    	   | 2.2.50 |
| BouncyCastle.Cryptography					   | 2.6.2     | 2.7.0  |
| IP2Region.Net								   | 3.0.0	   | 3.0.2  |
| Magicodes.IE.Excel    					   | 2.8.2	   | 2.9.0  |
| MailKit			    					   |    	   | 4.17.0 |
| Markdig			    					   | 0.44.0    | 1.3.2  |
| MimeCheck			    					   | 1.0.0     | 2.0.0  |
| MiniExcel			    					   | 1.43.0    | 1.45.0 |
| Microsoft.AspNetCore.*					   | 10.0.7	   | 10.0.9 |
| Microsoft.Bcl.AsyncInterfaces			       | 10.0.7	   | 10.0.9 |
| Microsoft.Data.Sqlite.Core                   | 10.0.7    | 10.0.9 |
| Microsoft.Data.SqlClient	                   | 6.1.1     | 7.0.2  |
| Microsoft.EntityFrameworkCore.*			   | 10.0.7	   | 10.0.9 |
| Microting.EntityFrameworkCore.MySql          | 10.0.7    | 10.0.9 |
| Microsoft.Extensions.*					   | 10.0.7    | 10.0.9 |
| Microsoft.IdentityModel.Tokens			   | 8.14.0    | 8.19.1 |
| MySqlConnector							   | 2.4.0     | 2.6.1  |
| OpenTelemetry.Exporter.Console               | 1.15.3    | 1.17.0 |
| OpenTelemetry.Exporter.OpenTelemetryProtocol | 1.15.3    | 1.17.3 |
| OpenTelemetry.Exporter.Zipkin                | 1.15.3    | 1.17.0 |
| OpenTelemetry.Extensions.Hosting             | 1.15.3    | 1.17.0 |
| OpenTelemetry.Instrumentation.AspNetCore     | 1.15.2    | 1.17.0 |
| OpenTelemetry.Instrumentation.EntityFrameworkCore     | 1.14.0-beta.2    | 1.17.0-beta.1 |
| OpenTelemetry.Instrumentation.Http	       | 1.15.1    | 1.17.0 |
| OpenTelemetry.Instrumentation.Quartz	       | 1.15.1-beta.1    | 1.17.0-beta.1 |
| OpenTelemetry.Instrumentation.Runtime	       | 1.15.1    | 1.17.0 |
| OpenTelemetry.Instrumentation.SqlClient      | 1.15.2    | 1.17.0 |
| Oracle.ManagedDataAccess.Core			       | 23.6.1    | 23.26.300 |
| QRCoder									   | 1.5.1     | 1.8.0  |
| RulesEngine								   | 5.0.5     | 6.0.0  |
| Scriban   								   | 7.0.0     | 7.2.5  |
| Serilog.Settings.Configuration               | 10.0.0    | 10.0.1 |
| Serilog.Sinks.InMemory		               |		   | 2.0.0  |
| Swashbuckle.AspNetCore                       | 10.0.1    | 10.2.3 |
| System.Security.Cryptography.Pkcs            |           | 10.0.9 |
| System.Threading.Channels                    | 10.0.7    | 10.0.9 |
| Volo.Abp.*                                   | 10.4.0    | 10.6.0 |
| Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite | 5.4.0     | 5.6.0  |



## 数据库迁移

| Project Name             | Database Provider | Scripts                                        | EF Core Version |
| ------------------------ | ----------------- | ---------------------------------------------- | --------------- |
| services/Single          | MySQL             | 20260626034342_Fix-MySql-Index-Key-Too-Lang-For-Resource-Permission-Grant		| 10.0.7          |
| services/BackendAdmin      | MySQL             | 20260626032421_Fix-MySql-Index-Key-Too-Lang-For-Resource-Permission-Grant		| 10.0.7          |


## 数据库连接字符串

| Module Name                   | ConnectionString Name        | Example(MySQL)                                               |
| ----------------------------- | ---------------------------- | ------------------------------------------------------------ |
| Default                       | Default                      | Server=127.0.0.1;Database=abp;User Id=root;Password=123456;SslMode=None |
| abp/Identity                  | AbpIdentity                  | Server=127.0.0.1;Database=abp;User Id=root;Password=123456;SslMode=None |
| abp/IdentityServer            | AbpIdentityServer            | Server=127.0.0.1;Database=abp;User Id=root;Password=123456;SslMode=None |
| abp/OpenIddict                | AbpOpenIddict                | Server=127.0.0.1;Database=abp;User Id=root;Password=123456;SslMode=None |
| abp/TenantManagement          | AbpTenantManagement(AbpSaas) | Server=127.0.0.1;Database=abp;User Id=root;Password=123456;SslMode=None |
| abp/FeatureManagement         | AbpFeatureManagement         | Server=127.0.0.1;Database=abp;User Id=root;Password=123456;SslMode=None |
| abp/SettingManagement         | AbpSettingManagement         | Server=127.0.0.1;Database=abp;User Id=root;Password=123456;SslMode=None |
| abp/PermissionManagement      | AbpPermissionManagement      | Server=127.0.0.1;Database=abp;User Id=root;Password=123456;SslMode=None |
| labp/AIManagement             | AbpAIManagement              | Server=127.0.0.1;Database=abp;User Id=root;Password=123456;SslMode=None |
| labp/BlobManagement           | AbpBlobManagement            | Server=127.0.0.1;Database=abp;User Id=root;Password=123456;SslMode=None |
| labp/DataProtectionManagement | AbpDataProtectionManagement  | Server=127.0.0.1;Database=abp;User Id=root;Password=123456;SslMode=None |
| labp/Gdpr                     | AbpGdpr                      | Server=127.0.0.1;Database=abp;User Id=root;Password=123456;SslMode=None |
| labp/Saas                     | AbpSaas                      | Server=127.0.0.1;Database=abp;User Id=root;Password=123456;SslMode=None |
| labp/LocalizationManagement   | AbpLocalizationManagement    | Server=127.0.0.1;Database=abp;User Id=root;Password=123456;SslMode=None |
| labp/TextTemplating           | AbpTextTemplating            | Server=127.0.0.1;Database=abp;User Id=root;Password=123456;SslMode=None |
| labp/Platform                 | AppPlatform                  | Server=127.0.0.1;Database=abp;User Id=root;Password=123456;SslMode=None |
| labp/MessageService           | MessageService               | Server=127.0.0.1;Database=abp;User Id=root;Password=123456;SslMode=None |
| labp/NotificationService      | Notifications                | Server=127.0.0.1;Database=abp;User Id=root;Password=123456;SslMode=None |
| labp/TaskManagement           | TaskManagement               | Server=127.0.0.1;Database=abp;User Id=root;Password=123456;SslMode=None |
| labp/WebhooksManagement       | WebhooksManagement           | Server=127.0.0.1;Database=abp;User Id=root;Password=123456;SslMode=None |
| labp/Elsa                     | Workflow                     | Server=127.0.0.1;Database=abp;User Id=root;Password=123456;SslMode=None |
