# LINGYUN.Abp.CachingManagement.Domain

缓存管理模块的领域层实现。

## 核心接口

### ICacheManager

缓存管理器接口，定义了缓存管理的核心功能：

* `GetKeysAsync`: 获取缓存键列表
* `GetValueAsync`: 获取缓存值
* `SetAsync`: 设置缓存值
* `RefreshAsync`: 刷新缓存
* `RemoveAsync`: 删除缓存

## 领域服务

### CacheManager

缓存管理器的抽象基类，提供了基础的缓存管理实现。具体的缓存提供程序（如Redis）需要继承此类并实现相应的方法。

## 安装

```bash
abp add-module LINGYUN.Abp.CachingManagement
```

## 扩展开发

如果需要支持新的缓存提供程序，需要：

1. 创建新的项目，继承 `CacheManager` 类
2. 实现所有抽象方法
3. 注册为 `ICacheManager` 的实现（使用 `[Dependency(ReplaceServices = true)]`）

## 更多

有关更多信息，请参阅以下资源：

* [应用服务实现](../LINGYUN.Abp.CachingManagement.Application/README.md)
* [应用服务契约](../LINGYUN.Abp.CachingManagement.Application.Contracts/README.md)
* [HTTP API](../LINGYUN.Abp.CachingManagement.HttpApi/README.md)
* [Redis实现](../LINGYUN.Abp.CachingManagement.StackExchangeRedis/README.md)
