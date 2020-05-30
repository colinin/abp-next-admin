using System;
using Volo.Abp.Auditing;

namespace LINGYUN.Abp.MessageService.Subscriptions
{
    public class RoleSubscribe : Subscribe, IHasCreationTime
    {
        public virtual string RoleName { get; set; }
        public virtual DateTime CreationTime { get; set; }
        protected RoleSubscribe() { }
        public RoleSubscribe(string notificationName, string roleName) : base(notificationName)
        {
            RoleName = roleName;
            CreationTime = DateTime.Now;
        }
    }
}
