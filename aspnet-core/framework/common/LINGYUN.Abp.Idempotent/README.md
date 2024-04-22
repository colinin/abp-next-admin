# LINGYUN.Abp.Idempotent

接口幂等性检查模块

## 配置使用

```csharp
[DependsOn(typeof(AbpIdempotentModule))]
public class YouProjectModule : AbpModule
{
	public override void ConfigureServices(ServiceConfigurationContext context)
	{
		Configure<AbpIdempotentOptions>(options =>
		{
			// 全局启用幂等检查
			options.IsEnabled = true;
			// 默认每个接口提供5秒超时
			options.DefaultTimeout = 5000;
			// 幂等token名称, 通过HttpHeader传递
			options.IdempotentTokenName = "X-With-Idempotent-Token";
			// 幂等校验失败时Http响应代码
			options.HttpStatusCode = 429;
        });
	}
}
```
## 配置项说明

## 其他

