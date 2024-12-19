# LINGYUN.Abp.Elsa.Server

Integration of Elsa.Server.Api, handling default Elsa endpoints.

## Features

* Integrates Elsa server API endpoints with ABP framework
* Provides SignalR support
* Configures API versioning
* Includes automapper profiles for data mapping
* Registers necessary services:
  * `ConnectionConverter`
  * `ActivityBlueprintConverter`
  * `WorkflowBlueprintMapper`
  * `EndpointContentSerializerSettingsProvider`

## Configuration and Usage

```csharp
[DependsOn(
    typeof(AbpElsaServerModule)
    )]
public class YouProjectModule : AbpModule
{
}
```

[中文文档](./README.md)
