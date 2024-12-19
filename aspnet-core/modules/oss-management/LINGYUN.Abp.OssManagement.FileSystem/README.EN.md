# LINGYUN.Abp.OssManagement.FileSystem

Local file system OSS container interface implementation

## Features

* Implements object storage management based on local file system
* Supports basic operations including file upload, download, and deletion
* Supports file sharding upload and breakpoint continuation
* Supports automatic creation and management of file directories
* Supports file system-based access control
* Supports storage and management of file metadata

## Configuration

Module reference as needed:

```csharp
[DependsOn(typeof(AbpOssManagementFileSystemModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration Details

Required configuration items:
* BasePath: Base path for file storage
* AppendContainerNameToBasePath: Whether to append container name to base path
* HttpServer: HTTP server configuration
  * Scheme: Protocol scheme (http/https)
  * Host: Host address
  * Port: Port number

## Related Modules

* LINGYUN.Abp.OssManagement.FileSystem.ImageSharp: Provides image processing functionality
* LINGYUN.Abp.OssManagement.FileSystem.Imaging: Provides basic image functionality support
* LINGYUN.Abp.OssManagement.FileSystem.Imaging.ImageSharp: ImageSharp-based image processing implementation

## Links

* [中文文档](./README.md)
* [Module documentation](../README.md)
