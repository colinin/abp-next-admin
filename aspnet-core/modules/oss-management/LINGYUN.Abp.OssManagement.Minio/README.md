# LINGYUN.Abp.OssManagement.Minio

Oss容器管理接口的Minio实现 

## 配置使用

模块按需引用

相关配置项请参考 [BlobStoring Minio](https://abp.io/docs/latest/framework/infrastructure/blob-storing/minio)  

```csharp
[DependsOn(typeof(AbpOssManagementMinioModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```


