# LINGYUN.Abp.Localization.Dynamic

动态本地化提供者组件,添加动态提供者可实现运行时替换本地化文本  

需要实现 ILocalizationStore 接口  

LocalizationManagement项目提供支持  


由于框架设计为延迟初始化,当某个本地化资源被使用的时候才会进行初始化  
当资源被第一次使用到的时候,才会注册动态变更事件  
详情见: [DynamicLocalizationResourceContributor](./LINGYUN/Abp/Localization/Dynamic/DynamicLocalizationResourceContributor.cs#L29-L34)

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
