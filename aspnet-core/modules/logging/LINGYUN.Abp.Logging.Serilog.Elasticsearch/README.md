# LINGYUN.Abp.Logging.Serilog.Elasticsearch

ILoggingManager 接口的ES实现, 从ES中检索日志信息  

## 模块引用

```csharp
[DependsOn(typeof(AbpLoggingSerilogElasticsearchModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

*  AbpLoggingSerilogElasticsearchOptions.IndexFormat	必须和Serilog配置项中的IndexFormat相同,否则无法定位到正确的索引  

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