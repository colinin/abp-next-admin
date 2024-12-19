# LINGYUN.Platform.Application

The application service implementation layer of the platform management module, implementing all functionality defined in the application service interfaces.

## Features

* User Favorite Menu Service
  * Create favorite menu
  * Update favorite menu
  * Delete favorite menu
  * Query favorite menu list
  * Manage other users' favorite menus

* Object Mapping Configuration
  * Automatic mapping from entities to DTOs
  * Support for custom mapping rules
  * Support for extra property mapping

* Permission Validation
  * Policy-based permission validation
  * Integration with ABP authorization system
  * Fine-grained permission control

## Module Reference

```csharp
[DependsOn(typeof(PlatformApplicationModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Application Service Implementation

* `UserFavoriteMenuAppService`: User favorite menu service implementation
  * Support for custom menu icons
  * Support for custom menu colors
  * Support for custom menu aliases
  * Support for multi-framework menu management

## Object Mapping

```csharp
public class PlatformApplicationMappingProfile : Profile
{
    public PlatformApplicationMappingProfile()
    {
        CreateMap<PackageBlob, PackageBlobDto>();
        CreateMap<Package, PackageDto>();
        CreateMap<DataItem, DataItemDto>();
        CreateMap<Data, DataDto>();
        CreateMap<Menu, MenuDto>();
        CreateMap<Layout, LayoutDto>();
        CreateMap<UserFavoriteMenu, UserFavoriteMenuDto>();
    }
}
```

## Base Services

* `PlatformApplicationServiceBase`: Platform management application service base class
  * Provides common functionality and helper methods
  * Unified exception handling
  * Unified permission validation

## More

For more information, please refer to [Platform](../README.md)
