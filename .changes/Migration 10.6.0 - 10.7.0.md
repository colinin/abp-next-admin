# 10.6.0 - 10.7.0

## 模块变更

1. [动态定义]   新增 LINGYUN.Abp.Dynamic.Definitions 动态定义模块,以统一的机制承载功能/权限/设置/通知/Webhook/AI 的定义扩展.
2. [功能管理]   重写 ABP 默认的 FeatureDefinitionManager,实现内置定义与动态定义的合并、忽略、覆盖三种策略.
3. [权限管理]   重写 PermissionDefinitionManager,实现合并、忽略、覆盖三种策略.
4. [设置管理]   重写 SettingDefinitionManager,实现合并、忽略、覆盖三种策略.
5. [通知管理]   重写 NotificationDefinitionManager,实现合并、忽略、覆盖三种策略.
6. [Webhook管理]   重写 WebhookDefinitionManager,实现合并、忽略、覆盖三种策略.
7. [AI管理]   重写 WorkspaceDefinitionManager 与 AIToolDefinitionManager,实现合并、忽略、覆盖三种策略.
8. [定义管理]   功能/权限/设置定义变更后触发本地事件,便于订阅方刷新缓存.
9. [定义管理]   优化定义管理页面与权限列表样式.
10. [账号模块]   增强用户安全:创建用户时校验双因素策略、登录时绑定双因素认证器、注册时校验双因素与邮箱确认、登录时校验邮箱确认.
11. [账号模块]   集成图片验证码(LazyCaptcha)与腾讯云验证码,覆盖登录、注册与验证码校验接口.
12. [账号模块]   重构登录页面,增加密码显示/隐藏、页面内容标题、"返回登录"按钮样式等交互.
13. [账号模块]   用户注册改为通过邮件确认;完善验证码与恢复代码的校验流程.
14. [身份模块]   增加用户操作安全日志记录,补齐验证码相关安全日志.
15. [身份模块]   重构会话模块,增强会话校验安全性,用户密码变更后使用户会话失效.
16. [身份模块]   admin 角色强制启用双因素认证,且禁止被设置为默认角色.
17. [身份模块]   提高安全相关设置的默认值.
18. [个人中心]   增加绑定信息校验器;同步 Identity 接口的 HttpClientProxy.
19. [审计日志]   审计日志不再记录租户连接字符串.
20. [OpenIddict模块]   增加 JsonWebKeySet 支持,并优化 OpenIddict 管理功能与身份令牌映射.
21. [OpenIddict模块]   企业微信授权拆分为网页授权与小程序两条授权流程(wecom_oauth / wecom_mp),并新增小程序临时登录凭证换取会话的实现.
22. [OpenIddict模块]   二维码登录改为使用二维码自身携带的租户,不再接受请求参数 tenant_id;并细化二维码登录的安全日志动作.
23. [IdentityServer模块]   企业微信授权校验器同步拆分为网页授权与小程序两个校验器.
24. [系统日志]   新增 OpenObserve 日志查询提供者,支持以 OpenObserve 替代 Elasticsearch 查询系统日志.
25. [审计日志]   新增基于 OpenObserve 的审计日志存储:审计日志、安全日志、实体变更、操作分流入不同数据流,并按租户分流.
26. [审计日志]   新增审计日志与安全日志的信息贡献者扩展点,允许模块在不替换存储实现的前提下补充日志信息.
27. [日志模块]   Elasticsearch 与 OpenObserve 日志提供者内置 Serilog 增强器(ApplicationName / UniqueId).
28. [系统日志]   审计/安全日志的索引名称启用滚动策略.
29. [OpenTelemetry模块]   增加 Redis 追踪仪表,移除已弃用的 Zipkin 导出器配置.
30. [IP定位]   优化 IP 地址地理位置的解析与名称本地化.
31. [IP定位]   位置备注规则改为可配置(按国家/省份移除),由独立的配置节绑定.
32. [IP定位]   所有服务、网关与 Aspire 宿主启用审计日志的 IP 位置解析.
33. [IP定位]   MaxMind GeoIP2 离线库改为由使用方在部署时自行提供,模块不再内置分发;库文件缺失时回退空解读器并记录警告.
34. [IP定位]   各宿主的默认位置解析提供者由 MaxMind GeoIP2 切换为 IP2Region(内置离线库).
35. [通知模块]   抽象模板卡片消息支持 JSON 往返:新增 Newtonsoft 与 System.Text.Json 两个转换器,按 card_type 还原具体卡片类型.
36. [通知模块]   通知数据扩展按消息类型拆分为多个分部文件,并补充保密消息、ID转译、重复消息检查等开关.
37. [通知模块]   新增按渠道声明通知载荷的机制,属性包按渠道前缀划分命名空间承载,渠道新增字段无需变更通知表结构.
38. [通知模块]   优化渠道载荷属性元数据的构建,去除全局锁并改为延迟初始化.
39. [TuiJuhe模块]   通知发布契约统一为 NotificationPublishContext(与其他渠道保持一致).
40. [Blob管理]   修复本地文件系统存储提供者的路径穿越漏洞,新增跨平台的名称与路径校验规则.
41. [Blob管理]   文件存在性检查改为按容器标识过滤,移除容器表关联查询.
42. [部署]   容器编排中为各服务开启审计日志的 IP 位置解析与备注移除规则;补充容器端口与运行配置说明.
43. [CLI]   新增 blob-migration 命令,用于从旧版本地文件系统存储迁移文件到 BlobManagement.
44. [CLI]   改进 blob-migration:补齐参数提示,指定目录不存在时告警并终止操作.
45. [项目结构]   微服务工程统一重命名为 LINGYUN.Abp.MicroService.*,单应用工程更名为 LINGYUN.Abp.MicroService.AllInOne.
46. [项目结构]   移除历史遗留的 IdentityServer 与 AIO 单体应用宿主工程.
47. [项目结构]   新增 AI 微服务工程及其数据迁移工程,并在 Aspire 编排中增加对应宿主.
48. [项目结构]   从解决方案中摘除 v1 版 VueVbenAdmin 平台模块(导航/设置/主题),保留 v5 版本导航.
49. [部署]   优化 Dockerfile、Docker Compose 与镜像构建脚本.
50. [模板]   微服务模板升级到 10.7.0,同步打包、织入与日志配置,并为模板 Host 新增 appsettings.Production.json.
51. [构建]   包输出与是否生成包改为按条件控制;新增 abppkg 打包内容与客户端代理描述文件自动内嵌;ConfigureAwait 织入仅在 Release 生效,并移除各工程冗余的 FodyWeavers 配置文件.

