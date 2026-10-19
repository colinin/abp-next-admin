# 通知渠道数据 — 使用指南

> 框架设计说明见 [CHANNEL-DATA.md](./CHANNEL-DATA.md)

---

## 0. 三个接入点

| 角色 | 用哪个 API | 典型位置 |
|---|---|---|
| 生产者(业务代码) | `INotificationChannelDataSerializer.Write` 或渠道扩展方法 | 发送通知的 AppService / DomainService |
| 通知定义 | `NotificationDefinition.WithProviders(...)` | `NotificationDefinitionProvider` |
| 渠道实现 | `INotificationChannelDataResolver` | 各 `*NotificationPublishProvider` |

渠道数据是"生产者 → 渠道"的**单向约定**；通知定义负责决定**哪些渠道会收到**。
两者是独立的：定义里没声明该渠道，渠道根本不会被调用。

---

## 1. 场景一：企业微信渠道

### 1.1 最简单——只用标准数据

不写任何渠道数据，企业微信渠道会按通知定义的 `ContentType` 自动推导载体：

| 定义的内容类型 | 自动生成的载体 |
|---|---|
| `Text` | 文本消息(取 `message`) |
| `Html` | 文本卡片消息(取 `title`/`description`/`message`) |
| `Markdown` | markdown消息(取 `message`) |

```csharp
public class MyAppService : ApplicationService
{
    private readonly INotificationSender _notificationSender;

    public MyAppService(INotificationSender notificationSender)
    {
        _notificationSender = notificationSender;
    }

    public async Task NotifyAsync(Guid userId)
    {
        var data = new NotificationData();
        data.WriteStandardData(
            title: "构建完成",
            message: "**v1.2.0** 已部署到生产环境",
            createTime: Clock.Now,
            formUser: CurrentUser.UserName ?? "system");

        await _notificationSender.SendNofiterAsync(
            name: "BuildFinished",
            data: data,
            user: new UserIdentifier(userId, "admin"));
    }
}
```

### 1.2 指定接收人 / 保密 / 去重

写入"投递选项"。这些选项在**持久化与失败重试后依然生效**：

```csharp
var data = new NotificationData();
data.WriteStandardData("标题", "内容", Clock.Now, "system");

data.TrySetData(WeChatWorkNotificationDataKeys.ToParty, "1|2");             // 指定部门
data.TrySetData(WeChatWorkNotificationDataKeys.ToTag, "3");                 // 指定标签
data.TrySetData(WeChatWorkNotificationDataKeys.Safe, true);                 // 保密消息
data.TrySetData(WeChatWorkNotificationDataKeys.EnableIdTrans, true);        // 开启 id 转译
data.TrySetData(WeChatWorkNotificationDataKeys.EnableDuplicateCheck, true);
data.TrySetData(WeChatWorkNotificationDataKeys.DuplicateCheckInterval, 600); // 秒

await _notificationSender.SendNofiterAsync("BuildFinished", data, user);
```

> 键名带本渠道前缀(`wx-work:`)；读取大小写不敏感。
> 成员(`touser`)由企业微信身份绑定关系自动解析，**不由通知数据指定**。
> `touser` / `toparty` / `totag` 三者不能同时为空。

### 1.3 文本卡片

```csharp
channelDataSerializer.Write(data, new WeChatWorkTextCardChannelData
{
    TextCard = new TextCardMessage(
        title: "审批通过",
        description: "您的请假申请已通过，请前往查看",
        url: "https://example.com/leave/123",
        buttonText: "去查看")
});
```

> `channelDataSerializer` 是注入的 `INotificationChannelDataSerializer`。
> 它按载荷类型上标注的 <xref:NotificationChannelPropertyAttribute> 写入对应键，
> 生产侧与渠道侧共用同一份契约定义。

### 1.4 模板卡片(推荐用于审批/告警类)

