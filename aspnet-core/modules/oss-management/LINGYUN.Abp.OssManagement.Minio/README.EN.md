# LINGYUN.Abp.OssManagement.Minio

MinIO implementation for OSS container management interface

## Features

* Implements object storage management based on MinIO
* Supports basic operations including file upload, download, and deletion
* Supports file sharding upload and breakpoint continuation
* Integrates with MinIO access control and security mechanisms
* Supports custom bucket policies

## Configuration

Module reference as needed

Please refer to [BlobStoring Minio](https://abp.io/docs/latest/framework/infrastructure/blob-storing/minio) for related configuration items.

```csharp
[DependsOn(typeof(AbpOssManagementMinioModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration Details

Required configuration items:
* EndPoint: MinIO server address
* AccessKey: Access key
* SecretKey: Secret key
* BucketName: Bucket name
* WithSSL: Whether to enable SSL connection
* CreateBucketIfNotExists: Whether to create bucket if it doesn't exist

## Links

* [中文文档](./README.md)
* [Module documentation](../README.md)
