# LINGYUN.Abp.Elasticsearch

[简体中文](./README.md) | English

Abp Elasticsearch integration module, providing a global singleton IElasticClient interface for access.

## Module Dependencies

```csharp
[DependsOn(typeof(AbpElasticsearchModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration Options

* AbpElasticsearchOptions.FieldCamelCase      Whether fields use camelCase format, default is false
* AbpElasticsearchOptions.NodeUris            ES endpoints, multiple endpoints separated by , or ;
* AbpElasticsearchOptions.TypeName            Document type name, default is _doc
* AbpElasticsearchOptions.ConnectionLimit     Maximum connection limit, see NEST documentation for details
* AbpElasticsearchOptions.UserName            Connection username, see NEST documentation for details
* AbpElasticsearchOptions.Password            User password, see NEST documentation for details
* AbpElasticsearchOptions.ConnectionTimeout   Connection timeout, see NEST documentation for details

## appsettings.json

```json
{
  "Elasticsearch": {
    "NodeUris": "http://localhost:9200"
  }
}
```

## Features

* Provides a global singleton IElasticClient interface for unified Elasticsearch access
* Supports multiple node configurations for cluster deployment
* Supports basic authentication
* Configurable field naming convention (camelCase)
* Customizable connection settings including timeout and connection limits
* Compatible with NEST client features