```csharp
var card = new TextTemplateCard(
    new TemplateCardCardAction("https://example.com/approval/123"))
{
    MainTitle = new TemplateCardMainTitle("请假通过通知", "您的请假申请已通过"),
    SubTitle = "请及时处理后续交接事项",
    HorizontalContents = new List<TemplateCardHorizontalContent>
    {
        TemplateCardHorizontalContent.None("审批单号", "QJ20251000000136"),
        TemplateCardHorizontalContent.None("请假日期", "2025/10/01-2025/10/10"),
        TemplateCardHorizontalContent.None("通过时间", "2025-10-01 15:30:00"),
        // 也可以带跳转/素材/人员
        TemplateCardHorizontalContent.FromUrl("详情", "https://example.com/oa/123", "点击查看"),
    },
    Jumps = new List<TemplateCardJump>
    {
        new TemplateCardJump("去OA查看", "https://example.com/oa/123"),
    }
};

channelDataSerializer.Write(data, new WeChatWorkTemplateCardChannelData { TemplateCard = card });
```

> 可用的 `TemplateCardHorizontalContent` 工厂：
> `None(keyName, value)`、`FromUrl(keyName, url, value)`、
> `FromMedia(keyName, mediaId, value)`、`FromUser(keyName, userId, value)`。
> `TemplateCardJump` 支持"跳转链接"与"跳转小程序"两个构造函数。
> 完整可运行示例见你仓库中的
> `tests/LINGYUN.Abp.WeChat.Work.Tests/.../WeChatWorkMessageSender_Tests.cs`。

### 1.5 小程序通知

```csharp
channelDataSerializer.Write(data, new WeChatWorkMiniProgramNoticeChannelData
{
    MiniProgram = new MiniProgramMessage(
        appId: "wx1234567890",
        title: "订单已发货",
        page: "pages/order/detail?id=123",
        description: "点击查看物流详情")
    {
        EmphasisFirstItem = true,
        ContentItems = new List<MiniProgramContent>
        {
            new MiniProgramContent("订单号", "SO20251001001"),
            new MiniProgramContent("物流公司", "顺丰速运"),
        }
    }
});
```

### 1.6 图文 / 视频 / 语音 / 文件

```csharp
channelDataSerializer.Write(data, new WeChatWorkMpNewChannelData
{
    MpNewMessage = new MpNewMessagePayload(new List<MpNewMessage>
    {
        new MpNewMessage(
            title: "月度报告",
            thumbMediaId: "MEDIA_ID",
            content: "<p>报告正文</p>",
            author: "运营部",
            contentSourceUrl: "https://example.com/report",
            description: "2025年10月经营月报")
    })
});

channelDataSerializer.Write(data, new WeChatWorkVideoChannelData
{
    Video = new VideoMessage("MEDIA_ID", "培训视频", "新人入职培训")
});

channelDataSerializer.Write(data, new WeChatWorkVoiceChannelData
{
    Voice = new MediaMessage("MEDIA_ID")
});

channelDataSerializer.Write(data, new WeChatWorkFileChannelData
{
    File = new MediaMessage("MEDIA_ID")
});
```

### 1.7 可用的载荷类型与选项支持

生产者只需选择合适的载荷类型；选项由渠道按载体能力自动取舍（不支持的字段**不会被设置**）。

| 载荷类型 | 载体 | `safe` | `enable_id_trans` | `enable_duplicate_check` |
|---|---|---|---|---|
| `WeChatWorkTextCardChannelData` | 文本卡片 | ❌ | ✅ | ✅ |
| `WeChatWorkTemplateCardChannelData` | 模板卡片 | ❌ | ✅ | ✅ |
| `WeChatWorkMiniProgramNoticeChannelData` | 小程序通知 | ❌ | ✅ | ✅ |
| `WeChatWorkMpNewChannelData` | 图文(mpnews) | ✅ | ✅ | ✅ |
| `WeChatWorkVideoChannelData` | 视频 | ✅ | ❌ | ✅ |
| `WeChatWorkFileChannelData` | 文件 | ✅ | ❌ | ✅ |
| `WeChatWorkVoiceChannelData` | 语音 | ❌ | ❌ | ✅ |
| (无，按 `ContentType=Markdown` 推导) | markdown | ❌ | ❌ | ✅ |
| (无，按 `ContentType=Text` 推导) | 文本 | ✅ | ✅ | ✅ |

### 1.8 多个载荷同时存在怎么处理

