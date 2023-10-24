# LINGYUN.Abp.Wrapper

包装器模块  

## 配置使用

```csharp
[DependsOn(typeof(AbpWrapperModule))]
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

*	AbpWrapperOptions.IsEnabled						是否包装返回结果,默认: false  
*	AbpWrapperOptions.CodeWithUnhandled				出现未处理异常时的返回错误代码，默认500  
*	AbpWrapperOptions.CodeWithSuccess				处理成功返回代码，默认0  
*	AbpWrapperOptions.ErrorWithEmptyResult			请求资源时，如果资源为空是否返回错误消息，默认false    
*	AbpWrapperOptions.HttpStatusCode				包装后的Http响应代码, 默认: 200
*	AbpWrapperOptions.CodeWithEmptyResult			当返回空对象时返回错误代码，默认: 404  
*	AbpWrapperOptions.MessageWithEmptyResult		当返回空对象时返回错误消息, 默认: Not Found  

*	AbpWrapperOptions.IgnorePrefixUrls				指定哪些Url开头的地址不需要处理  
*	AbpWrapperOptions.IgnoreNamespaces				指定哪些命名空间开头不需要处理  
*	AbpWrapperOptions.IgnoreControllers				指定哪些控制器不需要处理  
*	AbpWrapperOptions.IgnoreReturnTypes				指定哪些返回结果类型不需要处理  
*	AbpWrapperOptions.IgnoreExceptions				指定哪些异常类型不需要处理  
*	AbpWrapperOptions.IgnoredInterfaces				指定哪些接口不需要处理（默认实现**IWrapDisabled**接口不进行处理）  


## 其他

