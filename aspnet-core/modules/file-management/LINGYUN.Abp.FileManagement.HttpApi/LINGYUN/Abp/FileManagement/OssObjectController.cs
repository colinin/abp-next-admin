using LINGYUN.Abp.FileManagement.Settings;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Auditing;
using Volo.Abp.IO;
using Volo.Abp.Settings;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.FileManagement
{
    [RemoteService(Name = "AbpFileManagement")]
    [Area("file-management")]
    [Route("api/file-management/objects")]
    public class OssObjectController : AbpController, IOssObjectAppService
    {
        protected IFileValidater FileValidater { get; }
        protected IOssObjectAppService OssObjectAppService { get; }

        public OssObjectController(
            IFileValidater fileValidater,
            IOssObjectAppService ossObjectAppService)
        {
            FileValidater = fileValidater;
            OssObjectAppService = ossObjectAppService;
        }

        [HttpPost]
        public virtual async Task<OssObjectDto> CreateAsync(CreateOssObjectInput input)
        {
            return await OssObjectAppService.CreateAsync(input);
        }

        [HttpPost]
        [Route("upload")]
        [DisableAuditing]
        public virtual async Task UploadAsync([FromForm] UploadOssObjectInput input)
        {
            await FileValidater.ValidationAsync(input);
            // 以上传的文件名创建一个临时目录
            var tempFilePath = Path.Combine(
                Path.GetTempPath(),
                "lingyun-abp-application",
                "upload-tmp",
                string.Concat(input.Path ?? "", input.FileName).ToMd5());
            DirectoryHelper.CreateIfNotExists(tempFilePath);
            // 以上传的分片索引创建临时文件
            var tempSavedFile = Path.Combine(tempFilePath, $"{input.ChunkNumber}.uploadtmp");
            try
            {
                if (HttpContext.RequestAborted.IsCancellationRequested)
                {
                    // 如果取消请求,删除临时目录
                    Directory.Delete(tempFilePath, true);
                    return;
                }

                if (input.File != null)
                {
                    // 保存临时文件
                    using (var fs = new FileStream(tempSavedFile, FileMode.Create, FileAccess.Write))
                    {
                        // 写入当前分片文件
                        await input.File.CopyToAsync(fs);
                    }
                }

                if (input.ChunkNumber == input.TotalChunks)
                {
                    var createOssObjectInput = new CreateOssObjectInput
                    {
                        Bucket = input.Bucket,
                        Path = input.Path,
                        Object = input.FileName
                    };
                    // 合并文件
                    var mergeSavedFile = Path.Combine(tempFilePath, $"{input.FileName}");
                    // 获取并排序所有分片文件
                    var mergeFiles = Directory.GetFiles(tempFilePath).OrderBy(f => f.Length).ThenBy(f => f);
                    // 创建临时合并文件
                    using (var memoryStream = new MemoryStream())
                    {
                        foreach (var mergeFile in mergeFiles)
                        {
                            // 读取当前文件字节
                            var mergeFileBytes = await FileHelper.ReadAllBytesAsync(mergeFile);
                            // 写入到合并文件流
                            await memoryStream.WriteAsync(mergeFileBytes, HttpContext.RequestAborted);
                            Array.Clear(mergeFileBytes, 0, mergeFileBytes.Length);
                            // 删除已参与合并的临时文件分片
                            FileHelper.DeleteIfExists(mergeFile);
                        }
                        createOssObjectInput.SetContent(memoryStream);

                        await OssObjectAppService.CreateAsync(createOssObjectInput);
                        // 文件保存之后删除临时文件目录
                        Directory.Delete(tempFilePath, true);
                    }
                }
            }
            catch
            {
                // 发生异常删除临时文件目录
                Directory.Delete(tempFilePath, true);
                throw;
            }
        }

        [HttpDelete]
        [Route("bulk-delete")]
        public virtual async Task BulkDeleteAsync([FromBody] BulkDeleteOssObjectInput input)
        {
            await OssObjectAppService.BulkDeleteAsync(input);
        }

        [HttpDelete]
        public virtual async Task DeleteAsync(GetOssObjectInput input)
        {
            await OssObjectAppService.DeleteAsync(input);
        }

        [HttpGet]
        public virtual async Task<OssObjectDto> GetAsync(GetOssObjectInput input)
        {
            return await OssObjectAppService.GetAsync(input);
        }

        private static void ThrowValidationException(string message, string memberName)
        {
            throw new AbpValidationException(message,
                new List<ValidationResult>
                {
                    new ValidationResult(message, new[] {memberName})
                });
        }
    }
}
