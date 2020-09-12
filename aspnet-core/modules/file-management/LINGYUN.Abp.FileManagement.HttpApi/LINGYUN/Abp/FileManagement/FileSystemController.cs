using LINGYUN.Abp.FileManagement.Localization;
using LINGYUN.Abp.FileManagement.Permissions;
using LINGYUN.Abp.FileManagement.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.IO;
using Volo.Abp.Settings;
using Volo.Abp.Features;
using LINGYUN.Abp.FileManagement.Features;

namespace LINGYUN.Abp.FileManagement
{
    [Controller]
    [RemoteService(Name = "AbpFileManagement")]
    [Area("file-management")]
    [Route("api/file-management/file-system")]
    public class FileSystemController : AbpController
    {
        protected ISettingProvider SettingProvider { get; }
        protected IFileSystemAppService FileSystemAppService { get; }

        public FileSystemController(
            ISettingProvider settingProvider,
            IFileSystemAppService fileSystemAppService)
        {
            SettingProvider = settingProvider;
            FileSystemAppService = fileSystemAppService;
            LocalizationResource = typeof(AbpFileManagementResource);
        }

        [HttpPut]
        [Route("files/copy")]
        public virtual async Task CopyFileAsync(FileCopyOrMoveDto input)
        {
            await FileSystemAppService.CopyFileAsync(input);
        }

        [HttpPut]
        [Route("folders/copy")]
        public virtual async Task CopyFolderAsync([Required, StringLength(255)] string path, FolderCopyDto input)
        {
            await FileSystemAppService.CopyFolderAsync(path, input);
        }

