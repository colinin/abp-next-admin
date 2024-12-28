# LINGYUN.Platform.Domain.Shared

平台管理模块的共享领域层，定义了平台管理所需的基本类型、枚举和常量。

## 功能特性

* 定义平台管理基础枚举
* 定义平台管理常量
* 定义平台管理本地化资源
* 定义平台管理领域共享接口

## 模块引用

```csharp
[DependsOn(typeof(PlatformDomainSharedModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 基本用法

1. 引用模块
2. 使用定义的枚举和常量
3. 使用本地化资源

## 更多

更多信息请参考 [Platform](../README.md)
