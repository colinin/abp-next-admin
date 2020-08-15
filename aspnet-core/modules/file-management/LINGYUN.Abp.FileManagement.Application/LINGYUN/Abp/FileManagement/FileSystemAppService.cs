using LINGYUN.Abp.FileManagement.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;

namespace LINGYUN.Abp.FileManagement
{
    [Authorize(AbpFileManagementPermissions.FileSystem.Default)]
    public class FileSystemAppService : FileManagementApplicationServiceBase, IFileSystemAppService
    {
        protected IBlobContainer<FileSystemContainer> BlobContainer { get; }
        protected IBlobContainerConfigurationProvider BlobContainerConfigurationProvider { get; }
        public FileSystemAppService(
            IBlobContainer<FileSystemContainer> blobContainer,
            IBlobContainerConfigurationProvider blobContainerConfigurationProvider)
        {
            BlobContainer = blobContainer;
            BlobContainerConfigurationProvider = blobContainerConfigurationProvider;
        }

        [Authorize(AbpFileManagementPermissions.FileSystem.FileManager.Copy)]
        public virtual Task CopyFileAsync(FileCopyOrMoveDto input)
        {
            string fileSystemPath = GetFileSystemPath(input.Path);
            var fileFullName = Path.Combine(fileSystemPath, input.Name);
            if (!File.Exists(fileFullName))
            {
                throw new UserFriendlyException(L["FilePathNotFound"]);
            }
            var copyToFilePath = GetFileSystemPath(input.ToPath);
            var copyToFileFullName = Path.Combine(copyToFilePath, input.ToName ?? input.Name);
            if (File.Exists(copyToFileFullName))
            {
                throw new UserFriendlyException(L["FilePathAlreadyExists"]);
            }

            File.Copy(fileFullName, copyToFileFullName);

            return Task.CompletedTask;
        }

        [Authorize(AbpFileManagementPermissions.FileSystem.Copy)]
        public virtual Task CopyFolderAsync([Required, StringLength(255)] string path, FolderCopyDto input)
        {
            string fileSystemPath = GetFileSystemPath(path);
            if (!Directory.Exists(fileSystemPath))
            {
                throw new UserFriendlyException("指定目录不存在!");
            }
            var copyToFilePath = GetFileSystemPath(input.CopyToPath);
            if (Directory.Exists(copyToFilePath))
            {
                throw new UserFriendlyException("指定的路径中已经有同名的目录存在!");
            }

            CopyDirectory(fileSystemPath, copyToFilePath);

            return Task.CompletedTask;
        }

        [Authorize(AbpFileManagementPermissions.FileSystem.FileManager.Create)]
        public virtual async Task CreateFileAsync(FileCreateDto input)
        {
            string fileSystemPath = GetFileSystemPath(input.Path);
            fileSystemPath = Path.Combine(fileSystemPath, input.FileName);
            var blobName = GetFileSystemRelativePath(fileSystemPath);
            // 去除第一个路径标识符
            blobName = blobName.RemovePreFix("/", "\\");
            if (!input.Rewrite && await BlobContainer.ExistsAsync(blobName))
            {
                throw new UserFriendlyException(L["FilePathAlreadyExists"]);
            }
            await BlobContainer.SaveAsync(blobName, input.Data, input.Rewrite);
            Array.Clear(input.Data, 0, input.Data.Length);
        }

        [Authorize(AbpFileManagementPermissions.FileSystem.Create)]
        public virtual Task CreateFolderAsync(FolderCreateDto input)
        {
            string fileSystemPath = GetFileSystemBashPath();
            if (!input.Parent.IsNullOrWhiteSpace())
            {
                fileSystemPath = GetFileSystemPath(input.Parent);
            }
            var newFloderPath = Path.Combine(fileSystemPath, input.Path);
            if (Directory.Exists(newFloderPath))
            {
                throw new UserFriendlyException(L["PathAlreadyExists"]);
            }
            Directory.CreateDirectory(newFloderPath);

            return Task.CompletedTask;
        }

        [Authorize(AbpFileManagementPermissions.FileSystem.FileManager.Delete)]
        public virtual Task DeleteFileAsync(FileDeleteDto input)
        {
            var fileSystemPath = GetFileSystemPath(input.Path);
            fileSystemPath = Path.Combine(fileSystemPath, input.Name);
            if (File.Exists(fileSystemPath))
            {
                File.Delete(fileSystemPath);
            }
            return Task.CompletedTask;
        }

