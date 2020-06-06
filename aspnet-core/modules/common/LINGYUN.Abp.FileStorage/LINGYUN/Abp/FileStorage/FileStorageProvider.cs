using System;
using System.Threading;
using System.Threading.Tasks;

namespace LINGYUN.Abp.FileStorage
{
    public abstract class FileStorageProvider : IFileStorageProvider
    {
        public event EventHandler<FileDownloadProgressEventArges> FileDownloadProgressChanged;
        public event EventHandler<FileDownloadCompletedEventArges> FileDownloadCompleted;
        public event EventHandler<FileUploadProgressEventArges> FileUploadProgressChanged;
        public event EventHandler<FileUploadCompletedEventArges> FileUploadCompleted;

        protected IFileStore Store { get; }

        public FileStorageProvider(IFileStore store)
        {
            Store = store;
        }

        public async Task DeleteFileAsync(string hash, CancellationToken cancellationToken = default)
        {
            // 获取文件信息
            var file = await Store.GetFileAsync(hash);
            // 删除文件
            await RemoveFileAsync(file, cancellationToken);
            // 删除文件信息
            await Store.DeleteFileAsync(hash, cancellationToken);
        }

        public async Task<FileInfo> GetFileAsync(string hash, string saveLocalPath)
        {
            // 获取文件信息
            var file = await Store.GetFileAsync(hash);
            // 下载文件
            return await DownloadFileAsync(file, saveLocalPath);
        }

        public async Task StorageAsync(FileInfo fileInfo, int? expireIn = null, CancellationToken cancellationToken = default)
        {
            // step1 上传文件
            await UploadFileAsync(fileInfo, expireIn, cancellationToken);
            // step2 保存文件信息
            await Store.StorageAsync(fileInfo, expireIn, cancellationToken);
        }

        protected abstract Task UploadFileAsync(FileInfo fileInfo, int? expireIn = null, CancellationToken cancellationToken = default);

        protected abstract Task<FileInfo> DownloadFileAsync(FileInfo fileInfo, string saveLocalPath);

        protected abstract Task RemoveFileAsync(FileInfo fileInfo, CancellationToken cancellationToken = default);

        protected virtual void OnFileUploadProgressChanged(long sent, long total)
        {
            FileUploadProgressChanged?.Invoke(this, new FileUploadProgressEventArges(sent, total));
        }

        protected virtual void OnFileUploadConpleted()
        {
            FileUploadCompleted?.Invoke(this, new FileUploadCompletedEventArges());
        }
    }
}
