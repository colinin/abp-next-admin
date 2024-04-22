using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.WeChat.Work.Message;
/// <summary>
/// 企业微信消息接口
/// </summary>
public interface IWeChatWorkMessageAppService : IApplicationService
{
    /// <summary>
    /// 校验企业微信消息
    /// </summary>
    /// <remarks>
    /// 参考文档：<see cref="https://developer.work.weixin.qq.com/document/path/90238"/>
    /// </remarks>
    /// <param name="agentId"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<string> Handle(string agentId, MessageValidationInput input);
    /// <summary>
    /// 处理企业微信消息
    /// </summary>
    /// <remarks>
    /// 参考文档：<see cref="https://developer.work.weixin.qq.com/document/path/90238"/>
    /// </remarks>
    /// <param name="agentId"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<string> Handle(string agentId, MessageHandleInput input);
}
