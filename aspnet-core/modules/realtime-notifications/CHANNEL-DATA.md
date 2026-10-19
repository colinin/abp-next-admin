# 通知渠道数据结构解析

面向 **发布渠道** 的强类型载荷(渠道数据)框架。

解决的核心问题: 生产者用弱类型属性包表达渠道专属语义时,
"写了键但渠道永远读不到"、"经持久化后配置静默失效"、"载荷类型靠字段探测顺序猜测"。

---

## 1. 问题背景

通知数据 `NotificationData` 底层是 `ExtraPropertyDictionary`(弱类型属性包),
在以下链路中必然经历 JSON 往返:

| 链路 | 位置 |
|---|---|
| 持久化后回读 | `NotificationStore.InsertNotificationAsync` → `Notification.SetData` |
| 失败重试 | `NotificationPublishJob` 从存储读取后重新发布 |
| 跨进程投递 | `NotificationEto` 经 EventBus |

JSON 往返后值的运行时类型会改变:

```
byte 1  ->  long 1 / JsonElement(Number)
int 1   ->  long 1 / JsonElement(Number)
```

因此旧实现中的裸转换会**永远失败且没有任何日志**:

```csharp
// 写入 int, 读取 byte? —— as 操作符对 boxed int 恒返回 null
notificationData.TrySetData(Safe, safe ? 1 : 0);
var safe = notificationData.TryGetData(Safe) as byte?;   // 永远是 null
```

受影响的选项: 企业微信 `safe`、`enable_id_trans`、`enable_duplicate_check`。

---

## 2. 设计原则

1. **全部通过依赖注入获取, 不提供静态 API**
   —— 转换依赖 JSON 配置与日志, 静态入口会迫使框架维护一套"静态可用的降级实现",
   增加状态与初始化顺序的隐性耦合。
2. **JSON 通过专用接口抽象**, 默认复用 ABP 全局配置。
3. **存在性声明式表达 + 注册期强制校验**, 让配置错误在启动阶段暴露。
4. **转换逻辑在读写两侧共用同一份实现**(`INotificationChannelDataAccessor`),
   避免"写入方与读取方规则不一致"。

---

## 3. 框架组成

### 3.1 契约 (`LINGYUN.Abp.Notifications.Core`, `ChannelData/` 子目录)

| 文件 | 类型 | 作用 |
|---|---|---|
| `NotificationChannelData.cs` | `NotificationChannelData` | 载荷基类(空标记, 见 §4) |
| `NotificationChannelDataAttribute.cs` | `NotificationChannelDataAttribute` | 声明渠道、匹配顺序 `Priority`、存在性模式 `Presence` |
| `NotificationChannelDataPresenceMode.cs` | `NotificationChannelDataPresenceMode` | 存在性判定模式 (None / DeclaredProperties / AnyDeclaredProperty) |
| `NotificationChannelPropertyAttribute.cs` | `NotificationChannelPropertyAttribute` | 声明参与读写的属性; `Name` 兼容历史键; `IgnoreWhenMatching` 排除可选读取项 |
| `NotificationChannelDataFormat.cs` | `NotificationChannelDataFormat` | 属性存储格式 (Auto / Json) |
| `NotificationChannelDataDescriptor.cs` | `NotificationChannelDataDescriptor` | 载荷描述符 (只读元数据) |

### 3.2 实现 (`LINGYUN.Abp.Notifications`, `ChannelData/` 子目录)

每个类型单独一个文件(接口与实现分文件)。

| 接口 | 默认实现 | 作用 |
|---|---|---|
| `INotificationChannelDataJsonSerializer` | `AbpNotificationChannelDataJsonSerializer` | JSON 抽象, 默认委托 ABP `IJsonSerializer` |
| `INotificationChannelDataAccessor` | `NotificationChannelDataAccessor` | **弱类型转换 + JSON 的唯一实现**, 读写共用 |
| `INotificationChannelDataPresenceProvider` | `NotificationChannelDataPresenceProvider` | 存在性判定委托的创建与缓存(含注册期校验) |
| `INotificationChannelDataSerializer` | `NotificationChannelDataSerializer` | 生产者侧写入入口 |
| `INotificationChannelDataResolver` | `NotificationChannelDataResolver` | 渠道侧解析入口 |

