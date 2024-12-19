# LINGYUN.Abp.RealTime

## Introduction

`LINGYUN.Abp.RealTime` is a real-time communication foundation module that provides infrastructure for real-time message delivery. This module is mainly used for handling real-time notifications, messages, and event delivery.

## Features

* Real-time event delivery infrastructure
* Support for localized string handling
* Distributed event integration
* Extensible event handling mechanism

## Installation

```bash
dotnet add package LINGYUN.Abp.RealTime
```

## Usage

1. Add `[DependsOn(typeof(AbpRealTimeModule))]` to your module class.

```csharp
[DependsOn(typeof(AbpRealTimeModule))]
public class YourModule : AbpModule
{
    // ...
}
```

2. Create real-time event data transfer object:

```csharp
public class YourRealTimeEto : RealTimeEto<YourData>
{
    public YourRealTimeEto(YourData data) 
        : base(data)
    {
    }
}
```

3. Use localized strings:

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

## Advanced Usage

### 1. Custom Real-Time Events

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

### 2. Real-Time Event Handling

```csharp
public class YourRealTimeEventHandler : 
    IDistributedEventHandler<YourRealTimeEto>,
    ITransientDependency
{
    public async Task HandleEventAsync(YourRealTimeEto eventData)
    {
        // Handle real-time event
    }
}
```

## Links

* [中文文档](./README.md)
