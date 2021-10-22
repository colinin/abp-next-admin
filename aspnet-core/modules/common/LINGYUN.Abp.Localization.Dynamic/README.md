# LINGYUN.Abp.Localization.Dynamic

动态本地化提供者组件,添加动态提供者可实现运行时替换本地化文本  

需要实现 ILocalizationStore 接口  

LocalizationManagement项目提供支持  

## 配置使用

```csharp
[DependsOn(typeof(AbpLocalizationDynamicModule))]
public class YouProjectModule : AbpModule
{
  // other
  public override void ConfigureServices(ServiceConfigurationContext context)
  {
    Configure<AbpLocalizationOptions>(options =>
    {
      options.Resources
        .Get<YouProjectResource>()
        .AddDynamic(); // 添加动态本地化文档支持
      
      // 添加所有资源的动态文档支持,将监听所有的资源包文档变更事件
      // options.Resources.AddDynamic();

      // 添加所有资源的动态文档支持,忽略 IdentityResource 资源
      // options.Resources.AddDynamic(typeof(IdentityResource));
    });
  }
}
```

## 注意事项

动态资源在启动时加载，如果通过LocalizationManagement模块查询，可能受后端存储资源体量影响整体启动时间  

详情见: [DynamicLocalizationInitializeService](./LINGYUN/Abp/Localization/Dynamic/DynamicLocalizationInitializeService.cs#L25-L38)