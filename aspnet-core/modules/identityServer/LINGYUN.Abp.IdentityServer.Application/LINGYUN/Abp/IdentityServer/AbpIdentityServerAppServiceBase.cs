using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.IdentityServer.Localization;

namespace LINGYUN.Abp.IdentityServer
{
    public abstract class AbpIdentityServerAppServiceBase : ApplicationService
    {
        private IPermissionChecker _permissionChecker;
        protected IPermissionChecker PermissionChecker => LazyGetRequiredService(ref _permissionChecker);
        protected AbpIdentityServerAppServiceBase()
        {
            LocalizationResource = typeof(AbpIdentityServerResource);
        }

        protected virtual async Task<bool> IsGrantAsync(string policy)
        {
            return await PermissionChecker.IsGrantedAsync(policy);
        }
    }
}
