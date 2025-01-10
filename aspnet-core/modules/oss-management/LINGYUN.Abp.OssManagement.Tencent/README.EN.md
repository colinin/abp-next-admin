# LINGYUN.Abp.OssManagement.Tencent

Tencent Cloud OSS container interface implementation

## Features

* Implements object storage management based on Tencent Cloud COS
* Supports basic operations including file upload, download, and deletion
* Supports file sharding upload and breakpoint continuation
* Integrates with Tencent Cloud COS access control and security mechanisms
* Supports object expiration management
* Supports temporary key access

## Configuration

Module reference as needed:

```csharp
[DependsOn(typeof(AbpOssManagementTencentCloudModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration Details

Required configuration items:
* SecretId: Tencent Cloud access key ID
* SecretKey: Tencent Cloud access key secret
* Region: Region information
* AppId: Application ID
* Bucket: Bucket name
* SecurityToken: Optional temporary security token

## Notes

* Recommended to use sub-account keys for access
* Recommended to enable server-side encryption
* Recommended to configure appropriate bucket policies
* Recommended to enable logging functionality

## Links

* [中文文档](./README.md)
* [Module documentation](../README.md)
