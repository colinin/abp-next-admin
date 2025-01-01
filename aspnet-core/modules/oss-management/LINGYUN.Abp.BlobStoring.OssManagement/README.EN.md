# LINGYUN.Abp.BlobStoring.OssManagement

OSS Management implementation of ABP framework's object storage provider **IBlobProvider**

## Configuration

Module reference as needed, depends on the OssManagement module, so you need to configure the remote Oss management module's client proxy.

First, define the **appsettings.json** file:

```json
{
  "OssManagement": {
    "Bucket": "your-bucket-name"
  },
  "RemoteServices": {
    "AbpOssManagement": {
      "BaseUrl": "http://127.0.0.1:30025",
      "IdentityClient": "InternalServiceClient",
      "UseCurrentAccessToken": false
    }
  },
  "IdentityClients": {
    "InternalServiceClient": {
      "Authority": "http://127.0.0.1:44385",
      "RequireHttps": false,
      "GrantType": "client_credentials",
      "Scope": "lingyun-abp-application",
      "ClientId": "InternalServiceClient",
      "ClientSecret": "1q2w3E*"
    }
  }
}
```

```csharp
[DependsOn(typeof(AbpBlobStoringOssManagementModule))]
public class YouProjectModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var preActions = context.Services.GetPreConfigureActions<AbpBlobStoringOptions>();
        Configure<AbpBlobStoringOptions>(options =>
        {
            preActions.Configure(options);
            // YouContainer use oss management
            options.Containers.Configure<YouContainer>((containerConfiguration) =>
            {
                containerConfiguration.UseOssManagement(config =>
                {
                    config.Bucket = configuration[OssManagementBlobProviderConfigurationNames.Bucket];
                });
            });

            // all container use oss management
            options.Containers.ConfigureAll((containerName, containerConfiguration) =>
            {
                // use oss management
                containerConfiguration.UseOssManagement(config =>
                {
                    config.Bucket = configuration[OssManagementBlobProviderConfigurationNames.Bucket];
                });
            });
        });
    }
}
```

## Features

* Implements ABP framework's IBlobProvider interface
* Provides Blob storage implementation based on OssManagement
* Supports container-level configuration
* Supports global configuration
* Supports remote service calls

## Configuration Items

* Bucket: Storage bucket name
* RemoteServices.AbpOssManagement:
  * BaseUrl: OSS management service base URL
  * IdentityClient: Identity client name
  * UseCurrentAccessToken: Whether to use current access token
* IdentityClients: Identity client configuration
  * Authority: Authentication server address
  * RequireHttps: Whether HTTPS is required
  * GrantType: Authorization type
  * Scope: Authorization scope
  * ClientId: Client ID
  * ClientSecret: Client secret

## Links

* [中文文档](./README.md)
* [Module documentation](../README.md)
