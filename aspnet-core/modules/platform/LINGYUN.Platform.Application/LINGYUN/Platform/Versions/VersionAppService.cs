using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Platform.Versions
{
    public class VersionAppService : PlatformApplicationServiceBase, IVersionAppService
    {
        private readonly VersionManager _versionManager;
        public VersionAppService(
            VersionManager versionManager)
        {
            _versionManager = versionManager;
        }

        public virtual async Task AppendFileAsync(VersionFileCreateDto versionFileCreate)
        {
            await _versionManager.AppendFileAsync(versionFileCreate.VersionId,
                versionFileCreate.SHA256, versionFileCreate.FileName, versionFileCreate.FileVersion,
                versionFileCreate.TotalByte, versionFileCreate.FilePath, versionFileCreate.FileType);
        }

        public virtual async Task<VersionDto> CreateAsync(VersionCreateDto versionCreate)
        {
            if (await _versionManager.ExistsAsync(versionCreate.PlatformType,versionCreate.Version))
            {
                throw new UserFriendlyException("VersionAlreadyExists");
            }
            var version = new AppVersion(GuidGenerator.Create(), versionCreate.Title,
                versionCreate.Version, versionCreate.PlatformType, CurrentTenant.Id)
            {
                Description = versionCreate.Description,
                Level = versionCreate.Level
            };

            await _versionManager.CreateAsync(version);

            return ObjectMapper.Map<AppVersion, VersionDto>(version);
        }

        public virtual async Task DeleteAsync(VersionDeleteDto versionDelete)
        {
            var version = await _versionManager.GetByVersionAsync(versionDelete.PlatformType, versionDelete.Version);
            if (version != null)
            {
                await _versionManager.DeleteAsync(version.Id);
            }
        }

        public virtual async Task<PagedResultDto<VersionDto>> GetAsync(VersionGetByPagedDto versionGetByPaged)
        {
            var versionCount = await _versionManager.GetCountAsync(versionGetByPaged.PlatformType, versionGetByPaged.Filter);
            var versions = await _versionManager.GetPagedListAsync(versionGetByPaged.PlatformType, versionGetByPaged.Filter,
                versionGetByPaged.Sorting, true,
                versionGetByPaged.SkipCount, versionGetByPaged.MaxResultCount);

            return new PagedResultDto<VersionDto>(versionCount,
                ObjectMapper.Map<List<AppVersion>, List<VersionDto>>(versions));
        }

        public virtual async Task<VersionDto> GetAsync(VersionGetByIdDto versionGetById)
        {
            var version = await _versionManager.GetByIdAsync(versionGetById.Id);

            return ObjectMapper.Map<AppVersion, VersionDto>(version);
        }

        public virtual async Task<VersionDto> GetLastestAsync(PlatformType platformType)
        {
            var version = await _versionManager.GetLatestAsync(platformType);

            return ObjectMapper.Map<AppVersion, VersionDto>(version);
        }

        public virtual async Task RemoveAllFileAsync(VersionGetByIdDto versionGetById)
        {
            await _versionManager.RemoveAllFileAsync(versionGetById.Id);
        }

        public virtual async Task RemoveFileAsync(VersionFileDeleteDto versionFileDelete)
        {
            await _versionManager.RemoveFileAsync(versionFileDelete.VersionId, versionFileDelete.FileName);
        }
        
        public virtual Task DownloadFileAsync(VersionFileGetDto versionFileGet)
        {
            // TODO: 是否需要定义此接口用于 abp-definition-api ?
            // overrided implement HttpContext.Response.Body.Write(Stream fileStream)
            return Task.CompletedTask;
        }
    }
}