辅助类型: `AbpNotificationChannelDataOptions`(载荷注册)、
`NotificationChannelDataDescriptorList`、`AbpNotificationChannelDataConfigurationException`、
`NotificationChannelDataPresence`(存在性判定, 纯静态)、
`NotificationChannelDataProperty` / `NotificationChannelDataPropertyCache`(属性元数据)、
`NotificationChannelDataValueTypes`(值类型判定)。

### 3.3 注册方式: 模块 `PostConfigure<>`

渠道模块在自己的 `AbpModule.ConfigureServices` 中声明载荷:

```csharp
[DependsOn(typeof(AbpNotificationsModule))]
public class AbpNotificationsMyChannelModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpNotificationsPublishOptions>(options =>
        {
            options.PublishProviders.Add<MyNotificationPublishProvider>();
        });

        // 声明本渠道支持的载荷形态(注册期即校验存在性契约)
        Configure<AbpNotificationChannelDataOptions>(options =>
        {
            options.Add<MyChannelData>();
        });
    }
}
```

**为什么不需要额外的配置器类、也不存在顺序问题**:
`AbpNotificationChannelDataOptions` 是**纯数据**对象 —— 注册期的存在性校验只依赖
载荷类型上的声明元数据(`[NotificationChannelData]` / `[NotificationChannelProperty]`),
不依赖任何服务。判定逻辑位于静态的 `NotificationChannelDataPresence`,
因此 `options.Add(...)` 在容器构建之前、以任意模块顺序调用都能正常工作。

对应地, 之前为"解析存在性提供者"而引入的
`NotificationChannelDataPresenceConfigurer` 已删除, 选项对象上的
`PresenceProvider` 属性也已移除。`INotificationChannelDataPresenceProvider`
保留为依赖注入形态的薄封装(委托给静态实现), 供解析器与序列化器使用。

---

## 4. 数据键命名约定

### 4.1 必须带本渠道前缀

所有渠道自有键统一使用 `<渠道前缀><键名>` 形式, 避免不同渠道的键在同一个
通知数据属性包中相互冲突:

| 渠道 | 前缀 | 键 |
|---|---|---|
| 企业微信 | `wx-work:` | `wx-work:text_card_message`、`wx-work:template_card_message`、`wx-work:safe` … |
| 企业微信 Webhook | `wx-work-hook:` | `wx-work-hook:text`、`wx-work-hook:markdown_v2`、`wx-work-hook:template_card` … |
| 微信小程序 | `wx-mp:` | `wx-mp:template_id`、`wx-mp:redirect_page`、`wx-mp:weapp_state`、`wx-mp:weapp_language` |
| WxPusher | `wx-pusher:` | `wx-pusher:url` |
| PushPlus | `push-plus:` | `push-plus:webhook`、`push-plus:callback` |

> **渠道注册表键与键前缀都是独立命名空间**。企业微信应用消息与 Webhook 是两种传输方式、
> 两套载体模型, 因此注册表键(`WeChat.Work` / `WeChat.Work.Webhook`)与键前缀
> (`wx-work:` / `wx-work-hook:`)都必须分开 ——
> 否则同一个通知被两个渠道同时订阅时, 载荷会被两边都解析并各发一条。

每个渠道都以 `XxxNotificationDataKeys` 暴露常量, 并给出 `Prefix`:

```csharp
public static class MyChannelNotificationDataKeys
{
    public const string Prefix = "my-channel:";

    public const string TemplateId = Prefix + "template_id";
    public const string RedirectPage = Prefix + "redirect_page";
}
```

> 唯一例外是**通知标准数据**的键(`title` / `message` / `description` /
> `formUser` / `createTime`), 它们由 `NotificationData.WriteStandardData` 定义,
> 属于所有渠道共用的通用约定, 不加前缀。

### 4.2 读取大小写不敏感

