# LINGYUN.Abp.WeChat.Work.Application.Contracts

企业微信应用服务契约模块，提供企业微信应用开发的应用层服务接口定义。

## 功能特性

* 通讯录管理服务接口
* 应用管理服务接口
* 消息推送服务接口
* 客户联系服务接口
* 身份验证服务接口
* 企业支付服务接口
* 电子发票服务接口

## 模块引用

```csharp
[DependsOn(typeof(AbpWeChatWorkApplicationContractsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 服务接口

### 通讯录管理

* `IWeChatWorkContactService`
  * `CreateDepartmentAsync`: 创建部门
  * `UpdateDepartmentAsync`: 更新部门
  * `DeleteDepartmentAsync`: 删除部门
  * `CreateUserAsync`: 创建成员
  * `UpdateUserAsync`: 更新成员
  * `DeleteUserAsync`: 删除成员
  * `CreateTagAsync`: 创建标签
  * `UpdateTagAsync`: 更新标签
  * `DeleteTagAsync`: 删除标签

### 应用管理

* `IWeChatWorkAgentService`
  * `GetAgentAsync`: 获取应用
  * `SetAgentAsync`: 设置应用
  * `GetAgentListAsync`: 获取应用列表
  * `SetWorkbenchTemplateAsync`: 设置工作台模板

### 消息推送

* `IWeChatWorkMessageService`
  * `SendTextAsync`: 发送文本消息
  * `SendImageAsync`: 发送图片消息
  * `SendVoiceAsync`: 发送语音消息
  * `SendVideoAsync`: 发送视频消息
  * `SendFileAsync`: 发送文件消息
  * `SendTextCardAsync`: 发送文本卡片消息
  * `SendNewsAsync`: 发送图文消息
  * `SendTemplateCardAsync`: 发送模板卡片消息

## 权限

* `WeChatWork.Contact`: 通讯录管理
* `WeChatWork.Agent`: 应用管理
* `WeChatWork.Message`: 消息管理
* `WeChatWork.Customer`: 客户管理
* `WeChatWork.Payment`: 企业支付
* `WeChatWork.Invoice`: 电子发票

## 更多文档

* [企业微信应用服务契约模块文档](README.EN.md)
