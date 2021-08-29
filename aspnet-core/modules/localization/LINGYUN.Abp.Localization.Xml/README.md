# LINGYUN.Abp.Localization.Xml

## 模块说明

本地化组件的Xml文档集成，内置PhysicalFileProvider与VirtualFileProvider实现  

### 基础模块  

### 高阶模块  

### 权限定义  

### 功能定义  

### 配置定义  

### 如何使用


```csharp

    [DependsOn(
        typeof(AbpLocalizationXmlModule))]
    public class YouProjectModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<YouProjectModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<YouResource>("en")
                    // 当前项目中的虚拟文件系统目录,详情见: https://docs.abp.io/en/abp/latest/Virtual-File-System
                    .AddVirtualXml("/LINGYUN/Abp/Localization/Xml/Resources")
                    // 一般配置在宿主项目中, 写入宿主项目中存储xml文件的绝对路径(受PhysicalFileProvider的限制)
                    // 详情见: https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.fileproviders.physicalfileprovider?view=dotnet-plat-ext-5.0
                    .AddPhysicalXml(Path.Combine(Directory.GetCurrentDirectory(), "Resources"));
            });
        }
    }

```
Xml文件格式如下  

序列化: [XmlLocalizationFile](./LINGYUN/Abp/Localization/Xml/XmlLocalizationFile.cs)  类型实现  

```xml

<?xml version="1.0" encoding="utf-8"?>
<localization xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
 <culture name="en" />
 <texts>
  <text key="Hello China" value="Hello China!" />
  <text key="Welcome" value="Welcome!" />
 </texts>
</localization>

```

### 更新日志 
