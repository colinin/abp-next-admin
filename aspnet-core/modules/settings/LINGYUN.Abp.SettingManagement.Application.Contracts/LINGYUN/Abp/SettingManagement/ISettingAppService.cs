using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.SettingManagement
{
    public interface ISettingAppService : IApplicationService
    {
        Task SetGlobalAsync(UpdateSettingsDto input);

        Task SetCurrentTenantAsync(UpdateSettingsDto input);

        Task<ListResultDto<SettingGroupDto>> GetAllForGlobalAsync();

        Task<ListResultDto<SettingGroupDto>> GetAllForCurrentTenantAsync();
    }
}
