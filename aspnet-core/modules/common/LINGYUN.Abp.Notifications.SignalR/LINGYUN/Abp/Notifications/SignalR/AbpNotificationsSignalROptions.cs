namespace LINGYUN.Abp.Notifications.SignalR
{
    public class AbpNotificationsSignalROptions
    {
        /// <summary>
        /// 自定义的客户端订阅通知方法名称
        /// </summary>
        public string MethodName { get; set; }

        public AbpNotificationsSignalROptions()
        {
            MethodName = "get-notification";
        }
    }
}
