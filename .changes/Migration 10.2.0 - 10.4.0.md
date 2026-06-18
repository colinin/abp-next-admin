# 10.2.0 - 10.4.0

## 模块变更

1. [身份标识模块]   增加不活跃用户通知/清理作业,实现了三个通知模板(账户保持活跃通知、账户停用通知、账户删除通知)
2. [身份标识模块]   增加不活跃用户实体 `IdentityUserInactive` 需要执行数据库迁移  
3. [身份标识模块]   增加用户登录Url设置定义: `Abp.Identity.Link.UserLoginUri`, 用于在外部系统中关联用户登录页    
4. [身份认证模块]   增加用户登录Url定义: `Abp.Account.UserLogin`    
5. [身份认证模块]   增加关联用户登录功能, 可以实现跨租户用户关联, 切换关联用户登录      
6. [设置管理模块]   增加身份标识模块缺少的设置项: 启用防止密码重用、防止密码重用数量 
7. [设置基础模块]   增加设置定义分组、所属设置、优先级、依赖权限、依赖功能、选择项等常用结构扩展  
8. [设置管理模块]   重构设置管理应用服务接口, 通过预设定的设置定义分组、上下级关系等自动构成Dto结构

## 依赖项变更  

| 库名称                                       | 原版本    | 现版本 |
| -------------------------------------------- | --------- | ------ |
| Microsoft.AspNetCore.*					   | 10.0.2	   | 10.0.7 |
| Microsoft.Bcl.AsyncInterfaces			       | 10.0.2	   | 10.0.7 |
| Microsoft.Data.Sqlite.Core                   | 10.0.0    | 10.0.7 |
| Microsoft.EntityFrameworkCore.*			   | 10.0.2	   | 10.0.7 |
| Microting.EntityFrameworkCore.MySql          | 10.0.3    | 10.0.7 |
| Microsoft.Extensions.*					   | 10.0.2    | 10.0.7 |
| OpenTelemetry.Extensions.Hosting             | 1.14.0    | 1.15.3 |
| OpenTelemetry.Exporter.Console               | 1.14.0    | 1.15.3 |
| OpenTelemetry.Exporter.OpenTelemetryProtocol | 1.14.0    | 1.15.3 |
| OpenTelemetry.Exporter.Zipkin                | 1.14.0    | 1.15.3 |
| OpenTelemetry.Instrumentation.AspNetCore     | 1.14.0    | 1.15.2 |
| OpenTelemetry.Instrumentation.Http	       | 1.14.0    | 1.15.1 |
| OpenTelemetry.Instrumentation.Quartz	       | 1.14.0-beta.2    | 1.15.1-beta.1 |
| OpenTelemetry.Instrumentation.Runtime	       | 1.14.0    | 1.15.1 |
| OpenTelemetry.Instrumentation.SqlClient      | 1.14.0-beta.1    | 1.15.2 |
| OpenIddict.Server.DataProtection             | 7.2.0     | 7.5.0  |
| OpenIddict.Validation.DataProtection         | 7.2.0     | 7.5.0  |
| System.Threading.Channels                    | 10.0.0    | 10.0.7 |
| Volo.Abp.*                                   | 10.2.0    | 10.4.0 |
| Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite | 5.2.0     | 5.4.0  |



## 数据库迁移

| Project Name             | Database Provider | Scripts                                        | EF Core Version |
| ------------------------ | ----------------- | ---------------------------------------------- | --------------- |
| aspire/AuthServer        | PostgreSQL        | 20260530060327_Add-Identity-User-Inactive		| 10.0.2          |
| services/Single          | MySQL             | 20260529063257_Add-Identity-User-Inactive		| 10.0.3          |
| services/Single          | PostgreSQL        | 20260530055941_Add-Identity-User-Inactive		| 10.0.2          |
| services/Single          | SqlServer         | 20260530060009_Add-Identity-User-Inactive      | 10.0.2          |
| services/AuthServer      | MySQL             | 20260530061141_Add-Identity-User-Inactive		| 10.0.3          |


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