        [Authorize(AbpFileManagementPermissions.FileSystem.Delete)]
        public virtual Task DeleteFolderAsync([Required, StringLength(255)] string path)
        {
            string fileSystemPath = GetFileSystemPath(path);
            if (!Directory.Exists(fileSystemPath))
            {
                throw new UserFriendlyException(L["FilePathNotFound"]);
            }
            var fileSystemChildrenPath = Directory.GetDirectories(fileSystemPath);
            if (fileSystemChildrenPath.Length > 0)
            {
                throw new UserFriendlyException(L["PathCannotBeDeletedWithNotEmpty"]);
            }
            var fileSystemPathFiles = Directory.GetFiles(fileSystemPath);
            if (fileSystemPathFiles.Length > 0)
            {
                throw new UserFriendlyException(L["PathCannotBeDeletedWithNotEmpty"]);
            }
            Directory.Delete(fileSystemPath);
            return Task.CompletedTask;
        }

        [Authorize(AbpFileManagementPermissions.FileSystem.FileManager.Download)]
        public virtual async Task<Stream> DownloadFileAsync(FileSystemGetDto input)
        {
            var fileSystemPath = GetFileSystemPath(input.Path);
            fileSystemPath = Path.Combine(fileSystemPath, input.Name);
            var blobName = GetFileSystemRelativePath(fileSystemPath);
            // 去除第一个路径标识符
            blobName = blobName.RemovePreFix("/", "\\");
            return await BlobContainer.GetAsync(blobName);
        }

        public virtual Task<FileSystemDto> GetAsync(FileSystemGetDto input)
        {
            var fileSystemPath = GetFileSystemPath(input.Path);
            fileSystemPath = Path.Combine(fileSystemPath, input.Name);
            if (File.Exists(fileSystemPath))
            {
                var fileInfo = new FileInfo(fileSystemPath);
                var fileSystem = new FileSystemDto
                {
                    Type = FileSystemType.File,
                    Name = fileInfo.Name,
                    Size = fileInfo.Length,
                    Extension = fileInfo.Extension,
                    CreationTime = fileInfo.CreationTime,
                    LastModificationTime = fileInfo.LastWriteTime
                };
                if (fileInfo.Directory != null && !fileInfo.Directory.FullName.IsNullOrWhiteSpace())
                {
                    fileSystem.Parent = GetFileSystemRelativePath(fileInfo.Directory.FullName);
                }
                return Task.FromResult(fileSystem);
            }
            if (Directory.Exists(fileSystemPath))
            {
                var directoryInfo = new DirectoryInfo(fileSystemPath);
                var fileSystem = new FileSystemDto
                {
                    Type = FileSystemType.Folder,
                    Name = directoryInfo.Name,
                    CreationTime = directoryInfo.CreationTime,
                    LastModificationTime = directoryInfo.LastWriteTime
                };
                if (directoryInfo.Parent != null && !directoryInfo.Parent.FullName.IsNullOrWhiteSpace())
                {
                    fileSystem.Parent = GetFileSystemRelativePath(directoryInfo.Parent.FullName);
                }
                return Task.FromResult(fileSystem);
            }
            throw new UserFriendlyException(L["FilePathNotFound"]);
        }

