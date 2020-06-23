using System;

namespace LINGYUN.Abp.IM
{
    public class UserCard
    {
        public Guid? TenantId { get; set; }
        public Guid UserId { get; set; }

        #region 下一个版本细粒度的用户资料 与Identity集成

        public string UserName { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string AvatarlUrl { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Arg { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        public string Countriy { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string Sign { get; set; }

        #endregion
    }
}
