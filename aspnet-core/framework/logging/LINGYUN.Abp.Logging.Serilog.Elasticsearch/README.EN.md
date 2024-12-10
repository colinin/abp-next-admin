# LINGYUN.Abp.Logging.Serilog.Elasticsearch

[简体中文](./README.md) | English

Elasticsearch implementation of the ILoggingManager interface, retrieving log information from Elasticsearch.

## Module Dependencies

```csharp
[DependsOn(typeof(AbpLoggingSerilogElasticsearchModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration Options

* AbpLoggingSerilogElasticsearchOptions.IndexFormat - Must match the IndexFormat in Serilog configuration, otherwise the correct index cannot be located

## Features

1. Log Retrieval
   - Retrieve logs from Elasticsearch using ILoggingManager interface
   - Support for various log levels (Debug, Information, Warning, Error, Critical)
   - Automatic mapping between Serilog and Microsoft.Extensions.Logging log levels

2. Object Mapping
   - Automatic mapping of Serilog entities to application entities
   - Maps SerilogException to LogException
   - Maps SerilogField to LogField with unique ID support
   - Maps SerilogInfo to LogInfo with proper log level conversion

## appsettings.json

```json
{
  "Logging": {
    "Serilog": {
      "Elasticsearch": {
        "IndexFormat": "logstash-{0:yyyy.MM.dd}"
      }
    }
  }
}
```

## Important Notes

The IndexFormat configuration must be consistent between your Serilog settings and this module's configuration to ensure proper log retrieval. The default format is "logstash-{0:yyyy.MM.dd}".
