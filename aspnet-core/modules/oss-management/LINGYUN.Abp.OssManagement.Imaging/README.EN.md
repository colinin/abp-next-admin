# LINGYUN.Abp.OssManagement.Imaging

Object Storage Management Image Processing Module

## Features

* Provides basic image processing functionality for object storage
* Supports image format conversion
* Supports image resizing and cropping
* Supports watermark addition
* Supports image quality adjustment
* Supports image metadata processing

## Configuration

Module reference as needed:

```csharp
[DependsOn(typeof(AbpOssManagementImagingModule))]
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

## Related Modules

* LINGYUN.Abp.OssManagement.ImageSharp: Provides ImageSharp-based implementation

## Links

* [中文文档](./README.md)
* [Module documentation](../README.md)
