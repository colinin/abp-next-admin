using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.MessageService.Messages
{
    /// <summary>
    /// 群管理员
    /// </summary>
    public class ChatGroupAdmin : AuditedEntity<long>, IMultiTenant
    {
        /// <summary>
        /// 租户
        /// </summary>
        public virtual Guid? TenantId { get; protected set; }
        /// <summary>
        /// 群组标识
        /// </summary>
        public virtual long GroupId { get; protected set; }
        /// <summary>
        /// 管理员用户
        /// </summary>
        public virtual Guid UserId { get; protected set; }
        /// <summary>
        /// 是否群主
        /// </summary>
        public virtual bool IsSuperAdmin { get; protected set; }
        /// <summary>
        /// 允许禁言
        /// </summary>
        public virtual bool AllowSilence { get; set; }
        /// <summary>
        /// 允许踢人
        /// </summary>
        public virtual bool AllowKickPeople { get; set; }
        /// <summary>
        /// 允许加人
        /// </summary>
        public virtual bool AllowAddPeople { get; set; }
        /// <summary>
        /// 允许发送群公告
        /// </summary>
        public virtual bool AllowSendNotice { get; set; }
        /// <summary>
        /// 允许解散群组
        /// </summary>
        public virtual bool AllowDissolveGroup { get; set; }
        protected ChatGroupAdmin() { }
        public ChatGroupAdmin(long groupId, Guid userId, bool isSuperAdmin = false)
        {
            GroupId = groupId;
            UserId = userId;
            AllowSilence = true;
            AllowKickPeople = true;
            AllowAddPeople = true;
            AllowSendNotice = true;
            SetSuperAdmin(isSuperAdmin);
        }

        public void SetSuperAdmin(bool isSuperAdmin = false)
        {
            IsSuperAdmin = isSuperAdmin;
            if (isSuperAdmin)
            {
                AllowDissolveGroup = true;
            }
        }
    }
}
