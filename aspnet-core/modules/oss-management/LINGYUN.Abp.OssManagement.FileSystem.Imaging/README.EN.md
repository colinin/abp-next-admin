# LINGYUN.Abp.OssManagement.FileSystem.Imaging

Base image processing module for local file system

## Features

* Provides basic image processing functionality for local file system
* Defines image processing interfaces and abstract classes
* Supports image processing extensions
* Provides image processing pipeline
* Supports image processing cache

## Configuration

Module reference as needed:

```csharp
[DependsOn(typeof(AbpOssManagementFileSystemImagingModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

## Core Interfaces

* IImageProcessor: Image processor interface
* IImageProcessorFactory: Image processor factory interface
* IImageProcessorCache: Image processor cache interface
* IImageProcessorPipeline: Image processing pipeline interface

## Extension Features

* Supports custom image processors
* Supports custom image processing pipelines
* Supports custom image caching strategies
* Supports image processing parameter validation

## Related Modules

* LINGYUN.Abp.OssManagement.FileSystem.Imaging.ImageSharp: ImageSharp-based implementation

## Links

* [中文文档](./README.md)
* [Module documentation](../README.md)
