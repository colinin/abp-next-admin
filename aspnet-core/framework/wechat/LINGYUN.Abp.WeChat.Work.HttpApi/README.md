# LINGYUN.Abp.WeChat.Work.HttpApi

企业微信HTTP API模块，提供企业微信应用开发的HTTP API接口实现。

## 功能特性

* 通讯录管理API
* 应用管理API
* 消息推送API
* 客户联系API
* 身份验证API
* 企业支付API
* 电子发票API
* 企业微信回调接口

## 模块引用

```csharp
[DependsOn(typeof(AbpWeChatWorkHttpApiModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## API接口

### 通讯录管理

* POST /api/wechat/work/contact/department/create
* POST /api/wechat/work/contact/department/update
* DELETE /api/wechat/work/contact/department/{id}
* POST /api/wechat/work/contact/user/create
* POST /api/wechat/work/contact/user/update
* DELETE /api/wechat/work/contact/user/{id}

### 应用管理

* GET /api/wechat/work/agent/{agentId}
* POST /api/wechat/work/agent/set
* GET /api/wechat/work/agent/list
* POST /api/wechat/work/agent/workbench/template

### 消息推送

* POST /api/wechat/work/message/text
* POST /api/wechat/work/message/image
* POST /api/wechat/work/message/voice
* POST /api/wechat/work/message/video
* POST /api/wechat/work/message/file
* POST /api/wechat/work/message/textcard
* POST /api/wechat/work/message/news
* POST /api/wechat/work/message/templatecard

### 回调接口

* POST /api/wechat/work/callback/{corpId}

## 权限

* `WeChatWork.Contact`: 通讯录管理
* `WeChatWork.Agent`: 应用管理
* `WeChatWork.Message`: 消息管理
* `WeChatWork.Customer`: 客户管理
* `WeChatWork.Payment`: 企业支付
* `WeChatWork.Invoice`: 电子发票

## 更多文档

* [企业微信HTTP API模块文档](README.EN.md)
