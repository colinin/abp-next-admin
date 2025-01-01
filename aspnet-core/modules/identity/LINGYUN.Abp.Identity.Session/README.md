# LINGYUN.Abp.Identity.Session

用户会话基础模块,提供相关的通用接口   


## 功能特性

* 提供用户会话管理的基础接口
* 提供会话缓存与持久化同步机制
* 支持会话访问时间追踪
* 依赖AbpCachingModule模块

## 配置使用

```csharp
[DependsOn(typeof(AbpIdentitySessionModule))]
public class YouProjectModule : AbpModule
{
	public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<IdentitySessionCheckOptions>(options =>
        {
            // 设置会话缓存与持久化设施刷新间隔为10分钟
            options.KeepAccessTimeSpan = TimeSpan.FromMinutes(10);
        });
    }
}
```

## 配置项

### IdentitySessionCheckOptions

```json
{
  "Identity": {
    "Session": {
      "Check": {
        "KeepAccessTimeSpan": "00:01:00",    // 保持访问时长（刷新缓存会话间隔），默认：1分钟
        "SessionSyncTimeSpan": "00:10:00"    // 会话同步间隔（从缓存同步到持久化），默认：10分钟
      }
    }
  }
}
```

## 更多信息

* [ABP身份认证文档](https://docs.abp.io/en/abp/latest/Identity)
