# LINGYUN.Abp.Localization.Json

## 模块说明

本地化组件的Json本地文件系统集成，Abp内置组件仅集成了基于IVirtualFileProvider的实现  

此组件基于PhysicalFileProvider  

### 基础模块  

### 高阶模块  

### 权限定义  

### 功能定义  

### 配置定义  

### 如何使用


```csharp

    [DependsOn(
       typeof(AbpLocalizationJsonModule))]
    public class YouProjectModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<YouResource>("en")
                    // 一般配置在宿主项目中, 写入宿主项目中存储json文件的绝对路径(受PhysicalFileProvider的限制)
                    // json文件格式与Abp默认json格式保持一致
                    // 详情见: https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.fileproviders.physicalfileprovider?view=dotnet-plat-ext-5.0
                    .AddPhysicalJson(Path.Combine(Directory.GetCurrentDirectory(), "Resources"));
            });
        }
    }

```

```json

{
  "culture": "en",
  "texts": {
    "Hello China": "Hello China!"
  }
}

```

### 更新日志 
