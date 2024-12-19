# LINGYUN.Abp.OssManagement.HttpApi.Client

Object Storage Management HTTP API Client

## Features

* Provides HTTP API client proxy for object storage management
* Implements remote service calls
* Supports dynamic API client proxy generation

## Usage

1. Add module dependency:

```csharp
[DependsOn(typeof(AbpOssManagementHttpApiClientModule))]
public class YourModule : AbpModule
{
    // ...
}
```

2. Configure remote services:

```json
{
  "RemoteServices": {
    "Default": {
      "BaseUrl": "http://your-api-server/"
    }
  }
}
```

## Available Services

* IOssContainerAppService: Container management service
* IOssObjectAppService: Object management service
* IPublicFileAppService: Public file service
* IPrivateFileAppService: Private file service
* IShareFileAppService: Shared file service
* IStaticFilesAppService: Static file service

## Links

* [中文文档](./README.md)
* [Module documentation](../README.md)
