# LINGYUN.Abp.Tencent.TTS

Tencent Cloud Text-to-Speech (TTS) Service Module, integrating Tencent Cloud TTS service into ABP applications.

## Features

* Support for Tencent Cloud Text-to-Speech (TTS) service
* Multi-tenant configuration support
* Based on Tencent Cloud TTS SDK V20190823
* Provides TTS client factory for dynamic TTS client creation

## Basic Usage

1. Add module dependency
```csharp
[DependsOn(typeof(AbpTencentTTSModule))]
public class YourModule : AbpModule
{
    // ...
}
```

2. Configure Tencent Cloud service
```json
{
  "Settings": {
    "LINGYUN.Abp.Tencent": {
      "SecretId": "Your Tencent Cloud SecretId",
      "SecretKey": "Your Tencent Cloud SecretKey"
    }
  }
}
```

3. TTS service usage example
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
        // Use ttsClient to call Tencent Cloud TTS service APIs
        // For detailed API usage, please refer to Tencent Cloud TTS SDK documentation
    }
}
```

## Configuration Items

### Basic Configuration

```json
{
  "Settings": {
    "Abp.TencentCloud": {
      "SecretId": "Your Tencent Cloud SecretId", // Get from Tencent Cloud Console
      "SecretKey": "Your Tencent Cloud SecretKey", // Get from Tencent Cloud Console
      "DurationSecond": "600" // Session duration in seconds
    }
  }
}
```

### TTS Service Configuration

```json
{
  "Settings": {
    "Abp.TencentCloud.TTS": {
      "AppId": "", // TTS application ID
      "VoiceType": "", // Voice type, default is "0"
      "Language": "1", // Language, 1-Chinese, 2-English
      "Speed": "0", // Speech speed, range: -2~2
      "Volume": "0", // Volume, range: 0~10
      "ProjectId": "0" // Project ID
    }
  }
}
```

## More Documentation

* [Tencent Cloud TTS Service](https://cloud.tencent.com/document/product/1073)
* [Tencent Cloud TTS SDK Documentation](https://cloud.tencent.com/document/product/1073/37927)

[简体中文](./README.md)
