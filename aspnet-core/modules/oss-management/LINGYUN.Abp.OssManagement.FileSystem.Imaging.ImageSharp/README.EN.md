# LINGYUN.Abp.OssManagement.FileSystem.Imaging.ImageSharp

ImageSharp image processing implementation module for local file system

## Features

* Implements image processing functionality for local file system based on ImageSharp
* Implements IImageProcessor interface
* Provides high-performance image processing capabilities
* Supports multiple image formats
* Provides rich image processing options

## Configuration

Module reference as needed:

```csharp
[DependsOn(typeof(AbpOssManagementFileSystemImagingImageSharpModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

## Image Processing Features

Supports the following image processing operations:
* Scaling: Supports multiple scaling modes and algorithms
* Cropping: Supports rectangular and circular cropping
* Rotation: Supports rotation at any angle
* Watermark: Supports text and image watermarks
* Format Conversion: Supports conversion between multiple image formats
* Quality Adjustment: Supports compression quality control

## Performance Optimization

* Uses ImageSharp's high-performance algorithms
* Supports image processing cache
* Supports asynchronous processing
* Supports memory optimization

## Notes

* Requires installation of ImageSharp related NuGet packages
* Recommended to configure appropriate image processing limits
* Recommended to enable image caching
* Pay attention to memory usage management

## Links

* [中文文档](./README.md)
* [Module documentation](../README.md)
