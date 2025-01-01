# LINGYUN.Abp.CachingManagement.Application.Contracts

缓存管理应用服务契约模块。

## 接口

### ICacheAppService

缓存管理应用服务接口，提供以下功能：

* `GetKeysAsync`: 获取缓存键列表
* `GetValueAsync`: 获取缓存值
* `SetAsync`: 设置缓存值
* `RefreshAsync`: 刷新缓存
* `RemoveAsync`: 删除缓存

## 权限

* AbpCachingManagement.Cache：缓存管理
  * AbpCachingManagement.Cache.Refresh：刷新缓存
  * AbpCachingManagement.Cache.Delete：删除缓存
  * AbpCachingManagement.Cache.ManageValue：管理缓存值

## 安装

```bash
abp add-module LINGYUN.Abp.CachingManagement
```

## 更多

有关更多信息，请参阅以下资源：

* [应用服务实现](../LINGYUN.Abp.CachingManagement.Application/README.md)
* [HTTP API](../LINGYUN.Abp.CachingManagement.HttpApi/README.md)
* [领域层](../LINGYUN.Abp.CachingManagement.Domain/README.md)
