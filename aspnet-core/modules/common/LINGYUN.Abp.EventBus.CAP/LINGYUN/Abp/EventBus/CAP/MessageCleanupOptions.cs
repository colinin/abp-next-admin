namespace LINGYUN.Abp.EventBus.CAP
{
    /// <summary>
    /// 过期消息清理配置项
    /// </summary>
    public class MessageCleanupOptions
    {
        /// <summary>
        /// 批量清理数量
        /// default: 1000
        /// </summary>
        public int ItemBatch { get; set; } = 1000;
        /// <summary>
        /// 执行间隔(ms)
        /// default: 3600000 (1 hours)
        /// </summary>
        public int Interval { get; set; } = 3600000;
    }
}
