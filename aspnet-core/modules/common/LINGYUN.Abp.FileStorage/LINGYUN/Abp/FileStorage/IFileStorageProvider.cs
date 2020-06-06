using System;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.FileStorage
{
    /// <summary>
    /// 文件存储提供者
    /// </summary>
    public interface IFileStorageProvider
    {
        event EventHandler<FileDownloadProgressEventArges> FileDownloadProgressChanged;
        event EventHandler<FileDownloadCompletedEventArges> FileDownloadCompleted;

        event EventHandler<FileUploadProgressEventArges> FileUploadProgressChanged;
        event EventHandler<FileUploadCompletedEventArges> FileUploadCompleted;

        /// <summary>
        /// 存储文件
        /// </summary>
        /// <param name="fileInfo">文件信息</param>
        /// <param name="expireIn">过期时间,单位(s)</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task StorageAsync(FileInfo fileInfo, int? expireIn = null, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="hash">文件唯一标识</param>
        /// <param name="saveLocalPath">保存到本地路径</param>
        /// <returns></returns>
        Task<FileInfo> GetFileAsync(string hash, string saveLocalPath);
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="hash">文件唯一标识</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteFileAsync(string hash, CancellationToken cancellationToken = default);
    }
}
