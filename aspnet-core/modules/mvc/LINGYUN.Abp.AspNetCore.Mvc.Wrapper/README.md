# LINGYUN.Abp.AspNetCore.Mvc.Wrapper

返回值包装器

## 配置使用

```csharp
[DependsOn(typeof(AbpAspNetCoreMvcWrapperModule))]
public class YouProjectModule : AbpModule
{
	public override void ConfigureServices(ServiceConfigurationContext context)
	{
		Configure<AbpAspNetCoreMvcWrapperOptions>(options =>
		{
			// 启用包装器
			options.IsEnabled = true;
        });
	}
}
```
## 配置项说明

*	AbpAspNetCoreMvcWrapperOptions.IsEnabled						是否包装返回结果,默认: false  
*	AbpAspNetCoreMvcWrapperOptions.CodeWithFound					响应成功代码,默认: 0  
*	AbpAspNetCoreMvcWrapperOptions.HttpStatusCode					包装后的Http响应代码, 默认: 200
*	AbpAspNetCoreMvcWrapperOptions.CodeWithEmptyResult				当返回空对象时返回错误代码，默认: 404  
*	AbpAspNetCoreMvcWrapperOptions.MessageWithEmptyResult			当返回空对象时返回错误消息, 默认: 本地化之后的 NotFound  

*	AbpAspNetCoreMvcWrapperOptions.IgnorePrefixUrls					指定哪些Url开头的地址不需要处理  
*	AbpAspNetCoreMvcWrapperOptions.IgnoreNamespaces					指定哪些命名空间开头不需要处理  
*	AbpAspNetCoreMvcWrapperOptions.IgnoreControllers				指定哪些控制器不需要处理  
*	AbpAspNetCoreMvcWrapperOptions.IgnoreReturnTypes				指定哪些返回结果类型不需要处理  
*	AbpAspNetCoreMvcWrapperOptions.IgnoreExceptions					指定哪些异常类型不需要处理  


## 其他

