using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace LINGYUN.Abp.WeChat.Official.Account;
public interface IParametricQrCodeAppService : IApplicationService
{
    Task<IRemoteStreamContent> GenerateAsync(ParametricQrCodeGenerateInput input);
}
