using IdentityModel.Client;

namespace System.Net.Http
{
    public class WeChatTokenResponse : ProtocolResponse
    {
        /// <summary>
        /// 用户唯一标识
        /// </summary>
        public string OpenId => TryGet("openid");
        /// <summary>
        /// 会话密钥
        /// </summary>
        /// <remarks>
        /// 仅仅只是要一个openid，这个没多大用吧
        /// </remarks>
        public string SessionKey => TryGet("session_key");
        /// <summary>
        /// 用户在开放平台的唯一标识符，在满足 UnionID 下发条件的情况下会返回
        /// </summary>
        public string UnionId => TryGet("unionid");
        /// <summary>
        /// 微信认证成功没有errorcode或者errorcode为0
        /// </summary>
        public new bool IsError => !ErrorCode.IsNullOrWhiteSpace() && !"0".Equals(ErrorCode);
        /// <summary>
        /// 错误码
        /// </summary>
        public string ErrorCode => TryGet("errcode");
        /// <summary>
        /// 错误信息
        /// </summary>
        public new string ErrorMessage
        {
            get
            {
                return ErrorCode switch
                {
                    "-1" => "系统繁忙，此时请开发者稍候再试",
                    "0" => string.Empty,
                    "40029" => "code 无效",
                    "45011" => "频率限制，每个用户每分钟100次",
                    _ => "未知的异常",
                };
            }
        }
    }
}
