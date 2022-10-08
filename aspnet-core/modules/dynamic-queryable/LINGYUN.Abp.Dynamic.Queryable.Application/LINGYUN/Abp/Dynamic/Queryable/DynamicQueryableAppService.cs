using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.Dynamic.Queryable;

public abstract class DynamicQueryableAppService<TEntity, TEntityDto> : ApplicationService, IDynamicQueryableAppService<TEntityDto>
{
    /// <summary>
    /// 获取可用字段列表
    /// </summary>
    /// <returns></returns>
    public virtual Task<ListResultDto<DynamicParamterDto>> GetAvailableFieldsAsync()
    {
        var options = LazyServiceProvider.LazyGetRequiredService<IOptions<AbpDynamicQueryableOptions>>().Value;

        var entityType = typeof(TEntity);
        var dynamicParamters = new List<DynamicParamterDto>();

        var propertyInfos = entityType
            .GetProperties()
            .Where(p => !options.IgnoreFields.Contains(p.Name));

        foreach (var propertyInfo in propertyInfos)
        {
            // 字段本地化描述规则
            // 在本地化文件中定义 DisplayName:PropertyName
            var localizedProp = L[$"DisplayName:{propertyInfo.Name}"];
            dynamicParamters.Add(
                new DynamicParamterDto
                {
                    Name = propertyInfo.Name,
                    Type = propertyInfo.PropertyType.FullName,
                    Description = localizedProp.Value ?? propertyInfo.Name
                });
        }

        return Task.FromResult(new ListResultDto<DynamicParamterDto>(dynamicParamters));
    }

    /// <summary>
    /// 根据动态条件查询数据
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async virtual Task<PagedResultDto<TEntityDto>> GetListAsync(GetListByDynamicQueryableInput input)
    {
        Expression<Func<TEntity, bool>> condition = (e) => true;

        condition = condition.DynamicQuery(input.Queryable);

        var totalCount = await GetCountAsync(condition);
        var entities = await GetListAsync(condition, input);

        return new PagedResultDto<TEntityDto>(totalCount,
            MapToEntitiesDto(entities));
    }

    protected abstract Task<int> GetCountAsync(Expression<Func<TEntity, bool>> condition);

    protected abstract Task<List<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>> condition,
        PagedAndSortedResultRequestDto pageRequest);

    protected virtual List<TEntityDto> MapToEntitiesDto(List<TEntity> entities)
    {
        return ObjectMapper.Map<List<TEntity>, List<TEntityDto>>(entities);
    }
}
