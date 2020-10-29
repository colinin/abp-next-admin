using LINGYUN.Abp.IM.Contract;
using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.MessageService.Chat
{
    [Serializable]
    public class UserFriendCacheItem
    {
        public List<UserFriend> Friends { get; set; }

        public UserFriendCacheItem()
        {
            Friends = new List<UserFriend>();
        }

        public UserFriendCacheItem(List<UserFriend> friends)
        {
            Friends = friends;
        }

        public static string CalculateCacheKey(string userId)
        {
            return "uid:" + userId;
        }
    }
}
