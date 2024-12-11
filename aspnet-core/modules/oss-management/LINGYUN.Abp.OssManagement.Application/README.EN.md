# LINGYUN.Abp.OssManagement.Application

Object Storage Management Application Service Implementation

## Features

* Implements application service interfaces for object storage management
* Provides concrete implementations for container management, object management, and file management
* Supports file upload, download, and sharing functionality
* Implements permission validation and access control

## Service Implementations

### Base Services
* FileAppServiceBase: File service base class, implements IFileAppService interface
* OssManagementApplicationServiceBase: OSS management application service base class

### Container Management
* OssContainerAppService: Implements IOssContainerAppService interface
  * Provides container creation, query, and deletion functionality
  * Supports paginated container list queries
  * Supports object list queries within containers

### Object Management
* OssObjectAppService: Implements IOssObjectAppService interface
  * Provides object upload, download, and deletion functionality
  * Supports object metadata management
  * Supports object access control

### File Management
* PublicFileAppService: Implements IPublicFileAppService interface, handles public files
* PrivateFileAppService: Implements IPrivateFileAppService interface, handles private files
* ShareFileAppService: Implements IShareFileAppService interface, handles shared files
* StaticFilesAppService: Implements IStaticFilesAppService interface, handles static files

## Features

* Supports file sharding upload
* Supports breakpoint continuation
* Supports file access control
* Supports file metadata management
* Supports file sharing and expiration management

## Links

* [中文文档](./README.md)
* [Module documentation](../README.md)
