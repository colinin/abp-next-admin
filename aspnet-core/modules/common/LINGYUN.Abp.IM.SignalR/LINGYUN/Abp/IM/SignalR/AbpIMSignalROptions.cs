namespace LINGYUN.Abp.IM.SignalR
{
    public class AbpIMSignalROptions
    {
        /// <summary>
        /// 自定义的客户端接收消息方法名称
        /// </summary>
        public string GetChatMessageMethod { get; set; }
        /// <summary>
        /// 用户上线接收方法名称
        /// </summary>
        public string UserOnlineMethod { get; set; }
        public AbpIMSignalROptions()
        {
            GetChatMessageMethod = "getChatMessage";
            UserOnlineMethod = "onUserOnlined";
        }
    }
}
