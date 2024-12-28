# LINGYUN.Abp.OssManagement.HttpApi

Object Storage Management HTTP API Layer

## Features

* Provides HTTP API interfaces for object storage management
* Implements RESTful API design
* Supports HTTP interfaces for file upload, download, and sharing
* Provides permission validation and access control

## API Controllers

### Container Management
* OssContainerController
  * POST /api/oss-management/containers: Create container
  * GET /api/oss-management/containers/{name}: Get container information
  * GET /api/oss-management/containers: Get container list
  * DELETE /api/oss-management/containers/{name}: Delete container

### Object Management
* OssObjectController
  * POST /api/oss-management/objects: Upload object
  * GET /api/oss-management/objects/{*path}: Get object
  * DELETE /api/oss-management/objects/{*path}: Delete object

### File Management
* PublicFilesController: Handles public file access
* PrivateFilesController: Handles private file access
* ShareFilesController: Handles shared file access
* StaticFilesController: Handles static file access

## Features

* Supports file sharding upload
* Supports breakpoint continuation
* Supports file streaming download
* Supports file access control
* Supports file metadata management

## Links

* [中文文档](./README.md)
* [Module documentation](../README.md)
