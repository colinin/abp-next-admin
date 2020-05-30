using System;
using Volo.Abp.Auditing;

namespace LINGYUN.Abp.MessageService.Subscriptions
{
    public class UserSubscribe : Subscribe, IHasCreationTime
    {
        public virtual DateTime CreationTime { get; set; }
        public virtual Guid UserId { get; set; }
        protected UserSubscribe() { }
        public UserSubscribe(string notificationName, Guid userId) : base(notificationName)
        {
            CreationTime = DateTime.Now;
            UserId = userId;
        }
    }
}
