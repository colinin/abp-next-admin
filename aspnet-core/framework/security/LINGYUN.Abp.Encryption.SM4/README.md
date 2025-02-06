# LINGYUN.Abp.Encryption.SM4

数据加密模块,采用国密SM4算法,使用 **AbpStringEncryptionOptions** 配置无缝切换(密钥长度固定为128位以符合算法要求)  

## 配置使用


```csharp
[DependsOn(typeof(AbpEncryptionSM4Module))]
public class YouProjectModule : AbpModule
{
  // other
}
```
