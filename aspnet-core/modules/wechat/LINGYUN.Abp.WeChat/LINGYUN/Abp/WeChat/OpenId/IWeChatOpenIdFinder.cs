using System.Threading.Tasks;

namespace LINGYUN.Abp.WeChat.OpenId
{
    public interface IWeChatOpenIdFinder
    {
        Task<WeChatOpenId> FindAsync(string code, string appId, string appSecret);
        /// <summary>
        /// 获取当前登录用户OpenId
        /// </summary>
        /// <param name="appId">应用标识</param>
        /// <returns></returns>
        /// <exception cref="Volo.Abp.Authorization.AbpAuthorizationException">用户未登录时</exception>
        /// <exception cref="Volo.Abp.AbpException">微信sessionKey过期时</exception>
        Task<WeChatOpenId> FindAsync(string appId);
    }
}
