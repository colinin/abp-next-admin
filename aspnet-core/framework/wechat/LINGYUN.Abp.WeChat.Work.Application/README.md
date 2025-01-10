# LINGYUN.Abp.WeChat.Work.Application

企业微信应用服务模块，提供企业微信应用开发的应用层服务实现。

## 功能特性

* 通讯录管理服务
* 应用管理服务
* 消息推送服务
* 客户联系服务
* 身份验证服务
* 企业支付服务
* 电子发票服务

## 模块引用

```csharp
[DependsOn(typeof(AbpWeChatWorkApplicationModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 应用服务

### 通讯录管理

* `IWeChatWorkContactService`: 通讯录管理服务
  * 部门管理
  * 成员管理
  * 标签管理
  * 互联企业管理

### 应用管理

* `IWeChatWorkAgentService`: 应用管理服务
  * 应用创建
  * 应用配置
  * 应用可见范围设置
  * 应用主页设置

### 消息推送

* `IWeChatWorkMessageService`: 消息推送服务
  * 文本消息
  * 图片消息
  * 语音消息
  * 视频消息
  * 文件消息
  * 文本卡片消息
  * 图文消息
  * 模板卡片消息

## 更多文档

* [企业微信应用服务模块文档](README.EN.md)
