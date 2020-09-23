using Volo.Abp.Security.Claims;

namespace Volo.Abp.Users
{
    public static class CurrentUserExtensions
    {
        /// <summary>
        /// 获取用户微信id,如果不存在返回空值
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public static string FindWeChatId(this ICurrentUser currentUser)
        {
            var weChatClaim = currentUser.FindClaim(WeChatClaimTypes.OpenId);
            if (weChatClaim == null)
            {
                return null;
            }

            return weChatClaim.Value;
        }
    }
}
