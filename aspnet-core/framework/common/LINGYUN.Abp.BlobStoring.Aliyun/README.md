# LINGYUN.Abp.BlobStoring.Aliyun

ABP框架对象存储提供者**IBlobProvider**的阿里云OSS实现。

## 功能特性

* 实现ABP的IBlobProvider接口，使用阿里云OSS服务
* 支持多租户Blob存储
* 自动创建Bucket
* 可配置的Bucket访问控制
* 支持STS Token访问
* 可自定义的Blob命名策略

## 模块引用

首先定义**appsettings.json**文件：

```json
{
  "Aliyun": {
    "OSS": {
      "BucketName": "你定义的BucketName",
      "Endpoint": "http://oss-cn-shanghai.aliyuncs.com",
      "CreateBucketIfNotExists": true
    }
  }
}
```

然后在项目中引用模块：

```csharp
[DependsOn(typeof(AbpBlobStoringAliyunModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置说明

### OSS配置

* **BucketName**
  * 说明：OSS存储空间名称
  * 类型：可选
  * 默认值：容器名称

* **Endpoint**
  * 说明：OSS服务的接入点
  * 类型：必须
  * 示例：http://oss-cn-shanghai.aliyuncs.com

* **CreateBucketIfNotExists**
  * 说明：如果Bucket不存在是否自动创建
  * 类型：可选
  * 默认值：false

* **CreateBucketReferer**
  * 说明：Bucket访问白名单
  * 类型：可选

### Blob命名规则

* 容器（Bucket）名称规则：
  * 长度必须在3-63字符之间
  * 只能包含小写字母、数字和短横线
  * 必须以字母或数字开头
  * 不能以短横线开头或结尾

* Blob名称规则：
  * 租户：`tenants/{tenantId}/{blobName}`
  * 宿主：`host/{blobName}`

## 性能优化

* 使用分布式缓存存储STS Token
* 支持数据冗余配置
* 可配置的Bucket访问控制以提高安全性

## 相关模块

* [LINGYUN.Abp.Aliyun](../../cloud-aliyun/LINGYUN.Abp.Aliyun/README.md) - 提供阿里云基础集成

[Click to view English documentation](README.EN.md)