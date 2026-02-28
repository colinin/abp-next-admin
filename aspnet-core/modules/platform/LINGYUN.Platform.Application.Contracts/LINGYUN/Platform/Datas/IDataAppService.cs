using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Platform.Datas;
/// <summary>
/// 数据字典应用服务接口
/// </summary>
public interface IDataAppService : 
    ICrudAppService<
        DataDto,
        Guid,
        GetDataListInput,
        DataCreateDto,
        DataUpdateDto>
{
    /// <summary>
    /// 获取数据字典
    /// </summary>
    /// <param name="name">名称</param>
    /// <returns></returns>
    Task<DataDto> GetAsync(string name);
    /// <summary>
    /// 获取所有数据字典
    /// </summary>
    /// <returns></returns>
    Task<ListResultDto<DataDto>> GetAllAsync();
    /// <summary>
    /// 移除数据字典
    /// </summary>
    /// <param name="id">字典Id</param>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<DataDto> MoveAsync(Guid id, DataMoveDto input);
    /// <summary>
    /// 新增数据字典项目
    /// </summary>
    /// <param name="id">字典Id</param>
    /// <param name="input"></param>
    /// <returns></returns>
    Task CreateItemAsync(Guid id, DataItemCreateDto input);
    /// <summary>
    /// 更新数据字典项目
    /// </summary>
    /// <param name="id">字典Id</param>
    /// <param name="name">项目名称</param>
    /// <param name="input"></param>
    /// <returns></returns>
    Task UpdateItemAsync(Guid id, string name, DataItemUpdateDto input);
    /// <summary>
    /// 删除数据字典项目
    /// </summary>
    /// <param name="id">字典Id</param>
    /// <param name="name">项目名称</param>
    /// <returns></returns>
    Task DeleteItemAsync(Guid id, string name);
}
