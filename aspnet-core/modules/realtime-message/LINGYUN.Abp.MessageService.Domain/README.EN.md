# LINGYUN.Abp.MessageService.Domain

Message service domain layer module.

## Features

* Implements message service domain logic
* Integrates object extension functionality
* Integrates caching functionality
* Integrates notification functionality
* Supports auto mapping
* Supports multi-language

## Dependencies

* `AbpAutoMapperModule`
* `AbpCachingModule`
* `AbpNotificationsModule`
* [LINGYUN.Abp.MessageService.Domain.Shared](../LINGYUN.Abp.MessageService.Domain.Shared/README.EN.md)

## Installation

1. First, install the LINGYUN.Abp.MessageService.Domain package to your project:

```bash
dotnet add package LINGYUN.Abp.MessageService.Domain
```

2. Add `AbpMessageServiceDomainModule` to your module's dependency list:

```csharp
[DependsOn(typeof(AbpMessageServiceDomainModule))]
public class YourModule : AbpModule
{
}
```

## More

[中文文档](README.md)
