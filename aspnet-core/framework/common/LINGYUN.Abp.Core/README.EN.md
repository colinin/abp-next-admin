# LINGYUN.Abp.Core

## Introduction

`LINGYUN.Abp.Core` is a basic core module that provides some common functionalities and extensions.

## Features

* Dynamic Options Provider (`DynamicOptionsProvider<TValue>`)
  * Simplifies the complex steps of calling interfaces before using configuration
  * Supports lazy loading of configuration values
  * Provides one-time running mechanism to ensure configuration is loaded only once

## Installation

```bash
dotnet add package LINGYUN.Abp.Core
```

## Usage

1. Add `[DependsOn(typeof(AbpCommonModule))]` to your module class.

```csharp
[DependsOn(typeof(AbpCommonModule))]
public class YourModule : AbpModule
{
    // ...
}
```

2. Use Dynamic Options Provider:

```csharp
public class YourOptionsProvider : DynamicOptionsProvider<YourOptions>
{
    public YourOptionsProvider(IOptions<YourOptions> options) 
        : base(options)
    {
    }
}
```

## Links

* [中文文档](./README.md)
