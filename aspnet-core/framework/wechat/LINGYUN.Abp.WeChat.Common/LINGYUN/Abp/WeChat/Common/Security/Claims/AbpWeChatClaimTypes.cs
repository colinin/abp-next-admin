namespace LINGYUN.Abp.WeChat.Common.Security.Claims
{
    /// <summary>
    /// 微信认证身份类型,可以像 <see cref="Volo.Abp.Security.Claims.AbpClaimTypes"/> 自行配置
    /// <br />
    /// See: <see cref="https://developers.weixin.qq.com/doc/offiaccount/OA_Web_Apps/Wechat_webpage_authorization.html"/>
    /// </summary>
    public class AbpWeChatClaimTypes
    {
        /// <summary>
        /// 用户的唯一标识
        /// </summary>
        public static string OpenId { get; set; } = "wx-openid"; // 可变更
        /// <summary>
        /// 只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段。
        /// </summary>
        public static string UnionId { get; set; } = "wx-unionid"; //可变更
        /// <summary>
        /// 用户昵称
        /// </summary>
        public static string NickName { get; set; } = "nickname";
        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public static string Sex { get; set; } = "sex";
        /// <summary>
        /// 国家，如中国为CN
        /// </summary>
        public static string Country { get; set; } = "country";
        /// <summary>
        /// 用户个人资料填写的省份
        /// </summary>
        public static string Province { get; set; } = "province";
        /// <summary>
        /// 普通用户个人资料填写的城市
        /// </summary>
        public static string City { get; set; } = "city";
        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空。
        /// 若用户更换头像，原有头像URL将失效。
        /// </summary>
        public static string AvatarUrl { get; set; } = "avatar";
        /// <summary>
        /// 用户特权信息，json 数组，如微信沃卡用户为（chinaunicom）
        /// </summary>
        public static string Privilege { get; set; } = "privilege";
    }
}
