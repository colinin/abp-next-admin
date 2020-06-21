using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;

namespace LINGYUN.Abp.Account
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IdentityUserDto> RegisterAsync(PhoneNumberRegisterDto input);

        Task<IdentityUserDto> RegisterAsync(WeChatRegisterDto input);

        Task ResetPasswordAsync(PasswordResetDto passwordReset);

        Task VerifyPhoneNumberAsync(VerifyDto input);
    }
}
