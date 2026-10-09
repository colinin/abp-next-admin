using Duende.IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using Volo.Abp.AspNetCore;
using Volo.Abp.AspNetCore.Security.Claims;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;

namespace LINGYUN.Abp.AspNetCore.Authentication;

[DependsOn(typeof(AbpAspNetCoreModule))]
public class AbpAspNetCoreAuthenticationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpClaimsMapOptions>(options =>
        {
            options.Maps[JwtClaimTypes.Subject] = () => AbpClaimTypes.UserId;
            options.Maps[JwtClaimTypes.Role] = () => AbpClaimTypes.Role;
            options.Maps[JwtClaimTypes.Email] = () => AbpClaimTypes.Email;
            options.Maps[JwtClaimTypes.EmailVerified] = () => AbpClaimTypes.EmailVerified;
            options.Maps[JwtClaimTypes.PhoneNumber] = () => AbpClaimTypes.PhoneNumber;
            options.Maps[JwtClaimTypes.PhoneNumberVerified] = () => AbpClaimTypes.PhoneNumberVerified;
            options.Maps[JwtClaimTypes.Name] = () => AbpClaimTypes.Name;
            options.Maps[JwtClaimTypes.FamilyName] = () => AbpClaimTypes.SurName;
            options.Maps[JwtClaimTypes.GivenName] = () => AbpClaimTypes.Name;
            options.Maps[JwtClaimTypes.PreferredUserName] = () => AbpClaimTypes.UserName;
            options.Maps["unique_name"] = () => AbpClaimTypes.UserName;
        });

        context.Services.Replace(
            ServiceDescriptor.Transient<IClaimsTransformation, AbpClaimsMapClaimsTransformation>());
    }
}
