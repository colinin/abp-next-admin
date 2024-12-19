# LINGYUN.Abp.OssManagement.Application

对象存储管理应用服务实现

## 功能

* 实现对象存储管理的应用服务接口
* 提供容器管理、对象管理和文件管理的具体实现
* 支持文件上传、下载和共享功能
* 实现权限验证和访问控制

## 服务实现

### 基础服务
* FileAppServiceBase：文件服务基类，实现IFileAppService接口
* OssManagementApplicationServiceBase：OSS管理应用服务基类

### 容器管理
* OssContainerAppService：实现IOssContainerAppService接口
  * 提供容器的创建、查询和删除功能
  * 支持容器列表分页查询
  * 支持容器内对象列表查询

### 对象管理
* OssObjectAppService：实现IOssObjectAppService接口
  * 提供对象的上传、下载和删除功能
  * 支持对象元数据管理
  * 支持对象访问权限控制

### 文件管理
* PublicFileAppService：实现IPublicFileAppService接口，处理公共文件
* PrivateFileAppService：实现IPrivateFileAppService接口，处理私有文件
* ShareFileAppService：实现IShareFileAppService接口，处理共享文件
* StaticFilesAppService：实现IStaticFilesAppService接口，处理静态文件

## 特性

* 支持文件分片上传
* 支持断点续传
* 支持文件访问权限控制
* 支持文件元数据管理
* 支持文件共享和过期管理

## 链接

* [English documentation](./README.EN.md)
* [模块说明](../README.md)
