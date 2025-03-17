using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace LINGYUN.Abp.Gdpr;

public interface IGdprRequestAppService : IApplicationService
{
    /// <summary>
    /// 获取个人数据请求详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<GdprRequestDto> GetAsync(Guid id);
    /// <summary>
    /// 获取个人数据请求列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PagedResultDto<GdprRequestDto>> GetListAsync(GdprRequestGetListInput input);
    /// <summary>
    /// 下载已收集的用户数据
    /// </summary>
    /// <param name="requestId"></param>
    /// <returns></returns>
    Task<IRemoteStreamContent> DownloadPersonalDataAsync(Guid requestId);
    /// <summary>
    /// 删除个人数据请求
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(Guid id);
    /// <summary>
    /// 准备用户数据
    /// </summary>
    /// <returns></returns>
    Task PreparePersonalDataAsync();
    /// <summary>
    /// 删除用户数据
    /// </summary>
    /// <returns></returns>
    Task DeletePersonalDataAsync();
    /// <summary>
    /// 删除用户账户
    /// </summary>
    /// <returns></returns>
    Task DeletePersonalAccountAsync();
}
