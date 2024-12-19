# LINGYUN.Abp.Dynamic.Queryable.Application

Dynamic query application service module, providing dynamic query functionality implementation based on the ABP framework.

## Features

* Provides dynamic query application service base class `DynamicQueryableAppService<TEntity, TEntityDto>`
* Automatically generates available field list with localization support
* Supports field filtering and ignoring
* Supports enum type option list generation

## Configuration and Usage

1. Install the `LINGYUN.Abp.Dynamic.Queryable.Application` NuGet package

2. Add `[DependsOn(typeof(AbpDynamicQueryableApplicationModule))]` to your module class

3. Configure options (optional)

```csharp
Configure<AbpDynamicQueryableOptions>(options =>
{
    // Add fields to ignore
    options.IgnoreFields.Add("FieldName");
});
```

### Implementing Dynamic Query Service

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

    // Custom fields to ignore (optional)
    protected override IEnumerable<string> GetUserDefineIgnoreFields()
    {
        return new[] { "CustomField" };
    }
}
```

### Configuration Options

* `IgnoreFields` - List of fields to ignore in queries
  * Defaults include: Id, TenantId, IsDeleted, DeleterId, DeletionTime and other audit fields

## Related Links

* [LINGYUN.Linq.Dynamic.Queryable](../LINGYUN.Linq.Dynamic.Queryable/README.EN.md)
* [LINGYUN.Abp.Dynamic.Queryable.Application.Contracts](../LINGYUN.Abp.Dynamic.Queryable.Application.Contracts/README.EN.md)
