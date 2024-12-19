# LINGYUN.Abp.OssManagement.FileSystem

本地文件系统oss容器接口 

## 功能

* 实现基于本地文件系统的对象存储管理
* 支持文件上传、下载、删除等基本操作
* 支持文件分片上传和断点续传
* 支持文件目录的自动创建和管理
* 支持基于文件系统的访问控制
* 支持文件元数据的存储和管理

## 配置使用

模块按需引用

```csharp
[DependsOn(typeof(AbpOssManagementFileSystemModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置说明

需要配置以下关键信息：
* BasePath：文件存储的基础路径
* AppendContainerNameToBasePath：是否将容器名称附加到基础路径
* HttpServer：HTTP服务器配置
  * Scheme：协议方案（http/https）
  * Host：主机地址
  * Port：端口号

## 相关模块

* LINGYUN.Abp.OssManagement.FileSystem.ImageSharp：提供图像处理功能
* LINGYUN.Abp.OssManagement.FileSystem.Imaging：提供基础图像功能支持
* LINGYUN.Abp.OssManagement.FileSystem.Imaging.ImageSharp：基于ImageSharp的图像处理实现

## 链接

* [English documentation](./README.EN.md)
* [模块说明](../README.md)