**全部都会发送**，顺序由载荷的 `Priority` 决定(固定值，见下表)：

| 载荷 | Priority |
|---|---|
| 小程序通知 | 100 |
| 图文(mpnews) | 200 |
| 模板卡片 | 300 |
| 文本卡片 | 400 |
| 视频 | 500 |
| 语音 | 600 |
| 文件 | 700 |

即"同时设置文本卡片与小程序通知"会**发两条消息**。如果只想发一条，只写一个载荷。
只有当**没有任何显式载荷**时，才回退到按 `ContentType` 推导的默认载体。

---

## 2. 场景二：其他内置渠道

### 2.1 微信小程序订阅消息

> 注意：`TemplateId` 等键沿用历史命名(**无前缀**)，读取已改为**大小写不敏感**。

```csharp
var data = new NotificationData();
data.WriteStandardData("订单发货", "您的订单已发货", Clock.Now, "system");

// 键带本渠道前缀 wx-mp:; 不写则用 AbpNotificationsWeChatMiniProgramOptions 的默认值
data.TrySetData(WeChatMiniProgramNotificationDataKeys.TemplateId, "TEMPLATE_ID");
data.TrySetData(WeChatMiniProgramNotificationDataKeys.RedirectPage, "pages/order/detail?id=123");
data.TrySetData(WeChatMiniProgramNotificationDataKeys.WeAppState, "developer");
data.TrySetData(WeChatMiniProgramNotificationDataKeys.WeAppLanguage, "zh_CN");

// 模板数据本身就取自属性包，按微信模板字段名写入
data.TrySetData("character_string1", "SO20251001001");
data.TrySetData("thing2", "顺丰速运");

await _notificationSender.SendNofiterAsync("OrderShipped", data, user);
```

> 模板数据键(`character_string1`、`thing2` …)由**微信模板定义**决定,
> 不属于渠道自有键, 因此不加前缀。
>
> 早期版本使用的无前缀键(`TemplateId` 等)由渠道只读回退兼容
> (见 `WeChatMiniProgramNotificationDataKeys.Legacy`), 新代码请使用带前缀的常量。

### 2.2 WxPusher

```csharp
// 方式一: 用序列化器(推荐)
channelDataSerializer.Write(data, new WxPusherChannelData
{
    Url = "https://example.com/detail/123"
});

// 方式二: 直接写键
data.TrySetData(WxPusherNotificationDataKeys.Url, "https://example.com/detail/123");
```

### 2.3 PushPlus

```csharp
channelDataSerializer.Write(data, new PushPlusChannelData
{
    Webhook = "https://example.com/hook",
    CallbackUrl = "https://example.com/callback"
});
```

> PushPlus 的 `Presence` 是 `AnyDeclaredProperty`: 写入其中任意一个键即视为声明了载荷。

### 2.4 短信 / 邮件 / SignalR

**无渠道专属键**，只需要标准数据：

```csharp
var data = new NotificationData();
data.WriteStandardData("告警", "CPU 使用率超过 90%", Clock.Now, "monitor");

await _notificationSender.SendNofiterAsync(NotifyNames.SystemAlarm, data, user);
```

- 短信：只取 `message`
- 邮件：`title` 作为主题，`message` 作为正文（`ContentType=Markdown` 时自动转 HTML）
- SignalR：直接推送整个 `NotificationData`，由客户端解释

---

## 3. 场景三：企业微信 Webhook（群机器人）

项目：`LINGYUN.Abp.Notifications.Webhook.WeChat.Work`

### 3.1 与企业微信应用消息的区别

| | 应用消息 | Webhook（群机器人）|
|---|---|---|
| 渠道名（通知定义里声明） | `WeChat.Work` | `Webhook` |
| 载荷注册表键 | `WeChat.Work` | `WeChat.Work.Webhook` |
| 数据键前缀 | `wx-work:` | `wx-work-hook:` |
| 收件人 | 由身份绑定解析（`touser`） | 由 Webhook 定义决定（群）|

> **为什么要分成两个渠道**：两者是不同传输方式、不同载体模型。
> 若共用注册表键与键前缀，同一个通知被两个渠道同时订阅时，
> 载荷会被两边都解析并各发一条消息。

