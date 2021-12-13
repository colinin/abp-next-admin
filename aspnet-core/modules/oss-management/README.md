# Oss-Management

File-Management更名为Oss-Management  

## 模块说明

### 基础模块

* [LINGYUN.Abp.OssManagement.Domain.Shared](./LINGYUN.Abp.OssManagement.Domain.Shared)					领域层公共模块，定义了错误代码、本地化、模块设置  
* [LINGYUN.Abp.OssManagement.Domain](./LINGYUN.Abp.OssManagement.Domain)								领域层模块，定义了抽象的Oss容器与对象管理接口  
* [LINGYUN.Abp.OssManagement.Application.Contracts](./LINGYUN.Abp.OssManagement.Application.Contracts)	应用服务层公共模块，定义了管理Oss的外部接口、权限、功能限制策略  
* [LINGYUN.Abp.OssManagement.Application](./LINGYUN.Abp.OssManagement.Application)						应用服务层实现，实现了Oss管理接口  
* [LINGYUN.Abp.OssManagement.HttpApi](./LINGYUN.Abp.OssManagement.HttpApi)								RestApi实现，实现了独立的对外RestApi接口  
* [LINGYUN.Abp.OssManagement.HttpApi.Client](./LINGYUN.Abp.OssManagement.HttpApi.Client)				客户端代理模块, 简化远程调用过程  
* [LINGYUN.Abp.OssManagement.SettingManagement](./LINGYUN.Abp.OssManagement.SettingManagement)			设置管理模块，对外暴露自身的设置管理，用于网关聚合  

### 高阶模块

* [LINGYUN.Abp.BlobStoring.OssManagement](./LINGYUN.Abp.BlobStoring.OssManagement)						abp框架对象存储提供者**IBlobProvider**的Oss管理模块实现, 依赖于Oss客户端代理模块  
* [LINGYUN.Abp.OssManagement.Aliyun](./LINGYUN.Abp.OssManagement.Aliyun)									Oss管理的阿里云实现，实现了部分阿里云Oss服务的容器与对象管理  
* [LINGYUN.Abp.OssManagement.FileSystem](./LINGYUN.Abp.OssManagement.FileSystem)							Oss管理的本地文件系统实现，实现了部分本地文件系统的容器（目录）与对象（文件/目录）管理  
* [LINGYUN.Abp.OssManagement.FileSystem.ImageSharp](./LINGYUN.Abp.OssManagement.FileSystem.ImageSharp)	Oss本地对象的ImageSharp扩展，当前端传递需求处理对象时，此模块用于实现基于图形文件流的处理   

### 权限定义

* AbpOssManagement.Container						授权对象是否允许访问容器（bucket）
* AbpOssManagement.Container.Create					授权对象是否允许创建容器（bucket）
* AbpOssManagement.Container.Delete					授权对象是否允许删除容器（bucket）
* AbpOssManagement.OssObject						授权对象是否允许访问Oss对象
* AbpOssManagement.OssObject.Create					授权对象是否允许创建Oss对象
* AbpOssManagement.OssObject.Delete					授权对象是否允许删除Oss对象
* AbpOssManagement.OssObject.Download				授权对象是否允许下载Oss对象

### 功能定义

* AbpOssManagement.OssObject.DownloadFile			用户可以下载文件
* AbpOssManagement.OssObject.DownloadLimit			用户在周期内允许下载文件的最大次数，范围0-1000000
* AbpOssManagement.OssObject.DownloadInterval		用户限制下载文件次数的周期，时钟刻度：月，默认： 1，范围1-12
* AbpOssManagement.OssObject.UploadFile				用户可以上传文件
* AbpOssManagement.OssObject.UploadLimit			用户在周期内允许上传文件的最大次数，范围0-1000000
* AbpOssManagement.OssObject.UploadInterval			用户限制上传文件次数的周期，时钟刻度：月，默认： 1，范围1-12
* AbpOssManagement.OssObject.MaxUploadFileCount		单次上传文件的数量，未实现

### 配置定义

* Abp.OssManagement.DownloadPackageSize				下载分包大小，分块下载时单次传输的数据大小，未实现
* Abp.OssManagement.FileLimitLength					上传文件限制大小，默认：100
* Abp.OssManagement.AllowFileExtensions				允许的上传文件扩展名，多个扩展名以逗号分隔，默认：dll,zip,rar,txt,log,xml,config,json,jpeg,jpg,png,bmp,ico,xlsx,xltx,xls,xlt,docs,dots,doc,dot,pptx,potx,ppt,pot,chm

## 更新日志

*【2021-03-10】 变更FileManagement命名空间为OssManagement  
*【2021-10-22】	增加PublicFilesController用于身份认证通过的用户上传/下载文件,所有操作限定在用户目录下  
*【2021-12-13】	增加LINGYUN.Abp.BlobStoring.OssManagement用于实现Oss代理二进制文件存储  
