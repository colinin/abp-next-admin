# Oss-Management

File-Management更名为Oss-Management  


## 模块说明

### 基础模块

* LINGYUN.Abp.OssManagement.Domain.Shared			领域层公共模块，定义了错误代码、本地化、模块设置  
* LINGYUN.Abp.OssManagement.Domain					领域层模块，定义了抽象的Oss容器与对象管理接口  
* LINGYUN.Abp.OssManagement.Application.Contracts	应用服务层公共模块，定义了管理Oss的外部接口、权限、功能限制策略  
* LINGYUN.Abp.OssManagement.Application				应用服务层实现，实现了Oss管理接口  
* LINGYUN.Abp.OssManagement.HttpApi					RestApi实现，实现了独立的对外RestApi接口  
* LINGYUN.Abp.OssManagement.SettingManagement		设置管理模块，对外暴露自身的设置管理，用于网关聚合  

### 高阶模块

* LINGYUN.Abp.OssManagement.Aliyun					Oss管理的阿里云实现，实现了部分阿里云Oss服务的容器与对象管理  
* LINGYUN.Abp.OssManagement.FileSystem				Oss管理的本地文件系统实现，实现了部分本地文件系统的容器（目录）与对象（文件/目录）管理  
* LINGYUN.Abp.OssManagement.FileSystem.ImageSharp	Oss本地对象的ImageSharp扩展，当前端传递需求处理对象时，此模块用于实现基于图形文件流的处理   

## 更新日志

*【2021-03-10】 变更FileManagement命名空间为OssManagement  
