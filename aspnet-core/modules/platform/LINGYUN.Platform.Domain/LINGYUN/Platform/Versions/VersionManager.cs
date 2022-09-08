using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.BlobStoring;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;

namespace LINGYUN.Platform.Versions
{
    [Dependency(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient)]
    [ExposeServices(typeof(IVersionFileManager), typeof(VersionManager))]
    public class VersionManager : DomainService, IVersionFileManager
    {
        protected IVersionRepository VersionRepository { get; }
        protected IBlobContainer<VersionContainer> VersionBlobContainer { get; }

        public VersionManager(
            IBlobContainer<VersionContainer> container,
            IVersionRepository versionRepository)
        {
            VersionBlobContainer = container;
            VersionRepository = versionRepository;
        }

        public async virtual Task<bool> ExistsAsync(PlatformType platformType, string version)
        {
            return await VersionRepository.ExistsAsync(platformType, version);
        }

        public async virtual Task<AppVersion> GetByIdAsync(Guid id)
        {
            return await VersionRepository.GetAsync(id);
        }

        public async virtual Task<AppVersion> GetByVersionAsync(PlatformType platformType, string version)
        {
            return await VersionRepository.GetByVersionAsync(platformType, version);
        }

        public async virtual Task<long> GetCountAsync(PlatformType platformType, string filter)
        {
            return await VersionRepository.GetCountAsync(platformType, filter);
        }

        public async virtual Task<List<AppVersion>> GetPagedListAsync(PlatformType platformType, string filter = "", string soring = nameof(AppVersion.CreationTime), bool includeDetails = true, int skipCount = 1, int maxResultCount = 10)
        {
            return await VersionRepository.GetPagedListAsync(platformType, filter, soring, includeDetails, skipCount, maxResultCount);
        }

        [UnitOfWork]
        public async virtual Task CreateAsync(AppVersion version)
        {
            await VersionRepository.InsertAsync(version);
        }


        [UnitOfWork]
        public async virtual Task UpdateAsync(AppVersion version)
        {
            await VersionRepository.UpdateAsync(version);
        }

        [UnitOfWork]
        public async virtual Task DeleteAsync(Guid id)
        {
            await RemoveAllFileAsync(id);
            await VersionRepository.DeleteAsync(id);
        }

        public async virtual Task<AppVersion> GetLatestAsync(PlatformType platformType)
        {
            return await VersionRepository.GetLatestVersionAsync(platformType);
        }

        public async virtual Task<Stream> DownloadFileAsync(PlatformType platformType, string version, string filePath, string fileName, string fileVersion)
        {
            var appVersion = await GetByVersionAsync(platformType, version);
            var versionFile = appVersion.FindFile(filePath, fileName, fileVersion);
            if (versionFile == null)
            {
                throw new BusinessException(PlatformErrorCodes.VersionFileNotFound)
                    .WithData("FileName", fileName)
                    .WithData("FileVersion", fileVersion);
            }
            versionFile.Download();
            return await VersionBlobContainer.GetAsync(
                VersionFile.NormalizeBlobName(version, versionFile.Name, versionFile.Version, versionFile.Path));
        }

        public async virtual Task<Stream> GetFileAsync(VersionFile versionFile)
        {
            versionFile.Download();
            return await VersionBlobContainer.GetAsync(
                VersionFile.NormalizeBlobName(versionFile.AppVersion.Version, versionFile.Name, versionFile.Version, versionFile.Path));
        }

        public async virtual Task<string> SaveFileAsync(string version, string filePath, string fileName, string fileVersion, byte[] data)
        {
            // 计算指纹
            var sha256 = SHA256.Create();
            var checkHash = sha256.ComputeHash(data);
            var sha256Hash = BitConverter.ToString(checkHash).Replace("-", string.Empty);

            await VersionBlobContainer
                .SaveAsync(VersionFile.NormalizeBlobName(version, fileName, fileVersion, filePath), data, true);

            return sha256Hash;
        }

        [UnitOfWork]
        public async virtual Task AppendFileAsync(Guid versionId, string fileSha256,
            string fileName, string fileVersion,  
            long fileSize, string filePath = "", 
            FileType fileType = FileType.Stream)
        {
            var appVersion = await VersionRepository.GetAsync(versionId);
            if (appVersion.FileExists(fileName))
            {
                appVersion.RemoveFile(fileName);
            }
            appVersion.AppendFile(fileName, fileVersion, fileSize, fileSha256, filePath, fileType);
        }

        [UnitOfWork]
        public async virtual Task RemoveFileAsync(Guid versionId, string fileName)
        {
            var appVersion = await VersionRepository.GetAsync(versionId);
            var versionFile = appVersion.FindFile(fileName);
            if (versionFile != null)
            {
                await VersionBlobContainer
                    .DeleteAsync(VersionFile.NormalizeBlobName(appVersion.Version, versionFile.Name, versionFile.Version));
                appVersion.RemoveFile(fileName);
            }
        }

        [UnitOfWork]
        public async virtual Task RemoveAllFileAsync(Guid versionId)
        {
            var appVersion = await VersionRepository.GetAsync(versionId);
            foreach (var versionFile in appVersion.Files)
            {
                await VersionBlobContainer
                    .DeleteAsync(VersionFile.NormalizeBlobName(appVersion.Version, versionFile.Name, versionFile.Version));
            }
            appVersion.RemoveAllFile();
        }
    }
}
