namespace LINGYUN.Abp.MessageService
{
    public class MessageServiceErrorCodes
    {
        /// <summary>
        /// 管理员已开启全员禁言
        /// </summary>
        public const string GroupNotAllowedToSpeak = "Messages.Group:1001";
        /// <summary>
        /// 管理员不允许匿名发言
        /// </summary>
        public const string GroupNotAllowedToSpeakAnonymously = "Messages.Group:1002";
        /// <summary>
        /// 管理员已禁止用户发言
        /// </summary>
        public const string GroupUserHasBlack = "Messages.Group:1003";
        /// <summary>
        /// 用户已将发信人拉黑
        /// </summary>
        public const string UserHasBlack = "Messages.User:1003";
        /// <summary>
        /// 用户已拒接所有消息
        /// </summary>
        public const string UserHasRejectAllMessage = "Messages.User:1001";
        /// <summary>
        /// 用户不允许匿名发言
        /// </summary>
        public const string UserNotAllowedToSpeakAnonymously = "Messages.User:1002";
        /// <summary>
        /// 已经添加对方为好友
        /// </summary>
        public const string YouHaveAddedTheUserToFriend = "Messages.UserFriend:1001";
    }
}
