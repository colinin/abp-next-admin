using LINGYUN.Abp.WeChat.Common;
using Newtonsoft.Json;
using System;

namespace LINGYUN.Abp.WeChat.OpenId
{
    /// <summary>
    /// 微信OpenId返回对象
    /// </summary>
    public class WeChatOpenIdResponse
    {
        /// <summary>
        /// 错误码
        /// </summary>
        [JsonProperty("errcode")]
        public string ErrorCode { get; set; }
        /// <summary>
        /// 会话密钥
        /// </summary>
        [JsonProperty("session_key")]
        public string SessionKey { get; set; }
        /// <summary>
        /// 用户唯一标识
        /// </summary>
        [JsonProperty("openid")]
        public string OpenId { get; set; }
        /// <summary>
        /// 用户在开放平台的唯一标识符，在满足 UnionID 下发条件的情况下会返回
        /// </summary>
        [JsonProperty("unionid")]
        public string UnionId { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                return ErrorCode switch
                {
                    "-1" => "系统繁忙，此时请开发者稍候再试",
                    "0" => string.Empty,
                    "40029" => "code 无效",
                    "45011" => "频率限制，每个用户每分钟100次",
                    _ => $"未定义的异常代码:{ErrorCode},请重试",
                };
                ;
            }
        }
        /// <summary>
        /// 微信认证成功没有errorcode或者errorcode为0
        /// </summary>
        public bool IsError => !ErrorCode.IsNullOrWhiteSpace() && !"0".Equals(ErrorCode);

        public WeChatOpenId ToWeChatOpenId()
        {
            if(IsError)
            {
                throw new AbpWeChatException(ErrorCode, ErrorMessage);
            }
            return new WeChatOpenId(OpenId, SessionKey, UnionId);
        }
    }
}
