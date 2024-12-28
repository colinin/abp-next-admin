# LINGYUN.Abp.FeatureManagement.Application

Feature management application service module that provides implementation for feature definition management services.

## Features

* Feature Definition Management
  * Support creating, updating, and deleting feature definitions
  * Support feature definition localization
  * Support feature definition value type serialization
* Feature Group Definition Management
  * Support creating, updating, and deleting feature group definitions
  * Support feature group localization
* Support static and dynamic feature definition storage
* Integration with ABP feature management module

## Module Dependencies

```csharp
[DependsOn(
    typeof(AbpFeatureManagementApplicationContractsModule),
    typeof(VoloAbpFeatureManagementApplicationModule))]
public class AbpFeatureManagementApplicationModule : AbpModule
{
}
```

## Permission Definitions

* FeatureManagement.GroupDefinitions
  * FeatureManagement.GroupDefinitions.Create
  * FeatureManagement.GroupDefinitions.Update
  * FeatureManagement.GroupDefinitions.Delete
* FeatureManagement.Definitions
  * FeatureManagement.Definitions.Create
  * FeatureManagement.Definitions.Update
  * FeatureManagement.Definitions.Delete

## More Information

* [ABP Feature Management Documentation](https://docs.abp.io/en/abp/latest/Features)
* [Feature Management Best Practices](https://docs.abp.io/en/abp/latest/Best-Practices/Features)

[简体中文](README.md)
