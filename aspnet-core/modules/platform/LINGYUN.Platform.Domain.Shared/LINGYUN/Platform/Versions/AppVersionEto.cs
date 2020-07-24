using System;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Platform.Versions
{
    public class AppVersionEto : IMultiTenant 
    {
        /// <summary>
        /// 租户标识
        /// </summary>
        public Guid? TenantId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 重要级别
        /// </summary>
        public ImportantLevel Level { get; set; }
        /// <summary>
        /// 文件数量
        /// </summary>
        public int FileCount { get; set; }
    }
}
