using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Platform.Versions
{
    public interface IVersionAppService : IApplicationService
    {
        Task<VersionDto> GetLastestAsync(PlatformType platformType);

        Task<PagedResultDto<VersionDto>> GetAsync(VersionGetByPagedDto versionGetByPaged);

        Task<VersionDto> GetAsync(VersionGetByIdDto versionGetById);

        Task<VersionDto> CreateAsync(VersionCreateDto versionCreate);

        Task DeleteAsync(VersionDeleteDto versionDelete);

        Task AppendFileAsync(VersionFileCreateDto versionFileCreate);

        Task RemoveFileAsync(VersionFileDeleteDto versionFileDelete);

        Task RemoveAllFileAsync(VersionGetByIdDto versionGetById);

        Task DownloadFileAsync(VersionFileGetDto versionFileGet);
    }
}
