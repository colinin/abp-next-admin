using System;
using System.Threading.Tasks;
using LINGYUN.Abp.ElsaNext.ApprovalFlow.Templates.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.ElsaNext.ApprovalFlow.Templates;

public interface ITemplateAppService : IApplicationService
{
    /// <summary>
    /// 获取可用的审批模板列表
    /// </summary>
    /// <param name="onlyPublished">是否只返回已发布的模板</param>
    Task<ListResultDto<TemplateDto>> GetListAsync(bool onlyPublished = false);

    /// <summary>
    /// 获取审批模板分页列表
    /// </summary>
    Task<PagedResultDto<TemplateDto>> GetPagedListAsync(TemplateGetPagedListInput input);

    Task<TemplateDto> GetAsync(Guid id);

    Task<TemplateDto> CreateAsync(TemplateCreateDto input);

    Task<TemplateDto> UpdateAsync(Guid id, TemplateUpdateDto input);

    Task<TemplateDto> PublishAsync(Guid id, TemplatePublishDto input);

    Task<TemplateDto> SaveDesignAsync(Guid id, TemplateSaveDesignDto input);

    Task DeleteAsync(Guid id);
}
