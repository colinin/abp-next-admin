# LINGYUN.Abp.OssManagement.Application.Contracts

Object Storage Management Application Service Interface Definitions

## Features

* Defines application service interfaces for object storage management
* Defines DTO objects for object storage management
* Defines permission management
* Supports management of public files, private files, and shared files

## Interface Definitions

### Container Management
* IOssContainerAppService: Container management service interface
  * CreateAsync: Create container
  * GetAsync: Get container information
  * GetListAsync: Get container list
  * GetObjectListAsync: Get object list in container
  * DeleteAsync: Delete container

### Object Management
* IOssObjectAppService: Object management service interface
  * CreateAsync: Create object
  * GetAsync: Get object information
  * DeleteAsync: Delete object
  * DownloadAsync: Download object

### File Management
* IFileAppService: Base file service interface
* IPublicFileAppService: Public file service interface
* IPrivateFileAppService: Private file service interface
* IShareFileAppService: Shared file service interface
* IStaticFilesAppService: Static file service interface

## Permission Definitions

* AbpOssManagement.Container: Container management permissions
  * Create: Create container
  * Delete: Delete container
* AbpOssManagement.OssObject: Object management permissions
  * Create: Create object
  * Delete: Delete object
  * Download: Download object

## Links

* [中文文档](./README.md)
* [Module documentation](../README.md)
