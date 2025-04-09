﻿using LINGYUN.Abp.Identity;
using LINGYUN.Abp.Identity.Session;
using LINGYUN.Abp.Identity.Session.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace LINGYUN.Abp.OpenIddict.AspNetCore.Session;

[DependsOn(
    typeof(AbpIdentityDomainModule),
    typeof(AbpIdentitySessionAspNetCoreModule)
    )]
public class AbpOpenIddictAspNetCoreSessionModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<OpenIddictServerBuilder>(builder =>
        {
            builder.AddEventHandler(ProcessSignOutIdentitySession.Descriptor);
            builder.AddEventHandler(ProcessSignInIdentitySession.Descriptor);
            builder.AddEventHandler(RevocationIdentitySession.Descriptor);
            builder.AddEventHandler(UserInfoIdentitySession.Descriptor);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<IdentitySessionSignInOptions>(options =>
        {
            options.SignInSessionEnabled = true;
            options.SignOutSessionEnabled = true;
        });

        Configure<AbpOpenIddictAspNetCoreSessionOptions>(options =>
        {
            options.PersistentSessionGrantTypes.Add(GrantTypes.Password);
        });
    }
}
