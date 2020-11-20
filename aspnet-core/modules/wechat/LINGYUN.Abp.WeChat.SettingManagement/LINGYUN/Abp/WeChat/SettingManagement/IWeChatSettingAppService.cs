using LINGYUN.Abp.SettingManagement;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace LINGYUN.Abp.WeChat.SettingManagement
{
    public interface IWeChatSettingAppService
    {
        Task<ListResultDto<SettingGroupDto>> GetAllForGlobalAsync();

        Task<ListResultDto<SettingGroupDto>> GetAllForCurrentTenantAsync();
    }
}
