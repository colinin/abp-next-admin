# LINGYUN.Abp.OssManagement.Nexus

OSS Management implementation for Nexus Repository

## Features

* Implements object storage management based on Nexus Raw repository
* Supports basic operations including file upload, download, and deletion
* Supports file sharding upload and breakpoint continuation
* Integrates with Nexus access control and security mechanisms
* Supports object expiration management

## Configuration

Module reference as needed:

```csharp
[DependsOn(typeof(AbpOssManagementNexusModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

## Configuration Details

Required configuration items:
* BaseUrl: Nexus server address
* Repository: Raw repository name
* Username: Access username
* Password: Access password
* Format: Repository format, default is raw

## Notes

* This module requires Nexus server to support Raw repository type
* Ensure the configured user has sufficient permissions to access the Raw repository
* HTTPS is recommended for secure transmission

## Links

* [中文文档](./README.md)
* [Module documentation](../README.md)
