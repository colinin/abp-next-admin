using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;

namespace LINGYUN.Abp.MessageService.Groups
{
    /// <summary>
    /// 用户群组卡片
    /// </summary>
    public class UserGroupCard : AuditedAggregateRoot<long>, IMultiTenant
    {
        /// <summary>
        /// 租户
        /// </summary>
        public virtual Guid? TenantId { get; protected set; }
        /// <summary>
        /// 用户标识
        /// </summary>
        public virtual Guid UserId { get; protected set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public virtual string NickName { get; set; }
        /// <summary>
        /// 是否管理员
        /// </summary>
        public virtual bool IsAdmin { get; protected set; }
        /// <summary>
        /// 禁言期止
        /// </summary>
        public virtual DateTime? SilenceEnd { get; protected set; }
        protected UserGroupCard()
        {
        }
        public UserGroupCard(
            Guid userId,
            string nickName = "",
            bool isAdmin = false,
            DateTime? silenceEnd = null,
            Guid? tenantId = null)
        {
            UserId = userId;
            NickName = nickName;
            IsAdmin = isAdmin;
            SilenceEnd = silenceEnd;
            TenantId = tenantId;
        }
        /// <summary>
        /// 设置管理员
        /// </summary>
        /// <param name="admin"></param>
        public void SetAdmin(bool admin)
        {
            IsAdmin = admin;
        }
        /// <summary>
        /// 禁言
        /// </summary>
        /// <param name="clock"></param>
        /// <param name="seconds"></param>
        public void Silence(IClock clock, double seconds)
        {
            SilenceEnd = clock.Now.AddSeconds(seconds);
        }
        /// <summary>
        /// 解除禁言
        /// </summary>
        public void UnSilence()
        {
            SilenceEnd = null;
        }
    }
}
