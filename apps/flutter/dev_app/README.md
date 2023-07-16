# dev_app

A new Flutter project.

## Getting Started

This project is a starting point for a Flutter application.

A few resources to get you started if this is your first Flutter project:

- [Lab: Write your first Flutter app](https://docs.flutter.dev/get-started/codelab)
- [Cookbook: Useful Flutter samples](https://docs.flutter.dev/cookbook)

For help getting started with Flutter development, view the
[online documentation](https://docs.flutter.dev/), which offers tutorials,
samples, guidance on mobile development, and a full API reference.

## Environment

* **baseUrl**: 服务器连接地址(必输)
* **clientId**: 客户端标识(必输)
* **clientSecret**: 客户端密钥(可选)
* **authority**: 身份认证服务器地址(必输)
* **uploadFilesUrl**: 上传文件路径(可选)
* **staticFilesUrl**: 静态文件路径(可选)
* **tenantKey**: 多租户标识(可选)
* **defaultLanguage**: 应用程序默认语言, 优先级低于用户配置(可选)
* **notifications**: 通知相关设置(可选)
*  ***android***: android端通知设置(可选)
*    ***channelId***: 通道标识(配置了android节点后为必输)
*    ***channelName***: 通道名称(配置了android节点后为必输)
*    ***channelDescription***: 通道说明(可选)
*  ***linux***: linux端通知设置(可选)
*    ***defaultActionName***: 默认点击方法名称(配置了linux节点后为必输)
*  ***darwin***: iOS/Mac os端通知设置(可选)

```json
{
    "baseUrl": "http://127.0.0.1:30000",
    "clientId": "abp-flutter",
    "clientSecret": "1q2w3e*",
    "authority": "http://127.0.0.1:30000",
    "uploadFilesUrl": "",
    "staticFilesUrl": "",
    "tenantKey": "__tenant",
    "defaultLanguage": "en",
    "notifications": {
        "android": {
            "channelId": "abp-flutter",
            "channelName": "abp-flutter",
            "channelDescription": "适用于Android端的通知通道定义"
        },
        "linux": {
            "defaultActionName": "Open notification"
        }
    }
}
```