## 依赖项变更

| 库名称                                                  | 原版本          | 现版本        |
| ------------------------------------------------------- | --------------- | ------------- |
| AlibabaCloud.SDK.Captcha20230305                        |                 | 1.1.4         |
| Duende.IdentityModel                                    |                 | 7.1.0         |
| IdentityModel                                           | 7.0.0           | (移除)        |
| Lazy.Captcha.Core                                       |                 | 2.2.5         |
| MaxMind.GeoIP2                                          |                 | 6.1.0         |
| Microsoft.AspNetCore.*                                  | 10.0.9          | 10.0.11       |
| Microsoft.Bcl.AsyncInterfaces                           | 10.0.9          | 10.0.11       |
| Microsoft.Data.Sqlite.Core                              | 10.0.9          | 10.0.11       |
| Microsoft.EntityFrameworkCore.*                         | 10.0.9          | 10.0.11       |
| Microsoft.Extensions.*                                  | 10.0.9          | 10.0.11       |
| Microsoft.IdentityModel.*                               | 8.19.1          | 8.19.2        |
| Microsoft.SourceLink.GitHub                             |                 | 8.0.0         |
| Microting.EntityFrameworkCore.MySql                     | 10.0.9          | 10.0.11       |
| MySql.EntityFrameworkCore                               | 10.0.1          | 10.0.9        |
| Npgsql                                                  | 10.0.0          | 10.0.3        |
| OpenIddict.Server.DataProtection                        | 7.5.0           | 7.7.0         |
| OpenIddict.Validation.DataProtection                    | 7.5.0           | 7.7.0         |
| OpenTelemetry.Exporter.Console                          | 1.17.0          | 1.19.0        |
| OpenTelemetry.Exporter.OpenTelemetryProtocol            | 1.17.0          | 1.19.0        |
| OpenTelemetry.Exporter.Zipkin                           | 1.17.0          | 1.19.0        |
| OpenTelemetry.Extensions.Hosting                        | 1.17.0          | 1.19.0        |
| OpenTelemetry.Instrumentation.AspNetCore                | 1.17.0          | 1.19.0        |
| OpenTelemetry.Instrumentation.EntityFrameworkCore       | 1.17.0-beta.1   | 1.19.0-beta.1 |
| OpenTelemetry.Instrumentation.Http                      | 1.17.0          | 1.19.0        |
| OpenTelemetry.Instrumentation.Quartz                    | 1.17.0-beta.1   | 1.19.0-beta.1 |
| OpenTelemetry.Instrumentation.Runtime                   | 1.17.0          | 1.19.0        |
| OpenTelemetry.Instrumentation.SqlClient                 | 1.17.0          | 1.19.0        |
| OpenTelemetry.Instrumentation.StackExchangeRedis        |                 | 1.19.0-beta.1 |
| System.IdentityModel.Tokens.Jwt                         | 8.19.1          | 8.19.2        |
| System.Security.Cryptography.Pkcs                       | 10.0.9          | 10.0.11       |
| System.Threading.Channels                               | 10.0.9          | 10.0.11       |
| Volo.Abp.*                                              | 10.6.0          | 10.7.0        |
| Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite            | 5.6.0           | 5.7.0         |
| aliyun-net-sdk-core                                     | 1.6.2           | (移除)        |