        public virtual Task<PagedResultDto<FileSystemDto>> GetListAsync(GetFileSystemListDto input)
        {
            List<FileSystemDto> fileSystems = new List<FileSystemDto>();

            string fileSystemPath = GetFileSystemBashPath();
            if (!input.Parent.IsNullOrWhiteSpace())
            {
                fileSystemPath = GetFileSystemPath(input.Parent);
            }
            var directoryInfo = new DirectoryInfo(fileSystemPath);
            if (!directoryInfo.Exists)
            {
                return Task.FromResult(new PagedResultDto<FileSystemDto>(0, fileSystems));
            }
            // 查询全部文件系统
            var fileSystemInfos = directoryInfo.GetFileSystemInfos();
            // 指定搜索条件查询目录
            FileSystemInfo[] fileSystemInfoSearchChildren;
            if (!input.Filter.IsNullOrWhiteSpace())
            {
                var searchPattern = $"*{input.Filter}*";
                fileSystemInfoSearchChildren = directoryInfo.GetFileSystemInfos(searchPattern);
            }
            else
            {
                fileSystemInfoSearchChildren = directoryInfo.GetFileSystemInfos();
            }

            fileSystemInfoSearchChildren = fileSystemInfoSearchChildren
                .Skip((input.SkipCount - 1) * input.MaxResultCount)
                .Take(input.MaxResultCount)
                .ToArray();

            foreach (var fileSystemInfo in fileSystemInfoSearchChildren)
            {
                var fileSystem = new FileSystemDto
                {
                    Name = fileSystemInfo.Name,
                    CreationTime = fileSystemInfo.CreationTime,
                    LastModificationTime = fileSystemInfo.LastWriteTime,
                };

                if (fileSystemInfo is FileInfo fileInfo)
                {
                    fileSystem.Type = FileSystemType.File;
                    fileSystem.Size = fileInfo.Length;
                    fileSystem.Extension = fileInfo.Extension;
                    if (fileInfo.Directory != null && !fileInfo.Directory.FullName.IsNullOrWhiteSpace())
                    {
                        fileSystem.Parent = GetFileSystemRelativePath(fileInfo.Directory.FullName);
                    }
                }
                else if (fileSystemInfo is DirectoryInfo directory)
                {
                    fileSystem.Type = FileSystemType.Folder;
                    if (directory.Parent != null && !directory.Parent.FullName.IsNullOrWhiteSpace())
                    {
                        fileSystem.Parent = GetFileSystemRelativePath(directory.Parent.FullName);
                    }
                }
                fileSystems.Add(fileSystem);
            }

            fileSystems = fileSystems
                .OrderBy(f => f.Type)
                .ThenBy(f => f.Name)
                .ToList();

            return Task.FromResult(new PagedResultDto<FileSystemDto>(
                fileSystemInfos.Length, fileSystems
            ));
        }

        [Authorize(AbpFileManagementPermissions.FileSystem.FileManager.Move)]
        public virtual Task MoveFileAsync(FileCopyOrMoveDto input)
        {
            string fileSystemPath = GetFileSystemPath(input.Path);
            fileSystemPath = Path.Combine(fileSystemPath, input.Name);
            if (!File.Exists(fileSystemPath))
            {
                throw new UserFriendlyException(L["FilePathNotFound"]);
            }
            var moveToFilePath = GetFileSystemPath(input.ToPath);
            moveToFilePath = Path.Combine(moveToFilePath, input.ToName ?? input.Name);
            if (File.Exists(moveToFilePath))
            {
                throw new UserFriendlyException(L["FilePathAlreadyExists"]);
            }

            File.Move(fileSystemPath, moveToFilePath);

            return Task.CompletedTask;
        }

        [Authorize(AbpFileManagementPermissions.FileSystem.Move)]
        public virtual Task MoveFolderAsync([Required, StringLength(255)] string path, FolderMoveDto input)
        {
            string fileSystemPath = GetFileSystemPath(path);
            if (!Directory.Exists(fileSystemPath))
            {
                throw new UserFriendlyException(L["FilePathNotFound"]);
            }
            var moveToFilePath = GetFileSystemPath(input.MoveToPath);
            if (Directory.Exists(moveToFilePath))
            {
                throw new UserFriendlyException(L["FilePathAlreadyExists"]);
            }

            Directory.Move(fileSystemPath, moveToFilePath);

            return Task.CompletedTask;
        }

