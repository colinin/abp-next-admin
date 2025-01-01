# LINGYUN.Abp.OssManagement.ImageSharp

ImageSharp-based image processing interface for OSS objects

## Configuration

Module reference as needed:

```csharp
[DependsOn(typeof(AbpOssManagementFileSystemImageSharpModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Features

* Implements image processing functionality for OSS objects based on ImageSharp
* Supports image format conversion
* Supports image resizing and cropping
* Supports watermark addition
* Supports image quality adjustment
* Supports image metadata processing

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

## Links

* [中文文档](./README.md)
* [Module documentation](../README.md)
