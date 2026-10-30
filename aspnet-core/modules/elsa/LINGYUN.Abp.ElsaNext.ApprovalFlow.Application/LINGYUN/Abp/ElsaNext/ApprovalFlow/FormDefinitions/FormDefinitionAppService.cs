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

        return new PagedResultDto<FormDefinitionDto>(totalCount, definitions.Select(MapDefinition).ToList());
    }

    [Authorize(ElsaNextApprovalFlowPermissions.FormDefinitions.Definitions.Default)]
    public virtual async Task<FormDefinitionDto> GetAsync(Guid id)
    {
        var definition = await DefinitionRepository.GetAsync(id);
        return MapDefinition(definition);
    }

    [Authorize(ElsaNextApprovalFlowPermissions.FormDefinitions.Definitions.Create)]
    public virtual async Task<FormDefinitionDto> CreateAsync(FormDefinitionCreateDto input)
    {
        var definition = await DefinitionManager.CreateAsync(input.Name, input.Code, input.Version, input.Description);
        return MapDefinition(definition);
    }

    [Authorize(ElsaNextApprovalFlowPermissions.FormDefinitions.Definitions.Update)]
    public virtual async Task<FormDefinitionDto> UpdateAsync(Guid id, FormDefinitionUpdateDto input)
    {
        var definition = await DefinitionManager.UpdateAsync(id, input.Name, input.Description);
        return MapDefinition(definition);
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
        return MapDefinition(definition);
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

        return MapDefinition(definition);
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

        return MapDefinition(definition);
    }

    [Authorize(ElsaNextApprovalFlowPermissions.FormDefinitions.Definitions.Update)]
    public virtual async Task<FormDefinitionDto> RemoveFieldAsync(Guid id, Guid fieldId)
    {
        var definition = await DefinitionManager.RemoveFieldAsync(id, fieldId);
        return MapDefinition(definition);
    }

    [Authorize(ElsaNextApprovalFlowPermissions.FormDefinitions.Data.Edit)]
    public virtual async Task<FormDataDto> SaveDataAsync(Guid id, string instanceId, FormDataSaveDto input)
    {
        var data = await DataManager.SaveAsync(id, instanceId, input.DataJson);
        return MapData(data);
    }

    [Authorize(ElsaNextApprovalFlowPermissions.FormDefinitions.Data.Default)]
    public virtual async Task<FormDataDto?> GetDataAsync(Guid id, string instanceId)
    {
        var data = await DataManager.FindAsync(id, instanceId);
        return data == null ? null : MapData(data);
    }

    protected virtual FormDefinitionDto MapDefinition(FormDefinition definition)
    {
        var dto = ObjectMapper.Map<FormDefinition, FormDefinitionDto>(definition);
        dto.Fields = definition.Fields
            .OrderBy(x => x.Order)
            .Select(field => ObjectMapper.Map<FormFieldDefinition, FormFieldDto>(field))
            .ToList();
        return dto;
    }

    protected virtual FormDataDto MapData(FormData data)
    {
        return ObjectMapper.Map<FormData, FormDataDto>(data);
    }
}
