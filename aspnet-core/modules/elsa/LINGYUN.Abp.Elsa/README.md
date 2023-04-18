# LINGYUN.Abp.Elsa

[elsa-core](https://github.com/elsa-workflows/elsa-core) 工作流的abp集成  

## 特性

* 提供默认**AbpActivity**与多租户集成  
* 定义**AbpTenantAccessor**与多租户集成  
* 定义**AbpElsaIdGenerator**通过**IGuidGenerator**接口生成工作流标识  
* 定义**abp**相关JavaScript扩展  

## 配置使用

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
                // 自定义elsa相关配置
            });
        }
    }
```