# LINGYUN.Abp.BlobStoring.Aliyun

Aliyun OSS implementation of the ABP framework's object storage provider **IBlobProvider**.

## Features

* Implements ABP's IBlobProvider interface using Aliyun OSS service
* Supports multi-tenant Blob storage
* Automatic Bucket creation
* Configurable Bucket access control
* STS Token access support
* Customizable Blob naming strategy

## Module Reference

First, define the **appsettings.json** file:

```json
{
  "Aliyun": {
    "OSS": {
      "BucketName": "your-bucket-name",
      "Endpoint": "http://oss-cn-shanghai.aliyuncs.com",
      "CreateBucketIfNotExists": true
    }
  }
}
```

Then reference the module in your project:

```csharp
[DependsOn(typeof(AbpBlobStoringAliyunModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration

### OSS Configuration

* **BucketName**
  * Description: OSS storage space name
  * Type: Optional
  * Default: Container name

* **Endpoint**
  * Description: OSS service access point
  * Type: Required
  * Example: http://oss-cn-shanghai.aliyuncs.com

* **CreateBucketIfNotExists**
  * Description: Whether to automatically create the bucket if it doesn't exist
  * Type: Optional
  * Default: false

* **CreateBucketReferer**
  * Description: Bucket access whitelist
  * Type: Optional

### Blob Naming Rules

* Container (Bucket) naming rules:
  * Length must be between 3-63 characters
  * Can only contain lowercase letters, numbers, and hyphens
  * Must start with a letter or number
  * Cannot start or end with a hyphen

* Blob naming rules:
  * Tenant: `tenants/{tenantId}/{blobName}`
  * Host: `host/{blobName}`

## Performance Optimization

* Uses distributed caching for STS Token storage
* Supports data redundancy configuration
* Configurable Bucket access control for enhanced security

## Related Modules

* [LINGYUN.Abp.Aliyun](../../cloud-aliyun/LINGYUN.Abp.Aliyun/README.md) - Provides Aliyun basic integration

[点击查看中文文档](README.md)
