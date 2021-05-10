# LINGYUN.Abp.Rules.RulesEngine

## 模块说明

集成微软规则引擎的实现  

默认实现一个本地文件系统规则提供者,根据用户配置的 **PhysicalPath** 路径检索规则文件  

文件名如下:

PhysicalPath/CurrentTenant.Id[如果存在]/验证规则实体类型名称[typeof(Input).Name].json  

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
            Configure<AbpRulesEngineOptions>(options =>
            {
                // 指定真实存在的本地文件路径, 否则将不会检索本地规则文件
                options.PhysicalPath = Path.Combine(Directory.GetCurrentDirectory(), "Rules");
            });
        }
    }

```

### 更新日志 
