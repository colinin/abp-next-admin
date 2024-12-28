# LINGYUN.Abp.OpenApi.IdentityServer

OpenApi IdentityServer integration module, providing IdentityServer-based AppKey/AppSecret storage implementation.

## Features

* IdentityServer-based AppKey/AppSecret storage
* Automatic mapping of AppKey to IdentityServer client identifier
* Automatic mapping of AppSecret to client secret
* Support for signature validity period configuration
* Support for client name configuration
* Automatic creation and update of client information

## Installation

```bash
dotnet add package LINGYUN.Abp.OpenApi.IdentityServer
```

## Module Dependencies

```csharp
[DependsOn(typeof(AbpOpenApiIdentityServerModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## How It Works

1. AppKey Mapping
   * AppKey is mapped to IdentityServer's ClientId
   * Client query and management through the `IClientRepository` interface

2. AppSecret Mapping
   * AppSecret is mapped to client secret
   * Secret is stored with AppSecret as identifier

3. Signature Validity Period
   * SignLifetime is stored as a custom property of the client
   * Property name is "SignLifetime", value is validity period in seconds

## Basic Usage

1. Store Application Credentials
   ```csharp
   public class YourService
   {
       private readonly IAppKeyStore _appKeyStore;

       public YourService(IAppKeyStore appKeyStore)
       {
           _appKeyStore = appKeyStore;
       }

       public async Task CreateAppAsync()
       {
           var descriptor = new AppDescriptor(
               appName: "Test Application",
               appKey: "your-app-key",
               appSecret: "your-app-secret",
               signLifeTime: 300  // 5 minutes validity
           );

           await _appKeyStore.StoreAsync(descriptor);
       }
   }
   ```

2. Query Application Credentials
   ```csharp
   public class YourService
   {
       private readonly IAppKeyStore _appKeyStore;

       public YourService(IAppKeyStore appKeyStore)
       {
           _appKeyStore = appKeyStore;
       }

       public async Task<AppDescriptor> GetAppAsync(string appKey)
       {
           return await _appKeyStore.FindAsync(appKey);
       }
   }
   ```

## More Information

* [LINGYUN.Abp.OpenApi](../LINGYUN.Abp.OpenApi/README.md)
* [LINGYUN.Abp.OpenApi.Authorization](../LINGYUN.Abp.OpenApi.Authorization/README.md)
* [IdentityServer](https://identityserver4.readthedocs.io/)
