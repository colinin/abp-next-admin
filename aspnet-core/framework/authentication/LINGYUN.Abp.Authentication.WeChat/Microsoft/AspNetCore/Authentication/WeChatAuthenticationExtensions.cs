using LINGYUN.Abp.Authentication.WeChat;
using LINGYUN.Abp.WeChat;
using Microsoft.AspNetCore.Authentication.WeChat.Official;
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
                    AbpWeChatGlobalConsts.AuthenticationScheme,
                    AbpWeChatGlobalConsts.DisplayName, 
                    options => { });
        }

        /// <summary> 
        /// </summary>
        public static AuthenticationBuilder AddWeChat(
            this AuthenticationBuilder builder, 
            Action<WeChatOfficialOAuthOptions> configureOptions)
        {
            return builder
                .AddWeChat(
                    AbpWeChatGlobalConsts.AuthenticationScheme,
                    AbpWeChatGlobalConsts.DisplayName,
                    configureOptions);
        }

        /// <summary> 
        /// </summary>
        public static AuthenticationBuilder AddWeChat(
            this AuthenticationBuilder builder, 
            string authenticationScheme,
            Action<WeChatOfficialOAuthOptions> configureOptions)
        {
            return builder
                .AddWeChat(
                    authenticationScheme,
                    AbpAuthenticationWeChatConsts.DisplayName, 
                    configureOptions);
        }

        /// <summary> 
        /// </summary>
        public static AuthenticationBuilder AddWeChat(
            this AuthenticationBuilder builder, 
            string authenticationScheme, 
            string displayName, 
            Action<WeChatOfficialOAuthOptions> configureOptions)
        {
            return builder
                .AddOAuth<WeChatOfficialOAuthOptions, WeChatOfficialOAuthHandler>(
                    authenticationScheme, 
                    displayName, 
                    configureOptions);
        }
    }
}
