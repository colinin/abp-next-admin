# LINGYUN.Abp.FeatureManagement.Application.Contracts

Feature management application service contract module that defines interfaces, DTOs, and permissions required for feature management.

## Features

* Feature Definition Management Interfaces
  * IFeatureDefinitionAppService
  * Support CRUD operations for feature definitions
* Feature Group Definition Management Interfaces
  * IFeatureGroupDefinitionAppService
  * Support CRUD operations for feature group definitions
* Complete DTO Definitions
  * FeatureDefinitionDto
  * FeatureGroupDefinitionDto
  * Create, Update, and Query DTOs
* Permission Definitions
  * Feature definition management permissions
  * Feature group definition management permissions

## Module Dependencies

```csharp
[DependsOn(
    typeof(AbpFeatureManagementDomainSharedModule),
    typeof(VoloAbpFeatureManagementApplicationContractsModule))]
public class AbpFeatureManagementApplicationContractsModule : AbpModule
{
}
```

## Permission Constants

```csharp
public static class FeatureManagementPermissionNames
{
    public const string GroupName = "FeatureManagement";

    public static class GroupDefinition
    {
        public const string Default = GroupName + ".GroupDefinitions";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public static class Definition
    {
        public const string Default = GroupName + ".Definitions";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }
}
```

## Error Codes

* Error:100001 - Feature definition already exists
* Error:100002 - Feature group definition already exists
* Error:100003 - Cannot delete static feature definition
* Error:100004 - Cannot delete static feature group definition

## More Information

* [ABP Feature Management Documentation](https://docs.abp.io/en/abp/latest/Features)
* [ABP Application Services Documentation](https://docs.abp.io/en/abp/latest/Application-Services)

[简体中文](README.md)
