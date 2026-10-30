using LINGYUN.Abp.ElsaNext.ApprovalFlow.Instances;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.CarbonCopies.Dtos;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.Specifications;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.CarbonCopies;

[Authorize]
public class CarbonCopyAppService : ApprovalFlowAppServiceBase, ICarbonCopyAppService
{
    protected IApprovalCcRepository CarbonCopyRepository { get; }

    protected IApprovalInstanceRepository InstanceRepository { get; }

    public CarbonCopyAppService(
        IApprovalCcRepository carbonCopyRepository,
        IApprovalInstanceRepository instanceRepository)
    {
        CarbonCopyRepository = carbonCopyRepository;
        InstanceRepository = instanceRepository;
    }

    [Authorize(ElsaNextApprovalFlowPermissions.Instances.Default)]
    public virtual async Task<PagedResultDto<CarbonCopyDto>> GetListAsync(CarbonCopyGetPagedListInput input)
    {
        var actor = await GetRequiredActorAsync();
        var actorType = actor.Subject.Type.ToString();

        Expression<Func<ApprovalCc, bool>> filter = x =>
            x.RecipientId == actor.Subject.Id &&
            x.RecipientType == actorType;

        if (input.IsRead.HasValue)
        {
            filter = ExpressionFuncExtender.And(filter, x => x.IsRead == input.IsRead.Value);
        }
        if (!input.Filter.IsNullOrWhiteSpace())
        {
            filter = ExpressionFuncExtender.And(filter, x =>
                x.Title.Contains(input.Filter!) ||
                (x.Summary != null && x.Summary.Contains(input.Filter!)));
        }

        var specification = new ExpressionSpecification<ApprovalCc>(filter);
        var totalCount = await CarbonCopyRepository.GetCountAsync(specification);
        var items = await CarbonCopyRepository.GetListAsync(
            specification,
            sorting: input.Sorting.IsNullOrWhiteSpace()
                ? $"{nameof(ApprovalCc.NotifiedAt)} DESC"
                : input.Sorting,
            maxResultCount: input.MaxResultCount,
            skipCount: input.SkipCount);

        var result = new List<CarbonCopyDto>(items.Count);
        foreach (var carbonCopy in items)
        {
            var title = carbonCopy.Title;
            var instanceId = carbonCopy.InstanceId;

            var instance = instanceId.HasValue
                ? await InstanceRepository.FindAsync(instanceId.Value)
                : !string.IsNullOrWhiteSpace(carbonCopy.NodeInstanceId)
                    ? await InstanceRepository.FindByElsaWorkflowInstanceIdAsync(carbonCopy.NodeInstanceId)
                    : null;

            if (instance != null)
            {
                instanceId = instance.Id;
                title = instance.Title;
            }

            var dto = ObjectMapper.Map<ApprovalCc, CarbonCopyDto>(carbonCopy);
            dto.InstanceId = instanceId ?? dto.InstanceId;
            dto.InstanceReference = instanceId?.ToString("D") ?? dto.InstanceReference;
            dto.Title = title;
            result.Add(dto);
        }

        return new PagedResultDto<CarbonCopyDto>(totalCount, result);
    }

    [Authorize(ElsaNextApprovalFlowPermissions.Instances.Default)]
    public virtual async Task<CarbonCopyDto> MarkReadAsync(Guid id)
    {
        var actor = await GetRequiredActorAsync();
        var carbonCopy = await CarbonCopyRepository.GetAsync(id);
        if (!string.Equals(carbonCopy.RecipientId, actor.Subject.Id, StringComparison.Ordinal))
        {
            throw new AbpAuthorizationException("The current user is not a recipient of this carbon copy.");
        }

        carbonCopy.MarkRead(new DateTimeOffset(Clock.Now));
        await CarbonCopyRepository.UpdateAsync(carbonCopy, autoSave: true);
        return ObjectMapper.Map<ApprovalCc, CarbonCopyDto>(carbonCopy);
    }
}
