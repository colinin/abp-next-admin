using System;

namespace LINGYUN.Abp.Notifications
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserIdentifier
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        public UserIdentifier(Guid userId, string userName)
        {
            UserId = userId;
            UserName = userName;
        }
    }
}
