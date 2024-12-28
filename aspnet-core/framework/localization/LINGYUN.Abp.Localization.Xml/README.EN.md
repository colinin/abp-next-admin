# LINGYUN.Abp.Localization.Xml

## Module Description

XML document integration module for localization components, providing XML file-based localization resource support. It includes built-in implementations for both PhysicalFileProvider and VirtualFileProvider.

## Features

* Support reading localization resources from XML files
* Support XML files in virtual file system
* Support XML files in physical file system
* Support XML file serialization and deserialization
* Support UTF-8 encoded XML files

## Installation

```bash
dotnet add package LINGYUN.Abp.Localization.Xml
```

## Base Modules

* Volo.Abp.Localization

## Usage

1. Add module dependency:

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
                // Virtual file system directory in current project
                // See: https://docs.abp.io/en/abp/latest/Virtual-File-System
                .AddVirtualXml("/LINGYUN/Abp/Localization/Xml/Resources")
                // Usually configured in the host project, write the absolute path where XML files are stored
                // See: https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.fileproviders.physicalfileprovider
                .AddPhysicalXml(Path.Combine(Directory.GetCurrentDirectory(), "Resources"));
        });
    }
}
```

## XML File Format

This module uses the [XmlLocalizationFile](./LINGYUN/Abp/Localization/Xml/XmlLocalizationFile.cs) type for XML file serialization and deserialization.

Example XML file format:

```xml
<?xml version="1.0" encoding="utf-8"?>
<localization>
    <culture name="en"/>
    <texts>
        <text name="Welcome">Welcome</text>
        <text name="HelloWorld">Hello, World!</text>
        <text name="ThisFieldIsRequired">This field is required</text>
    </texts>
</localization>
```

## Extension Methods

The module provides two extension methods for adding XML localization resources:

1. AddVirtualXml: Add XML files from virtual file system
```csharp
localizationResource.AddVirtualXml("/YourVirtualPath/Localization");
```

2. AddPhysicalXml: Add XML files from physical file system
```csharp
localizationResource.AddPhysicalXml("C:/YourPath/Localization");
```

## Best Practices

1. Recommended usage for virtual files:
   * Embed XML files into the assembly
   * Suitable for default localization resources that don't need dynamic modification

2. Recommended usage for physical files:
   * Store in a specific directory in the host project
   * Suitable for localization resources that need dynamic modification or are managed by external systems

## More Information

* [中文文档](./README.md)
* [ABP Localization Documentation](https://docs.abp.io/en/abp/latest/Localization)
* [ABP Virtual File System](https://docs.abp.io/en/abp/latest/Virtual-File-System)
