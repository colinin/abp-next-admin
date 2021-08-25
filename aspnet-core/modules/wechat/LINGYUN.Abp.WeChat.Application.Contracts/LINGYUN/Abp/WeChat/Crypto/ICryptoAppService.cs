using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.WeChat.Crypto
{
    public interface ICryptoAppService : IApplicationService
    {
        Task<UserInfoDto> GetUserInfoAsync(GetUserInfoInput input);
    }
}
