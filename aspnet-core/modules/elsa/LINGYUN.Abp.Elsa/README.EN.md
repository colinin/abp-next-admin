# LINGYUN.Abp.Elsa

ABP Framework integration for [elsa-core](https://github.com/elsa-workflows/elsa-core) workflow engine.

## Features

* Provides default **AbpActivity** with multi-tenancy integration
* Defines **AbpTenantAccessor** for multi-tenancy integration
* Defines **AbpElsaIdGenerator** that generates workflow identifiers through the **IGuidGenerator** interface
* Defines **abp** related JavaScript extensions

## Configuration and Usage

```csharp
[DependsOn(
    typeof(AbpElsaModule)
    )]
public class YouProjectModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<ElsaOptionsBuilder>(elsa =>
        {
            // Custom elsa related configuration
        });
    }
}
```

[中文文档](./README.md)
