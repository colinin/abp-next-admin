using LINGYUN.Abp.WeChat.Work.Tags.Request;
using LINGYUN.Abp.WeChat.Work.Tags.Response;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.Work.Tags;
/// <summary>
/// 标签管理接口
/// </summary>
public interface IWeChatWorkTagProvider
{
    /// <summary>
    /// 创建标签
    /// </summary>
    /// <remarks>
    /// 详情见：https://developer.work.weixin.qq.com/document/path/90210
    /// </remarks>
    /// <param name="request">创建标签请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns>创建标签响应参数</returns>
    Task<WeChatWorkTagCreateResponse> CreateAsync(
        WeChatWorkTagCreateRequest request, 
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 更新标签名字
    /// </summary>
    /// <remarks>
    /// 详情见：https://developer.work.weixin.qq.com/document/path/90211
    /// </remarks>
    /// <param name="request">更新标签名字请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns>更新标签名字响应参数</returns>
    Task<WeChatWorkResponse> UpdateAsync(
        WeChatWorkTagUpdateRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 删除标签
    /// </summary>
    /// <remarks>
    /// 详情见：https://developer.work.weixin.qq.com/document/path/90212
    /// </remarks>
    /// <param name="request">删除标签请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns>删除标签响应参数</returns>
    Task<WeChatWorkResponse> DeleteAsync(
        WeChatWorkGetTagRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取标签成员
    /// </summary>
    /// <remarks>
    /// 详情见：https://developer.work.weixin.qq.com/document/path/90213
    /// </remarks>
    /// <param name="request">获取标签成员请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns>获取标签成员响应参数</returns>
    Task<WeChatWorkTagMemberInfoResponse> GetMemberAsync(
        WeChatWorkGetTagRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 增加标签成员
    /// </summary>
    /// <remarks>
    /// 详情见：https://developer.work.weixin.qq.com/document/path/90214
    /// </remarks>
    /// <param name="request">增加标签成员请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns>增加标签成员响应参数</returns>
    Task<WeChatWorkTagChangeMemberResponse> AddMemberAsync(
        WeChatWorkTagChangeMemberRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 删除标签成员
    /// </summary>
    /// <remarks>
    /// 详情见：https://developer.work.weixin.qq.com/document/path/90214
    /// </remarks>
    /// <param name="request">增加标签成员请求参数</param>
    /// <param name="cancellationToken"></param>
    /// <returns>增加标签成员响应参数</returns>
    Task<WeChatWorkTagChangeMemberResponse> DeleteMemberAsync(
        WeChatWorkTagChangeMemberRequest request,
        CancellationToken cancellationToken = default);
    /// <summary>
    /// 获取标签列表
    /// </summary>
    /// <remarks>
    /// 详情见：https://developer.work.weixin.qq.com/document/path/90214
    /// </remarks>
    /// <param name="cancellationToken"></param>
    /// <returns>获取标签列表响应参数</returns>
    Task<WeChatWorkTagListResponse> GetListAsync(CancellationToken cancellationToken = default);
}
