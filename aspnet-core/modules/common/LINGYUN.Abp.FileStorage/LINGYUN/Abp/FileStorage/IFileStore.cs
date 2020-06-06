using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.FileStorage
{
    /// <summary>
    /// 文件存储接口
    /// </summary>
    public interface IFileStore
    {
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
        /// <returns></returns>
        Task<FileInfo> GetFileAsync(string hash);
        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="hash">文件唯一标识</param>
        /// <returns></returns>
        Task<bool> FileHasExistsAsync(string hash);
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="hash">文件唯一标识</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteFileAsync(string hash, CancellationToken cancellationToken = default);
    }
}
