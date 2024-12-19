# LINGYUN.Abp.Serilog.Enrichers.Application

[简体中文](./README.md) | English

A Serilog enricher that adds application identifier to log properties.

## Features

* Adds application name to log events
* Configurable application name field
* Support for both code-based and configuration-based setup
* Caches log event property for better performance
* Seamless integration with Serilog

## Module Dependencies

```csharp
[DependsOn(typeof(AbpSerilogEnrichersApplicationModule))]
public class YouProjectModule : AbpModule
{
  public override void PreConfigureServices(ServiceConfigurationContext context)
  {
    AbpSerilogEnrichersConsts.ApplicationName = "demo-app";
  }
}
```

## Configuration Options

The following are field constants that need to be explicitly changed:

* AbpSerilogEnrichersConsts.ApplicationNamePropertyName - Used to customize the name of the ApplicationName field
* AbpSerilogEnrichersConsts.ApplicationName - The name of the current application to be identified in logs

## Usage

### Code-based Configuration

```csharp
Log.Logger = new LoggerConfiguration()
    .Enrich.WithApplicationName()
    // ...other configuration...
    .CreateLogger();
```

### JSON Configuration

```json
{
   "Serilog": {
    "MinimumLevel": {
      "Default": "Information"
    },
    "Enrich": [ "WithApplicationName" ]
  }
}
```

## Implementation Details

The enricher adds a property named "ApplicationName" (configurable) to each log event with the value specified in `AbpSerilogEnrichersConsts.ApplicationName`. The property is cached for better performance.

## Best Practices

1. Set the application name early in your application's startup:
```csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    AbpSerilogEnrichersConsts.ApplicationName = "your-app-name";
}
```

2. Use a consistent naming convention for your applications to make log filtering easier.

3. Consider setting the application name through configuration:
```json
{
  "App": {
    "Name": "your-app-name"
  }
}
```
```csharp
AbpSerilogEnrichersConsts.ApplicationName = configuration["App:Name"];
```

## Notes

1. The application name is static once set and will be the same for all log entries from the application.
2. The enricher uses property caching to improve performance.
3. The property will only be added if it doesn't already exist in the log event.