### 3.2 定义侧

```csharp
context.AddGroup("Monitor", L("Monitor"))
    .AddNotification("SystemAlarm", L("SystemAlarm"))
    .WithProviders(
        "Webhook");   // WebhookNotificationPublishProvider.ProviderName
```

Webhook 的投递目标由**订阅关系**决定（`IWebhookPublisher`），
通知数据只负责"发什么"。

### 3.3 生产者侧

```csharp
var data = new NotificationData();
data.WriteStandardData("构建失败", "构建 #1234 失败", Clock.Now, "ci");

// 用序列化器写载荷(推荐)
channelDataSerializer.Write(data, new WeChatWorkWebhookTextChannelData
{
    Content = "构建 #1234 失败，请查看日志"
});

// 也可以直接写键
data.TrySetData(WeChatWorkWebhookNotificationDataKeys.Markdown, "**构建失败**");
```

### 3.4 可用的载荷类型

| 载荷类型 | `msgtype` | 用途 |
|---|---|---|
| `WeChatWorkWebhookTextChannelData` | `text` | 纯文本 |
| `WeChatWorkWebhookMarkdownChannelData` | `markdown` | markdown |
| `WeChatWorkWebhookMarkdownV2ChannelData` | `markdown_v2` | markdown_v2（支持表格、代码块）|
| `WeChatWorkWebhookImageChannelData` | `image` | 图片（base64 + md5）|
| `WeChatWorkWebhookNewsChannelData` | `news` | 图文 |
| `WeChatWorkWebhookFileChannelData` | `file` | 文件（素材 ID）|
| `WeChatWorkWebhookVoiceChannelData` | `voice` | 语音（素材 ID）|
| `WeChatWorkWebhookTemplateCardChannelData` | `template_card` | 模板卡片 |

**每种载荷 1:1 映射到自己的 `msgtype`，不做隐式降级。**
（改造前只支持 `text` / `markdown` / `markdown_v2` 三种，其余内容类型被静默丢弃。）

模板卡片示例：

```csharp
channelDataSerializer.Write(data, new WeChatWorkWebhookTemplateCardChannelData
{
    TemplateCard = new WebhookTextNoticeCardMessage(
        action: WebhookTemplateCardAction.Link("https://example.com/build/1234"),
        mainTitle: new WebhookTemplateCardMainTitle("构建失败", "请及时处理"),
        subTitleText: "构建 #1234 于 15:30 失败",
        horizontalContents: new List<WebhookTemplateCardHorizontalContent>
        {
            WebhookTemplateCardHorizontalContent.Default("分支", "main"),
            WebhookTemplateCardHorizontalContent.Default("提交", "a1b2c3d"),
        })
});
```

图文消息示例：

```csharp
channelDataSerializer.Write(data, new WeChatWorkWebhookNewsChannelData
{
    News = new WebhookNewsMessage(new List<WebhookArticleMessage>
    {
        new WebhookArticleMessage(
            title: "月度经营报告",
            url: "https://example.com/report",
            description: "2025年10月",
            pictureUrl: "https://example.com/cover.png")
    })
});
```

### 3.5 向后兼容

**载荷缺失时**回退到按 `ContentType` 推导（即改造前的行为）：

| `ContentType` | 推导出的载体 |
|---|---|
| `Text` | `text` |
| `Html` | HTML → markdown，再按 `UseMarkdownV2` 决定 `markdown_v2` 或 `markdown` |
| `Markdown` | 按 `UseMarkdownV2` 决定 `markdown_v2` 或 `markdown` |

因此只写标准数据的历史生产者**无需改动**。

`UseMarkdownV2` 默认 `true`，可在模块的 `PreConfigure` 中覆盖：

```csharp
[DependsOn(typeof(AbpNotificationsWebhookWeChatWorkModule))]
public class MyModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpNotificationsWebhookWeChatWorkOptions>(options =>
        {
            options.UseMarkdownV2 = false;
        });
    }
}
```

### 3.6 一个 Webhook 只承载一条消息

声明多个载荷时取 `Priority` 最小的那个：

