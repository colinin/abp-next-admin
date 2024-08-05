# LINGYUN.Abp.Identity.Session.AspNetCore

身份服务用户会话扩展模块

## 接口描述

### AbpSessionMiddleware 在请求管道中从用户令牌提取 *sessionId* 作为全局会话标识, 可用于注销会话  
  注意: 当匿名用户访问时, 以请求 *CorrelationId* 作为标识;
        当 *CorrelationId* 不存在时, 使用随机 *Guid.NewGuid()*.

### HttpContextDeviceInfoProvider 从请求参数中提取设备标识  

出于模块职责分离原则, 请勿与 *LINGYUN.Abp.Identity.AspNetCore.Session* 模块混淆  

## 配置使用

```csharp
[DependsOn(typeof(AbpIdentitySessionAspNetCoreModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```
