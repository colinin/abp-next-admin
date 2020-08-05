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

        Task<ListResultDto<SettingDto>> GetAllForTenantAsync();

        Task<ListResultDto<SettingDto>> GetAllForUserAsync([Required] Guid userId);

        Task<ListResultDto<SettingDto>> GetAllForCurrentUserAsync();

        [Obsolete("The best way to do this is to separate the individual configurations")]
        Task<ListResultDto<SettingDto>> GetAsync([NotNull] string providerName, [NotNull] string providerKey);

        Task UpdateAsync([NotNull] string providerName, [NotNull] string providerKey, UpdateSettingsDto input);
    }
}
