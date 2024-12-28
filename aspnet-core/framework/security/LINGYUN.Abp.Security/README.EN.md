# LINGYUN.Abp.Security

## Module Description

Extends the Abp Security module, providing additional security features.

### Base Modules  

* Volo.Abp.Security

### Features  

* Provides JWT Claims type mapping functionality
* Extends standard JWT Claims type definitions

### Configuration  

No special configuration items

### How to Use

1. Add `AbpSecurityModule` dependency

```csharp
[DependsOn(typeof(AbpSecurityModule))]
public class YouProjectModule : AbpModule
{
}
```

2. Use JWT Claims mapping

```csharp
// JWT Claims type mapping is done automatically, no manual configuration needed
// If you need custom mapping, you can configure it in the module's ConfigureServices method
```

[Back to TOC](../../../README.md)
