# LINGYUN.Abp.IdentityServer.Session

IdentityServer集成模块用户会话扩展,通过IdentityServer暴露的事件接口处理用户会话  

## 参考实现

* [Session Management](https://github.com/abpio/abp-commercial-docs/blob/dev/en/modules/identity/session-management.md#identitysessioncleanupoptions)

## 配置使用

```csharp
[DependsOn(typeof(AbpIdentityServerSessionModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```
