using LINGYUN.Abp.DataProtectionManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.DataProtectionManagement;

[Authorize(DataProtectionManagementPermissionNames.EntityTypeInfo.Default)]
public class EntityTypeInfoAppService : DataProtectionManagementApplicationServiceBase, IEntityTypeInfoAppService
{
    private readonly EntityTypeInfoManager _entityTypeInfoManager;
    private readonly IEntityTypeInfoRepository _entityTypeInfoRepository;

    public EntityTypeInfoAppService(
        EntityTypeInfoManager entityTypeInfoManager, 
        IEntityTypeInfoRepository entityTypeInfoRepository)
    {
        _entityTypeInfoManager = entityTypeInfoManager;
        _entityTypeInfoRepository = entityTypeInfoRepository;
    }

    public async virtual Task<EntityTypeInfoDto> GetAsync(Guid id)
    {
        var entityTypeInfo = await _entityTypeInfoRepository.GetAsync(id);

        return ObjectMapper.Map<EntityTypeInfo, EntityTypeInfoDto>(entityTypeInfo);
    }

    public async virtual Task<PagedResultDto<EntityTypeInfoDto>> GetListAsync(GetEntityTypeInfoListInput input)
    {
        var specification = new GetEntityTypeInfoListSpecification(input);

        var totalCount = await _entityTypeInfoRepository.GetCountAsync(specification);
        var entities = await _entityTypeInfoRepository.GetListAsync(specification,
            input.Sorting, input.MaxResultCount, input.SkipCount);

        return new PagedResultDto<EntityTypeInfoDto>(totalCount,
            ObjectMapper.Map<List<EntityTypeInfo>, List<EntityTypeInfoDto>>(entities));
    }

    private class GetEntityTypeInfoListSpecification : Specification<EntityTypeInfo>
    {
        protected GetEntityTypeInfoListInput Input { get; }

        public GetEntityTypeInfoListSpecification(GetEntityTypeInfoListInput input)
        {
            Input = input;
        }

        public override Expression<Func<EntityTypeInfo, bool>> ToExpression()
        {
            Expression<Func<EntityTypeInfo, bool>> expression = _ => true;

            return expression
                .AndIf(Input.IsAuditEnabled.HasValue, x => x.IsAuditEnabled == Input.IsAuditEnabled)
                .AndIf(!Input.Filter.IsNullOrWhiteSpace(), x => x.Name.Contains(Input.Filter) ||
                    x.DisplayName.Contains(Input.Filter) || x.TypeFullName.Contains(Input.Filter));
        }
    }
}
