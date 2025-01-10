# LINGYUN.Abp.Tencent.TTS

腾讯云语音合成服务模块，集成腾讯云语音合成服务到ABP应用程序。

## 功能特性

* 支持腾讯云语音合成服务（TTS）
* 支持多租户配置
* 基于腾讯云TTS SDK V20190823
* 提供TTS客户端工厂，支持动态创建TTS客户端

## 基本用法

1. 添加模块依赖
```csharp
[DependsOn(typeof(AbpTencentTTSModule))]
public class YourModule : AbpModule
{
    // ...
}
```

2. 配置腾讯云服务
```json
{
  "Settings": {
    "LINGYUN.Abp.Tencent": {
      "SecretId": "您的腾讯云SecretId",
      "SecretKey": "您的腾讯云SecretKey"
    }
  }
}
```

3. 使用TTS服务示例
```csharp
public class YourService
{
    private readonly TencentCloudTTSClientFactory _ttsClientFactory;

    public YourService(TencentCloudTTSClientFactory ttsClientFactory)
    {
        _ttsClientFactory = ttsClientFactory;
    }

    public async Task TextToSpeechAsync(string text)
    {
        var ttsClient = await _ttsClientFactory.CreateAsync();
        // 使用ttsClient调用腾讯云TTS服务API
        // 详细API使用方法请参考腾讯云TTS SDK文档
    }
}
```

## 配置项说明

### 基础配置

```json
{
  "Settings": {
    "Abp.TencentCloud": {
      "SecretId": "您的腾讯云SecretId", // 从腾讯云控制台获取
      "SecretKey": "您的腾讯云SecretKey", // 从腾讯云控制台获取
      "DurationSecond": "600" // 会话持续时间（秒）
    }
  }
}
```

### TTS服务配置

```json
{
  "Settings": {
    "Abp.TencentCloud.TTS": {
      "AppId": "", // TTS应用ID
      "VoiceType": "", // 音色，默认为"0"
      "Language": "1", // 语言，1-中文，2-英文
      "Speed": "0", // 语速，范围：-2~2
      "Volume": "0", // 音量，范围：0~10
      "ProjectId": "0" // 项目ID
    }
  }
}
```

## 更多文档

* [腾讯云语音合成服务](https://cloud.tencent.com/document/product/1073)
* [腾讯云TTS SDK文档](https://cloud.tencent.com/document/product/1073/37927)

[English](./README.EN.md)
