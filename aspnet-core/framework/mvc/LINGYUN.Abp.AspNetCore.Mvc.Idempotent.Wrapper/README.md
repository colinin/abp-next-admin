# LINGYUN.Abp.AspNetCore.Mvc.Idempotent.Wrapper

MVC 接口幂等性包装器模块, 启用包装器模块后, 写入校验失败的请求头

## 配置使用

```csharp
[DependsOn(typeof(AbpAspNetCoreMvcIdempotentWrapperModule))]
public class YouProjectModule : AbpModule
{

}
```
## 配置项说明

## 其他

