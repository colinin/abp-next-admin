namespace LINGYUN.Abp.EventBus.CAP
{
    /// <summary>
    /// 过期消息清理配置项
    /// </summary>
    public class AbpCAPEventBusOptions
    {
        /// <summary>
        /// 发布消息处理失败通知
        /// </summary>
        public bool NotifyFailedCallback { get; set; } = false;
        /// <summary>
        /// 批量清理数量
        /// default: 1000
        /// </summary>
        public int CleanUpExpiresMessageBatch { get; set; } = 1000;
        /// <summary>
        /// 执行间隔(ms)
        /// default: 3600000 (1 hours)
        /// </summary>
        public int CleanUpExpiresMessageInterval { get; set; } = 3600000;
    }
}
