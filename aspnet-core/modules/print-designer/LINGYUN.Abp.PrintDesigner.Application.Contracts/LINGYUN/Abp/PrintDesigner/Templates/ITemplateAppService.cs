using LINGYUN.Abp.PrintDesigner.Templates.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.PrintDesigner.Templates;

public interface ITemplateAppService : IApplicationService
{
    /// <summary>
    /// 获取全部模板列表
    /// </summary>
    /// <returns></returns>
    Task<ListResultDto<TemplateDto>> GetAllAsync();
    /// <summary>
    /// 获取模板列表
    /// </summary>
    /// <returns></returns>
    Task<PagedResultDto<TemplateDto>> GetListAsync(TemplateGetPagedListInput input);
    /// <summary>
    /// 获取模板详情
    /// </summary>
    /// <param name="id">模板Id</param>
    /// <returns></returns>
    Task<TemplateDto> GetAsync(Guid id);
    /// <summary>
    /// 获取模板详情
    /// </summary>
    /// <param name="name">模板名称</param>
    /// <returns></returns>
    Task<TemplateDto> GetByNameAsync(string name);
    /// <summary>
    /// 新增模板
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<TemplateDto> CreateAsync(TemplateCreateDto input);
    /// <summary>
    /// 编辑模板
    /// </summary>
    /// <param name="id">模板Id</param>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<TemplateDto> UpdateAsync(Guid id, TemplateUpdateDto input);
    /// <summary>
    /// 新增或编辑模板
    /// </summary>
    /// <remarks>
    /// 兼容upsert接口
    /// </remarks>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<TemplateDto> UpsertAsync(TemplateUpsertDto input);
    /// <summary>
    /// 删除模板
    /// </summary>
    /// <param name="id">模板Id</param>
    /// <returns></returns>
    Task<TemplateDeleteDto> DeleteAsync(Guid id);
}
