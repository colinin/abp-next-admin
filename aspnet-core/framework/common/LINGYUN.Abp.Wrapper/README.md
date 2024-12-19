# LINGYUN.Abp.Wrapper

包装器模块，用于统一包装API返回结果和异常处理。

## 功能

* 统一的返回结果包装
* 灵活的异常处理机制
* 支持多种忽略策略
* 可配置的空结果处理
* 自定义异常处理器

## 安装

```bash
dotnet add package LINGYUN.Abp.Wrapper
```

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
            
            // 自定义未处理异常的错误代码
            options.CodeWithUnhandled = "500";
            
            // 忽略特定前缀的URL
            options.IgnorePrefixUrls.Add("/api/health");
            
            // 添加自定义异常处理器
            options.AddHandler<CustomException>(new CustomExceptionHandler());
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


## 使用示例

### 1. 基本使用

```csharp
public class TestController : AbpController
{
    public async Task<WrapResult<string>> GetAsync()
    {
        return new WrapResult<string>("0", "Hello World");
    }
}
```

### 2. 忽略包装

```csharp
[IgnoreWrapResult]
public class HealthController : AbpController
{
    public async Task<string> GetAsync()
    {
        return "OK";
    }
}
```

### 3. 自定义异常处理

```csharp
public class CustomExceptionHandler : IExceptionWrapHandler
{
    public void Wrap(ExceptionWrapContext context)
    {
        context.WithCode("CUSTOM_ERROR")
               .WithMessage("发生自定义异常")
               .WithDetails(context.Exception.Message);
    }
}
```

## 链接

* [English document](./README.EN.md)
