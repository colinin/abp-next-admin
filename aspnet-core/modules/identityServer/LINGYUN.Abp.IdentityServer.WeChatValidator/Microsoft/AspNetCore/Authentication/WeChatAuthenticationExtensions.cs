using LINGYUN.Abp.WeChat.Authorization;
using Microsoft.AspNetCore.Authentication.WeChat;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Microsoft.AspNetCore.Authentication
{
    public static class WeChatAuthenticationExtensions
    {
        /// <summary> 
        /// </summary>
        public static AuthenticationBuilder AddWeChat(
            this AuthenticationBuilder builder)
        {
            return builder
                .AddWeChat(
                    AbpWeChatAuthorizationConsts.AuthenticationScheme, 
                    AbpWeChatAuthorizationConsts.DisplayName, 
                    options => { });
        }

        /// <summary> 
        /// </summary>
        public static AuthenticationBuilder AddWeChat(
            this AuthenticationBuilder builder, 
            Action<WeChatAuthenticationOptions> configureOptions)
        {
            return builder
                .AddWeChat(
                    AbpWeChatAuthorizationConsts.AuthenticationScheme, 
                    AbpWeChatAuthorizationConsts.DisplayName,
                    configureOptions);
        }

        /// <summary> 
        /// </summary>
        public static AuthenticationBuilder AddWeChat(
            this AuthenticationBuilder builder, 
            string authenticationScheme,
            Action<WeChatAuthenticationOptions> configureOptions)
        {
            return builder
                .AddWeChat(
                    authenticationScheme,
                    AbpWeChatAuthorizationConsts.DisplayName, 
                    configureOptions);
        }

        /// <summary> 
        /// </summary>
        public static AuthenticationBuilder AddWeChat(
            this AuthenticationBuilder builder, 
            string authenticationScheme, 
            string displayName, 
            Action<WeChatAuthenticationOptions> configureOptions)
        {
            return builder
                .AddOAuth<WeChatAuthenticationOptions, WeChatAuthenticationHandler>(
                    authenticationScheme, 
                    displayName, 
                    configureOptions);
        }
    }
}
