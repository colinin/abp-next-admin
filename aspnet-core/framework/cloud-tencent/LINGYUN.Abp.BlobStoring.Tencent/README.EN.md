# LINGYUN.Abp.BlobStoring.Tencent

Tencent Cloud Object Storage (COS) Module, integrating Tencent Cloud Object Storage service into ABP BlobStoring system.

## Features

* Support for Tencent Cloud Object Storage service
* Multi-tenant configuration support
* Automatic bucket creation support
* Bucket referer configuration support
* Multi-region configuration support
* File size limit support
* Tenant-isolated storage support

## Configuration Items

### Basic Configuration

```json
{
  "Settings": {
    "Abp.TencentCloud": {
      "SecretId": "Your Tencent Cloud SecretId", // Get from Tencent Cloud Console
      "SecretKey": "Your Tencent Cloud SecretKey", // Get from Tencent Cloud Console
      "DurationSecond": "600" // Session duration in seconds
    }
  }
}
```

### Object Storage Configuration

```json
{
  "Tencent": {
    "OSS": {
      "AppId": "", // Tencent Cloud AppId
      "Region": "", // Bucket region
      "BucketName": "", // Bucket name
      "CreateBucketIfNotExists": false, // Create bucket if not exists
      "CreateBucketReferer": [] // Referer settings when creating bucket
    }
  }
}
```

### Bucket Naming Rules

* Only lowercase letters and numbers are supported, i.e., [a-z, 0-9], hyphen "-" and their combinations
* Cannot start or end with a hyphen (-)
* The maximum allowed characters for bucket names are affected by the region abbreviation and APPID, with a total limit of 60 characters for the complete request domain
* For more rules, refer to [Tencent Cloud Bucket Naming Rules](https://cloud.tencent.com/document/product/436/13312)

### Object Naming Rules

* Cannot start with forward slash / or backslash \\
* ASCII control characters are not supported in object keys: up arrow (↑), down arrow (↓), right arrow (→), left arrow (←)
* For more rules, refer to [Tencent Cloud Object Naming Rules](https://cloud.tencent.com/document/product/436/13324)

## Basic Usage

1. Add module dependency
```csharp
[DependsOn(typeof(AbpBlobStoringTencentCloudModule))]
public class YourModule : AbpModule
{
    // ...
}
```

2. Configure Tencent Cloud Object Storage
```csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.Configure<YourContainer>(container =>
    {
        container.UseTencentCloud(tencent =>
        {
            tencent.AppId = "Your Tencent Cloud AppId";
            tencent.Region = "ap-guangzhou";
            tencent.BucketName = "your-bucket-name";
            tencent.CreateBucketIfNotExists = true;
            tencent.CreateBucketReferer = new List<string>
            {
                "*.example.com",
                "example.com"
            };
        });
    });
});
```

3. Use BLOB storage
```csharp
public class YourService
{
    private readonly IBlobContainer<YourContainer> _blobContainer;

    public YourService(IBlobContainer<YourContainer> blobContainer)
    {
        _blobContainer = blobContainer;
    }

    public async Task SaveBlobAsync(string name, Stream stream)
    {
        await _blobContainer.SaveAsync(name, stream);
    }

    public async Task<Stream> GetBlobAsync(string name)
    {
        return await _blobContainer.GetAsync(name);
    }
}
```

## Advanced Features

### Multi-tenant Support

The module supports tenant-isolated storage with the following path format:
* Host: `host/{blobName}`
* Tenant: `tenants/{tenantId}/{blobName}`

### Feature Management

The module provides the following feature switches:

* TencentBlobStoring - Controls enabling/disabling of Tencent Cloud Object Storage service
* TencentBlobStoringMaximumStreamSize - Controls the maximum file size limit (MB) for uploads

## More Documentation

* [Tencent Cloud Object Storage](https://cloud.tencent.com/document/product/436)
* [Tencent Cloud COS Console](https://console.cloud.tencent.com/cos)
* [ABP BlobStoring System](https://docs.abp.io/en/abp/latest/Blob-Storing)

[简体中文](./README.md)
