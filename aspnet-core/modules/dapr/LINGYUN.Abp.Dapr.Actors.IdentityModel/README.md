# LINGYUN.Abp.Dapr.Actors.IdentityModel

Dapr.Actors内部使用Http进行服务间调用,此模块用于传递服务间调用令牌  

## 配置使用

模块按需引用

```csharp
[DependsOn(typeof(AbpDaprActorsIdentityModelModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```
## 配置项说明


## 其他
