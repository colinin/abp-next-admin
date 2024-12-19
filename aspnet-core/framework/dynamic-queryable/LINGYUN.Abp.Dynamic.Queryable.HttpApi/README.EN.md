# LINGYUN.Abp.Dynamic.Queryable.HttpApi

Dynamic query HTTP API module, providing HTTP API implementation for dynamic querying based on the ABP framework.

## Features

* Provides dynamic query controller base class `DynamicQueryableControllerBase<TEntity, TEntityDto>`
* Automatically generates REST API endpoints
* Supports HTTP transport of dynamic query parameters

## Configuration and Usage

1. Install the `LINGYUN.Abp.Dynamic.Queryable.HttpApi` NuGet package

2. Add `[DependsOn(typeof(AbpDynamicQueryableHttpApiModule))]` to your module class

### Implementing Dynamic Query Controller

```csharp
[Route("api/my-entity")]
public class MyEntityController : DynamicQueryableControllerBase<MyEntity, MyEntityDto>
{
    public MyEntityController(IDynamicQueryableAppService<MyEntityDto> dynamicQueryableAppService)
        : base(dynamicQueryableAppService)
    {
    }
}
```

### API Endpoints

* GET `/api/my-entity/available-fields` - Get available fields list
* POST `/api/my-entity/search` - Query data based on dynamic conditions

### Query Example

```json
POST /api/my-entity/search
{
    "maxResultCount": 10,
    "skipCount": 0,
    "queryable": {
        "paramters": [
            {
                "field": "Name",
                "comparison": "Equal",
                "value": "test"
            }
        ]
    }
}
```

## Related Links

* [LINGYUN.Linq.Dynamic.Queryable](../LINGYUN.Linq.Dynamic.Queryable/README.EN.md)
* [LINGYUN.Abp.Dynamic.Queryable.Application](../LINGYUN.Abp.Dynamic.Queryable.Application/README.EN.md)
* [LINGYUN.Abp.Dynamic.Queryable.Application.Contracts](../LINGYUN.Abp.Dynamic.Queryable.Application.Contracts/README.EN.md)
