# LINGYUN.Abp.Dapr.Actors.IdentityModel.Web

Dapr.Actors内部使用Http进行服务间调用,此模块用于获取应用当前状态中的身份令牌并传递到远程Actor服务  

## 配置使用

模块按需引用

```csharp
[DependsOn(typeof(AbpDaprActorsIdentityModelWebModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```
## 配置项说明


## 其他
