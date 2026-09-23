using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.PermissionManagement;
using VoloPermissionsController = Volo.Abp.PermissionManagement.PermissionsController;

namespace LINGYUN.Abp.PermissionManagement.HttpApi;

[Dependency(ReplaceServices = true)]
[ExposeServices(
    typeof(VoloPermissionsController),
    IncludeSelf = true)]

[RemoteService(Name = PermissionManagementRemoteServiceConsts.RemoteServiceName)]
[Area(PermissionManagementRemoteServiceConsts.ModuleName)]
[Route("api/permission-management/permissions")]
public class PermissionsController : VoloPermissionsController, IPermissionAppService
{
    private readonly IPermissionAppService _permissionAppService;
    public PermissionsController(IPermissionAppService permissionAppService) : base(permissionAppService)
    {
        _permissionAppService = permissionAppService;
    }

    [HttpGet]
    [Route("granted/by-provider")]
    public virtual Task<GetPermissionGrantedWithProviderListResultDto> GetGrantedByProviderAsync([NotNull] string permissionName, [NotNull] string providerName)
    {
        return _permissionAppService.GetGrantedByProviderAsync(permissionName, providerName);
    }
}
