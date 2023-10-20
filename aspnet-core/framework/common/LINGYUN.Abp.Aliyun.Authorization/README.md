# LINGYUN.Abp.Aliyun.Authorization

阿里云基础认证模块, 目前只需要访问控制功能, 因此只定义了AppKeyId和AccessKeySecret。

## 配置使用

模块按需引用，使用到阿里云的模块基本都需要依赖于此

事先定义**appsettings.json**文件

```json
{
  "Aliyun": {
    "Auth": {
      "AccessKeyId": "你的阿里云访问标识",
      "AccessKeySecret": "你的阿里云访问密钥"
    }
  }
}

```

```csharp
[DependsOn(typeof(AbpAliyunAuthorizationModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```