| 载荷 | Priority |
|---|---|
| 文本 | 100 |
| markdown | 200 |
| markdown_v2 | 300 |
| 图片 | 400 |
| 图文 | 500 |
| 文件 | 600 |
| 语音 | 700 |
| 模板卡片 | 800 |

> 这与企业微信**应用消息**渠道不同——后者多个载荷会**发多条**消息。
> Webhook 一次发布对应一个订阅回调，因此只能承载一条。

---

## 4. 场景四：自定义渠道

以一个"钉钉机器人"渠道为例。

### 4.1 定义载荷

```csharp
using LINGYUN.Abp.Notifications;

namespace MyProject.Notifications.DingTalk;

public static class DingTalkNotificationDataKeys
{
    public const string Prefix = "dingtalk:";
    public const string Webhook = Prefix + "webhook";
    public const string AtMobiles = Prefix + "at_mobiles";
}

[NotificationChannelData(DingTalkNotificationPublishProvider.ProviderName, Priority = 100)]
public class DingTalkChannelData : NotificationChannelData
{
    [NotificationChannelProperty(DingTalkNotificationDataKeys.Webhook)]
    public virtual string? Webhook { get; set; }

    [NotificationChannelProperty(DingTalkNotificationDataKeys.AtMobiles)]
    public virtual string? AtMobiles { get; set; }
}
```

> ✅ **不需要实现 `IsSet`**。
> 存在性由 `Presence` 声明式推导(默认"参与匹配的属性全部存在")。
> 若希望"只写 atMobiles 也算声明", 改为:
> `[NotificationChannelData(..., Presence = NotificationChannelDataPresenceMode.AnyDeclaredProperty)]`。
>
> 若某属性只是可选读取项(不参与存在性判定), 给它加 `IgnoreWhenMatching = true`。
>
> 若既未声明 `Presence`、又没有任何可匹配属性, **注册时会抛
> `AbpNotificationChannelDataConfigurationException`**。这是刻意设计:
> 把"忘了定义存在性"从生产环境静默丢消息变成启动即失败。
>
> 为什么不用"抽象静态方法强制实现"? C# 不允许 `static` 成员标记为 `abstract`(CS0112),
> 基类无法强制派生类实现静态成员; 而提供"默认返回 false"的兜底实现会导致
> **忘记实现者静默失效**。详见 [CHANNEL-DATA.md §3](./CHANNEL-DATA.md)。

### 4.2 注册

渠道模块在自己的 `AbpModule` 中用 `Configure<>` 声明载荷:

```csharp
using LINGYUN.Abp.Notifications;
using Volo.Abp.Modularity;

namespace MyProject.Notifications.DingTalk;

[DependsOn(typeof(AbpNotificationsModule))]
public class AbpNotificationsDingTalkModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpNotificationsPublishOptions>(options =>
        {
            options.PublishProviders.Add<DingTalkNotificationPublishProvider>();
        });

        // 声明本渠道支持的载荷形态(注册期即校验存在性契约)
        Configure<AbpNotificationChannelDataOptions>(options =>
        {
            options.Add<DingTalkChannelData>();
        });
    }
}
```

> `AbpNotificationChannelDataOptions` 是纯数据对象, 注册期的存在性校验只依赖声明元数据、
> 不依赖任何服务, 因此 `Add()` 不要求任何初始化顺序, 也不需要额外的配置器类。

### 4.3 在渠道实现中解析

```csharp
public class DingTalkNotificationPublishProvider : NotificationPublishProvider
{
    public const string ProviderName = "DingTalk";
    public override string Name => ProviderName;

    protected INotificationChannelDataResolver ChannelDataResolver
        => ServiceProvider.LazyGetRequiredService<INotificationChannelDataResolver>();

    protected async override Task PublishAsync(
        NotificationPublishContext context,
        CancellationToken cancellationToken = default)
    {
        var data = context.Notification.Data;

        // ① 载荷已声明但解析失败 => 生产者数据非法，明确报错而不是静默回退
        if (ChannelDataResolver.IsDeclared(data, typeof(DingTalkChannelData))
            && ChannelDataResolver.Resolve<DingTalkChannelData>(data) is not { } payload)
        {
            context.Cancel("The dingtalk payload can not be parsed, please check the notification data.");
            Logger.LogWarning(context.Reason);
            return;
        }

        // ② 未声明则走默认行为
        var webhook = ChannelDataResolver.Resolve<DingTalkChannelData>(data)?.Webhook;
        if (webhook.IsNullOrWhiteSpace())
        {
            context.Cancel("Unable to send dingtalk messages because the webhook is not set.");
            return;
        }

        // ③ 需要标量选项时用类型化读取(不要用 as)
        var timeout = ChannelDataResolver.GetValue<int>(data, "dingtalk:timeout") ?? 30;

        await SendAsync(webhook, context, cancellationToken);
    }
}
```

