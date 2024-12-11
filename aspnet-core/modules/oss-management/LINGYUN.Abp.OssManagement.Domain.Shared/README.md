# LINGYUN.Abp.OssManagement.Domain.Shared

对象存储管理模块共享领域层

## 功能

* 定义对象存储管理的基础类型和常量
* 定义错误代码
* 定义功能特性
* 定义设置项
* 提供本地化资源

## 错误代码

* ContainerDeleteWithStatic：尝试删除静态容器
* ContainerDeleteWithNotEmpty：尝试删除非空容器
* ContainerAlreadyExists：容器已存在
* ContainerNotFound：容器不存在
* ObjectDeleteWithNotEmpty：尝试删除非空对象
* ObjectAlreadyExists：对象已存在
* ObjectNotFound：对象不存在
* OssNameHasTooLong：OSS名称过长

## 功能特性

* PublicAccess：公共访问
* OssObject.Enable：启用对象存储
* OssObject.AllowSharedFile：允许文件共享
* OssObject.DownloadFile：允许文件下载
* OssObject.DownloadLimit：下载限制
* OssObject.DownloadInterval：下载间隔
* OssObject.UploadFile：允许文件上传
* OssObject.UploadLimit：上传限制
* OssObject.UploadInterval：上传间隔
* OssObject.MaxUploadFileCount：最大上传文件数

## 设置项

* DownloadPackageSize：下载包大小
* FileLimitLength：文件大小限制，默认：100
* AllowFileExtensions：允许的文件扩展名，默认：dll,zip,rar,txt,log,xml,config,json,jpeg,jpg,png,bmp,ico,xlsx,xltx,xls,xlt,docs,dots,doc,dot,pptx,potx,ppt,pot,chm

## 链接

* [English documentation](./README.EN.md)
* [模块说明](../README.md)
