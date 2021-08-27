# LINGYUN.Abp.Rules.RulesEngine

## 模块说明

集成微软规则引擎的实现  

默认实现一个本地文件系统规则提供者,根据用户配置的 **AbpRulesEnginePthsicalFileResolveOptions.PhysicalPath** 路径检索规则文件  

文件名如下:

PhysicalPath/CurrentTenant.Id[如果存在]/验证规则实体类型名称[typeof(Input).Name].json  

自定义的规则提供者需要实现 **IWorkflowRulesResolveContributor** 接口,可能不需要实现初始化与释放资源,因此提供了一个抽象的 **WorkflowRulesResolveContributorBase**  

并添加到 **AbpRulesEngineResolveOptions.WorkflowRulesResolvers** 中  

### 基础模块  

### 高阶模块  

### 权限定义  

### 功能定义  

### 配置定义  

### 如何使用


```csharp

    [DependsOn(
        typeof(AbpRulesEngineModule))]
    public class YouProjectModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpRulesEngineResolveOptions>(options =>
            {
                // 添加自行实现的规则解析提供者
                options.WorkflowRulesResolvers.Add(new FakeWorkflowRulesResolveContributor());
            });

            Configure<AbpRulesEnginePhysicalFileResolveOptions>(options =>
            {
                // 指定真实存在的本地文件路径, 否则将不会检索本地规则文件
                options.PhysicalPath = Path.Combine(Directory.GetCurrentDirectory(), "Rules");
            });
        }
    }

```

### 更新日志 
