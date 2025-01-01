# LINGYUN.Abp.MessageService.Domain.Shared

Message service domain shared layer module.

## Features

* Defines message service infrastructure
* Provides multi-language resources
* Defines exception localization
* Defines error codes

## Dependencies

* `AbpLocalizationModule`

## Installation

1. First, install the LINGYUN.Abp.MessageService.Domain.Shared package to your project:

```bash
dotnet add package LINGYUN.Abp.MessageService.Domain.Shared
```

2. Add `AbpMessageServiceDomainSharedModule` to your module's dependency list:

```csharp
[DependsOn(typeof(AbpMessageServiceDomainSharedModule))]
public class YourModule : AbpModule
{
}
```

## More

[中文文档](README.md)