### 4.4 定义侧(必须声明渠道，否则渠道不会被调用)

```csharp
public class MyNotificationDefinitionProvider : NotificationDefinitionProvider
{
    public override void Define(INotificationDefinitionContext context)
    {
        context.AddGroup("MyGroup", L("MyGroup"))
            .AddNotification("OrderShipped", L("OrderShipped"))
            .WithProviders(
                DingTalkNotificationPublishProvider.ProviderName,
                NotificationProviderNames.SignalR);
    }
}
```

### 4.5 生产者侧

```csharp
var data = new NotificationData();
data.WriteStandardData("订单发货", "您的订单已发货", Clock.Now, "system");

// 载荷由类型声明驱动(推荐)
channelDataSerializer.Write(data, new DingTalkChannelData
{
    Webhook = "https://oapi.dingtalk.com/robot/send?access_token=xxx",
    AtMobiles = "13800000000"
});

await _notificationSender.SendNofiterAsync("OrderShipped", data, user);
```

> **不要为渠道添加扩展方法**(如 `WithDingTalkRobot`)。
> 本框架已移除所有渠道专属的写入扩展方法: 载荷类型 + 序列化器
> 已经提供了带前缀、可校验、可发现的统一入口,
> 再叠一层扩展方法只会造成两套并行的写入约定。

---

## 5. 场景五：从旧写法迁移

### 5.1 渠道专属扩展方法已全部移除

早期版本在渠道模块中提供了一组写入/读取扩展方法:

| 已移除 | 取代方式 |
|---|---|
| `data.WithTextCardMessage(...)`、`WithTemplateCardMessage`、`WithMiniProgramMessage`、`WithMpNewMessage`、`WithVideoMessage`、`WithVoiceMessage`、`WithFileMessage` | `channelDataSerializer.Write(data, new WeChatWorkXxxChannelData { ... })` |
| `data.SetSafe(...)`、`SetEnableIdTrans`、`SetEnableDuplicateCheck`、`SetDuplicateCheckInterval` | `data.TrySetData(WeChatWorkNotificationDataKeys.Xxx, value)` |
| `data.SetParty(...)`、`SetTag(...)` | `data.TrySetData(WeChatWorkNotificationDataKeys.ToParty / ToTag, value)` |
| `data.SetUrl(...)` (WxPusher) | `data.TrySetData(WxPusherNotificationDataKeys.Url, value)` 或 `channelDataSerializer.Write(data, new WxPusherChannelData { ... })` |
| `data.SetWebhook(...)`、`SetCallbackUrl(...)` (PushPlus) | `data.TrySetData(PushPlusNotificationDataKeys.Webhook / CallbackUrl, value)` |
| `GetSafe()`、`GetUrlOrNull()`、`TryGetMessagePayload()` 等读取方法 | `INotificationChannelDataResolver` |

**键常量(`XxxNotificationDataKeys`)全部保留** —— 它们定义的是数据契约,
不是便捷 API。渠道的 `NotificationDefinition` 扩展方法(如
`WithAgentId`、`WithTag`)是**定义侧** API, 与数据键无关, 同样保留。

### 5.2 渠道实现：替换裸转换

```csharp
// ❌ 旧写法 —— 持久化后必然失效，且无日志
var safe = data.TryGetData("wx-work:safe") as byte? ?? 0;
var url = data.TryGetData("wx-pusher:url")?.ToString();

// ✅ 新写法(注入 INotificationChannelDataResolver)
var safe = channelDataResolver.GetValue<bool>(data, WeChatWorkNotificationDataKeys.Safe);
var payload = channelDataResolver.Resolve<WxPusherChannelData>(data);
var url = payload?.Url;
```

