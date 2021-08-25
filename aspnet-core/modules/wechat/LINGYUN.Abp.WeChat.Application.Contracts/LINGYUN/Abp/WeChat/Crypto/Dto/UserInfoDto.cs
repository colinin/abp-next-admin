namespace LINGYUN.Abp.WeChat.Crypto
{
    public class UserInfoDto
    {
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 用户头像图片的 URL。
        /// URL 最后一个数值代表正方形头像大小（有 0、46、64、96、132 数值可选，0 代表 640x640 的正方形头像，46 表示 46x46 的正方形头像，剩余数值以此类推。默认132），
        /// 用户没有头像时该项为空。
        /// 若用户更换头像，原有头像 URL 将失效。
        /// </summary>
        public string AvatarUrl { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public Gender Gender { get; set; }
        /// <summary>
        /// 用户所在国家
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 用户所在省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 用户所在城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 显示 country，province，city 所用的语言
        /// </summary>
        public string Language { get; set; }
    }
}
