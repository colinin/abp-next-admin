using JetBrains.Annotations;
using System.Threading.Tasks;

namespace LINGYUN.Abp.PermissionManagement;

public interface IPermissionAppService : Volo.Abp.PermissionManagement.IPermissionAppService
{
    Task<GetPermissionGrantedWithProviderListResultDto> GetGrantedByProviderAsync(
        [NotNull] string permissionName, 
        [NotNull] string providerName);
}
