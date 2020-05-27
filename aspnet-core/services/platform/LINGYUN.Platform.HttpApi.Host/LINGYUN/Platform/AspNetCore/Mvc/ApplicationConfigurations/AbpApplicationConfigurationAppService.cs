using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ObjectExtending;
using Volo.Abp.AspNetCore.Mvc.MultiTenancy;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Clients;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Settings;
using Volo.Abp.Users;

namespace LINGYUN.Platform.AspNetCore.Mvc.ApplicationConfigurations
{
    [Dependency(ServiceLifetime.Transient)]
    [ExposeServices(typeof(IAbpApplicationConfigurationAppService))]
    public class AbpApplicationConfigurationAppService : ApplicationService, IAbpApplicationConfigurationAppService
    {
        private readonly AbpLocalizationOptions _localizationOptions;
        private readonly AbpMultiTenancyOptions _multiTenancyOptions;
        private readonly IServiceProvider _serviceProvider;
        private readonly ISettingProvider _settingProvider;
        private readonly ISettingDefinitionManager _settingDefinitionManager;
        private readonly IFeatureDefinitionManager _featureDefinitionManager;
        private readonly IPermissionGrantRepository _permissionGrantRepository;
        private readonly IPermissionDefinitionManager _permissionDefinitionManager;
        private readonly ILanguageProvider _languageProvider;
        private readonly ICachedObjectExtensionsDtoService _cachedObjectExtensionsDtoService;

        private ICurrentClient _currentClient;

        protected ICurrentClient CurrentClient => LazyGetRequiredService(ref _currentClient);

        private ILocalEventBus _localEventBus;
        //用于发布权限事件,每次请求此接口后,通过事件总线来缓存权限
        protected ILocalEventBus LocalEventBus => LazyGetRequiredService(ref _localEventBus);

        public AbpApplicationConfigurationAppService(
            IOptions<AbpLocalizationOptions> localizationOptions,
            IOptions<AbpMultiTenancyOptions> multiTenancyOptions,
            IServiceProvider serviceProvider,
            ISettingProvider settingProvider,
            ISettingDefinitionManager settingDefinitionManager,
            IFeatureDefinitionManager featureDefinitionManager,
            IPermissionGrantRepository permissionGrantRepository,
            IPermissionDefinitionManager permissionDefinitionManager,
            ILanguageProvider languageProvider,
            ICachedObjectExtensionsDtoService cachedObjectExtensionsDtoService)
        {
            _serviceProvider = serviceProvider;
            _settingProvider = settingProvider;
            _settingDefinitionManager = settingDefinitionManager;
            _featureDefinitionManager = featureDefinitionManager;
            _permissionGrantRepository = permissionGrantRepository;
            _permissionDefinitionManager = permissionDefinitionManager;
            _languageProvider = languageProvider;
            _cachedObjectExtensionsDtoService = cachedObjectExtensionsDtoService;
            _localizationOptions = localizationOptions.Value;
            _multiTenancyOptions = multiTenancyOptions.Value;
        }

        public virtual async Task<ApplicationConfigurationDto> GetAsync()
        {
            //TODO: Optimize & cache..?

            Logger.LogDebug("Executing AbpApplicationConfigurationAppService.GetAsync()...");

            var result = new ApplicationConfigurationDto
            {
                Auth = await GetAuthConfigAsync(),
                Features = await GetFeaturesConfigAsync(),
                Localization = await GetLocalizationConfigAsync(),
                CurrentUser = GetCurrentUser(),
                Setting = await GetSettingConfigAsync(),
                MultiTenancy = GetMultiTenancy(),
                CurrentTenant = GetCurrentTenant(),
                ObjectExtensions = _cachedObjectExtensionsDtoService.Get()
            };

            Logger.LogDebug("Executed AbpApplicationConfigurationAppService.GetAsync().");

            return result;
        }

        protected virtual CurrentTenantDto GetCurrentTenant()
        {
            return new CurrentTenantDto()
            {
                Id = CurrentTenant.Id,
                Name = CurrentTenant.Name,
                IsAvailable = CurrentTenant.IsAvailable
            };
        }

        protected virtual MultiTenancyInfoDto GetMultiTenancy()
        {
            return new MultiTenancyInfoDto
            {
                IsEnabled = _multiTenancyOptions.IsEnabled
            };
        }

        protected virtual CurrentUserDto GetCurrentUser()
        {
            return new CurrentUserDto
            {
                IsAuthenticated = CurrentUser.IsAuthenticated,
                Id = CurrentUser.Id,
                TenantId = CurrentUser.TenantId,
                UserName = CurrentUser.UserName,
                Email = CurrentUser.Email
            };
        }

