# LINGYUN.Abp.OssManagement.HttpApi.Client

对象存储管理HTTP API客户端

## 功能

* 提供对象存储管理的HTTP API客户端代理
* 实现远程服务调用
* 支持动态API客户端代理生成

## 使用方式

1. 添加模块依赖：

```csharp
[DependsOn(typeof(AbpOssManagementHttpApiClientModule))]
public class YourModule : AbpModule
{
    // ...
}
```

2. 配置远程服务：

```json
{
  "RemoteServices": {
    "Default": {
      "BaseUrl": "http://your-api-server/"
    }
  }
}
```

## 可用服务

* IOssContainerAppService：容器管理服务
* IOssObjectAppService：对象管理服务
* IPublicFileAppService：公共文件服务
* IPrivateFileAppService：私有文件服务
* IShareFileAppService：共享文件服务
* IStaticFilesAppService：静态文件服务

## 链接

* [English documentation](./README.EN.md)
* [模块说明](../README.md)
