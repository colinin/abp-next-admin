namespace LINGYUN.Abp.Authentication.QQ
{
    /// <summary>
    /// QQ互联身份类型,可以像 <see cref="Volo.Abp.Security.Claims.AbpClaimTypes"/> 自行配置
    /// <br />
    /// See: <see cref="https://wiki.connect.qq.com/get_user_info"/>
    /// </summary>
    public class AbpQQClaimTypes
    {
        /// <summary>
        /// 用户的唯一标识
        /// </summary>
        public static string OpenId { get; set; } = "qq-openid"; // 可变更
        /// <summary>
        /// 用户昵称
        /// </summary>
        public static string NickName { get; set; } = "nickname";
        /// <summary>
        /// 性别。 如果获取不到则默认返回"男"
        /// </summary>
        public static string Gender { get; set; } = "gender";
        /// <summary>
        /// 用户头像, 取自字段: figureurl_qq_1
        /// </summary>
        /// <remarks>
        /// 根据QQ互联文档, 40x40的头像是一定会存在的, 只取40x40的头像
        /// see: https://wiki.connect.qq.com/get_user_info
        /// </remarks>
        public static string AvatarUrl { get; set; } = "avatar";
    }
}
