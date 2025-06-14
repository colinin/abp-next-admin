using JetBrains.Annotations;
using LINGYUN.Abp.Account.Web.OAuth.ExternalProviders;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;
using Volo.Abp;

namespace LINGYUN.Abp.Account.Web.OAuth.Microsoft.Extensions.DependencyInjection;

public static class AuthenticationBuilderExtensions
{
    public static AuthenticationBuilder UseSettingProvider<TOptions, THandler, TOptionsProvider>(
        [NotNull] this AuthenticationBuilder authenticationBuilder)
        where TOptions : RemoteAuthenticationOptions, new()
        where THandler : RemoteAuthenticationHandler<TOptions>
        where TOptionsProvider : IOAuthHandlerOptionsProvider<TOptions>
    {
        Check.NotNull(authenticationBuilder, nameof(authenticationBuilder));

        var handler = authenticationBuilder.Services.LastOrDefault(x => x.ServiceType == typeof(THandler));
        authenticationBuilder.Services.Replace(new ServiceDescriptor(
            typeof(THandler),
            provider => new AccountAuthenticationRequestHandler<TOptions, THandler>(
                (THandler)ActivatorUtilities.CreateInstance(provider, typeof(THandler)),
                provider.GetRequiredService<TOptionsProvider>()),
            handler?.Lifetime ?? ServiceLifetime.Transient));

        return authenticationBuilder;
    }
}
