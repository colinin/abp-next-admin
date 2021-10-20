namespace LINGYUN.Abp.MessageService
{
    /// <summary>
    /// 消息系统错误码设计
    /// 状态码分为两部分 前2位领域 后3位状态
    /// 
    /// <list type="table">
    /// 领域部分：
    ///     01  输入
    ///     02  群组
    ///     03  用户
    ///     04  应用
    ///     05  内部
    ///     10  输出
    /// </list>
    /// 
    /// <list type="table">
    /// 状态部分：
    ///     200-299 成功
    ///     300-399 成功但有后续操作
    ///     400-499 业务异常
    ///     500-599 内部异常
    ///     900-999 输入输出异常
    /// </list>
    /// 
    /// </summary>
    public class MessageServiceErrorCodes
    {
        public const string Namespace = "LINGYUN.Abp.Message";
        /// <summary>
        /// 试图撤回过期消息
        /// </summary>
        public const string ExpiredMessageCannotBeReCall = Namespace + ":01303";
        /// <summary>
        /// 消息不完整
        /// </summary>
        public const string MessageIncomplete = Namespace + ":01400";
        /// <summary>
        /// 您还未加入群组,不能进行操作
        /// </summary>
        public const string YouHaveNotJoinedGroup = Namespace + ":01401";
        /// <summary>
        /// 已发送群组申请,等待管理员同意
        /// </summary>
        public const string YouHaveAddingToGroup = Namespace + ":02301";
        /// <summary>
        /// 你需要验证问题才能加入群聊
        /// </summary>
        public const string YouNeedValidationQuestingByAddGroup = Namespace + ":02302";
        /// <summary>
        /// 管理员已开启全员禁言
        /// </summary>
        public const string GroupNotAllowedToSpeak = Namespace + ":02400";
        /// <summary>
        /// 管理员已禁止用户发言
        /// </summary>
        public const string GroupUserHasBlack = Namespace + ":02403";
        /// <summary>
        /// 管理员不允许匿名发言
        /// </summary>
        public const string GroupNotAllowedToSpeakAnonymously = Namespace + ":02401";
        /// <summary>
        /// 群组不存在或已解散
        /// </summary>
        public const string GroupNotFount = Namespace + ":02404";
        /// <summary>
        /// 用户已拒接所有消息
        /// </summary>
        public const string UserHasRejectAllMessage = Namespace + ":03400";
        /// <summary>
        /// 用户已将发信人拉黑
        /// </summary>
        public const string UserHasBlack = Namespace + ":03401";
        /// <summary>
        /// 用户不允许匿名发言
        /// </summary>
        public const string UserNotAllowedToSpeakAnonymously = Namespace + ":03402";
        /// <summary>
        /// 用户不接收非好友发言
        /// </summary>
        public const string UserHasRejectNotFriendMessage = Namespace + ":03403";
        /// <summary>
        /// 接收消息用户不存在或已注销
        /// </summary>
        public const string UseNotFount = Namespace + ":03404";
        /// <summary>
        /// 用户拒绝添加好友
        /// </summary>
        public const string UseRefuseToAddFriend = Namespace + ":03410";
        /// <summary>
        /// 对方已是您的好友或已发送验证请求,不能重复操作
        /// </summary>
        public const string UseHasBeenAddedTheFriendOrSendAuthorization = Namespace + ":03411";
        /// <summary>
        /// 已发送好友申请,等待对方同意
        /// </summary>
        public const string YouHaveAddingTheUserToFriend = Namespace + ":03301";
        /// <summary>
        /// 你需要验证问题才能添加好友
        /// </summary>
        public const string YouNeedValidationQuestingByAddFriend = Namespace + ":03302";
    }
}
