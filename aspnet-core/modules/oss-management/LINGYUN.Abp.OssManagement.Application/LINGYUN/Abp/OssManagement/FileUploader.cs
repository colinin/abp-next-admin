using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IO;

namespace LINGYUN.Abp.OssManagement
{
    public class FileUploader : IFileUploader, ITransientDependency
    {
        private readonly IFileValidater _fileValidater;
        private readonly FileUploadMerger _fileUploadMerger;

        public FileUploader(
            IFileValidater fileValidater,
            FileUploadMerger fileUploadMerger)
        {
            _fileValidater = fileValidater;
            _fileUploadMerger = fileUploadMerger;
        }

        public virtual async Task UploadAsync(UploadFileChunkInput input, CancellationToken cancellationToken = default)
        {
            await _fileValidater.ValidationAsync(input);
            // 以上传的文件名创建一个临时目录
            var tempFilePath = Path.Combine(
                Path.GetTempPath(),
                "lingyun-abp-application",
                "oss-upload-tmp",
                string.Concat(input.Path ?? "", input.FileName).ToMd5());
            
            try
            {
                DirectoryHelper.CreateIfNotExists(tempFilePath);

                if (cancellationToken.IsCancellationRequested)
                {
                    // 如果取消请求,删除临时目录
                    DirectoryHelper.DeleteIfExists(tempFilePath, true);
                    return;
                }

                // 以上传的分片索引创建临时文件
                var tempSavedFile = Path.Combine(tempFilePath, $"{input.ChunkNumber}.upd");
                if (input.File != null)
                {
                    // 保存临时文件
                    using (var fs = new FileStream(tempSavedFile, FileMode.Create, FileAccess.Write))
                    {
                        // 写入当前分片文件
                        await input.File.GetStream().CopyToAsync(fs);
                    }
                }

                if (input.ChunkNumber == input.TotalChunks)
                {
                    var mergeTmpFile = Path.Combine(tempFilePath, input.FileName);
                    // 创建临时合并文件流
                    using (var mergeFileStream = new FileStream(mergeTmpFile, FileMode.Create, FileAccess.ReadWrite))
                    {
                        var createOssObjectInput = new CreateOssObjectInput
                        {
                            Bucket = input.Bucket,
                            Path = input.Path,
                            FileName = input.FileName
                        };
                        // 合并文件
                        var mergeSavedFile = Path.Combine(tempFilePath, $"{input.FileName}");
                        // 获取并排序所有分片文件
                        var mergeFiles = Directory.GetFiles(tempFilePath, "*.upd").OrderBy(f => f.Length).ThenBy(f => f);
                        // 创建临时合并文件
                        foreach (var mergeFile in mergeFiles)
                        {
                            // 读取当前文件字节
                            var mergeFileBytes = await FileHelper.ReadAllBytesAsync(mergeFile);
                            // 写入到合并文件流
                            await mergeFileStream.WriteAsync(mergeFileBytes, 0, mergeFileBytes.Length, cancellationToken);
                            Array.Clear(mergeFileBytes, 0, mergeFileBytes.Length);
                            // 删除已参与合并的临时文件分片
                            FileHelper.DeleteIfExists(mergeFile);
                        }
                        createOssObjectInput.SetContent(mergeFileStream);
                        // 分离出合并接口,合并时计算上传次数
                        await _fileUploadMerger.MergeAsync(createOssObjectInput);
                    }
                    // 文件保存之后删除临时文件目录
                    DirectoryHelper.DeleteIfExists(tempFilePath, true);
                }
            }
            catch
            {
                // 发生异常删除临时文件目录
                Directory.Delete(tempFilePath, true);
                throw;
            }
        }
    }
}
