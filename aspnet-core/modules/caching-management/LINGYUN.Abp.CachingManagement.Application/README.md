# LINGYUN.Abp.CachingManagement.Application

缓存管理应用服务模块的实现。

## 功能

* 实现了 `ICacheAppService` 接口，提供缓存管理的基本功能：
  * 获取缓存键列表
  * 获取缓存值
  * 设置缓存值
  * 刷新缓存
  * 删除缓存

## 权限

* AbpCachingManagement.Cache：缓存管理
  * AbpCachingManagement.Cache.Refresh：刷新缓存
  * AbpCachingManagement.Cache.Delete：删除缓存
  * AbpCachingManagement.Cache.ManageValue：管理缓存值

## 安装

```bash
abp add-module LINGYUN.Abp.CachingManagement
```

## 配置使用

1. 首先需要安装相应的缓存实现模块，例如：[LINGYUN.Abp.CachingManagement.StackExchangeRedis](../LINGYUN.Abp.CachingManagement.StackExchangeRedis/README.md)

2. 在模块的 `ConfigureServices` 方法中添加以下代码：

```csharp
Configure<AbpAutoMapperOptions>(options =>
{
    options.AddProfile<AbpCachingManagementApplicationAutoMapperProfile>(validate: true);
});
```

## 更多

有关更多信息，请参阅以下资源：

* [应用服务契约](../LINGYUN.Abp.CachingManagement.Application.Contracts/README.md)
* [HTTP API](../LINGYUN.Abp.CachingManagement.HttpApi/README.md)
* [领域层](../LINGYUN.Abp.CachingManagement.Domain/README.md)
