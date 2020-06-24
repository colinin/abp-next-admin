# LINGYUN.Abp.Sms.Aliyun

abp框架短信发送接口**ISmsSender**的阿里云实现

## 配置使用

模块按需引用，需要引用阿里云公共基础认证模块

事先定义**appsettings.json**文件

```json
{
  "Aliyun": {
    "Auth": {
      "AccessKeyId": "你的阿里云访问标识",
      "AccessKeySecret": "你的阿里云访问密钥"
    },
    "Sms": {
      "RegionId": "cn-hangzhou",
      "Domain": "dysmsapi.aliyuncs.com",
      "Version": "2017-05-25",
      "DefaultSignName": "你的阿里云短信签名,SmsMessage.Properties[SignName]不存在则使用DefaultSignName",
      "DefaultTemplateCode": "你的阿里云短信模板,SmsMessage.Properties[TemplateCode]不存在则使用DefaultTemplateCode",
      "DeveloperPhoneNumber": "这是用于在开发模式下的短信号码，用于统一接收短信",
      "VisableErrorToClient": true
    }
  }
}

```

```csharp
[DependsOn(typeof(AbpAliyunSmsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```