### 5.3 破坏性变更清单

| 变更 | 影响 |
|---|---|
| **渠道专属扩展方法已全部删除** | `NotificationDataWeChatWorkExtensions`、WxPusher / PushPlus 的 `NotificationDataExtensions` 均已移除。传递方式见 §4.1 对照表 |
| **`NotificationChannelValues` 静态类已删除** | 全部能力改为实例服务: `INotificationChannelDataAccessor`(转换) 与 `INotificationChannelDataResolver`(渠道读取)。静态 API 无处可查即编译失败 |
| 载荷不再需要(也不应再写) `IsSet` | 存在性改为 `Presence` 声明式; 旧 `IsSet` 不会被调用 |
| `NotificationChannelDataSerializer.AddJsonConverter` 已移除 | 改为 `INotificationChannelDataJsonSerializer`; 抽象载荷的转换器注册到 JSON 选项 |
| 企业微信 `agentId` 数据键 | 键常量保留(`WeChatWorkNotificationDataKeys.AgentId`); 仅写入扩展方法被移除 |

> **保留**: `XxxNotificationDataKeys`(数据契约)、
> `NotificationDefinition` 的渠道扩展方法(定义侧 API, 如 `WithAgentId`、`WithTag`)、
> 各渠道的 `NotificationDefinitionProvider`。

### 5.4 读取迁移对照表

| 旧扩展方法 | 新写法 |
|---|---|
| `data.GetSafe()` | `resolver.GetValue(data, WeChatWorkNotificationDataKeys.Safe, false)` |
| `data.GetEnableIdTrans()` / `GetEnableDuplicateCheck()` | `resolver.GetValue(data, WeChatWorkNotificationDataKeys.EnableIdTrans / EnableDuplicateCheck, false)` |
| `data.GetDuplicateCheckInterval()` | `resolver.GetValue(data, WeChatWorkNotificationDataKeys.DuplicateCheckInterval, 1800)` |
| `data.GetAgentIdOrNull()` / `GetTagOrNull()` / `GetPartyOrNull()` | `resolver.GetOrNull(data, WeChatWorkNotificationDataKeys.AgentId / ToTag / ToParty)` |
| `data.GetUrlOrNull()` (WxPusher) | `resolver.Resolve<WxPusherChannelData>(data)?.Url` |
| `data.GetWebhookOrNull()` (PushPlus) | `resolver.Resolve<PushPlusChannelData>(data)?.Webhook` |
| `data.GetCallbackUrlOrNull()` (PushPlus) | `resolver.Resolve<PushPlusChannelData>(data)?.CallbackUrl` |
| `data.TryGetMessagePayload<T>(key, out var m)` | `resolver.Resolve<TChannelData>(data)` |

---

## 6. 常见问题

| 现象 | 原因 | 处理 |
|---|---|---|
| 渠道没被调用 | 通知定义没声明该渠道名 | `WithProviders(...)` 或发送时传 `useProviders` |
| 载荷"写了但没生效" | 存在性判定把该键排除了(标了 `IgnoreWhenMatching`，或 `Presence` 与预期不符) | 检查 `Presence` 与 `IgnoreWhenMatching` 标注 |
| 启动即抛 `AbpNotificationChannelDataConfigurationException` | 未声明 `Presence` 且没有可匹配属性；或同渠道 `Priority` 重复；或缺 `[NotificationChannelData]` | 按异常信息修正 |
| 同时收到两条企业微信消息 | 写了多个载荷，每个都会发送 | 只保留一个载荷 |
| 模板卡片报 `abstract class and cannot be instantiated` | 抽象类型缺少 JSON 转换器 | 由 `AbpNotificationsWeChatWorkModule` 自动注册到 `AbpSystemTextJsonSerializerOptions`；手工构建容器时需自行注册 |
| 生产环境静默丢消息 | 载荷数据非法(解析失败) | 渠道侧用 `IsDeclared` + `Resolve` 组合明确报错；日志中会输出失败键名 |

