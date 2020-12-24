using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace LINGYUN.Abp.PermissionManagement
{
    // fix: https://github.com/abpframework/abp/issues/6022
    // TODO: 在4.0正式版中已修复此问题,升级后需要移除
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(ClientPermissionValueProvider))]
    public class FixClientPermissionValueProvider : ClientPermissionValueProvider
    {
        protected ICurrentTenant CurrentTenant { get; }
        public FixClientPermissionValueProvider(
            IPermissionStore permissionStore,
            ICurrentTenant currentTenant) : base(permissionStore, currentTenant)
        {
            CurrentTenant = currentTenant;
        }

        public override async Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context)
        {
            using (CurrentTenant.Change(null))
            {
                return await base.CheckAsync(context);
            }
        }
    }
}
