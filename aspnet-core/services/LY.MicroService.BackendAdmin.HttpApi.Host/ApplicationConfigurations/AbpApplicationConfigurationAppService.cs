using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RequestLocalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ObjectExtending;
using Volo.Abp.Authorization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace LY.MicroService.BackendAdmin.ApplicationConfigurations;

[Dependency(ReplaceServices = true)]
public class AbpApplicationConfigurationAppService : Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.AbpApplicationConfigurationAppService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly AbpApplicationConfigurationOptions _options;
    private readonly ICachedObjectExtensionsDtoService _cachedObjectExtensionsDtoService;

    private readonly ISettingProvider _settingProvider;
    private readonly ISettingDefinitionManager _settingDefinitionManager;

    public AbpApplicationConfigurationAppService(
        IOptions<AbpLocalizationOptions> localizationOptions,
        IOptions<AbpRequestLocalizationOptions> requestLocalizationOptions,
        IOptions<AbpMultiTenancyOptions> multiTenancyOptions,
        IServiceProvider serviceProvider,
        IAbpAuthorizationPolicyProvider abpAuthorizationPolicyProvider,
        IPermissionDefinitionManager permissionDefinitionManager,
        DefaultAuthorizationPolicyProvider defaultAuthorizationPolicyProvider,
        IPermissionChecker permissionChecker,
        IAuthorizationService authorizationService,
        ICurrentUser currentUser,
        ISettingProvider settingProvider,
        ISettingDefinitionManager settingDefinitionManager,
        IFeatureDefinitionManager featureDefinitionManager,
        ILanguageProvider languageProvider,
        ITimezoneProvider timezoneProvider,
        IOptions<AbpClockOptions> abpClockOptions,
        ICachedObjectExtensionsDtoService cachedObjectExtensionsDtoService,
        IOptions<AbpApplicationConfigurationOptions> options) 
        : base(
            localizationOptions,
            requestLocalizationOptions,
            multiTenancyOptions,
            serviceProvider,
            abpAuthorizationPolicyProvider,
            permissionDefinitionManager,
            defaultAuthorizationPolicyProvider,
            permissionChecker,
            authorizationService,
            currentUser,
            settingProvider,
            settingDefinitionManager,
            featureDefinitionManager,
            languageProvider,
            timezoneProvider,
            abpClockOptions,
            cachedObjectExtensionsDtoService,
            options)
    {
        _options = options.Value;
        _serviceProvider = serviceProvider;
        _cachedObjectExtensionsDtoService = cachedObjectExtensionsDtoService;

        _settingProvider = settingProvider;
        _settingDefinitionManager = settingDefinitionManager;
    }

    public override async Task<ApplicationConfigurationDto> GetAsync(ApplicationConfigurationRequestOptions options)
    {
        //TODO: Optimize & cache..?

        Logger.LogDebug("Executing AbpApplicationConfigurationAppService.GetAsync()...");

        var result = new ApplicationConfigurationDto
        {
            Auth = await GetAuthConfigAsync(),
            Features = await GetFeaturesConfigAsync(),
            GlobalFeatures = await GetGlobalFeaturesConfigAsync(),
            Localization = await GetLocalizationConfigAsync(options),
            CurrentUser = GetCurrentUser(),
            Setting = await GetSettingConfigAsync(),
            MultiTenancy = GetMultiTenancy(),
            CurrentTenant = GetCurrentTenant(),
            Timing = await GetTimingConfigAsync(),
            Clock = GetClockConfig(),
            ObjectExtensions = _cachedObjectExtensionsDtoService.Get(),
            ExtraProperties = new ExtraPropertyDictionary()
        };

        if (_options.Contributors.Any())
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = new ApplicationConfigurationContributorContext(scope.ServiceProvider, result);
                foreach (var contributor in _options.Contributors)
                {
                    await contributor.ContributeAsync(context);
                }
            }
        }

        Logger.LogDebug("Executed AbpApplicationConfigurationAppService.GetAsync().");

        return result;
    }

    protected async virtual Task<ApplicationSettingConfigurationDto> GetSettingConfigAsync()
    {
        var result = new ApplicationSettingConfigurationDto
        {
            Values = new Dictionary<string, string>()
        };

        var settingDefinitions = (await _settingDefinitionManager.GetAllAsync()).Where(x => x.IsVisibleToClients);
        var settingNames = settingDefinitions.Select(x => x.Name).ToArray();
        if (settingNames.Length > 0)
        {
            var settingValues = await _settingProvider.GetAllAsync(settingNames);

            foreach (var settingValue in settingValues)
            {
                result.Values[settingValue.Name] = settingValue.Value;
            }
        }

        return result;
    }
}
