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
        /// <summary>
        /// 事件名称定义在事件参数类型
        /// default: true
        /// </summary>
        public bool NameInEventDataType { get; set; } = true;
        /// <summary>
        /// 批量清理数量
        /// default: 1000
        /// </summary>
        public int CleanUpExpiresMessageBatch { get; set; } = 1000;
        /// <summary>
        /// 执行间隔(ms)
        /// default: 3600000 (1 hours)
        /// </summary>
        public int CleanUpExpiresMessageInterval { get; set; } = 360_0000;
    }
}
