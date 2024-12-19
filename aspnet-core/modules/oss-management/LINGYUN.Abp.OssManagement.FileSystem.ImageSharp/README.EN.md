# LINGYUN.Abp.OssManagement.FileSystem.ImageSharp

ImageSharp image processing implementation for local file system

## Features

* Implements image processing for local file system based on ImageSharp
* Supports image format conversion
* Supports image resizing and cropping
* Supports watermark addition
* Supports image quality adjustment
* Supports image metadata processing

## Configuration

Module reference as needed:

```csharp
[DependsOn(typeof(AbpOssManagementFileSystemImageSharpModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

## Image Processing Features

Supports the following image processing operations:
* resize: Adjust image size
* crop: Crop image
* rotate: Rotate image
* watermark: Add watermark
* format: Convert image format
* quality: Adjust image quality

## Usage Example

Image processing parameters are passed through URL query string:

```
http://your-domain/api/oss-management/objects/my-image.jpg?process=image/resize,w_100,h_100/watermark,text_Hello
```

## Notes

* Requires installation of ImageSharp related NuGet packages
* Recommended to configure appropriate image processing limits to prevent resource abuse
* Recommended to enable image caching for better performance

## Links

* [中文文档](./README.md)
* [Module documentation](../README.md)
