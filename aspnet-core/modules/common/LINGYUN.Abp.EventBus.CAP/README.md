# LINGYUN.Abp.EventBus.CAP

分布式事件总线 CAP 集成

#### 注意

* 由于 CAP 官方模块中, MySqlConnector 为高版本,与 Volo.Abp.EntityFrameworkCore.MySQL 依赖版本不兼容  
当 Abp 框架升级到 4.0 版本之后,此模块升级为 CAP 最新版本

## 配置使用

```csharp
[DependsOn(typeof(AbpCAPEventBusModule))]
public class YouProjectModule : AbpModule
{
  // other
}
