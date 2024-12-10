# LINGYUN.Abp.OpenApi.Authorization

OpenApi authentication authorization middleware module, providing AppKey/AppSecret based API signature authentication middleware functionality for ABP applications.

## Features

* Provides OpenApi authentication middleware
* Supports request signature verification
* Supports replay attack prevention (Nonce random number verification)
* Supports request timestamp verification
* Supports client whitelist verification
* Supports IP address whitelist verification
* Supports custom authentication logic
* Supports exception handling and error message wrapping

## Installation

```bash
dotnet add package LINGYUN.Abp.OpenApi.Authorization
```

## Module Dependencies

```csharp
[DependsOn(typeof(AbpOpenApiAuthorizationModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Basic Usage

1. Enable OpenApi Authentication Middleware
   ```csharp
   public void Configure(IApplicationBuilder app)
   {
       // Add OpenApi authentication middleware
       app.UseOpenApiAuthorization();
   }
   ```

2. Custom Authentication Service (Optional)
   ```csharp
   public class CustomOpenApiAuthorizationService : OpenApiAuthorizationService
   {
       public CustomOpenApiAuthorizationService(
           INonceStore nonceStore,
           IAppKeyStore appKeyStore,
           IClientChecker clientChecker,
           IIpAddressChecker ipAddressChecker,
           IWebClientInfoProvider clientInfoProvider,
           IOptionsMonitor<AbpOpenApiOptions> options,
           IOptions<AbpExceptionHandlingOptions> exceptionHandlingOptions)
           : base(nonceStore, appKeyStore, clientChecker, ipAddressChecker,
                 clientInfoProvider, options, exceptionHandlingOptions)
       {
       }

       // Override authentication logic
       public override async Task<bool> AuthorizeAsync(HttpContext httpContext)
       {
           // Implement custom authentication logic
           return await base.AuthorizeAsync(httpContext);
       }
   }
   ```

## Authentication Process

1. Validate Client IP Address
   * Verify if the client IP address is within the allowed range through the `IIpAddressChecker` interface

2. Validate Application Credentials
   * Verify AppKey, signature, nonce, and timestamp in request headers
   * Get application information through the `IAppKeyStore` interface
   * Verify client access permission through the `IClientChecker` interface
   * Validate signature validity and timeliness

## Signature Rules

1. Sort request parameters by parameter name in ASCII order
2. Convert sorted parameters to query string using URL encoding (UTF-8)
3. Concatenate request path and query string, then perform MD5 encryption to get the signature

Example:
```
Request path: /api/test
Parameters:
  appKey=test
  appSecret=123456
  nonce=abc
  t=1577808000000

Signature calculation:
1. Sort and concatenate parameters: appKey=test&appSecret=123456&nonce=abc&t=1577808000000
2. Concatenate with request path: /api/test?appKey=test&appSecret=123456&nonce=abc&t=1577808000000
3. URL encode and MD5 encrypt to get the final signature
```

## More Information

* [LINGYUN.Abp.OpenApi](../LINGYUN.Abp.OpenApi/README.md)
* [ABP Framework](https://abp.io/)
