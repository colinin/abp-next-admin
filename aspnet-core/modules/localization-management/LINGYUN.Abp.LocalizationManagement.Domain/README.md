# LINGYUN.Abp.LocalizationManagement.Domain

本地化管理领域层模块，实现了动态本地化资源存储和管理功能。

## 功能特性

* 实现 `ILocalizationStore` 接口，提供本地化资源的存储和检索功能
* 支持语言管理（增删改查）
* 支持资源管理（增删改查）
* 支持文本管理（增删改查）
* 支持本地化资源的内存缓存
* 支持分布式缓存同步

## 模块引用

```csharp
[DependsOn(typeof(AbpLocalizationManagementDomainModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

```json
{
  "LocalizationManagement": {
    "LocalizationCacheStampTimeOut": "00:02:00",  // 本地化缓存时间戳超时时间，默认2分钟
    "LocalizationCacheStampExpiration": "00:30:00" // 本地化缓存过期时间，默认30分钟
  }
}
```

## 领域服务

* `LanguageManager`: 语言管理服务，提供语言的创建、更新、删除等功能
* `ResourceManager`: 资源管理服务，提供资源的创建、更新、删除等功能
* `TextManager`: 文本管理服务，提供文本的创建、更新、删除等功能
* `LocalizationStore`: 本地化存储服务，实现了 `ILocalizationStore` 接口
* `LocalizationStoreInMemoryCache`: 本地化资源内存缓存服务

## 更多信息

* [English documentation](./README.EN.md)
