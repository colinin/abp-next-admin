# LINGYUN.Abp.Localization.Persistence

## 模块说明

本地化组件持久层模块, 引用模块可将需要的本地化文档持久化到存储设施  

### 基础模块  

### 高阶模块  

### 权限定义  

### 功能定义  

### 配置定义  

### 如何使用


```csharp

    [DependsOn(
        typeof(AbpLocalizationPersistenceModule))]
    public class YouProjectModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationPersistenceOptions>(options =>
            {
                // 启用持久化设施
                options.SaveStaticLocalizationsToPersistence = true;

                // 指定你的本地化资源类型, 此类型下定义的静态文档将被持久化到存储设施
                options.AddPersistenceResource<YouProjectResource>();
            });

            // 或者使用扩展方法持久化本地化资源类型
            Configure<AbpLocalizationOptions>(options =>
            {
                // 效果如上
                options.UsePersistence<YouProjectResource>();
            });
        }
    }

```

### 更新日志 
