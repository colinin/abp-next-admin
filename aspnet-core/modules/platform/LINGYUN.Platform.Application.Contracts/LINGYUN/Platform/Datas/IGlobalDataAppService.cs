using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Platform.Datas;
/// <summary>
/// 公用数据字典应用服务接口
/// </summary>
public interface IGlobalDataAppService : IApplicationService
{
    /// <summary>
    /// 获取数据字典
    /// </summary>
    /// <param name="name">名称</param>
    /// <returns></returns>
    Task<DataDto> GetByNameAsync(string name);
    /// <summary>
    /// 获取数据字典
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns></returns>
    Task<DataDto> GetAsync(Guid id);
    /// <summary>
    /// 获取所有数据字典
    /// </summary>
    /// <returns></returns>
    Task<ListResultDto<DataDto>> GetAllAsync();
}