键匹配规则是**精确匹配优先, 未命中再忽略大小写**,
因此生产者使用 `wx-work:text_card_message`、`WX-WORK:TEXT_CARD_MESSAGE`
或 `Wx-Work:Text_Card_Message` 都能命中。

该规则在 `INotificationChannelDataAccessor.TryGetRawValue` 中统一实现,
`NotificationChannelDataPresence.ContainsKey` 使用同一规则,
保证"存在性判定"与"实际读取"不会出现规则不一致。

### 4.3 历史遗留的无前缀键

微信小程序渠道在早期版本使用了无前缀键(`TemplateId` 等)。
为让已安装的生产者平滑升级, 这些键保留在
`WeChatMiniProgramNotificationDataKeys.Legacy` 中, 由渠道在解析时**只读回退**
(带前缀的键优先), 并在命中遗留键时记录警告以推动迁移。

新代码请一律使用带前缀的常量。

### 4.4 一个数据对象中可共存多个渠道的键

不同前缀互不冲突, 因此同一个通知可以同时驱动多个渠道:

```csharp
data.TrySetData(WeChatWorkNotificationDataKeys.ToParty, "1|2");          // 应用消息
data.TrySetData(WeChatWorkWebhookNotificationDataKeys.Markdown, "# 告警"); // Webhook
```

各渠道只解析自己前缀的载荷, 不会互相干扰(见 §9)。

---

## 5. 存在性判定

**声明式存在性 + 注册期强制校验**:

```csharp
public enum NotificationChannelDataPresenceMode
{
    None = 0,                  // 始终存在(不依赖通知数据)
    DeclaredProperties = 1,    // 参与匹配的属性全部存在(默认)
    AnyDeclaredProperty = 2    // 参与匹配的属性存在任意一个
}
```

校验规则(注册时执行, 即启动阶段):

| 情况 | 结果 |
|---|---|
| 声明了按键判定, 却没有任何可匹配属性 | 抛 `AbpNotificationChannelDataConfigurationException` |
| 显式声明 `Presence = None` | 合法, 载荷始终存在 |
| 缺少 `[NotificationChannelData]` | 抛 `AbpNotificationChannelDataConfigurationException` |
| 同渠道 `Priority` 重复 | 抛 `AbpNotificationChannelDataConfigurationException` |

这样"忘了定义存在性"从**静默丢消息**变成**启动即失败**。

---

## 6. JSON 序列化抽象

```csharp
public interface INotificationChannelDataJsonSerializer
{
    string Serialize(object value, Type valueType);
    object? Deserialize(string json, Type targetType);
}

[Dependency(TryRegister = true)]
public class AbpNotificationChannelDataJsonSerializer
    : INotificationChannelDataJsonSerializer, ISingletonDependency
{
    protected IJsonSerializer JsonSerializer { get; }

    public string Serialize(object value, Type valueType) => JsonSerializer.Serialize(value);

    public object? Deserialize(string json, Type targetType)
        => json.IsNullOrWhiteSpace() ? null : JsonSerializer.Deserialize(targetType, json);
}
```

复用到的全局能力: 驼峰策略、时间格式、枚举转换、
`AbpSystemTextJsonSerializerModifiersOptions` 修饰器、自定义 Converter。

需要加密/压缩/私有格式时替换该接口实现即可。

> **注意**: 项目使用的 JSON 集成是 `Volo.Abp.Json.SystemTextJson`,
> 因此复杂载荷的 `JsonConverter` 必须实现 System.Text.Json 版本
> (参见 `TemplateCardSystemTextJsonConverter`)。

---

## 7. 与 JSON 实现解耦

`NotificationChannelDataAccessor` 在转换前会**还原 JSON 中间表示**:

| JSON 实现 | 属性包中的值 |
|---|---|
| System.Text.Json | `JsonElement` |
| Newtonsoft.Json | `JValue` / 原始 CLR 值 |

两者都会先还原为 CLR 原始值(数字还原为最小可用数值类型), 再做弱类型转换。
因此同一份逻辑在两种 JSON 实现下行为一致。

---

## 8. 生产者用法

### 8.1 声明载荷

