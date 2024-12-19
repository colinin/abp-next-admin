# LINGYUN.Abp.OssManagement.Nexus

Nexus仓库的OSS管理模块实现

## 功能

* 实现基于Nexus Raw仓库的对象存储管理
* 支持文件上传、下载和删除等基本操作
* 支持文件分片上传和断点续传
* 集成Nexus的访问控制和安全机制
* 支持对象过期管理

## 配置使用

模块按需引用：

```csharp
[DependsOn(typeof(AbpOssManagementNexusModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

## 配置说明

需要配置以下关键信息：
* BaseUrl：Nexus服务器地址
* Repository：Raw仓库名称
* Username：访问用户名
* Password：访问密码
* Format：仓库格式，默认为raw

## 注意事项

* 此模块需要Nexus服务器支持Raw仓库类型
* 需要确保配置的用户具有足够的权限访问Raw仓库
* 建议使用HTTPS进行安全传输

## 链接

* [English documentation](./README.EN.md)
* [模块说明](../README.md)
