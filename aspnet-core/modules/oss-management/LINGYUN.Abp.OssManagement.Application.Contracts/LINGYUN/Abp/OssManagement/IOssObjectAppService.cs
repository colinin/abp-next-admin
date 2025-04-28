using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace LINGYUN.Abp.OssManagement;

public interface IOssObjectAppService : IApplicationService
{
    Task<OssObjectDto> CreateAsync(CreateOssObjectInput input);

    Task<OssObjectDto> GetAsync(GetOssObjectInput input);
    /// <summary>
    /// 生成下载链接
    /// </summary>
    /// <remarks>
    /// 由于a标签无法传递token, 由后端生成一次性下载链接
    /// </remarks>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<string> GenerateUrlAsync(GetOssObjectInput input);
    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="urlKey">生成的一次性链接key</param>
    /// <returns></returns>
    Task<IRemoteStreamContent> DownloadAsync(string urlKey);

    [Obsolete("请使用 GenerateUrlAsync 与 DownloadAsync的组合")]
    Task<IRemoteStreamContent> GetContentAsync(GetOssObjectInput input);

    Task DeleteAsync(GetOssObjectInput input);

    Task BulkDeleteAsync(BulkDeleteOssObjectInput input);
}
