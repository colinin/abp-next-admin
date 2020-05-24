using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace LINGYUN.Abp.Identity
{
    /// <summary>
    /// 重写IdentityOptions,主要替换配置工厂组件,不再从SettingProvider获取IdentityOptions配置
    /// 在跨服务器时,从SettingProvider获取配置,用户需等待很长的时间,严重影响体验
    /// 如果是本地服务器,可以忽略这些性能影响,不需要引用此模块
    /// </summary>
    [DependsOn(typeof(AbpIdentityDomainModule))]
    public class AbpIdentityOverrideOptionsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // TODO:配置文件指定IdentityOptions,避免从数据库读取的超长时间等待
            // 问题点：https://github.com/abpframework/abp/blob/dev/modules/identity/src/Volo.Abp.Identity.Domain/Volo/Abp/Identity/AbpIdentityOptionsFactory.cs
            // 有11个同步等待任务去获取身份认证配置,如果缓存不存在,会执行11条数据库指令
            // 运行过程中严重影响体验
            context.Services.Replace(ServiceDescriptor.Transient<IOptionsFactory<IdentityOptions>, AbpIdentityOverrideOptionsFactory>());
            context.Services.Replace(ServiceDescriptor.Transient<IOptions<IdentityOptions>, OptionsManager<IdentityOptions>>());

            var configuration = context.Services.GetConfiguration();
            Configure<IdentityOptions>(configuration.GetSection("Identity"));
            Configure<IdentityOptions>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(configuration.GetSection("Identity:Lockout:LockoutDuration").Get<int>());
            });
        }
    }
}
