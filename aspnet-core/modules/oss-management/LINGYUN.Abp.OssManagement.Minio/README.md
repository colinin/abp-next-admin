# LINGYUN.Abp.OssManagement.Minio

Oss容器管理接口的Minio实现 

## 功能

* 实现基于MinIO的对象存储管理
* 支持文件上传、下载、删除等基本操作
* 支持文件分片上传和断点续传
* 集成MinIO的访问控制和安全机制
* 支持自定义存储桶策略

## 配置使用

模块按需引用

相关配置项请参考 [BlobStoring Minio](https://abp.io/docs/latest/framework/infrastructure/blob-storing/minio)  

```csharp
[DependsOn(typeof(AbpOssManagementMinioModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置说明

需要配置以下关键信息：
* EndPoint：MinIO服务器地址
* AccessKey：访问密钥
* SecretKey：密钥密码
* BucketName：存储桶名称
* WithSSL：是否启用SSL连接
* CreateBucketIfNotExists：不存在时是否创建存储桶

## 链接

* [English documentation](./README.EN.md)
* [模块说明](../README.md)
