# LINGYUN.Abp.WeChat.Work.OA

企业微信办公模块，提供企业微信应用开发的办公模块功能实现。  

## 功能特性


## 模块引用

```csharp
[DependsOn(typeof(AbpWeChatWorkOAModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项


## 消息处理


## 事件处理

* [会议室预定事件](https://developer.work.weixin.qq.com/document/path/95333#%E4%BC%9A%E8%AE%AE%E5%AE%A4%E9%A2%84%E5%AE%9A%E4%BA%8B%E4%BB%B6)
* [会议室取消事件](https://developer.work.weixin.qq.com/document/path/95333#%E4%BC%9A%E8%AE%AE%E5%AE%A4%E5%8F%96%E6%B6%88%E4%BA%8B%E4%BB%B6)
* [删除日历事件](https://developer.work.weixin.qq.com/document/path/97728)
* [修改日历事件](https://developer.work.weixin.qq.com/document/path/97730)
* [修改日程事件](https://developer.work.weixin.qq.com/document/path/97731)
* [删除日程事件](https://developer.work.weixin.qq.com/document/path/97732)
* [日程回执事件](https://developer.work.weixin.qq.com/document/path/98111)


## 更多文档

* [企业微信日程文档](https://developer.work.weixin.qq.com/document/path/93624)
* [企业微信会议室文档](https://developer.work.weixin.qq.com/document/path/93618)
