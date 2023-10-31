using LINGYUN.Abp.SettingManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace LY.MicroService.Applications.Single.Controllers;

[ExposeServices(
    typeof(UserSettingController),
    typeof(UserSettingMergeController))]
public class UserSettingMergeController : UserSettingController
{
    private readonly SettingManagementMergeOptions _mergeOptions;
    public UserSettingMergeController(
        IUserSettingAppService service,
        IOptions<SettingManagementMergeOptions> mergeOptions)
        : base(service)
    {
        _mergeOptions = mergeOptions.Value;
    }

    [HttpGet]
    [Route("by-current-user")]
    public async override Task<SettingGroupResult> GetAllForCurrentUserAsync()
    {
        var result = new SettingGroupResult();
        var markTypeMap = new List<Type>
        {
            typeof(UserSettingMergeController),
        };
        foreach (var serviceType in _mergeOptions.UserSettingProviders
            .Where(type => !markTypeMap.Any(markType => type.IsAssignableFrom(markType))))
        {
            var settingService = LazyServiceProvider.LazyGetRequiredService(serviceType).As<IUserSettingAppService>();
            var currentResult = await settingService.GetAllForCurrentUserAsync();
            foreach (var group in currentResult.Items)
            {
                result.AddGroup(group);
            }
            markTypeMap.Add(serviceType);
        }

        return result;
    }
}
