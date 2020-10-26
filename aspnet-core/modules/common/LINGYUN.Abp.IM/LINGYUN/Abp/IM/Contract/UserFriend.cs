using System;

namespace LINGYUN.Abp.IM.Contract
{
    public class UserFriend : UserCard
    {
        /// <summary>
        /// 好友标识
        /// </summary>
        public Guid FriendId { get; set; }
        /// <summary>
        /// 已添加黑名单
        /// </summary>
        public bool Black { get; set; }
        /// <summary>
        /// 特别关注
        /// </summary>
        public bool SpecialFocus { get; set; }
        /// <summary>
        /// 消息免打扰
        /// </summary>
        public bool DontDisturb { get; set; }
        /// <summary>
        /// 备注名称
        /// </summary>
        public string RemarkName { get; set; }

        public override int GetHashCode()
        {
            return FriendId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj is UserFriend friend)
            {
                return friend.FriendId.Equals(FriendId);
            }
            return false;
        }
    }
}
