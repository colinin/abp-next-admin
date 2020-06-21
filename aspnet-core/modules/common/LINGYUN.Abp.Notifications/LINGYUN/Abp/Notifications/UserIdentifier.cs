using System;

namespace LINGYUN.Abp.Notifications
{
    public class UserIdentifier
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }

        public UserIdentifier(Guid userId, string userName)
        {
            UserId = userId;
            UserName = userName;
        }
    }
}
