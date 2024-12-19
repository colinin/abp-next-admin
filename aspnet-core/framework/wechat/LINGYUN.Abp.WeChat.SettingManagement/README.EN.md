# LINGYUN.Abp.WeChat.SettingManagement

WeChat settings management module, providing management functionality for WeChat-related configurations.

## Features

* WeChat configuration management
* Official Account configuration management
* Mini Program configuration management
* WeChat Work configuration management
* Configuration UI integration
* Multi-tenant support

## Module Reference

```csharp
[DependsOn(typeof(AbpWeChatSettingManagementModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Settings Configuration

### Official Account Configuration

* `WeChat.Official.AppId`: Official Account AppId
* `WeChat.Official.AppSecret`: Official Account AppSecret
* `WeChat.Official.Token`: Official Account message Token
* `WeChat.Official.EncodingAESKey`: Official Account message encryption key
* `WeChat.Official.IsSandBox`: Whether in sandbox environment
* `WeChat.Official.Url`: Official Account server URL

### Mini Program Configuration

* `WeChat.MiniProgram.AppId`: Mini Program AppId
* `WeChat.MiniProgram.AppSecret`: Mini Program AppSecret
* `WeChat.MiniProgram.Token`: Mini Program message Token
* `WeChat.MiniProgram.EncodingAESKey`: Mini Program message encryption key
* `WeChat.MiniProgram.IsDebug`: Whether to enable debug mode
* `WeChat.MiniProgram.DefaultEnvironment`: Default environment

### WeChat Work Configuration

* `WeChat.Work.CorpId`: Enterprise ID
* `WeChat.Work.AgentId`: Application ID
* `WeChat.Work.Secret`: Application secret
* `WeChat.Work.Token`: Message Token
* `WeChat.Work.EncodingAESKey`: Message encryption key

## Permissions

* `WeChat.Setting`: WeChat settings management
* `WeChat.Setting.Official`: Official Account settings management
* `WeChat.Setting.MiniProgram`: Mini Program settings management
* `WeChat.Setting.Work`: WeChat Work settings management

## More Documentation

* [Chinese Documentation](README.md)