        protected virtual async Task<ApplicationAuthConfigurationDto> GetAuthConfigAsync()
        {
            var authConfig = new ApplicationAuthConfigurationDto();

            var permissions = _permissionDefinitionManager.GetPermissions();

            IEnumerable<PermissionGrant> grantPermissions = new List<PermissionGrant>();

            // TODO: 重写为每次调用接口都在数据库统一查询权限
            // 待框架改进权限Provider机制后再移除

            // 如果用户已登录，获取用户和角色权限
            if (CurrentUser.IsAuthenticated)
            {
                var userPermissions = await _permissionGrantRepository.GetListAsync(UserPermissionValueProvider.ProviderName,
                    CurrentUser.GetId().ToString());
                grantPermissions = grantPermissions.Union(userPermissions);
                foreach(var userRole in CurrentUser.Roles)
                {
                    var rolePermissions = await _permissionGrantRepository.GetListAsync(RolePermissionValueProvider.ProviderName,
                        userRole);
                    grantPermissions = grantPermissions.Union(rolePermissions);
                }
            }

            // 如果客户端已验证，获取客户端权限
            if(CurrentClient.IsAuthenticated)
            {
                var clientPermissions = await _permissionGrantRepository.GetListAsync(ClientPermissionValueProvider.ProviderName,
                    CurrentClient.Id);
                grantPermissions = grantPermissions.Union(clientPermissions);
            }

            foreach(var permission in permissions)
            {
                authConfig.Policies[permission.Name] = true;
                if(grantPermissions.Any(p => p.Name.Equals(permission.Name)))
                {
                    authConfig.GrantedPolicies[permission.Name] = true;
                }
            }

            return authConfig;
        }

        protected virtual async Task<ApplicationLocalizationConfigurationDto> GetLocalizationConfigAsync()
        {
            var localizationConfig = new ApplicationLocalizationConfigurationDto();

            localizationConfig.Languages.AddRange(await _languageProvider.GetLanguagesAsync());

            foreach (var resource in _localizationOptions.Resources.Values)
            {
                var dictionary = new Dictionary<string, string>();

                var localizer = _serviceProvider.GetRequiredService(
                    typeof(IStringLocalizer<>).MakeGenericType(resource.ResourceType)
                ) as IStringLocalizer;

                foreach (var localizedString in localizer.GetAllStrings())
                {
                    dictionary[localizedString.Name] = localizedString.Value;
                }

                localizationConfig.Values[resource.ResourceName] = dictionary;
            }

            localizationConfig.CurrentCulture = GetCurrentCultureInfo();

            if (_localizationOptions.DefaultResourceType != null)
            {
                localizationConfig.DefaultResourceName = LocalizationResourceNameAttribute.GetName(
                    _localizationOptions.DefaultResourceType
                );
            }

            return localizationConfig;
        }

        private static CurrentCultureDto GetCurrentCultureInfo()
        {
            return new CurrentCultureDto
            {
                Name = CultureInfo.CurrentUICulture.Name,
                DisplayName = CultureInfo.CurrentUICulture.DisplayName,
                EnglishName = CultureInfo.CurrentUICulture.EnglishName,
                NativeName = CultureInfo.CurrentUICulture.NativeName,
                IsRightToLeft = CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft,
                CultureName = CultureInfo.CurrentUICulture.TextInfo.CultureName,
                TwoLetterIsoLanguageName = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName,
                ThreeLetterIsoLanguageName = CultureInfo.CurrentUICulture.ThreeLetterISOLanguageName,
                DateTimeFormat = new DateTimeFormatDto
                {
                    CalendarAlgorithmType = CultureInfo.CurrentUICulture.DateTimeFormat.Calendar.AlgorithmType.ToString(),
                    DateTimeFormatLong = CultureInfo.CurrentUICulture.DateTimeFormat.LongDatePattern,
                    ShortDatePattern = CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern,
                    FullDateTimePattern = CultureInfo.CurrentUICulture.DateTimeFormat.FullDateTimePattern,
                    DateSeparator = CultureInfo.CurrentUICulture.DateTimeFormat.DateSeparator,
                    ShortTimePattern = CultureInfo.CurrentUICulture.DateTimeFormat.ShortTimePattern,
                    LongTimePattern = CultureInfo.CurrentUICulture.DateTimeFormat.LongTimePattern,
                }
            };
        }

        private async Task<ApplicationSettingConfigurationDto> GetSettingConfigAsync()
        {
            var result = new ApplicationSettingConfigurationDto
            {
                Values = new Dictionary<string, string>()
            };

            foreach (var settingDefinition in _settingDefinitionManager.GetAll())
            {
                if (!settingDefinition.IsVisibleToClients)
                {
                    continue;
                }

                result.Values[settingDefinition.Name] = await _settingProvider.GetOrNullAsync(settingDefinition.Name);
            }

            return result;
        }

        protected virtual async Task<ApplicationFeatureConfigurationDto> GetFeaturesConfigAsync()
        {
            var result = new ApplicationFeatureConfigurationDto();

            foreach (var featureDefinition in _featureDefinitionManager.GetAll())
            {
                if (!featureDefinition.IsVisibleToClients)
                {
                    continue;
                }

                result.Values[featureDefinition.Name] = await FeatureChecker.GetOrNullAsync(featureDefinition.Name);
            }

            return result;
        }
    }
}
