# LINGYUN.Abp.AspNetCore.Mvc.Idempotent

MVC 接口幂等性检查模块

## 配置使用

```csharp
[DependsOn(typeof(AbpAspNetCoreMvcIdempotentModule))]
public class YouProjectModule : AbpModule
{
	public override void ConfigureServices(ServiceConfigurationContext context)
	{
		Configure<AbpMvcIdempotentOptions>(options =>
		{
			// 例如: 对 DELETE 请求方法进行幂等校验
			options.SupportedMethods.Add("DELETE");
        });
	}
}
```
## 配置项说明

## 其他

