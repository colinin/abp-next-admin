using LINGYUN.Abp.ElsaNext.ApprovalFlow.Permissions;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Templates.Dtos;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Workflows;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Specifications;
using ExpressionFuncExtender = Volo.Abp.Specifications.ExpressionFuncExtender;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Templates;

[Authorize]
public class TemplateAppService : ApprovalFlowAppServiceBase, ITemplateAppService
{
    protected IApprovalWorkflowCompiler WorkflowCompiler => LazyServiceProvider.LazyGetRequiredService<IApprovalWorkflowCompiler>();

    [Authorize(ElsaNextApprovalFlowPermissions.Templates.Default)]
    public virtual async Task<ListResultDto<TemplateDto>> GetListAsync(bool onlyPublished = false)
    {
        Expression<Func<ApprovalTemplate, bool>> filter = x => x.IsEnabled;
        if (onlyPublished)
        {
            filter = ExpressionFuncExtender.And(filter, x => x.IsPublished);
        }
        var specification = new ExpressionSpecification<ApprovalTemplate>(filter);

        var totalCount = await TemplateRepository.GetCountAsync(specification);
        var templates = await TemplateRepository.GetListAsync(specification, maxResultCount: totalCount);
        return new ListResultDto<TemplateDto>(templates.Select(MapTemplate).ToList());
    }

    [Authorize(ElsaNextApprovalFlowPermissions.Templates.Default)]
    public virtual async Task<PagedResultDto<TemplateDto>> GetPagedListAsync(TemplateGetPagedListInput input)
    {
        Expression<Func<ApprovalTemplate, bool>> filter = _ => true;

        if (!input.Filter.IsNullOrWhiteSpace())
        {
            filter = ExpressionFuncExtender.And(filter, x => x.Name.Contains(input.Filter!) || x.DisplayName.Contains(input.Filter!));
        }
        if (!input.Category.IsNullOrWhiteSpace())
        {
            filter = ExpressionFuncExtender.And(filter, x => x.Category == input.Category);
        }
        if (input.IsPublished.HasValue)
        {
            filter = ExpressionFuncExtender.And(filter, x => x.IsPublished == input.IsPublished.Value);
        }
        if (input.IsEnabled.HasValue)
        {
            filter = ExpressionFuncExtender.And(filter, x => x.IsEnabled == input.IsEnabled.Value);
        }

        var specification = new ExpressionSpecification<ApprovalTemplate>(filter);

        var totalCount = await TemplateRepository.GetCountAsync(specification);
        var templates = await TemplateRepository.GetListAsync(
            specification,
            sorting: input.Sorting!,
            maxResultCount: input.MaxResultCount,
            skipCount: input.SkipCount);

        return new PagedResultDto<TemplateDto>(totalCount, templates.Select(MapTemplate).ToList());
    }

    [Authorize(ElsaNextApprovalFlowPermissions.Templates.Default)]
    public virtual async Task<TemplateDto> GetAsync(Guid id)
    {
        var template = await TemplateRepository.GetAsync(id);
        return MapTemplate(template);
    }

    [Authorize(ElsaNextApprovalFlowPermissions.Templates.Create)]
    public virtual async Task<TemplateDto> CreateAsync(TemplateCreateDto input)
    {
        var template = new ApprovalTemplate(
            GuidGenerator.Create(),
            input.Name,
            input.DisplayName,
            input.Description,
            input.Category,
            input.Icon,
            input.BusinessKeyHint);

        await TemplateRepository.InsertAsync(template, autoSave: true);
        return MapTemplate(template);
    }

    [Authorize(ElsaNextApprovalFlowPermissions.Templates.Update)]
    public virtual async Task<TemplateDto> UpdateAsync(Guid id, TemplateUpdateDto input)
    {
        var template = await TemplateRepository.GetAsync(id);
        template.Update(input.DisplayName, input.Description, input.Category, input.Icon, input.BusinessKeyHint);
        await TemplateRepository.UpdateAsync(template, autoSave: true);
        return MapTemplate(template);
    }

    [Authorize(ElsaNextApprovalFlowPermissions.Templates.Publish)]
    public virtual async Task<TemplateDto> PublishAsync(Guid id, TemplatePublishDto input)
    {
        var template = await TemplateRepository.GetAsync(id);

        // Compile the designer JSON into an Elsa workflow definition and publish it;
        // callers no longer provide an externally created definition version id.
        var definition = await WorkflowCompiler.CompileAsync(template);
        template.Publish(definition.Id, definition.DefinitionId, definition.Version);
        template.BindFormDefinition(input.FormDefinitionId);
        await TemplateRepository.UpdateAsync(template, autoSave: true);
        return MapTemplate(template);
    }

    [Authorize(ElsaNextApprovalFlowPermissions.Templates.Update)]
    public virtual async Task<TemplateDto> SaveDesignAsync(Guid id, TemplateSaveDesignDto input)
    {
        var template = await TemplateRepository.GetAsync(id);
        template.SetFlowDefinitionJson(input.FlowDefinitionJson);
        if (input.FormDefinitionId.HasValue)
        {
            template.BindFormDefinition(input.FormDefinitionId);
        }
        await TemplateRepository.UpdateAsync(template, autoSave: true);
        return MapTemplate(template);
    }

    [Authorize(ElsaNextApprovalFlowPermissions.Templates.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await TemplateRepository.DeleteAsync(id, autoSave: true);
    }
}
