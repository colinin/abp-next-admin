# LINGYUN.Abp.OpenApi

OpenApi authentication module, providing AppKey/AppSecret based API signature authentication functionality for ABP applications.

## Features

* Supports AppKey/AppSecret authentication
* Supports request signature verification
* Supports replay attack prevention (Nonce random number verification)
* Supports request timestamp verification
* Supports client whitelist
* Supports IP address whitelist
* Supports multilingual error messages

## Installation

```bash
dotnet add package LINGYUN.Abp.OpenApi
```

## Module Dependencies

```csharp
[DependsOn(typeof(AbpOpenApiModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Configuration

```json
{
  "OpenApi": {
    "IsEnabled": true,                   // Enable API signature check, default: true
    "RequestNonceExpireIn": "00:10:00",  // Request nonce expiration time, default: 10 minutes
    "AppDescriptors": [                  // AppKey configuration list
      {
        "AppName": "Test Application",   // Application name
        "AppKey": "your-app-key",        // Application key
        "AppSecret": "your-app-secret",  // Application secret
        "AppToken": "optional-token",    // Optional application token
        "SignLifetime": 300              // Signature validity period (seconds)
      }
    ]
  }
}
```

## Basic Usage

1. Configure AppKey/AppSecret
   * Add AppKey and AppSecret in the configuration file
   * Or implement custom `IAppKeyStore` interface to manage AppKey

2. Enable OpenApi Authentication
   ```csharp
   public override void ConfigureServices(ServiceConfigurationContext context)
   {
       var configuration = context.Services.GetConfiguration();
       Configure<AbpOpenApiOptions>(configuration.GetSection("OpenApi"));
   }
   ```

3. Custom Client Verification (Optional)
   ```csharp
   public class CustomClientChecker : IClientChecker
   {
       public Task<bool> IsGrantAsync(string clientId, CancellationToken cancellationToken = default)
       {
           // Implement custom client verification logic
           return Task.FromResult(true);
       }
   }
   ```

4. Custom IP Address Verification (Optional)
   ```csharp
   public class CustomIpAddressChecker : IIpAddressChecker
   {
       public Task<bool> IsGrantAsync(string ipAddress, CancellationToken cancellationToken = default)
       {
           // Implement custom IP address verification logic
           return Task.FromResult(true);
       }
   }
   ```

## Error Codes

* AbpOpenApi:9100 - Invalid AppKey
* AbpOpenApi:9101 - AppKey not found
* AbpOpenApi:9110 - Invalid sign
* AbpOpenApi:9111 - Sign not found
* AbpOpenApi:9210 - Request timed out or session expired
* AbpOpenApi:9211 - Timestamp not found
* AbpOpenApi:9220 - Repeatedly initiated requests
* AbpOpenApi:9221 - Nonce not found
* AbpOpenApi:9300 - Client is not within the allowed range
* AbpOpenApi:9400 - Client IP is not within the allowed range

## More Information

* [ABP Framework](https://abp.io/)
