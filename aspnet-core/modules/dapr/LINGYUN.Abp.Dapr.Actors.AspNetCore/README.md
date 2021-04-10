# LINGYUN.Abp.Dapr.Actors.AspNetCore

Dapr.Asp.NetCore的Abp框架集成,扫描程序集内部实现的Actor服务列表,批量注册为Dapr.Actors  

## 配置使用

模块按需引用

```csharp
[DependsOn(typeof(AbpDaprActorsAspNetCoreModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```
## 配置项说明


## 其他
