# LINGYUN.Abp.RealTime

## 介绍

`LINGYUN.Abp.RealTime` 是一个实时通信基础模块，提供了实时消息传递的基础设施。该模块主要用于处理实时通知、消息和事件的传递。

## 功能

* 实时事件传递基础设施
* 支持本地化字符串处理
* 分布式事件集成
* 可扩展的事件处理机制

## 安装

```bash
dotnet add package LINGYUN.Abp.RealTime
```

## 使用

1. 添加 `[DependsOn(typeof(AbpRealTimeModule))]` 到你的模块类上。

```csharp
[DependsOn(typeof(AbpRealTimeModule))]
public class YourModule : AbpModule
{
    // ...
}
```

2. 创建实时事件数据传输对象：

```csharp
public class YourRealTimeEto : RealTimeEto<YourData>
{
    public YourRealTimeEto(YourData data) 
        : base(data)
    {
    }
}
```

3. 使用本地化字符串：

```csharp
public class LocalizedMessage
{
    private readonly LocalizableStringInfo _localizableString;

    public LocalizedMessage()
    {
        _localizableString = new LocalizableStringInfo(
            "YourResource",
            "MessageKey",
            new Dictionary<object, object>
            {
                { "param1", "value1" }
            });
    }
}
```

## 高级用法

### 1. 自定义实时事件

```csharp
[EventName("your-custom-event")]
public class CustomRealTimeEto : RealTimeEto<CustomData>
{
    public CustomRealTimeEto(CustomData data) 
        : base(data)
    {
    }
}
```

### 2. 实时事件处理

```csharp
public class YourRealTimeEventHandler : 
    IDistributedEventHandler<YourRealTimeEto>,
    ITransientDependency
{
    public async Task HandleEventAsync(YourRealTimeEto eventData)
    {
        // 处理实时事件
    }
}
```

## 链接

* [English document](./README.EN.md)
