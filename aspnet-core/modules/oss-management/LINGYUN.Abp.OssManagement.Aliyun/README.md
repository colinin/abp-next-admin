# LINGYUN.Abp.OssManagement.Aliyun

阿里云oss容器接口 

## 功能

* 实现基于阿里云OSS的对象存储管理
* 支持文件上传、下载、删除等基本操作
* 支持文件分片上传和断点续传
* 集成阿里云OSS的访问控制和安全机制

## 配置使用

模块按需引用

```csharp
[DependsOn(typeof(AbpOssManagementAliyunModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置说明

请参考阿里云OSS的配置说明：[阿里云OSS配置](https://help.aliyun.com/document_detail/32009.html)

需要配置以下关键信息：
* AccessKeyId：阿里云访问密钥ID
* AccessKeySecret：阿里云访问密钥密码
* Endpoint：阿里云OSS访问域名
* SecurityToken：可选的安全令牌

## 链接

* [English documentation](./README.EN.md)
* [模块说明](../README.md)
