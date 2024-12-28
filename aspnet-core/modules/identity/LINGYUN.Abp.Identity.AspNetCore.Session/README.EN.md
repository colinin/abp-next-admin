# LINGYUN.Abp.Identity.AspNetCore.Session

User session login extension module, mainly handling login/logout events provided by *AspNetCore.Identity* to manage sessions.

Due to module responsibility separation principle, please do not confuse with *LINGYUN.Abp.Identity.Session.AspNetCore* module.

## Configuration Usage

```csharp
[DependsOn(typeof(AbpIdentityAspNetCoreSessionModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Features

* Extends AbpIdentityAspNetCoreModule module
* Provides session management functionality in AspNetCore environment
* Custom authentication service implementation
* Integrates with AspNetCore.Identity login/logout events

## Service Implementation

* AbpIdentitySessionAuthenticationService - Custom authentication service
  * Handles user login/logout events
  * Manages user session state
  * Integrates with Identity session system

## Basic Usage

1. Configure authentication service
```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    context.Services.AddTransient<IAuthenticationService, AbpIdentitySessionAuthenticationService>();
}
```

2. Use authentication service
```csharp
public class YourService
{
    private readonly IAuthenticationService _authenticationService;

    public YourService(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task SignInAsync(HttpContext context, string scheme, ClaimsPrincipal principal, AuthenticationProperties properties)
    {
        await _authenticationService.SignInAsync(context, scheme, principal, properties);
    }

    public async Task SignOutAsync(HttpContext context, string scheme, AuthenticationProperties properties)
    {
        await _authenticationService.SignOutAsync(context, scheme, properties);
    }
}
```

## More Information

* [ABP Identity Documentation](https://docs.abp.io/en/abp/latest/Identity)
* [ASP.NET Core Identity Documentation](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity)
