using System;

namespace LINGYUN.Abp.IM
{
    public class UserCard
    {
        public Guid? TenantId { get; set; }

        public Guid UserId { get; set; }

        #region 细粒度的用户资料

        public string UserName { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string AvatarUrl { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public Sex Sex { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string Sign { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }

        public bool Online { get; set; }

        #endregion
    }
}
