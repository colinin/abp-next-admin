# LINGYUN.Abp.OssManagement.Domain.Shared

Object Storage Management Module Shared Domain Layer

## Features

* Defines basic types and constants for object storage management
* Defines error codes
* Defines features
* Defines settings
* Provides localization resources

## Error Codes

* ContainerDeleteWithStatic: Attempt to delete static container
* ContainerDeleteWithNotEmpty: Attempt to delete non-empty container
* ContainerAlreadyExists: Container already exists
* ContainerNotFound: Container not found
* ObjectDeleteWithNotEmpty: Attempt to delete non-empty object
* ObjectAlreadyExists: Object already exists
* ObjectNotFound: Object not found
* OssNameHasTooLong: OSS name too long

## Features

* PublicAccess: Public access
* OssObject.Enable: Enable object storage
* OssObject.AllowSharedFile: Allow file sharing
* OssObject.DownloadFile: Allow file download
* OssObject.DownloadLimit: Download limit
* OssObject.DownloadInterval: Download interval
* OssObject.UploadFile: Allow file upload
* OssObject.UploadLimit: Upload limit
* OssObject.UploadInterval: Upload interval
* OssObject.MaxUploadFileCount: Maximum upload file count

## Settings

* DownloadPackageSize: Download package size
* FileLimitLength: File size limit, default: 100
* AllowFileExtensions: Allowed file extensions, default: dll,zip,rar,txt,log,xml,config,json,jpeg,jpg,png,bmp,ico,xlsx,xltx,xls,xlt,docs,dots,doc,dot,pptx,potx,ppt,pot,chm

## Links

* [中文文档](./README.md)
* [Module documentation](../README.md)
