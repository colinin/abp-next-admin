# LINGYUN.Abp.FeatureManagement.HttpApi

Feature management HTTP API module that provides REST API interfaces for feature definition management.

## Features

* Feature Definition Management API
  * Create, update, delete feature definitions
  * Query feature definition list
* Feature Group Definition Management API
  * Create, update, delete feature group definitions
  * Query feature group definition list
* Localization support
* Integration with ABP feature management module

## Module Dependencies

```csharp
[DependsOn(
    typeof(AbpFeatureManagementApplicationContractsModule),
    typeof(VoloAbpFeatureManagementHttpApiModule))]
public class AbpFeatureManagementHttpApiModule : AbpModule
{
}
```

## API Routes

### Feature Definitions

* GET /api/feature-management/definitions
* GET /api/feature-management/definitions/{name}
* POST /api/feature-management/definitions
* PUT /api/feature-management/definitions/{name}
* DELETE /api/feature-management/definitions/{name}

### Feature Group Definitions

* GET /api/feature-management/group-definitions
* GET /api/feature-management/group-definitions/{name}
* POST /api/feature-management/group-definitions
* PUT /api/feature-management/group-definitions/{name}
* DELETE /api/feature-management/group-definitions/{name}

## Localization Configuration

The module uses ABP's localization system with the following resources:
* AbpFeatureManagementResource
* AbpValidationResource

## More Information

* [ABP Web API Documentation](https://docs.abp.io/en/abp/latest/API/Auto-API-Controllers)
* [ABP Feature Management Documentation](https://docs.abp.io/en/abp/latest/Features)

[简体中文](README.md)
