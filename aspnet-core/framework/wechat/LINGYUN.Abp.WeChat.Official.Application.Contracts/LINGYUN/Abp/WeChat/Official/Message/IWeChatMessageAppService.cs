using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.WeChat.Official.Message;
/// <summary>
/// 微信消息接口
/// </summary>
public interface IWeChatMessageAppService : IApplicationService
{
    /// <summary>
    /// 校验微信消息
    /// </summary>
    /// <remarks>
    /// 参考文档：<see cref="https://developers.weixin.qq.com/doc/offiaccount/Basic_Information/Access_Overview.html"/>
    /// </remarks>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<string> Handle(MessageValidationInput input);
    /// <summary>
    /// 处理微信消息
    /// </summary>
    /// <remarks>
    /// 参考文档：<see cref="https://developers.weixin.qq.com/doc/offiaccount/Basic_Information/Access_Overview.html"/>
    /// </remarks>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<string> Handle(MessageHandleInput input);
}
