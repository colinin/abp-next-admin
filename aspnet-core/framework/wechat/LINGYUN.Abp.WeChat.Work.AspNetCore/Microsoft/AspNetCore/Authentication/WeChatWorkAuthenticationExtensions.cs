using Microsoft.AspNetCore.Authentication.WeChat.Work;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Microsoft.AspNetCore.Authentication;

public static class WeChatWorkAuthenticationExtensions
{
    /// <summary> 
    /// </summary>
    public static AuthenticationBuilder AddWeChatWork(
        this AuthenticationBuilder builder)
    {
        return builder
            .AddWeChatWork(
                WeChatWorkOAuthConsts.AuthenticationScheme,
                WeChatWorkOAuthConsts.DisplayName, 
                options => { });
    }

    /// <summary> 
    /// </summary>
    public static AuthenticationBuilder AddWeChatWork(
        this AuthenticationBuilder builder, 
        Action<WeChatWorkOAuthOptions> configureOptions)
    {
        return builder
            .AddWeChatWork(
                WeChatWorkOAuthConsts.AuthenticationScheme,
                WeChatWorkOAuthConsts.DisplayName,
                configureOptions);
    }

    /// <summary> 
    /// </summary>
    public static AuthenticationBuilder AddWeChatWork(
        this AuthenticationBuilder builder, 
        string authenticationScheme,
        Action<WeChatWorkOAuthOptions> configureOptions)
    {
        return builder
            .AddWeChatWork(
                authenticationScheme,
                WeChatWorkOAuthConsts.DisplayName, 
                configureOptions);
    }

    /// <summary> 
    /// </summary>
    public static AuthenticationBuilder AddWeChatWork(
        this AuthenticationBuilder builder, 
        string authenticationScheme, 
        string displayName, 
        Action<WeChatWorkOAuthOptions> configureOptions)
    {
        return builder
            .AddOAuth<WeChatWorkOAuthOptions, WeChatWorkOAuthHandler>(
                authenticationScheme, 
                displayName, 
                configureOptions);
    }
}
