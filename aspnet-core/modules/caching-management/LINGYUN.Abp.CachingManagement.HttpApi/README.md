# LINGYUN.Abp.CachingManagement.HttpApi

缓存管理模块的 HTTP API 实现。

## API 接口

### /api/caching-management/cache

* GET `/api/caching-management/cache/keys`: 获取缓存键列表
  * 参数：
    * prefix (string, optional): 键前缀
    * filter (string, optional): 过滤条件
    * marker (string, optional): 分页标记
  * 权限：AbpCachingManagement.Cache

* GET `/api/caching-management/cache/{key}`: 获取指定键的缓存值
  * 参数：
    * key (string, required): 缓存键
  * 权限：AbpCachingManagement.Cache

* POST `/api/caching-management/cache`: 设置缓存值
  * 参数：
    * key (string, required): 缓存键
    * value (string, required): 缓存值
    * absoluteExpiration (DateTime, optional): 绝对过期时间
    * slidingExpiration (DateTime, optional): 滑动过期时间
  * 权限：AbpCachingManagement.Cache.ManageValue

* PUT `/api/caching-management/cache/refresh`: 刷新缓存
  * 参数：
    * key (string, required): 缓存键
    * absoluteExpiration (DateTime, optional): 绝对过期时间
    * slidingExpiration (DateTime, optional): 滑动过期时间
  * 权限：AbpCachingManagement.Cache.Refresh

* DELETE `/api/caching-management/cache`: 删除缓存
  * 参数：
    * key (string, required): 缓存键
  * 权限：AbpCachingManagement.Cache.Delete

## 安装

```bash
abp add-module LINGYUN.Abp.CachingManagement
```

## 更多

有关更多信息，请参阅以下资源：

* [应用服务实现](../LINGYUN.Abp.CachingManagement.Application/README.md)
* [应用服务契约](../LINGYUN.Abp.CachingManagement.Application.Contracts/README.md)
* [领域层](../LINGYUN.Abp.CachingManagement.Domain/README.md)
