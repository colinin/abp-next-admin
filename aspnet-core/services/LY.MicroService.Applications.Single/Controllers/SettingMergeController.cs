using LINGYUN.Abp.SettingManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace LY.MicroService.Applications.Single.Controllers;

[ExposeServices(
    typeof(SettingController),
    typeof(SettingMergeController))]
public class SettingMergeController : SettingController
{
    private readonly SettingManagementMergeOptions _mergeOptions;
    public SettingMergeController(
        ISettingAppService settingAppService,
        ISettingTestAppService settingTestAppService,
        IOptions<SettingManagementMergeOptions> mergeOptions)
        : base(settingAppService, settingTestAppService)
    {
        _mergeOptions = mergeOptions.Value;
    }

    [HttpGet]
    [Route("by-current-tenant")]
    public async override Task<SettingGroupResult> GetAllForCurrentTenantAsync()
    {
        var result = new SettingGroupResult();
        var markTypeMap = new List<Type>
        {
            typeof(SettingMergeController),
        };
        foreach (var serviceType in _mergeOptions.GlobalSettingProviders
            .Where(type => !markTypeMap.Any(markType => type.IsAssignableFrom(markType))))
        {
            var settingService = LazyServiceProvider.LazyGetRequiredService(serviceType).As<IReadonlySettingAppService>();
            var currentResult = await settingService.GetAllForCurrentTenantAsync();
            foreach (var group in currentResult.Items)
            {
                result.AddGroup(group);
            }
            markTypeMap.Add(serviceType);
        }

        return result;
    }

    [HttpGet]
    [Route("by-global")]
    public async override Task<SettingGroupResult> GetAllForGlobalAsync()
    {
        var result = new SettingGroupResult();
        var markTypeMap = new List<Type>
        {
            typeof(SettingMergeController),
        };
        foreach (var serviceType in _mergeOptions.GlobalSettingProviders
            .Where(type => !markTypeMap.Any(markType => type.IsAssignableFrom(markType))))
        {
            var settingService = LazyServiceProvider.LazyGetRequiredService(serviceType).As<IReadonlySettingAppService>();
            var currentResult = await settingService.GetAllForGlobalAsync();
            foreach (var group in currentResult.Items)
            {
                result.AddGroup(group);
            }
            markTypeMap.Add(serviceType);
        }

        return result;
    }
}
