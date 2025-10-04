using LINGYUN.Abp.WeChat.Work.JsSdk.Dtos;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.WeChat.Work.JsSdk;
public interface IWeChatWorkJsSdkAppService : IApplicationService
{
    Task<AgentConfigDto> GetAgentConfigAsync();

    Task<JsApiSignatureDto> GetSignatureAsync(string url);

    Task<JsApiSignatureDto> GetAgentSignatureAsync(string url);
}
