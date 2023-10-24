# LINGYUN.Abp.AspNetCore.Mvc.Wrapper

包装器 MVC 实现模块

## 配置使用

```csharp
[DependsOn(typeof(AbpAspNetCoreMvcWrapperModule))]
public class YouProjectModule : AbpModule
{
	public override void ConfigureServices(ServiceConfigurationContext context)
	{
		Configure<AbpWrapperOptions>(options =>
		{
			// 启用包装器
			options.IsEnabled = true;
        });
	}
}
```
## 配置项说明

## 其他

