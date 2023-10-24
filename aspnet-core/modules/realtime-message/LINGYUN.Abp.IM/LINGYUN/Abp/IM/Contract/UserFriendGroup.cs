using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.IM.Contract
{
    public class UserFriendGroup
    {
        public Guid? TenantId { get; set; }
        public string DisplayName { get; set; }
        public List<UserFriend> UserFriends { get; set; } = new List<UserFriend>();

        public void AddFriend(UserFriend friend)
        {
            UserFriends.AddIfNotContains(friend);
        }
    }
}
