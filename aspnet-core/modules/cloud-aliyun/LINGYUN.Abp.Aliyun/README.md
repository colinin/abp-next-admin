# LINGYUN.Abp.Aliyun

阿里云sdk集成  

参照:https://help.aliyun.com/document_detail/28763.html

## 配置使用

模块按需引用

```csharp
[DependsOn(typeof(AbpAliyunModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```
## 配置项说明

*	AliyunSettingNames.Authorization.RegionId						可选,区域,默认 default  
*	AliyunSettingNames.Authorization.AccessKeyId					必须,阿里云RAM账号的AccessKey ID  
*	AliyunSettingNames.Authorization.AccessKeySecret				必须,RAM账号的AccessKey Secret  
*	AliyunSettingNames.Authorization.UseSecurityTokenService		可选,建议,使用STS Token访问,按照阿里云文档,建议使用Sts Token访问API,默认false  
*	AliyunSettingNames.Authorization.RamRoleArn						可选,启用Sts Token之后必须配置,阿里云RAM角色ARN  
*	AliyunSettingNames.Authorization.RoleSessionName				可选,启用Sts Token之后的用户自定义令牌名称,用于访问审计  
*	AliyunSettingNames.Authorization.DurationSeconds				可选,用户令牌的过期时间,单位为秒,默认3000  
*	AliyunSettingNames.Authorization.Policy							可选,权限策略,为json字符串  

## 其他

网络因素在高并发下可能会出现预期外的异常,考虑使用二级缓存
