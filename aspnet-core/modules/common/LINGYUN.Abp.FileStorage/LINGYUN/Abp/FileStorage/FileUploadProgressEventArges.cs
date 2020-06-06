using System;

namespace LINGYUN.Abp.FileStorage
{
    public class FileUploadProgressEventArges : EventArgs
    {
        /// <summary>
        /// 上传数据大小
        /// </summary>
        public long BytesSent { get; }
        /// <summary>
        /// 总数据大小
        /// </summary>
        public long TotalBytesSent { get; }
        public FileUploadProgressEventArges(long sent, long total)
        {
            BytesSent = sent;
            TotalBytesSent = total;
        }
    }
}
