# LINGYUN.Abp.Aliyun.SettingManagement

阿里云配置管理模块,引用此模块可管理阿里云相关的配置,可通过网关聚合对外公布的API接口  

API接口: api/setting-management/aliyun

## 配置使用

模块按需引用,建议在配置管理承载服务引用此模块  

```csharp
[DependsOn(typeof(AbpAliyunSettingManagementModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```
## 注意

因后台管理模块负责管理所有配置,此模块仅提供查询接口
