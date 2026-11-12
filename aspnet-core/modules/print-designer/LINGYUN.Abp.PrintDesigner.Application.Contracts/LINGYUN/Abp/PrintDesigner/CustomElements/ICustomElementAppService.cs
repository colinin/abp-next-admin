using LINGYUN.Abp.PrintDesigner.CustomElements.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.PrintDesigner.CustomElements;

public interface ICustomElementAppService : IApplicationService
{
    /// <summary>
    /// 获取全部自定义元素列表
    /// </summary>
    /// <returns></returns>
    Task<ListResultDto<CustomElementDto>> GetAllAsync();
    /// <summary>
    /// 获取自定义元素列表
    /// </summary>
    /// <param name="input">查询过滤参数</param>
    /// <returns></returns>
    Task<PagedResultDto<CustomElementDto>> GetListAsync(CustomElementGetPagedListInput input);
    /// <summary>
    /// 获取自定义元素详情
    /// </summary>
    /// <param name="id">自定义元素Id</param>
    /// <returns></returns>
    Task<CustomElementDto> GetAsync(Guid id);
    /// <summary>
    /// 新增自定义元素
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<CustomElementDto> CreateAsync(CustomElementCreateDto input);
    /// <summary>
    /// 编辑自定义元素
    /// </summary>
    /// <param name="id">自定义元素Id</param>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<CustomElementDto> UpdateAsync(Guid id, CustomElementUpdateDto input);
    /// <summary>
    /// 新增或编辑自定义元素
    /// </summary>
    /// <remarks>
    /// 兼容upsert接口
    /// </remarks>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<CustomElementDto> UpsertAsync(CustomElementUpsertDto input);
    /// <summary>
    /// 删除自定义元素
    /// </summary>
    /// <param name="id">自定义元素Id</param>
    /// <returns></returns>
    Task<CustomElementDeleteDto> DeleteAsync(Guid id);
}