## 数据库迁移

| Project Name       | Database Provider | Scripts                              | EF Core Version |
| ------------------ | ----------------- | ------------------------------------ | --------------- |
| services/AIService | MySQL             | 20261007004411_Initial-AI-Service    | 10.0.11         |

> 说明:本版本新增 AI 微服务,其数据迁移工程为 `LINGYUN.Abp.MicroService.AIService.EntityFrameworkCore` / `.DbMigrator`,连接字符串名为 `AIServiceDbMigrator`.
>
> 其余数据库无新增迁移脚本,仅迁移工程名随本版本的工程重命名而变化:原 `LY.*` 系列统一为 `LINGYUN.Abp.MicroService.*`,`services/Single` 更名为 `LINGYUN.Abp.MicroService.AllInOne`,原 `services/BackendAdmin` 已随旧版宿主一并移除.升级时请按新的工程名执行迁移,历史数据无需结构变更.

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

> 本版本未新增或改名连接字符串键,配置结构与 10.6.0 一致;上表按模块归并,项目名称请按本版本的工程命名对应.

## 升级注意事项

1. **MaxMind GeoIP2 的破坏性变更**:模块不再内置 `GeoLite2-City.mmdb`,公开常量 `AbpMaxMindGeoIP2Module.DefaultDatabaseFile` 已移除.如需继续使用,请自行下载 GeoLite2 离线库并配置 `MaxMindGeoIP2:DatabaseFile`(或环境变量 `MaxMindGeoIP2__DatabaseFile`);未配置时位置解析返回空结果并记录警告.各宿主默认已切换为 IP2Region,通常无需额外处理.

2. **企业微信授权类型变更**:授权类型由 `wecom` 改为 `wecom_oauth`,并新增小程序授权类型 `wecom_mp`.旧客户端若仍在发送 `grant_type=wecom` 将认证失败,需同步升级.同时 `WeChatWorkTokenExtensionGrant` / `WeChatWorkGrantValidator` 已改为抽象类.

3. **二维码登录不再接受 `tenant_id`**:租户信息取自二维码自身,请求参数中的 `tenant_id` 会被忽略(不报错).此前依赖该参数指定租户的调用方需移除该参数.

4. **Fody 织入配置变更**:`ConfigureAwait` 织入仅在 Release 配置生效,并通过公共属性文件内联 `WeaverConfiguration`;各工程原有的 `FodyWeavers.xml` / `.xsd` 已移除.若有自定义 Fody 配置,请改为在工程内显式声明.

5. **打包行为变更**:`GeneratePackageOnBuild` 改为按条件控制(不再对所有 `LINGYUN*` 程序集生效),并新增 `*.abppkg` 内容打包与 `HttpApi.Client` 程序集自动内嵌 `*generate-proxy.json`.如依赖旧的打包产物路径,请确认 `LocalNuget` 输出.
