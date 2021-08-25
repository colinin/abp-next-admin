using System;

namespace LINGYUN.Abp.WeChat.OpenId
{
    public class WeChatOpenIdCacheItem
    {
        public string Code { get; set; }

        public WeChatOpenId WeChatOpenId { get; set; }
        public WeChatOpenIdCacheItem()
        {

        }

        public WeChatOpenIdCacheItem(string code, WeChatOpenId weChatOpenId)
        {
            Code = code;
            WeChatOpenId = weChatOpenId;
        }

        public static string CalculateCacheKey(string appId, Guid userId)
        {
            return "app:" + appId + ";user:" + userId.ToString("D");
        }


        public static string CalculateCacheKey(string appId, string code)
        {
            return "app:" + appId + ";code:" + code;
        }
    }
}
