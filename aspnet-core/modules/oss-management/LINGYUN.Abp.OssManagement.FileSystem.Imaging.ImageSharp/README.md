# LINGYUN.Abp.OssManagement.FileSystem.Imaging.ImageSharp

本地文件系统的ImageSharp图像处理实现模块

## 功能

* 基于ImageSharp实现本地文件系统的图像处理功能
* 实现IImageProcessor接口
* 提供高性能的图像处理能力
* 支持多种图像格式
* 提供丰富的图像处理选项

## 配置使用

模块按需引用：

```csharp
[DependsOn(typeof(AbpOssManagementFileSystemImagingImageSharpModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

## 图像处理功能

支持以下图像处理操作：
* 缩放：支持多种缩放模式和算法
* 裁剪：支持矩形裁剪和圆形裁剪
* 旋转：支持任意角度旋转
* 水印：支持文字水印和图片水印
* 格式转换：支持多种图片格式之间的转换
* 质量调整：支持压缩质量控制

## 性能优化

* 使用ImageSharp的高性能算法
* 支持图像处理缓存
* 支持异步处理
* 支持内存优化

## 注意事项

* 需要安装ImageSharp相关的NuGet包
* 建议配置适当的图像处理限制
* 建议启用图像缓存
* 注意内存使用管理

## 链接

* [English documentation](./README.EN.md)
* [模块说明](../README.md)