        [Authorize(AbpFileManagementPermissions.FileSystem.Update)]
        public virtual Task<FileSystemDto> UpdateAsync([Required, StringLength(255)] string name, FileSystemUpdateDto input)
        {
            string fileSystemPath = GetFileSystemPath(name);
            var renameFilePath = GetFileSystemPath(input.NewName);
            if (File.Exists(fileSystemPath))
            {
                if (File.Exists(renameFilePath))
                {
                    throw new UserFriendlyException(L["FilePathAlreadyExists"]);
                }
                File.Move(fileSystemPath, renameFilePath);

                var fileInfo = new FileInfo(renameFilePath);
                var fileSystem = new FileSystemDto
                {
                    Type = FileSystemType.File,
                    Name = fileInfo.Name,
                    Size = fileInfo.Length,
                    Extension = fileInfo.Extension,
                    CreationTime = fileInfo.CreationTime,
                    LastModificationTime = fileInfo.LastWriteTime
                };
                if (fileInfo.Directory != null && !fileInfo.Directory.FullName.IsNullOrWhiteSpace())
                {
                    fileSystem.Parent = GetFileSystemRelativePath(fileInfo.Directory.FullName);
                }
                return Task.FromResult(fileSystem);
            }
            if (Directory.Exists(fileSystemPath))
            {
                if (Directory.Exists(renameFilePath))
                {
                    throw new UserFriendlyException(L["FilePathAlreadyExists"]);
                }

                Directory.Move(fileSystemPath, renameFilePath);

                var directoryInfo = new DirectoryInfo(renameFilePath);
                var fileSystem = new FileSystemDto
                {
                    Type = FileSystemType.Folder,
                    Name = directoryInfo.Name,
                    CreationTime = directoryInfo.CreationTime,
                    LastModificationTime = directoryInfo.LastWriteTime
                };
                if (directoryInfo.Parent != null && !directoryInfo.Parent.FullName.IsNullOrWhiteSpace())
                {
                    fileSystem.Parent = GetFileSystemRelativePath(directoryInfo.Parent.FullName);
                }
                return Task.FromResult(fileSystem);
            }
            throw new UserFriendlyException(L["FilePathNotFound"]);
        }
        /// <summary>
        /// 获取文件系统相对路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected virtual string GetFileSystemRelativePath(string path)
        {
            // 去除完整路径中的文件系统根目录
            var fileSystemConfiguration = GetFileSystemBlobProviderConfiguration();
            var blobPath = fileSystemConfiguration.BasePath;
            // 去除租户或宿主目录
            if (CurrentTenant.Id == null)
            {
                blobPath = Path.Combine(blobPath, "host");
            }
            else
            {
                blobPath = Path.Combine(blobPath, "tenants", CurrentTenant.Id.Value.ToString("D"));
            }
            // 去除完整路径中的容器根目录
            var containerName = BlobContainerNameAttribute.GetContainerName<FileSystemContainer>();
            if (path.Contains(containerName))
            {
                blobPath = Path.Combine(blobPath, containerName);
            }
            path = path.Replace(blobPath, "");
            path = path.Replace('/', Path.DirectorySeparatorChar);
            path = path.Replace('\\', Path.DirectorySeparatorChar);
            return path;
        }
        /// <summary>
        /// 获取合并的文件路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected virtual string GetFileSystemPath(string path)
        {
            var fileSystemConfiguration = GetFileSystemBlobProviderConfiguration();
            var blobPath = GetFileSystemBashPath();

            if (!path.IsNullOrWhiteSpace() && fileSystemConfiguration.AppendContainerNameToBasePath)
            {
                path = path.Replace('/', Path.DirectorySeparatorChar);
                path = path.Replace('\\', Path.DirectorySeparatorChar);
                // 去除第一个路径标识符
                path = path.RemovePreFix("/", "\\");
                blobPath = Path.Combine(blobPath, path);
            }

            return blobPath;
        }
        /// <summary>
        /// 获取文件系统存储路径
        /// </summary>
        /// <returns></returns>
        protected virtual string GetFileSystemBashPath()
        {
            var fileSystemConfiguration = GetFileSystemBlobProviderConfiguration();
            var blobPath = fileSystemConfiguration.BasePath;
            blobPath = Path.Combine(Directory.GetCurrentDirectory(), blobPath);
            if (CurrentTenant.Id == null)
            {
                blobPath = Path.Combine(blobPath, "host");
            }
            else
            {
                blobPath = Path.Combine(blobPath, "tenants", CurrentTenant.Id.Value.ToString("D"));
            }
            var containerName = BlobContainerNameAttribute.GetContainerName<FileSystemContainer>();

            blobPath = Path.Combine(blobPath, containerName);

            if (!Directory.Exists(blobPath))
            {
                Directory.CreateDirectory(blobPath);
            }

            return blobPath;
        }

        protected virtual FileSystemBlobProviderConfiguration GetFileSystemBlobProviderConfiguration()
        {
            var blobConfiguration = BlobContainerConfigurationProvider
                .Get<FileSystemContainer>();
            return blobConfiguration.GetFileSystemConfiguration();
        }

        protected void CopyDirectory(string sourcePath, string copyToPath)
        {
            var sourceDirectory = new DirectoryInfo(sourcePath);
            var fileSystemInfos = sourceDirectory.GetFileSystemInfos();

            foreach (var fileSystemInfo in fileSystemInfos)
            {
                var copyToFilePath = Path.Combine(copyToPath, fileSystemInfo.Name);
                if (fileSystemInfo is DirectoryInfo)
                {
                    if (!Directory.Exists(copyToFilePath))
                    {
                        Directory.CreateDirectory(copyToFilePath);
                    }
                    CopyDirectory(fileSystemInfo.FullName, copyToFilePath);
                }
                else
                {
                    File.Copy(fileSystemInfo.FullName, copyToFilePath, true);
                }
            }

        }
    }
}
