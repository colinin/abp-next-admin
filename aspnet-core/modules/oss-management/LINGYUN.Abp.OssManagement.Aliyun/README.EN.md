# LINGYUN.Abp.OssManagement.Aliyun

Aliyun OSS container interface implementation

## Features

* Implements object storage management based on Aliyun OSS
* Supports basic operations including file upload, download, and deletion
* Supports file sharding upload and breakpoint continuation
* Integrates with Aliyun OSS access control and security mechanisms

## Configuration

Module reference as needed:

```csharp
[DependsOn(typeof(AbpOssManagementAliyunModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration Details

Please refer to Aliyun OSS configuration documentation: [Aliyun OSS Configuration](https://help.aliyun.com/document_detail/32009.html)

Required configuration items:
* AccessKeyId: Aliyun access key ID
* AccessKeySecret: Aliyun access key secret
* Endpoint: Aliyun OSS access domain
* SecurityToken: Optional security token

## Links

* [中文文档](./README.md)
* [Module documentation](../README.md)
