using LINGYUN.Abp.IdentityServer.QQ;
using Microsoft.AspNetCore.Authentication.QQ;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Microsoft.AspNetCore.Authentication
{
    public static class QQAuthenticationExtensions
    {
        /// <summary> 
        /// </summary>
        public static AuthenticationBuilder AddQQConnect(
            this AuthenticationBuilder builder)
        {
            return builder
                .AddQQConnect(
                    AbpIdentityServerQQConsts.AuthenticationScheme,
                    AbpIdentityServerQQConsts.DisplayName, 
                    options => { });
        }

        /// <summary> 
        /// </summary>
        public static AuthenticationBuilder AddQQConnect(
            this AuthenticationBuilder builder, 
            Action<QQConnectOAuthOptions> configureOptions)
        {
            return builder
                .AddQQConnect(
                    AbpIdentityServerQQConsts.AuthenticationScheme,
                    configureOptions);
        }

        /// <summary> 
        /// </summary>
        public static AuthenticationBuilder AddQQConnect(
            this AuthenticationBuilder builder, 
            string authenticationScheme,
            Action<QQConnectOAuthOptions> configureOptions)
        {
            return builder
                .AddQQConnect(
                    authenticationScheme,
                    AbpIdentityServerQQConsts.DisplayName,
                    configureOptions);
        }

        /// <summary> 
        /// </summary>
        public static AuthenticationBuilder AddQQConnect(
            this AuthenticationBuilder builder, 
            string authenticationScheme, 
            string displayName, 
            Action<QQConnectOAuthOptions> configureOptions)
        {
            return builder
                .AddOAuth<QQConnectOAuthOptions, QQConnectOAuthHandler>(
                    authenticationScheme, 
                    displayName, 
                    configureOptions);
        }
    }
}
