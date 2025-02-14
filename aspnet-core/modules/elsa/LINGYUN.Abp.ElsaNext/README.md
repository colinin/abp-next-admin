# LINGYUN.Abp.ElsaNext

[elsa-core](https://github.com/elsa-workflows/elsa-core) 工作流的abp集成(3.x版本)  

## 特性

* 定义**AbpTenantResolver**与多租户集成,使elsa支持abp租户解析    
* 定义**AbpTenantsProvider**与多租户集成,使elsa从abp中取租户信息    
* 定义**AbpElsaIdGenerator**通过**IGuidGenerator**接口生成工作流标识  
* 定义**abp**相关JavaScript扩展  

## 配置使用

```csharp

    [DependsOn(
        typeof(AbpElsaNextModule)
        )]
    public class YouProjectModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IModule>(elsa =>
            {
                // 自定义elsa相关配置
            });
        }
    }
```