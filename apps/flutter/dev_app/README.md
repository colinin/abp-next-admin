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

## Translations

### 预制本地化文档    
* [English](./translation.json)   
* [简体中文](./translation.zh-Hans.json)

### 使用方法

- 本地文件    

 **替换 [AbpTranslations](./lib/utils/localization.dart) 中keys为如下格式：**

	```json
	{
		"zh-Hans": {
			"Center:Feedback": "意见反馈",
			"Center:Help": "在线帮助",
			"Center:Info": "关于",
			...其他本地化内容
		},
		"en": {
			"Center:Feedback": "Feedback",
			"Center:Help": "Help",
			"Center:Info": "Info",
			...other localizations,
		},
	}
	```
	
- 后台服务    

	**将预制的本地化文档复制到后台服务的本地化目录即可, 具体见abp多语言文档**

## Environment 环境配置

> **application** 应用程序配置    
>>	*name* 应用程序名称   

> **auth** 身份认证服务器配置    
>>	*<font color=red size=4>authority</font>* 身份认证服务器地址   
>>	*<font color=red size=4>clientId</font>* 客户端标识   
>>	*clientSecret* 客户端密钥    

> **tenant** 多租户配置    
>>	*tenantKey* 多租户标识    

> **localization** 国际化配置    
>>	*defaultLanguage* 应用程序默认语言

> **remoteServices** 远程服务配置     
>>	<font color=red size=4>default</font> 默认连接配置 <font color=red size=4>必须指定</font>  
>>> url 连接地址    
>>> rootNamespace 根命名空间（保留配置）    

>>	<font color=red size=4>avatar</font> 头像接口配置（如果用户头像设定为相对路径, 需要指定接口地址前缀）  
>>> url 连接地址    

> **remoteEnvironment** 远程环境配置（按照给定的应用策略替换当前环境配置）   
>>	*url* 连接地址（从此连接拉取配置信息）    
>>	*method* 请求方法    
>>	*headers* 请求头    
>>	*strategy* 应用策略，deepmerge（合并）、overwrite（替换）     

> **notifications** 通知相关设置
>>  *android* android端通知设置(可选)
>>>    - *channelId*: 通道标识(配置了android节点后为必输)    
>>>    - *channelName*: 通道名称(配置了android节点后为必输)    
>>>    - *channelDescription*: 通道说明(可选)    

>>  *linux* linux端通知设置(可选)
>>>    - *defaultActionName*: 默认点击方法名称(配置了linux节点后为必输)    

>>  *darwin* iOS/Mac os端通知设置(可选)


# 配置示例
[demo.json](./res/config/demo.json)
```json
{
    "application": {
        "name": "app-flutter"
    },
    "auth": {
        "clientId": "abp-flutter",
        "clientSecret": "1q2w3e*",
        "authority": "http://127.0.0.1:30000"
    },
    "tenant": {
        "tenantKey": "__tenant"
    },
    "localization": {
        "defaultLanguage": "zh-Hans"
    },
    "remoteServices": {
        "default": {
            "url": "http://127.0.0.1:30000"
        },
        "avatar": {
            "url": "http://127.0.0.1:30000/api/files/static/users/p/"
        }
    },
    "remoteEnvironment": {
        "url": "",
        "strategy": "deepmerge"
    },
    "notifications": {
        "serverUrl": "http://127.0.0.1:30000/signalr-hubs/notifications",
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