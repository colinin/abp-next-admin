# LINGYUN.Abp.OssManagement.FileSystem.Imaging

本地文件系统的图像处理基础模块

## 功能

* 提供本地文件系统的图像处理基础功能
* 定义图像处理接口和抽象类
* 支持图像处理扩展
* 提供图像处理管道
* 支持图像处理缓存

## 配置使用

模块按需引用：

```csharp
[DependsOn(typeof(AbpOssManagementFileSystemImagingModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

## 核心接口

* IImageProcessor：图像处理器接口
* IImageProcessorFactory：图像处理器工厂接口
* IImageProcessorCache：图像处理缓存接口
* IImageProcessorPipeline：图像处理管道接口

## 扩展功能

* 支持自定义图像处理器
* 支持自定义图像处理管道
* 支持自定义图像缓存策略
* 支持图像处理参数验证

## 相关模块

* LINGYUN.Abp.OssManagement.FileSystem.Imaging.ImageSharp：基于ImageSharp的具体实现

## 链接

* [English documentation](./README.EN.md)
* [模块说明](../README.md)
