using LINGYUN.Abp.WeChat.Work.Media.Models;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Content;

namespace LINGYUN.Abp.WeChat.Work.Media;
/// <summary>
/// 素材管理接口
/// </summary>
/// <remarks>
/// API: <see cref="https://developer.work.weixin.qq.com/document/path/91054"/>
/// </remarks>
public interface IWeChatWorkMediaProvider
{
    /// <summary>
    /// 上传临时素材
    /// </summary>
    /// <remarks>
    /// API: <see cref="https://developer.work.weixin.qq.com/document/path/90253"/>
    /// </remarks>
    /// <param name="agentId">应用标识</param>
    /// <param name="type">媒体文件类型</param>
    /// <param name="media">待上传文件</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkMediaResponse> UploadAsync(
        string agentId,
        string type,
        IRemoteStreamContent media,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取临时素材
    /// </summary>
    /// <remarks>
    /// API: <see cref="https://developer.work.weixin.qq.com/document/path/90254"/>
    /// </remarks>
    /// <param name="agentId">应用标识</param>
    /// <param name="mediaId">媒体文件id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// 
    Task<IRemoteStreamContent> GetAsync(
        string agentId,
        string mediaId,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 上传图片
    /// </summary>
    /// <remarks>
    /// API: <see cref="https://developer.work.weixin.qq.com/document/path/90256"/>
    /// </remarks>
    /// <param name="agentId">应用标识</param>
    /// <param name="image">待上传图片</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<WeChatWorkImageResponse> UploadImageAsync(
        string agentId,
        IRemoteStreamContent image,
        CancellationToken cancellationToken = default);
}
