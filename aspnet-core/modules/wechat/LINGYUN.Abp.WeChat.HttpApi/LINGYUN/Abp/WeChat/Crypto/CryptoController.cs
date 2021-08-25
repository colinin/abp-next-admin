using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.WeChat.Crypto
{
    [RemoteService(Name = WeChatRemoteServiceConsts.RemoteServiceName)]
    [Area("account")]
    [ApiVersion("1.0")]
    [Route("api/wechat")]
    public class CryptoController : AbpController, ICryptoAppService
    {
        private readonly ICryptoAppService _service;

        public CryptoController(
            ICryptoAppService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("getUserInfo")]
        public virtual async Task<UserInfoDto> GetUserInfoAsync(GetUserInfoInput input)
        {
            return await _service.GetUserInfoAsync(input);
        }
    }
}
