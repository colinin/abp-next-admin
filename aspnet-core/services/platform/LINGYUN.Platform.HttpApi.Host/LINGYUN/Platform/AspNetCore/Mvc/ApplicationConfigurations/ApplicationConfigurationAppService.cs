using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ObjectExtending;
using Volo.Abp.Authorization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Clients;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Settings;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace LINGYUN.Platform.AspNetCore.Mvc.ApplicationConfigurations
{
    [Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
    [ExposeServices(typeof(IAbpApplicationConfigurationAppService), typeof(AbpApplicationConfigurationAppService))]
    public class ApplicationConfigurationAppService : AbpApplicationConfigurationAppService
    {
        private readonly IPermissionGrantRepository _permissionGrantRepository;
        private readonly IPermissionDefinitionManager _permissionDefinitionManager;

        private ICurrentClient _currentClient;

        protected ICurrentClient CurrentClient => LazyGetRequiredService(ref _currentClient);

        public ApplicationConfigurationAppService(
            IOptions<AbpLocalizationOptions> localizationOptions, 
            IOptions<AbpMultiTenancyOptions> multiTenancyOptions, 
            IServiceProvider serviceProvider, 
            IAbpAuthorizationPolicyProvider abpAuthorizationPolicyProvider, 
            IAuthorizationService authorizationService, 
            ICurrentUser currentUser, 
            ISettingProvider settingProvider, 
            ISettingDefinitionManager settingDefinitionManager, 
            IFeatureDefinitionManager featureDefinitionManager, 
            ILanguageProvider languageProvider, 
            ITimezoneProvider timezoneProvider, 
            IOptions<AbpClockOptions> abpClockOptions, 
            ICachedObjectExtensionsDtoService cachedObjectExtensionsDtoService,
            IPermissionGrantRepository permissionGrantRepository,
            IPermissionDefinitionManager permissionDefinitionManager) 
            : base(
                  localizationOptions, 
                  multiTenancyOptions, 
                  serviceProvider, 
                  abpAuthorizationPolicyProvider, 
                  authorizationService, 
                  currentUser, 
                  settingProvider, 
                  settingDefinitionManager, 
                  featureDefinitionManager, 
                  languageProvider, 
                  timezoneProvider, 
                  abpClockOptions, 
                  cachedObjectExtensionsDtoService)
        {
            _permissionGrantRepository = permissionGrantRepository;
            _permissionDefinitionManager = permissionDefinitionManager;

        }
        protected override async Task<ApplicationAuthConfigurationDto> GetAuthConfigAsync()
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
    }
}
