# LINGYUN.Abp.OssManagement.Tencent

腾讯云oss容器接口 

## 功能

* 实现基于腾讯云COS的对象存储管理
* 支持文件上传、下载和删除等基本操作
* 支持文件分片上传和断点续传
* 集成腾讯云COS的访问控制和安全机制
* 支持对象过期管理
* 支持临时密钥访问

## 配置使用

模块按需引用

```csharp
[DependsOn(typeof(AbpOssManagementTencentCloudModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置说明

需要配置以下关键信息：
* SecretId：腾讯云访问密钥ID
* SecretKey：腾讯云访问密钥密码
* Region：地域信息
* AppId：应用ID
* Bucket：存储桶名称
* SecurityToken：可选的临时安全令牌

## 注意事项

* 建议使用子账号密钥进行访问
* 建议开启服务端加密
* 建议配置适当的存储桶策略
* 建议启用日志记录功能

## 链接

* [English documentation](./README.EN.md)
* [模块说明](../README.md)
