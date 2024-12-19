# LINGYUN.Abp.Dynamic.Queryable.HttpApi

动态查询HTTP API模块，提供基于ABP框架的动态查询HTTP API实现。

## 功能特性

* 提供动态查询控制器基类 `DynamicQueryableControllerBase<TEntity, TEntityDto>`
* 自动生成REST API端点
* 支持动态查询参数的HTTP传输

## 配置使用

1. 安装 `LINGYUN.Abp.Dynamic.Queryable.HttpApi` NuGet包

2. 添加 `[DependsOn(typeof(AbpDynamicQueryableHttpApiModule))]` 到你的模块类

### 实现动态查询控制器

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

### API端点

* GET `/api/my-entity/available-fields` - 获取可用字段列表
* POST `/api/my-entity/search` - 根据动态条件查询数据

### 查询示例

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

## 相关链接

* [LINGYUN.Linq.Dynamic.Queryable](../LINGYUN.Linq.Dynamic.Queryable/README.md)
* [LINGYUN.Abp.Dynamic.Queryable.Application](../LINGYUN.Abp.Dynamic.Queryable.Application/README.md)
* [LINGYUN.Abp.Dynamic.Queryable.Application.Contracts](../LINGYUN.Abp.Dynamic.Queryable.Application.Contracts/README.md)
