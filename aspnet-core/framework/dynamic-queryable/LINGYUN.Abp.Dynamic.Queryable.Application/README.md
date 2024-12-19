# LINGYUN.Abp.Dynamic.Queryable.Application

动态查询应用服务模块，提供基于ABP框架的动态查询功能实现。

## 功能特性

* 提供动态查询应用服务基类 `DynamicQueryableAppService<TEntity, TEntityDto>`
* 自动生成可用字段列表，支持字段本地化
* 支持字段过滤和忽略
* 支持枚举类型的选项列表生成

## 配置使用

1. 安装 `LINGYUN.Abp.Dynamic.Queryable.Application` NuGet包

2. 添加 `[DependsOn(typeof(AbpDynamicQueryableApplicationModule))]` 到你的模块类

3. 配置选项（可选）

```csharp
Configure<AbpDynamicQueryableOptions>(options =>
{
    // 添加需要忽略的字段
    options.IgnoreFields.Add("FieldName");
});
```

### 实现动态查询服务

```csharp
public class MyEntityAppService : DynamicQueryableAppService<MyEntity, MyEntityDto>
{
    private readonly IRepository<MyEntity> _repository;

    public MyEntityAppService(IRepository<MyEntity> repository)
    {
        _repository = repository;
    }

    protected override async Task<int> GetCountAsync(Expression<Func<MyEntity, bool>> condition)
    {
        return await _repository.CountAsync(condition);
    }

    protected override async Task<List<MyEntity>> GetListAsync(
        Expression<Func<MyEntity, bool>> condition,
        GetListByDynamicQueryableInput dynamicInput)
    {
        return await _repository
            .Where(condition)
            .PageBy(dynamicInput.SkipCount, dynamicInput.MaxResultCount)
            .ToListAsync();
    }

    protected override List<TEntityDto> MapToEntitiesDto(List<MyEntity> entities)
    {
        return ObjectMapper.Map<List<MyEntity>, List<MyEntityDto>>(entities);
    }

    // 自定义需要忽略的字段（可选）
    protected override IEnumerable<string> GetUserDefineIgnoreFields()
    {
        return new[] { "CustomField" };
    }
}
```

### 配置项说明

* `IgnoreFields` - 需要在查询中忽略的字段列表
  * 默认包含：Id、TenantId、IsDeleted、DeleterId、DeletionTime等审计字段

## 相关链接

* [LINGYUN.Linq.Dynamic.Queryable](../LINGYUN.Linq.Dynamic.Queryable/README.md)
* [LINGYUN.Abp.Dynamic.Queryable.Application.Contracts](../LINGYUN.Abp.Dynamic.Queryable.Application.Contracts/README.md)
