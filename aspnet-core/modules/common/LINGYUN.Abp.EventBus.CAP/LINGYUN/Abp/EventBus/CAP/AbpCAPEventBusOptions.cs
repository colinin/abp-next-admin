namespace LINGYUN.Abp.EventBus.CAP
{
    /// <summary>
    /// 过期消息清理配置项
    /// </summary>
    public class AbpCAPEventBusOptions
    {
        /// <summary>
        /// 发布消息处理失败通知
        /// default: false
        /// </summary>
        public bool NotifyFailedCallback { get; set; } = false;
    }
}
