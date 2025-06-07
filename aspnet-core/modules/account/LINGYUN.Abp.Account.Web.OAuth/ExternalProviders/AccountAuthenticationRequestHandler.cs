using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LINGYUN.Abp.Account.Web.OAuth.ExternalProviders;

public class AccountAuthenticationRequestHandler<TOptions, THandler> : IAuthenticationRequestHandler
    where TOptions : RemoteAuthenticationOptions, new()
    where THandler : RemoteAuthenticationHandler<TOptions>
{
    protected THandler InnerHandler { get; }
    protected IOAuthHandlerOptionsProvider<TOptions> OptionsProvider { get; }
    public AccountAuthenticationRequestHandler(
        THandler innerHandler,
        IOAuthHandlerOptionsProvider<TOptions> optionsProvider)
    {
        InnerHandler = innerHandler;
        OptionsProvider = optionsProvider;
    }

    public virtual async Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
    {
        await InnerHandler.InitializeAsync(scheme, context);
    }

    public virtual async Task<AuthenticateResult> AuthenticateAsync()
    {
        return await InnerHandler.AuthenticateAsync();
    }

    public virtual async Task ChallengeAsync(AuthenticationProperties? properties)
    {
        await InitializeOptionsAsync();

        await InnerHandler.ChallengeAsync(properties);
    }

    public virtual async Task ForbidAsync(AuthenticationProperties? properties)
    {
        await InnerHandler.ForbidAsync(properties);
    }

    public async Task SignOutAsync(AuthenticationProperties properties)
    {
        if (!(InnerHandler is IAuthenticationSignOutHandler signOutHandler))
        {
            throw new InvalidOperationException($"The authentication handler registered for scheme '{InnerHandler.Scheme}' is '{InnerHandler.GetType().Name}' which cannot be used for SignOutAsync");
        }

        await InitializeOptionsAsync();
        await signOutHandler.SignOutAsync(properties);
    }

    public async Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties properties)
    {
        if (!(InnerHandler is IAuthenticationSignInHandler signInHandler))
        {
            throw new InvalidOperationException($"The authentication handler registered for scheme '{InnerHandler.Scheme}' is '{InnerHandler.GetType().Name}' which cannot be used for SignInAsync");
        }

        await InitializeOptionsAsync();
        await signInHandler.SignInAsync(user, properties);
    }

    public virtual async Task<bool> HandleRequestAsync()
    {
        if (await InnerHandler.ShouldHandleRequestAsync())
        {
            await InitializeOptionsAsync();
        }

        return await InnerHandler.HandleRequestAsync();
    }

    protected async virtual Task InitializeOptionsAsync()
    {
        await OptionsProvider.SetOptionsAsync(InnerHandler.Options);
    }
}
