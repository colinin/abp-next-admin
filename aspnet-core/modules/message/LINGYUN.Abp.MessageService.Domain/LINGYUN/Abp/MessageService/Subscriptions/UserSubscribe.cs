using System;
using Volo.Abp.Auditing;

namespace LINGYUN.Abp.MessageService.Subscriptions
{
    public class UserSubscribe : Subscribe, IHasCreationTime
    {
        public virtual Guid UserId { get; set; }
        public virtual string UserName { get; set; }
        protected UserSubscribe() { }
        public UserSubscribe(string notificationName, Guid userId, string userName) : base(notificationName)
        {
            UserId = userId;
            UserName = userName;
        }
    }
}
