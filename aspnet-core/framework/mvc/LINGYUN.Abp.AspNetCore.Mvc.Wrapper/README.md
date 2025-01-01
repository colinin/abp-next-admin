# LINGYUN.Abp.AspNetCore.Mvc.Wrapper

包装器 MVC 实现模块，用于统一包装ASP.NET Core MVC的响应结果。

[English](./README.EN.md)

## 功能特性

* 自动包装MVC响应结果
* 支持自定义包装规则
* 支持异常结果包装
* 支持本地化错误消息
* 支持API文档包装描述
* 支持忽略特定控制器、命名空间、返回类型的包装

## 安装

```bash
dotnet add package LINGYUN.Abp.AspNetCore.Mvc.Wrapper
```

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
            
            // 忽略特定返回类型
            options.IgnoreReturnTypes.Add<IRemoteStreamContent>();
            
            // 忽略特定控制器
            options.IgnoreControllers.Add<AbpApiDefinitionController>();
            
            // 忽略特定URL前缀
            options.IgnorePrefixUrls.Add("/connect");
            
            // 自定义空结果消息
            options.MessageWithEmptyResult = (serviceProvider) =>
            {
                var localizer = serviceProvider.GetRequiredService<IStringLocalizer<AbpMvcWrapperResource>>();
                return localizer["Wrapper:NotFound"];
            };
        });
    }
}
```

## 配置项说明

* `IsEnabled`: 是否启用包装器
* `IsWrapUnauthorizedEnabled`: 是否包装未授权响应
* `HttpStatusCode`: 包装响应的HTTP状态码
* `IgnoreReturnTypes`: 忽略包装的返回类型列表
* `IgnoreControllers`: 忽略包装的控制器列表
* `IgnoreNamespaces`: 忽略包装的命名空间列表
* `IgnorePrefixUrls`: 忽略包装的URL前缀列表
* `IgnoreExceptions`: 忽略包装的异常类型列表
* `ErrorWithEmptyResult`: 空结果是否返回错误信息
* `CodeWithEmptyResult`: 空结果的错误代码
* `MessageWithEmptyResult`: 空结果的错误消息
* `CodeWithSuccess`: 成功时的代码

## 高级用法

### 1. 使用特性标记忽略包装

```csharp
[IgnoreWrapResult]
public class TestController : AbpController
{
    // 该控制器下所有Action都不会被包装
}

public class TestController : AbpController
{
    [IgnoreWrapResult]
    public IActionResult Test()
    {
        // 该Action不会被包装
    }
}
```

### 2. 通过请求头控制包装

* 添加 `_AbpDontWrapResult` 请求头可以禁用包装
* 添加 `_AbpWrapResult` 请求头可以强制启用包装

## 源码位置

[LINGYUN.Abp.AspNetCore.Mvc.Wrapper](https://github.com/colinin/abp-next-admin/tree/master/aspnet-core/framework/mvc/LINGYUN.Abp.AspNetCore.Mvc.Wrapper)