        [HttpPost]
        [Route("files")]
        [RequiresFeature(AbpFileManagementFeatureNames.FileSystem.UploadFile)]
        [Authorize(AbpFileManagementPermissions.FileSystem.FileManager.Create)]
        public virtual async Task CreateFileAsync([FromForm] FileUploadDto input)
        {
            // 检查文件大小
            var fileSizeLimited = await SettingProvider
                .GetAsync(
                    AbpFileManagementSettingNames.FileLimitLength,
                    AbpFileManagementSettingNames.DefaultFileLimitLength);
            if (fileSizeLimited * 1024 * 1024 < input.TotalSize)
            {
                throw new UserFriendlyException(L["UploadFileSizeBeyondLimit", fileSizeLimited]);
            }
            // 采用分块模式上传文件

            // 保存分块到临时目录
            var fileName = input.FileName;
            // 文件扩展名
            var fileExtensionName = FileHelper.GetExtension(fileName);
            var fileAllowExtension = await SettingProvider
                .GetOrDefaultAsync(AbpFileManagementSettingNames.AllowFileExtensions, ServiceProvider);
            // 检查文件扩展名
            if (!fileAllowExtension.Split(',')
                .Any(fe => fe.Equals(fileExtensionName, StringComparison.CurrentCultureIgnoreCase)))
            {
                throw new UserFriendlyException(L["NotAllowedFileExtensionName", fileExtensionName]);
            }
            // 以上传的文件名创建一个临时目录
            var tempFilePath = Path.Combine(
                Path.GetTempPath(),
                "lingyun-abp-file-management",
                "upload",
                string.Concat(input.Path ?? "", input.FileName).ToMd5());
            DirectoryHelper.CreateIfNotExists(tempFilePath);
            // 以上传的分片索引创建临时文件
            var tempSavedFile = Path.Combine(tempFilePath, $"{input.ChunkNumber}.{fileExtensionName}");
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
                    // 合并文件
                    var mergeSavedFile = Path.Combine(tempFilePath, $"{fileName}");
                    // 获取并排序所有分片文件
                    var mergeFiles = Directory.GetFiles(tempFilePath).OrderBy(f => f.Length).ThenBy(f => f);
                    // 创建临时合并文件
                    input.Data = new byte[0];
                    foreach (var mergeFile in mergeFiles)
                    {
                        // 读取当前文件字节
                        var mergeFileBytes = await FileHelper.ReadAllBytesAsync(mergeFile);
                        // 写入到合并文件流
                        input.Data = input.Data.Concat(mergeFileBytes).ToArray();
                        Array.Clear(mergeFileBytes,0, mergeFileBytes.Length);
                        // 删除已参与合并的临时文件分片
                        FileHelper.DeleteIfExists(mergeFile);
                    }
                    await FileSystemAppService.CreateFileAsync(input);
                    // 文件保存之后删除临时文件目录
                    Directory.Delete(tempFilePath, true);
                }
            }
            catch
            {
                // 发生异常删除临时文件目录
                Directory.Delete(tempFilePath, true);
                throw;
            }
        }

        [HttpPost]
        [Route("folders")]
        public virtual async Task CreateFolderAsync(FolderCreateDto input)
        {
            await FileSystemAppService.CreateFolderAsync(input);
        }

        [HttpDelete]
        [Route("files")]
        public virtual async Task DeleteFileAsync(FileDeleteDto input)
        {
            await FileSystemAppService.DeleteFileAsync(input);
        }

        [HttpDelete]
        [Route("folders")]
        public virtual async Task DeleteFolderAsync([Required, StringLength(255)] string path)
        {
            await FileSystemAppService.DeleteFolderAsync(path);
        }

        [HttpGet]
        [Route("files")]
        [RequiresFeature(AbpFileManagementFeatureNames.FileSystem.DownloadFile)]
        [Authorize(AbpFileManagementPermissions.FileSystem.FileManager.Download)]
        public virtual async Task<IActionResult> DownloadFileAsync(FileSystemDownloadDto input)
        {
            var tempFileName = string.Concat(input.Path ?? "", input.Name);
            tempFileName = tempFileName.ToMd5() + Path.GetExtension(input.Name);
            // 以上传的文件名创建一个临时目录
            var tempFilePath = Path.Combine(
                Path.GetTempPath(),
                "lingyun-abp-file-management",
                "download");
            DirectoryHelper.CreateIfNotExists(tempFilePath);
            tempFilePath = Path.Combine(tempFilePath, tempFileName);
            long contentLength = 0L;
            // 单个分块大小 2MB
            int bufferSize = 2 * 1024 * 1024;
            using (new DisposeAction(() =>
            {
                // 最终下载完毕后，删除临时文件
                if (bufferSize + input.CurrentByte > contentLength)
                {
                    FileHelper.DeleteIfExists(tempFilePath);
                }
            }))
            {
                if (!System.IO.File.Exists(tempFilePath))
                {
                    var blobStream = await FileSystemAppService.DownloadFileAsync(input);
                    using (var tempFileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
                    {
                        blobStream.Position = 0;
                        await blobStream.CopyToAsync(tempFileStream);
                    }
                }
                // 读取缓存文件
                using var fileStream = new FileStream(tempFilePath, FileMode.Open, FileAccess.Read);
                // 得到文件扩展名
                var fileExt = Path.GetExtension(input.Name);
                var provider = new FileExtensionContentTypeProvider();
                // Http响应标头的文件类型
                string memi = provider.Mappings[fileExt];
                // 文件大小
                contentLength = fileStream.Length;
                if (bufferSize > contentLength)
                {
                    var currentTransferBytes = await fileStream.GetAllBytesAsync();
                    // 写入响应流
                    return File(currentTransferBytes, memi, input.Name);
                }
                else
                {
                    // 当前分页传输字节
                    byte[] currentTransferBytes = new byte[bufferSize];
                    if (input.CurrentByte + bufferSize >= contentLength)
                    {
                        currentTransferBytes = new byte[contentLength - input.CurrentByte];
                    }
                    // 读取文件流中的当前分块区段
                    fileStream.Seek(input.CurrentByte, SeekOrigin.Begin);
                    await fileStream.ReadAsync(currentTransferBytes, 0, currentTransferBytes.Length);
                    // 写入响应流
                    return File(currentTransferBytes, memi, input.Name);
                }
            }
        }

        [HttpGet]
        [Route("profile")]
        public virtual async Task<FileSystemDto> GetAsync(FileSystemGetDto input)
        {
            return await FileSystemAppService.GetAsync(input);
        }

        [HttpGet]
        public virtual async Task<PagedResultDto<FileSystemDto>> GetListAsync(GetFileSystemListDto input)
        {
            return await FileSystemAppService.GetListAsync(input);
        }

        [HttpPut]
        [Route("files/move")]
        public virtual async Task MoveFileAsync(FileCopyOrMoveDto input)
        {
            await FileSystemAppService.MoveFileAsync(input);
        }

        [HttpPut]
        [Route("folders/move")]
        public virtual async Task MoveFolderAsync([Required, StringLength(255)] string path, FolderMoveDto input)
        {
            await FileSystemAppService.MoveFolderAsync(path, input);
        }

        [HttpPut]
        public virtual async Task<FileSystemDto> UpdateAsync([Required, StringLength(255)] string name, FileSystemUpdateDto input)
        {
            return await FileSystemAppService.UpdateAsync(name, input);
        }
    }
}
