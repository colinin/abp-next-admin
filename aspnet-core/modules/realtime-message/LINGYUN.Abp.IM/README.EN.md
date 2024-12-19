# LINGYUN.Abp.IM

The foundation module for instant messaging.

## Features

* Provides instant messaging infrastructure
* Provides message sender provider interface
* Extensible message sender providers

## Configuration

The module uses `AbpIMOptions` configuration class:

```csharp
Configure<AbpIMOptions>(options =>
{
    // Add custom message sender provider
    options.Providers.Add<CustomMessageSenderProvider>();
});
```

## More

[中文文档](README.md)
