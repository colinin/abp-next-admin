# LINGYUN.Abp.Identity.Session

用户会话基础模块,提供相关的通用接口   


## 配置使用

```csharp
[DependsOn(typeof(AbpIdentitySessionModule))]
public class YouProjectModule : AbpModule
{
	public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<IdentitySessionCheckOptions>(options =>
        {
            // 设置会话缓存与数据库刷新间隔为10分钟
            options.KeepAccessTimeSpan = TimeSpan.FromMinutes(10);
        });
    }
}
```
