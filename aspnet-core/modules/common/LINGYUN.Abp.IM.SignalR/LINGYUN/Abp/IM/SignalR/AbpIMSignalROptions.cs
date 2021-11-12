namespace LINGYUN.Abp.IM.SignalR
{
    public class AbpIMSignalROptions
    {
        /// <summary>
        /// 自定义的客户端接收消息方法名称
        /// </summary>
        public string GetChatMessageMethod { get; set; }
        /// <summary>
        /// 自定义的客户端撤回消息方法名称
        /// </summary>
        public string ReCallChatMessageMethod { get; set; }
        /// <summary>
        /// 用户上线接收方法名称
        /// </summary>
        public string UserOnlineMethod { get; set; }
        /// <summary>
        /// 用户下线接收方法名称
        /// </summary>
        public string UserOfflineMethod { get; set; }

        public AbpIMSignalROptions()
        {
            GetChatMessageMethod = "get-chat-message";
            ReCallChatMessageMethod = "recall-chat-message";
            UserOnlineMethod = "on-user-onlined";
            UserOfflineMethod = "on-user-offlined";
        }
    }
}
