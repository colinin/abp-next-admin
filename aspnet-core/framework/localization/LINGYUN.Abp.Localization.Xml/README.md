# LINGYUN.Abp.Localization.Xml

## 模块说明

本地化组件的XML文档集成模块，提供基于XML文件的本地化资源支持。内置了物理文件提供程序（PhysicalFileProvider）和虚拟文件提供程序（VirtualFileProvider）的实现。

## 功能特性

* 支持从XML文件读取本地化资源
* 支持虚拟文件系统中的XML文件
* 支持物理文件系统中的XML文件
* 支持XML文件的序列化和反序列化
* 支持UTF-8编码的XML文件

## 安装

```bash
dotnet add package LINGYUN.Abp.Localization.Xml
```

## 基础模块

* Volo.Abp.Localization

## 使用方法

1. 添加模块依赖：

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
                // 当前项目中的虚拟文件系统目录
                // 详情见: https://docs.abp.io/zh-Hans/abp/latest/Virtual-File-System
                .AddVirtualXml("/LINGYUN/Abp/Localization/Xml/Resources")
                // 一般配置在宿主项目中, 写入宿主项目中存储xml文件的绝对路径
                // 详情见: https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.fileproviders.physicalfileprovider
                .AddPhysicalXml(Path.Combine(Directory.GetCurrentDirectory(), "Resources"));
        });
    }
}
```

## XML文件格式

本模块使用 [XmlLocalizationFile](./LINGYUN/Abp/Localization/Xml/XmlLocalizationFile.cs) 类型来序列化和反序列化XML文件。

示例XML文件格式：

```xml
<?xml version="1.0" encoding="utf-8"?>
<localization>
    <culture name="zh-Hans"/>
    <texts>
        <text name="Welcome">欢迎</text>
        <text name="HelloWorld">你好，世界！</text>
        <text name="ThisFieldIsRequired">这是必填字段</text>
    </texts>
</localization>
```

## 扩展方法

模块提供了两个扩展方法来添加XML本地化资源：

1. AddVirtualXml：添加虚拟文件系统中的XML文件
```csharp
localizationResource.AddVirtualXml("/YourVirtualPath/Localization");
```

2. AddPhysicalXml：添加物理文件系统中的XML文件
```csharp
localizationResource.AddPhysicalXml("C:/YourPath/Localization");
```

## 最佳实践

1. 虚拟文件推荐用法：
   * 将XML文件嵌入到程序集中
   * 适用于默认的、不需要动态修改的本地化资源

2. 物理文件推荐用法：
   * 存放在宿主项目的特定目录中
   * 适用于需要动态修改或由外部系统管理的本地化资源

## 更多信息

* [English Documentation](./README.EN.md)
* [ABP本地化文档](https://docs.abp.io/zh-Hans/abp/latest/Localization)
* [ABP虚拟文件系统](https://docs.abp.io/zh-Hans/abp/latest/Virtual-File-System)
