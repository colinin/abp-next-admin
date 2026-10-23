using LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDatas;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDefinitions.Dtos;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Specifications;
using ExpressionFuncExtender = Volo.Abp.Specifications.ExpressionFuncExtender;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.FormDefinitions;

[Authorize]
public class FormDefinitionAppService : ApplicationService, IFormDefinitionAppService
{
    protected IFormDefinitionRepository DefinitionRepository { get; }
    protected FormDefinitionManager DefinitionManager { get; }
    protected FormDataManager DataManager { get; }

    public FormDefinitionAppService(
        IFormDefinitionRepository definitionRepository,
        FormDefinitionManager definitionManager,
        FormDataManager dataManager)
    {
        DefinitionRepository = definitionRepository;
        DefinitionManager = definitionManager;
        DataManager = dataManager;
    }

    private class FormDefinitionGetPagedListSpecification : Specification<FormDefinition>
    {
        protected FormDefinitionGetPagedListInput Input { get; }

        public FormDefinitionGetPagedListSpecification(FormDefinitionGetPagedListInput input)
        {
            Input = input;
        }

        public override Expression<Func<FormDefinition, bool>> ToExpression()
        {
            Expression<Func<FormDefinition, bool>> expression = _ => true;

            if (!Input.Filter.IsNullOrWhiteSpace())
            {
                expression = ExpressionFuncExtender.And(expression, x => x.Name.Contains(Input.Filter) || x.Code.Contains(Input.Filter));
            }
            if (Input.IsPublished.HasValue)
            {
                expression = ExpressionFuncExtender.And(expression, x => x.IsPublished == Input.IsPublished);
            }

            return expression;
        }
    }

    [Authorize(ElsaNextApprovalFlowPermissions.FormDefinitions.Definitions.Default)]
    public virtual async Task<PagedResultDto<FormDefinitionDto>> GetPagedListAsync(FormDefinitionGetPagedListInput input)
    {
        var specification = new FormDefinitionGetPagedListSpecification(input);

        var totalCount = await DefinitionRepository.GetCountAsync(specification);
        var definitions = await DefinitionRepository.GetListAsync(
            specification,
            input.Sorting,
            maxResultCount: input.MaxResultCount,
            skipCount: input.SkipCount);

        return new PagedResultDto<FormDefinitionDto>(totalCount, definitions.Select(Map).ToList());
    }

    [Authorize(ElsaNextApprovalFlowPermissions.FormDefinitions.Definitions.Default)]
    public virtual async Task<FormDefinitionDto> GetAsync(Guid id)
    {
        var definition = await DefinitionRepository.GetAsync(id);
        return Map(definition);
    }

    [Authorize(ElsaNextApprovalFlowPermissions.FormDefinitions.Definitions.Create)]
    public virtual async Task<FormDefinitionDto> CreateAsync(FormDefinitionCreateDto input)
    {
        var definition = await DefinitionManager.CreateAsync(input.Name, input.Code, input.Version, input.Description);
        return Map(definition);
    }

    [Authorize(ElsaNextApprovalFlowPermissions.FormDefinitions.Definitions.Update)]
    public virtual async Task<FormDefinitionDto> UpdateAsync(Guid id, FormDefinitionUpdateDto input)
    {
        var definition = await DefinitionManager.UpdateAsync(id, input.Name, input.Description);
        return Map(definition);
    }

    [Authorize(ElsaNextApprovalFlowPermissions.FormDefinitions.Definitions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await DefinitionManager.DeleteAsync(id);
    }

    [Authorize(ElsaNextApprovalFlowPermissions.FormDefinitions.Definitions.Publish)]
    public virtual async Task<FormDefinitionDto> PublishAsync(Guid id)
    {
        var definition = await DefinitionManager.PublishAsync(id);
        return Map(definition);
    }

    [Authorize(ElsaNextApprovalFlowPermissions.FormDefinitions.Definitions.Update)]
    public virtual async Task<FormDefinitionDto> AddFieldAsync(Guid id, FormFieldCreateDto input)
    {
        var definition = await DefinitionManager.AddFieldAsync(
            id,
            input.Key,
            input.Label,
            input.Type,
            input.Permission,
            input.Order,
            input.IsRequired,
            input.Hint,
            input.OptionsJson);

        return Map(definition);
    }

    [Authorize(ElsaNextApprovalFlowPermissions.FormDefinitions.Definitions.Update)]
    public virtual async Task<FormDefinitionDto> UpdateFieldAsync(Guid id, Guid fieldId, FormFieldUpdateDto input)
    {
        var definition = await DefinitionManager.UpdateFieldAsync(
            id,
            fieldId,
            input.Label,
            input.Permission,
            input.Order,
            input.IsRequired,
            input.Hint,
            input.OptionsJson);

        return Map(definition);
    }

    [Authorize(ElsaNextApprovalFlowPermissions.FormDefinitions.Definitions.Update)]
    public virtual async Task<FormDefinitionDto> RemoveFieldAsync(Guid id, Guid fieldId)
    {
        var definition = await DefinitionManager.RemoveFieldAsync(id, fieldId);
        return Map(definition);
    }

    [Authorize(ElsaNextApprovalFlowPermissions.FormDefinitions.Data.Edit)]
    public virtual async Task<FormDataDto> SaveDataAsync(Guid id, string instanceId, FormDataSaveDto input)
    {
        var data = await DataManager.SaveAsync(id, instanceId, input.DataJson);
        return Map(data);
    }

    [Authorize(ElsaNextApprovalFlowPermissions.FormDefinitions.Data.Default)]
    public virtual async Task<FormDataDto?> GetDataAsync(Guid id, string instanceId)
    {
        var data = await DataManager.FindAsync(id, instanceId);
        return data == null ? null : Map(data);
    }

    protected virtual FormDefinitionDto Map(FormDefinition definition)
    {
        return new FormDefinitionDto
        {
            Id = definition.Id,
            Name = definition.Name,
            Code = definition.Code,
            Version = definition.Version,
            Description = definition.Description,
            IsPublished = definition.IsPublished,
            PublishedTime = definition.PublishedTime,
            Fields = definition.Fields.OrderBy(x => x.Order).Select(Map).ToList(),
        };
    }

    protected virtual FormFieldDto Map(FormFieldDefinition field)
    {
        return new FormFieldDto
        {
            Id = field.Id,
            Key = field.Key,
            Label = field.Label,
            Type = field.Type,
            Permission = field.Permission,
            Order = field.Order,
            IsRequired = field.IsRequired,
            Hint = field.Hint,
            OptionsJson = field.OptionsJson,
        };
    }

    protected virtual FormDataDto Map(FormData data)
    {
        return new FormDataDto
        {
            Id = data.Id,
            FormDefinitionId = data.FormDefinitionId,
            FormCode = data.FormCode,
            FormVersion = data.FormVersion,
            InstanceId = data.InstanceId,
            DataJson = data.DataJson,
        };
    }
}