---

## 7. 单元测试

### 7.1 按模块拆分

测试项目与模块一一对应。**框架契约**与**具体渠道**分开测：

| 测试项目 | 覆盖范围 |
|---|---|
| `tests/LINGYUN.Abp.Notifications.ChannelData.Tests` | **框架契约**: 弱类型转换、存在性判定(三种模式)、注册期校验、优先级排序、JSON 往返。用本文件内的 stub 载荷 |
| `tests/LINGYUN.Abp.Notifications.WeChat.Work.Tests` | 企业微信**应用消息**渠道 |
| `tests/LINGYUN.Abp.Notifications.Webhook.WeChat.Work.Tests` | 企业微信 **Webhook** 渠道 |
| `tests/LINGYUN.Abp.Notifications.WeChat.MiniProgram.Tests` | 微信小程序订阅消息渠道 |
| `tests/LINGYUN.Abp.Notifications.WxPusher.Tests` | WxPusher 渠道 |
| `tests/LINGYUN.Abp.Notifications.PushPlus.Tests` | PushPlus 渠道 |

### 7.2 统一的测试写法

每个渠道测试都走同一条路径：**先 new 消息对象 → 通过 `IJsonSerializer` 序列化写入通知数据 → 渠道侧验证**。

```csharp
public class WeChatWorkChannelData_Tests : AbpNotificationsWeChatWorkTestsBase
{
    [Fact]
    public void Should_Resolve_TextCard_Message()
    {
        // 1. 先 new 消息对象
        var message = new TextCardMessage(
            title: "审批通过",
            description: "您的请假申请已通过",
            url: "https://example.com/leave/123",
            buttonText: "去查看");

        // 2. 经 IJsonSerializer 序列化写入通知数据的载荷键
        var data = WriteChannelData(new WeChatWorkTextCardChannelData { TextCard = message });

        // 3. 渠道侧验证还原结果
        var payload = ChannelDataResolver.Resolve<WeChatWorkTextCardChannelData>(data);

        payload.ShouldNotBeNull();
        payload!.TextCard!.Title.ShouldBe(message.Title);
        payload.TextCard.Url.ShouldBe(message.Url);
    }
}
```

公共基类 `NotificationChannelDataTestsBase<TStartupModule>`(位于
`tests/LINGYUN.Abp.Notifications.Tests`)提供:

| 成员 | 说明 |
|---|---|
| `JsonSerializer` | ABP 的 `IJsonSerializer`，用于断言 JSON 契约 |
| `ChannelDataAccessor` | 生产侧写入(序列化) |
| `ChannelDataResolver` | 渠道侧解析 |
| `ChannelDataSerializer` | 生产者侧序列化器 |
| `Serialize(payload)` | 仅序列化，用于断言字段名契约 |
| `WriteChannelData(channelData)` | new → 序列化 → 写入，返回 `NotificationData` |
| `MergeChannelData(data, channelData)` | 合并多个载荷到同一份通知数据 |

> 测试基座是真实的 ABP 集成测试(`AbpIntegratedTest`)，因此 `IJsonSerializer`
> 就是生产环境那一份 —— 包含全局 JSON 策略与渠道注册的转换器。
> 渠道的**真实发送**被替换为捕获式假实现(`FakeSubscribeMessager` /
> `FakeWxPusherMessageSender` / `FakePushPlusMessageSender`)，
> 从而能断言"渠道最终发出的消息内容"，而不真正调用第三方接口。

### 7.3 运行

```bash
# 全部
dotnet test aspnet-core/tests/LINGYUN.Abp.Notifications.ChannelData.Tests
dotnet test aspnet-core/tests/LINGYUN.Abp.Notifications.WeChat.Work.Tests
dotnet test aspnet-core/tests/LINGYUN.Abp.Notifications.Webhook.WeChat.Work.Tests
dotnet test aspnet-core/tests/LINGYUN.Abp.Notifications.WeChat.MiniProgram.Tests
dotnet test aspnet-core/tests/LINGYUN.Abp.Notifications.WxPusher.Tests
dotnet test aspnet-core/tests/LINGYUN.Abp.Notifications.PushPlus.Tests
```
