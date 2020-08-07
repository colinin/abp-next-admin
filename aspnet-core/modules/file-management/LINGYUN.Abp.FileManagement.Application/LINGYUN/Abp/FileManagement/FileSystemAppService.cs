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

        public virtual Task CopyFileAsync(FileCopyOrMoveDto input)
        {
            string fileSystemPath = GetFileSystemPath(input.Path);
            var fileFullName = Path.Combine(fileSystemPath, input.Name);
            if (!File.Exists(fileFullName))
            {
                throw new UserFriendlyException("指定的文件不存在!");
            }
            var copyToFilePath = GetFileSystemPath(input.ToPath);
            var copyToFileFullName = Path.Combine(copyToFilePath, input.ToName ?? input.Name);
            if (File.Exists(copyToFileFullName))
            {
                throw new UserFriendlyException("指定的路径中已经有相同的文件名存在!");
            }

            File.Copy(fileFullName, copyToFileFullName);

            return Task.CompletedTask;
        }

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

        public virtual async Task CreateFileAsync(FileCreateDto input)
        {
            string fileSystemPath = GetFileSystemPath(input.Path);
            var fileFullName = Path.Combine(fileSystemPath, input.Name);
            if (File.Exists(fileFullName) && !input.Rewrite)
            {
                throw new UserFriendlyException("指定的文件已经存在!");
            }
            await BlobContainer.SaveAsync(input.Name, input.Data, input.Rewrite);
        }

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
                throw new UserFriendlyException("指定目录已经存在!");
            }
            Directory.CreateDirectory(newFloderPath);

            return Task.CompletedTask;
        }

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

        public virtual Task DeleteFolderAsync([Required, StringLength(255)] string path)
        {
            string fileSystemPath = GetFileSystemPath(path);
            if (!Directory.Exists(fileSystemPath))
            {
                throw new UserFriendlyException("指定目录不存在!");
            }
            var fileSystemChildrenPath = Directory.GetDirectories(fileSystemPath);
            if (fileSystemChildrenPath.Length > 0)
            {
                throw new UserFriendlyException("指定的目录不为空,不可删除此目录!");
            }
            var fileSystemPathFiles = Directory.GetFiles(fileSystemPath);
            if (fileSystemPathFiles.Length > 0)
            {
                throw new UserFriendlyException("指定的目录不为空,不可删除此目录!");
            }
            Directory.Delete(fileSystemPath);
            return Task.CompletedTask;
        }

        public virtual async Task<Stream> DownloadFileAsync(FileSystemGetDto input)
        {
            var fileSystemPath = GetFileSystemPath(input.Path);
            fileSystemPath = Path.Combine(fileSystemPath, input.Name);
            var blobName = GetFileSystemRelativePath(fileSystemPath);

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
                    CreationTime = fileInfo.CreationTime,
                    LastModificationTime = fileInfo.LastWriteTime
                };
                if (fileInfo.Directory?.Parent != null && !fileInfo.Directory.Parent.Name.IsNullOrWhiteSpace())
                {
                    fileSystem.Parent = GetFileSystemRelativePath(fileInfo.Directory.Parent.FullName);
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
                if (directoryInfo.Parent != null && !directoryInfo.Parent.Name.IsNullOrWhiteSpace())
                {
                    fileSystem.Parent = GetFileSystemRelativePath(directoryInfo.Parent.FullName);
                }
                return Task.FromResult(fileSystem);
            }
            throw new UserFriendlyException("文件或目录不存在!");
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
            // 查询全部
            var fileSystemInfos = directoryInfo.GetFileSystemInfos();
            // 指定搜索条件查询目录
            FileSystemInfo[] fileSystemInfoSearchChildren;// = directoryInfo.GetDirectories(input.Filter ?? "*", SearchOption.TopDirectoryOnly);
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
                    if (fileInfo.Directory?.Parent != null && !fileInfo.Directory.Parent.Name.IsNullOrWhiteSpace())
                    {
                        fileSystem.Parent = GetFileSystemRelativePath(fileInfo.Directory.Parent.FullName);
                    }
                }
                else if (fileSystemInfo is DirectoryInfo directory)
                {
                    fileSystem.Type = FileSystemType.Folder;
                    if (directory.Parent != null && !directory.Parent.Name.IsNullOrWhiteSpace())
                    {
                        fileSystem.Parent = GetFileSystemRelativePath(directory.Parent.FullName);
                    }
                }
                fileSystems.Add(fileSystem);
            }

            return Task.FromResult(new PagedResultDto<FileSystemDto>(
                fileSystemInfos.Length, fileSystems
            ));
        }

        public virtual Task MoveFileAsync(FileCopyOrMoveDto input)
        {
            string fileSystemPath = GetFileSystemPath(input.Path);
            fileSystemPath = Path.Combine(fileSystemPath, input.Name);
            if (!File.Exists(fileSystemPath))
            {
                throw new UserFriendlyException("指定目录不存在!");
            }
            var moveToFilePath = GetFileSystemPath(input.ToPath);
            moveToFilePath = Path.Combine(moveToFilePath, input.ToName ?? input.Name);
            if (Directory.Exists(moveToFilePath))
            {
                throw new UserFriendlyException("指定的路径中已经有同名的文件存在!");
            }

            File.Move(fileSystemPath, moveToFilePath);

            return Task.CompletedTask;
        }

        public virtual Task MoveFolderAsync([Required, StringLength(255)] string path, FolderMoveDto input)
        {
            string fileSystemPath = GetFileSystemPath(path);
            if (!Directory.Exists(fileSystemPath))
            {
                throw new UserFriendlyException("指定目录不存在!");
            }
            var moveToFilePath = GetFileSystemPath(input.MoveToPath);
            if (Directory.Exists(moveToFilePath))
            {
                throw new UserFriendlyException("指定的路径中已经有同名的目录存在!");
            }

            Directory.Move(fileSystemPath, moveToFilePath);

            return Task.CompletedTask;
        }

        public virtual Task<FileSystemDto> UpdateAsync([Required, StringLength(255)] string name, FileSystemUpdateDto input)
        {
            string fileSystemPath = GetFileSystemPath(name);
            var renameFilePath = GetFileSystemPath(input.NewName);
            if (File.Exists(fileSystemPath))
            {
                if (File.Exists(renameFilePath))
                {
                    throw new UserFriendlyException("指定的文件名已经存在!");
                }
                File.Move(fileSystemPath, renameFilePath);

                var fileInfo = new FileInfo(renameFilePath);
                var fileSystem = new FileSystemDto
                {
                    Type = FileSystemType.File,
                    Name = fileInfo.Name,
                    Size = fileInfo.Length,
                    CreationTime = fileInfo.CreationTime,
                    LastModificationTime = fileInfo.LastWriteTime
                };
                if (fileInfo.Directory?.Parent != null && !fileInfo.Directory.Parent.Name.IsNullOrWhiteSpace())
                {
                    fileSystem.Parent = GetFileSystemRelativePath(fileInfo.Directory.Parent.FullName);
                }
                return Task.FromResult(fileSystem);
            }
            if (Directory.Exists(fileSystemPath))
            {
                if (Directory.Exists(renameFilePath))
                {
                    throw new UserFriendlyException("指定的路径中已经有同名的目录存在!");
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
                if (directoryInfo.Parent != null && !directoryInfo.Parent.Name.IsNullOrWhiteSpace())
                {
                    fileSystem.Parent = GetFileSystemRelativePath(directoryInfo.Parent.FullName);
                }
                return Task.FromResult(fileSystem);
            }
            throw new UserFriendlyException("文件或目录不存在!");
        }

        protected virtual string GetFileSystemRelativePath(string path)
        {
            return path.Replace(Directory.GetCurrentDirectory(), "");
        }

        protected virtual string GetFileSystemPath(string path)
        {
            var fileSystemConfiguration = GetFileSystemBlobProviderConfiguration();
            var blobPath = GetFileSystemBashPath();

            if (fileSystemConfiguration.AppendContainerNameToBasePath)
            {
                blobPath = Path.Combine(blobPath, path);
            }

            return blobPath;
        }

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
