# LINGYUN.Abp.OssManagement.HttpApi

对象存储管理HTTP API层

## 功能

* 提供对象存储管理的HTTP API接口
* 实现RESTful风格的API设计
* 支持文件上传、下载和共享的HTTP接口
* 提供权限验证和访问控制

## API控制器

### 容器管理
* OssContainerController
  * POST /api/oss-management/containers：创建容器
  * GET /api/oss-management/containers/{name}：获取容器信息
  * GET /api/oss-management/containers：获取容器列表
  * DELETE /api/oss-management/containers/{name}：删除容器

### 对象管理
* OssObjectController
  * POST /api/oss-management/objects：上传对象
  * GET /api/oss-management/objects/{*path}：获取对象
  * DELETE /api/oss-management/objects/{*path}：删除对象

### 文件管理
* PublicFilesController：处理公共文件访问
* PrivateFilesController：处理私有文件访问
* ShareFilesController：处理共享文件访问
* StaticFilesController：处理静态文件访问

## 特性

* 支持文件分片上传
* 支持断点续传
* 支持文件流式下载
* 支持文件访问权限控制
* 支持文件元数据管理

## 链接

* [English documentation](./README.EN.md)
* [模块说明](../README.md)
