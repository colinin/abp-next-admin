# LINGYUN.Abp.OssManagement.SettingManagement

Object Storage Management Settings Management Module

## Features

* Provides settings management functionality for object storage management
* Implements settings reading and modification
* Supports multi-tenant configuration
* Supports different levels of settings management

## Configuration

Module reference as needed:

```csharp
[DependsOn(typeof(AbpOssManagementSettingManagementModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

## Settings

### Basic Settings
* DownloadPackageSize: Download package size
* FileLimitLength: File size limit
* AllowFileExtensions: Allowed file extensions

### API Endpoints

* GET /api/oss-management/settings: Get settings
* PUT /api/oss-management/settings: Update settings

### Permissions

* AbpOssManagement.Setting: Settings management permission

## Notes

* Appropriate permissions are required to access settings
* Some settings may require application restart to take effect
* It's recommended to backup current configuration before modifying settings

## Links

* [中文文档](./README.md)
* [Module documentation](../README.md)
