# LINGYUN.Abp.Aliyun.SettingManagement

Alibaba Cloud configuration management module. By referencing this module, you can manage Alibaba Cloud-related configurations and access the API interfaces published through the gateway aggregation.

API endpoint: api/setting-management/aliyun

## Module Reference

The module should be referenced as needed. It is recommended to reference this module in the configuration management hosting service.

```csharp
[DependsOn(typeof(AbpAliyunSettingManagementModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## Note

Since the background management module is responsible for managing all configurations, this module only provides query interfaces.

[查看中文文档](README.md)
