using Consul;
using Elsa.Studio.Authentication.Abstractions.ComponentProviders;
using Elsa.Studio.Authentication.Abstractions.Contracts;
using Elsa.Studio.Authentication.Abstractions.Models;
using Elsa.Studio.Authentication.OpenIdConnect.BlazorServer.Components;
using Elsa.Studio.Authentication.OpenIdConnect.BlazorServer.Services;
using Elsa.Studio.Authentication.OpenIdConnect.Contracts;
using Elsa.Studio.Authentication.OpenIdConnect.Models;
using Elsa.Studio.Authentication.OpenIdConnect.Services;
using Elsa.Studio.Contracts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Polly;
using System;
using System.Linq;
using System.Net.Http;

namespace LINGYUN.Abp.MicroService.WorkflowService.Extensions;

public static class ElsaStudioServiceCollectionExtensions
{
    internal const string AnonymousHttpClientName = "Elsa.Studio.Authentication.OpenIdConnect.BlazorServer.Anonymous";

    public static IServiceCollection AddOpenIdConnectAuth(
        this IServiceCollection services,
        Action<OidcOptions> configure,
        Func<IAsyncPolicy<HttpResponseMessage>> configureRetryPolicy = null)
    {
        var options = new OidcOptions();
        configure(options);
        services.AddSingleton<StudioAuthenticationProviderRegistration>(new StudioAuthenticationProviderRegistration(StudioAuthenticationProvider.OpenIdConnect));
        // Ensure we always request the minimal identity scopes.
        var configuredScopes = options.AuthenticationScopes?.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct(StringComparer.OrdinalIgnoreCase).ToArray() ?? Array.Empty<string>();
        if (configuredScopes.Length == 0)
            configuredScopes = ["openid", "profile", "offline_access"];
        options.AuthenticationScopes = configuredScopes;

        // Set Blazor Server defaults for callback paths if not explicitly specified.
        options.CallbackPath ??= "/signin-oidc";
        options.SignedOutCallbackPath ??= "/signout-callback-oidc";

        // Register core services
        services.AddHttpContextAccessor();
        services.AddSingleton(options);
        services.AddScoped<ITokenProvider, ServerTokenProvider>();
        services.AddScoped<IHttpConnectionOptionsConfigurator, OidcHttpConnectionOptionsConfigurator>();

        // Shared token refresh service used by both session refresh and backend API token acquisition
        services.AddScoped<TokenRefreshService>();

        // Cookie authentication events for automatic session token refresh (standard ASP.NET Core pattern)
        services.AddScoped<AuthCookieEvents>();

        // Configure ASP.NET Core authentication with cookie and OIDC
        services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, cookieOptions =>
            {
                cookieOptions.Cookie.Name = "ElsaStudio.Auth";
                cookieOptions.Cookie.HttpOnly = true;
                cookieOptions.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
                cookieOptions.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
                cookieOptions.ExpireTimeSpan = TimeSpan.FromHours(8);
                cookieOptions.SlidingExpiration = true;
                cookieOptions.LoginPath = "/Account/Login";
                cookieOptions.AccessDeniedPath = "/Account/Login";

                // Use custom events for automatic token refresh on every request validation
                cookieOptions.EventsType = typeof(AuthCookieEvents);
            })
            .AddAbpOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, oidcOptions =>
            {
                oidcOptions.Authority = options.Authority;
                oidcOptions.ClientId = options.ClientId;
                oidcOptions.ClientSecret = options.ClientSecret;
                oidcOptions.ResponseType = options.ResponseType;
                oidcOptions.UsePkce = options.UsePkce;
                oidcOptions.SaveTokens = options.SaveTokens;
                oidcOptions.CallbackPath = options.CallbackPath;
                oidcOptions.SignedOutCallbackPath = options.SignedOutCallbackPath;
                oidcOptions.RequireHttpsMetadata = options.RequireHttpsMetadata;
                oidcOptions.GetClaimsFromUserInfoEndpoint = options.GetClaimsFromUserInfoEndpoint;
                oidcOptions.PushedAuthorizationBehavior = PushedAuthorizationBehavior.Disable;

                // Configure scopes
                oidcOptions.Scope.Clear();
                foreach (var scope in options.AuthenticationScopes)
                {
                    oidcOptions.Scope.Add(scope);
                }

                // Map token response properties to enable token refresh
                oidcOptions.MapInboundClaims = false;

                if (!string.IsNullOrWhiteSpace(options.MetadataAddress))
                {
                    oidcOptions.MetadataAddress = options.MetadataAddress;
                }

                // Configure token validation parameters
                oidcOptions.TokenValidationParameters = new()
                {
                    NameClaimType = options.NameClaimType,
                    RoleClaimType = options.RoleClaimType,
                    ValidateIssuer = true
                };
            });

        // Add authorization services
        services.AddAuthorizationCore();

        // Use an OIDC-aware unauthorized component that initiates a challenge.
        services.AddScoped<IUnauthorizedComponentProvider, UnauthorizedComponentProvider<ChallengeToLogin>>();
        services.AddScoped<ILoginMethodCatalog, DirectOpenIdConnectLoginMethodCatalog>();
        services.AddScoped<ILoginMethodComponentProvider, DirectOpenIdConnectLoginMethodComponentProvider>();

        // HTTP client for token refresh requests with retry policy
        var retryPolicy = configureRetryPolicy?.Invoke() ?? Elsa.Studio.Authentication.OpenIdConnect.BlazorServer.Extensions.ServiceCollectionExtensions.DefaultRetryPolicy;
        services.AddHttpClient(AnonymousHttpClientName)
            .AddPolicyHandler(retryPolicy);

        return services;
    }
}
