# LINGYUN.Abp.Core

## 介绍

`LINGYUN.Abp.Core` 是一个基础核心模块，提供了一些通用的功能和扩展。

## 功能

* 动态选项提供者 (`DynamicOptionsProvider<TValue>`)
  * 简化需要在使用配置前自行调用接口的繁复步骤
  * 支持延迟加载配置值
  * 提供一次性运行机制，确保配置只被加载一次

## 安装

```bash
dotnet add package LINGYUN.Abp.Core
```

## 使用

1. 添加 `[DependsOn(typeof(AbpCommonModule))]` 到你的模块类上。

```csharp
[DependsOn(typeof(AbpCommonModule))]
public class YourModule : AbpModule
{
    // ...
}
```

2. 使用动态选项提供者：

```csharp
public class YourOptionsProvider : DynamicOptionsProvider<YourOptions>
{
    public YourOptionsProvider(IOptions<YourOptions> options) 
        : base(options)
    {
    }
}
```

## 链接

* [English document](./README.EN.md)
