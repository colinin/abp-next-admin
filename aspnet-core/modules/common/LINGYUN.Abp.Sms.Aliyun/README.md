# LINGYUN.Abp.Sms.Aliyun

abp框架短信发送接口**ISmsSender**的阿里云实现

## 配置使用

模块按需引用

```csharp
[DependsOn(typeof(AbpAliyunSmsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```
## 配置项说明

*	AliyunSettingNames.Sms.Domain						可选,阿里云sms服务域名,默认 dysmsapi.aliyuncs.com  
*	AliyunSettingNames.Sms.ActionName					必须,调用方法名称,默认 SendSms
*	AliyunSettingNames.Sms.Version						必须,R默认版本号,默认 2017-05-25
*	AliyunSettingNames.Sms.DefaultSignName				可选,默认签名,当用户调用未传递签名时,默认使用的短信签名  
*	AliyunSettingNames.Sms.DefaultTemplateCode			可选,默认短信模板号,,当用户调用未传递短信模板时,默认使用的短信模板
*	AliyunSettingNames.Sms.DefaultPhoneNumber			可选,默认发送号码,当用户调用未传递接收者号码时,默认使用的接收人号码  
*	AliyunSettingNames.Sms.VisableErrorToClient			可选,展示错误给客户端,当调用短信接口出现错误时,是否返回错误明细给客户端,默认 false  

## 其他

网络因素在高并发下可能会出现预期外的异常,考虑使用二级缓存
