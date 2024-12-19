# LINGYUN.Abp.IdentityServer.Domain

IdentityServer domain module, extending the domain layer functionality of IdentityServer4.

## Features

* Event Service Extensions
  * Custom Event Service Implementation - `AbpEventService`
  * Configurable Event Handlers - `IAbpIdentityServerEventServiceHandler`
  * Default Event Handler - `AbpIdentityServerEventServiceHandler`
    * Support for Failure Event Logging
    * Support for Information Event Logging
    * Support for Success Event Logging
    * Support for Error Event Logging
  * Event Handler Registration Mechanism
    * Configure Event Handlers through `AbpIdentityServerEventOptions`

## Module Dependencies

```csharp
[DependsOn(
    typeof(AbpIdentityServerDomainModule)
)]
public class YourModule : AbpModule
{
    // ...
}
```

## Required Modules

* `Volo.Abp.IdentityServer.AbpIdentityServerDomainModule` - ABP IdentityServer Domain Module

## Configuration and Usage

### Event Handler Configuration

```csharp
Configure<AbpIdentityServerEventOptions>(options =>
{
    // Add custom event handler
    options.EventServiceHandlers.Add<YourEventServiceHandler>();
});
```

### Event Handler Implementation

```csharp
public class YourEventServiceHandler : IAbpIdentityServerEventServiceHandler
{
    public virtual bool CanRaiseEventType(EventTypes evtType)
    {
        // Implement event type validation logic
        return true;
    }

    public virtual Task RaiseAsync(Event evt)
    {
        // Implement event handling logic
        return Task.CompletedTask;
    }
}
```

Related Documentation:
* [IdentityServer4 Documentation](https://identityserver4.readthedocs.io/)
* [ABP IdentityServer Documentation](https://docs.abp.io/en/abp/latest/Modules/IdentityServer)

[查看中文文档](README.md)
