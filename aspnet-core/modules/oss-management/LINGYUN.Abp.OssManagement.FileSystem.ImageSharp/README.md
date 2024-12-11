# LINGYUN.Abp.OssManagement.FileSystem.ImageSharp

本地文件系统的ImageSharp图像处理实现

## 功能

* 基于ImageSharp实现本地文件系统的图像处理
* 支持图像格式转换
* 支持图像缩放和裁剪
* 支持图像水印添加
* 支持图像质量调整
* 支持图像元数据处理

## 配置使用

模块按需引用：

```csharp
[DependsOn(typeof(AbpOssManagementFileSystemImageSharpModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

## 图像处理功能

支持以下图像处理操作：
* resize：调整图像大小
* crop：裁剪图像
* rotate：旋转图像
* watermark：添加水印
* format：转换图像格式
* quality：调整图像质量

## 使用示例

图像处理参数通过URL查询字符串传递：

```
http://your-domain/api/oss-management/objects/my-image.jpg?process=image/resize,w_100,h_100/watermark,text_Hello
```

## 注意事项

* 需要安装ImageSharp相关的NuGet包
* 建议配置适当的图像处理限制以避免资源滥用
* 建议启用图像缓存以提高性能

## 链接

* [English documentation](./README.EN.md)
* [模块说明](../README.md)
