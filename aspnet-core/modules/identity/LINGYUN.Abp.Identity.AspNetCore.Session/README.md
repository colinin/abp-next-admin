# LINGYUN.Abp.Identity.AspNetCore.Session

用户会话登录扩展模块,主要处理 *AspNetCore.Identity* 提供的登录/登出事件来管理会话   

出于模块职责分离原则, 请勿与 *LINGYUN.Abp.Identity.Session.AspNetCore* 模块混淆  

## 配置使用

```csharp
[DependsOn(typeof(AbpIdentityAspNetCoreSessionModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```
