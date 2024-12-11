# LINGYUN.Abp.OssManagement.Imaging

对象存储管理图像处理模块

## 功能

* 提供对象存储的图像处理基础功能
* 支持图像格式转换
* 支持图像缩放和裁剪
* 支持图像水印添加
* 支持图像质量调整
* 支持图像元数据处理

## 配置使用

模块按需引用：

```csharp
[DependsOn(typeof(AbpOssManagementImagingModule))]
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

## 相关模块

* LINGYUN.Abp.OssManagement.ImageSharp：提供基于ImageSharp的具体实现

## 链接

* [English documentation](./README.EN.md)
* [模块说明](../README.md)