```csharp
[NotificationChannelData(MyProvider.ProviderName, Priority = 100)]
public class MyChannelData : NotificationChannelData
{
    [NotificationChannelProperty("my:template_id")]
    public string? TemplateId { get; set; }

    // 可选读取项: 不参与存在性判定
    [NotificationChannelProperty("my:page", IgnoreWhenMatching = true)]
    public string? Page { get; set; }
}
```

### 8.2 在模块中注册

```csharp
[DependsOn(typeof(AbpNotificationsModule))]
public class AbpNotificationsMyChannelModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpNotificationsPublishOptions>(options =>
        {
            options.PublishProviders.Add<MyNotificationPublishProvider>();
        });

        Configure<AbpNotificationChannelDataOptions>(options =>
        {
            options.Add<MyChannelData>();
        });
    }
}
```

### 8.3 写入通知数据

```csharp
public class MyProducer
{
    private readonly INotificationChannelDataSerializer _channelDataSerializer;

    public MyProducer(INotificationChannelDataSerializer channelDataSerializer)
    {
        _channelDataSerializer = channelDataSerializer;
    }

    public void Build(NotificationData data)
    {
        _channelDataSerializer.Write(data, new MyChannelData { TemplateId = "tpl-1" });
    }
}
```

---

## 9. 渠道用法

```csharp
public class MyProvider : NotificationPublishProvider
{
    protected INotificationChannelDataResolver ChannelDataResolver
        => ServiceProvider.LazyGetRequiredService<INotificationChannelDataResolver>();

    protected override Task PublishAsync(NotificationPublishContext context, CancellationToken ct)
    {
        var data = context.Notification.Data;

        // 按类型解析
        var payload = ChannelDataResolver.Resolve<MyChannelData>(data);

        // 区分"未写入"与"写入了但数据非法"
        if (ChannelDataResolver.IsDeclared(data, typeof(MyChannelData)) && payload == null)
        {
            context.Cancel("The payload can not be parsed.");
            return Task.CompletedTask;
        }

        // 标量选项
        var safe = ChannelDataResolver.GetValue(data, "my:safe", false);

        return Task.CompletedTask;
    }
}
```

---

## 10. 行为保证

| 场景 | 行为 |
|---|---|
| 未设置的值 | 返回 `null`(而不是被默认值吞掉), 可区分"未设置"与"显式 false/0" |
| JSON 往返后 | `byte`/`int`/`long`/`string`/`bool`/`JsonElement` 均可正确转换 |
| 键大小写不一致 | 大小写不敏感匹配, 兼容历史遗留键(如 `TemplateId`) |
| 数据非法 | 不抛异常; `Resolve` 返回 `null` 并记录警告日志, `IsDeclared` 仍为 `true` |
| 未声明存在性规则 | 注册期抛 `AbpNotificationChannelDataConfigurationException` |
| 缺少渠道特性 | 注册期抛 `AbpNotificationChannelDataConfigurationException` |
| 同渠道优先级冲突 | 注册期抛 `AbpNotificationChannelDataConfigurationException` |
| 载荷类型未映射消息载体 | 构造时抛 `AbpNotificationChannelDataConfigurationException`(快速失败) |

---

## 11. 扩展新渠道的检查清单

1. 定义载荷类型并标注 `[NotificationChannelData]`;
2. 至少给一个"标识属性"标注 `[NotificationChannelProperty]`(可选读取项加 `IgnoreWhenMatching = true`);
3. 需要"任意一个键即可"时把 `Presence` 设为 `AnyDeclaredProperty`; 需要"始终存在"时设为 `None`;
4. 在本模块的 `Configure<AbpNotificationChannelDataOptions>` 中注册载荷;
5. 渠道实现中**不要**出现 `as byte?` / `as int?` 这类裸转换, 改用 `INotificationChannelDataResolver`;
6. 需要区分"未声明"与"数据非法"时, 组合使用 `IsDeclared` 与 `Resolve`;
7. 抽象类型载荷需要注册 JSON 类型转换器(注意 JSON 实现: STJ 或 Newtonsoft)。
