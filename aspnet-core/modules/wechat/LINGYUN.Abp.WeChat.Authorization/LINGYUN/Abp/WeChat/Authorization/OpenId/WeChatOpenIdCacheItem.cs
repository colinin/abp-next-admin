using System;

namespace LINGYUN.Abp.WeChat.Authorization
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

        public static string CalculateCacheKey(string code)
        {
            return "c:" + code;
        }
    }
}
