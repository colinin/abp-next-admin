using System;

namespace LINGYUN.Abp.FileStorage
{
    /// <summary>
    /// 文件信息
    /// </summary>
    public class FileInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 大小
        /// </summary>
        public long Size { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string Directory { get; set; }
        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string Extension { get; set; }
        /// <summary>
        /// 文件哈希码,用于唯一标识
        /// </summary>
        public string Hash { get; set; }
        /// <summary>
        /// 文件链接
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 文件数据
        /// </summary>
        public byte[] Data { get; set; }
        /// <summary>
        /// 媒体类型
        /// </summary>
        public MediaType MediaType { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public Guid? CreatorId { get; set; }
        /// <summary>
        /// 上次变更时间
        /// </summary>
        public DateTime? LastModificationTime { get; set; }
        /// <summary>
        /// 上次变更人
        /// </summary>
        public Guid? LastModifierId { get; set; }
    }
}
