using LINGYUN.Abp.FileManagement.Settings;
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
        [Route("files/upload")]
        public virtual async Task CreateFileAsync(FileCreateDto input)
        {
            // 检查文件大小
            var fileSizeLimited = await SettingProvider
                .GetAsync(
                    AbpFileManagementSettingNames.FileLimitLength,
                    AbpFileManagementSettingNames.DefaultFileLimitLength);
            if (fileSizeLimited * 1024 * 1024 < input.TotalByte)
            {
                throw new UserFriendlyException(L["UploadFileSizeBeyondLimit", fileSizeLimited]);
            }
            // 采用分块模式上传文件

            // 保存分块到临时目录
            var fileName = input.Name;
            // 文件扩展名
            var fileExtensionName = FileHelper.GetExtension(fileName);
            var fileAllowExtension = await SettingProvider
                .GetOrNullAsync(AbpFileManagementSettingNames.AllowFileExtensions);
            if (fileAllowExtension.IsNullOrWhiteSpace())
            {
                fileAllowExtension = AbpFileManagementSettingNames.DefaultAllowFileExtensions;
            }
            // 检查文件扩展名
            if (!fileAllowExtension.Split(',')
                .Any(fe => fe.Equals(fileExtensionName, StringComparison.CurrentCultureIgnoreCase)))
            {
                throw new UserFriendlyException(L["NotAllowedFileExtensionName", fileExtensionName]);
            }
            // 当前计算机临时目录
            var tempFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Templates);
            // 以上传的文件名创建一个临时目录
            tempFilePath = Path.Combine(tempFilePath, "lingyun-abp-file-management", Path.GetFileNameWithoutExtension(fileName));
            // 以上传的分片索引创建临时文件
            var tempSavedFile = Path.Combine(tempFilePath, $"{input.CurrentByte}.{fileExtensionName}");
            if (!Directory.Exists(tempFilePath))
            {
                // 临时目录不存在则创建
                Directory.CreateDirectory(tempFilePath);
            }
            try
            {
                if (HttpContext.RequestAborted.IsCancellationRequested)
                {
                    // 如果取消请求,删除临时目录
                    Directory.Delete(tempFilePath, true);
                    return;
                }
                // 保存临时文件
                using (var fs = new FileStream(tempSavedFile, FileMode.Create, FileAccess.Write))
                {
                    // 写入当前分片文件
                    await Request.Body.CopyToAsync(fs);
                }

                if (input.CurrentByte == input.TotalByte)
                {
                    // 合并文件
                    var mergeSavedFile = Path.Combine(tempFilePath, $"{fileName}");
                    // 获取并排序所有分片文件
                    var mergeFiles = Directory.GetFiles(tempFilePath).OrderBy(f => f.Length).ThenBy(f => f);
                    // 创建临时合并文件
                    using (var mergeSavedFileStream = new FileStream(mergeSavedFile, FileMode.Create))
                    {
                        foreach (var mergeFile in mergeFiles)
                        {
                            // 读取当前文件字节
                            var mergeFileBytes = await FileHelper.ReadAllBytesAsync(mergeFile);
                            // 写入到合并文件流
                            await mergeSavedFileStream.WriteAsync(mergeFileBytes, 0, mergeFileBytes.Length);
                            // 删除已参与合并的临时文件分片
                            FileHelper.DeleteIfExists(mergeFile);
                        }
                        // 读取文件数据
                        var fileData = await mergeSavedFileStream.GetAllBytesAsync();
                        input.Data = fileData;
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
        [Route("folders/add")]
        public virtual async Task CreateFolderAsync(FolderCreateDto input)
        {
            await FileSystemAppService.CreateFolderAsync(input);
        }

        [HttpDelete]
        [Route("files/delete")]
        public virtual async Task DeleteFileAsync(FileDeleteDto input)
        {
            await FileSystemAppService.DeleteFileAsync(input);
        }

        [HttpDelete]
        [Route("folders/delete")]
        public virtual async Task DeleteFolderAsync([Required, StringLength(255)] string path)
        {
            await FileSystemAppService.DeleteFolderAsync(path);
        }

        [HttpGet]
        [Route("files/download")]
        public virtual async Task DownloadFileAsync(FileSystemGetDto input)
        {
            var fileStream = await FileSystemAppService.DownloadFileAsync(input);

            // 得到文件扩展名
            var fileExt = Path.GetExtension(input.Name);
            var provider = new FileExtensionContentTypeProvider();
            // Http响应标头的文件类型
            string memi = provider.Mappings[fileExt];
            using (Response.Body)
            {
                // Http响应标头的文件类型
                Response.ContentType = memi;
                // 文件大小
                byte[] contentBytes = await fileStream.GetAllBytesAsync();
                long contentLength = contentBytes.Length;
                // 指定响应内容大小
                Response.ContentLength = contentLength;
                // 单个分块大小 2MB
                int bufferSize = 2 * 1024 * 1024;
                // 分块总数
                int contentByteCount = Math.DivRem(contentBytes.Length, bufferSize, out int modResult);
                for (int index = 0; index < contentByteCount; index++)
                {
                    // 当前分页传输字节
                    byte[] currentTransferBytes = new byte[bufferSize];
                    if (index == contentByteCount - 1)
                    {
                        // 最后一个分块和余数大小一起传输
                        if (modResult > 0)
                        {
                            currentTransferBytes = new byte[bufferSize + modResult];
                        }
                    }
                    // 复制文件流中的当前分块区段
                    Array.Copy(contentBytes, index * bufferSize, currentTransferBytes, 0, currentTransferBytes.Length);
                    // 写入响应流
                    await Response.Body.WriteAsync(currentTransferBytes, 0, currentTransferBytes.Length);
                    // 清空缓冲区
                    await Response.Body.FlushAsync();
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
