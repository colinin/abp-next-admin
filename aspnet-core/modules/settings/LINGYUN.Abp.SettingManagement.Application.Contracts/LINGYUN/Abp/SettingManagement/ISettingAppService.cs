using JetBrains.Annotations;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.SettingManagement
{
    public interface ISettingAppService : IApplicationService
    {
        Task<ListResultDto<SettingDto>> GetAsync([NotNull] string providerName, [NotNull] string providerKey);

        Task UpdateAsync([NotNull] string providerName, [NotNull] string providerKey, UpdateSettingsDto input);
    }
}
