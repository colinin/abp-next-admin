using JetBrains.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.SettingManagement
{
    public interface ISettingAppService : IApplicationService
    {
        Task<ListResultDto<SettingDto>> GetAllGlobalAsync();

        Task SetGlobalAsync(UpdateSettingsDto input);

        Task<ListResultDto<SettingDto>> GetAllForCurrentTenantAsync();

        Task SetCurrentTenantAsync(UpdateSettingsDto input);

        Task<ListResultDto<SettingDto>> GetAllForUserAsync([Required] Guid userId);

        Task SetForUserAsync([Required] Guid userId, UpdateSettingsDto input);

        Task<ListResultDto<SettingDto>> GetAllForCurrentUserAsync();

        Task SetCurrentUserAsync(UpdateSettingsDto input);
    }
}
