# LINGYUN.Abp.OssManagement.Application.Contracts

对象存储管理应用服务接口定义

## 功能

* 定义对象存储管理的应用服务接口
* 定义对象存储管理的DTO对象
* 定义权限管理
* 支持公共文件、私有文件和共享文件的管理

## 接口定义

### 容器管理
* IOssContainerAppService：容器管理服务接口
  * CreateAsync：创建容器
  * GetAsync：获取容器信息
  * GetListAsync：获取容器列表
  * GetObjectListAsync：获取容器中的对象列表
  * DeleteAsync：删除容器

### 对象管理
* IOssObjectAppService：对象管理服务接口
  * CreateAsync：创建对象
  * GetAsync：获取对象信息
  * DeleteAsync：删除对象
  * DownloadAsync：下载对象

### 文件管理
* IFileAppService：基础文件服务接口
* IPublicFileAppService：公共文件服务接口
* IPrivateFileAppService：私有文件服务接口
* IShareFileAppService：共享文件服务接口
* IStaticFilesAppService：静态文件服务接口

## 权限定义

* AbpOssManagement.Container：容器管理权限
  * Create：创建容器
  * Delete：删除容器
* AbpOssManagement.OssObject：对象管理权限
  * Create：创建对象
  * Delete：删除对象
  * Download：下载对象

## 链接

* [English documentation](./README.EN.md)
* [模块说明](../README.md)
