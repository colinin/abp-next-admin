# LINGYUN.Abp.Http.Client.Wrapper

HTTP客户端包装器模块，用于在HTTP客户端请求中自动添加包装器请求头。

[English](./README.EN.md)

## 功能特性

* 自动添加包装器请求头
* 与ABP HTTP客户端集成
* 支持全局配置包装器开关

## 安装

```bash
dotnet add package LINGYUN.Abp.Http.Client.Wrapper
```

## 配置使用

```csharp
[DependsOn(typeof(AbpHttpClientWrapperModule))]
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

## 工作原理

当启用包装器时（`AbpWrapperOptions.IsEnabled = true`），模块会自动为所有HTTP客户端请求添加 `_AbpWrapResult` 请求头。
当禁用包装器时（`AbpWrapperOptions.IsEnabled = false`），模块会自动为所有HTTP客户端请求添加 `_AbpDontWrapResult` 请求头。

这样可以确保HTTP客户端的请求结果与服务器端的包装配置保持一致。

## 源码位置

[LINGYUN.Abp.Http.Client.Wrapper](https://github.com/colinin/abp-next-admin/tree/master/aspnet-core/framework/common/LINGYUN.Abp.Http.Client.Wrapper)
