# LINGYUN.Abp.Rules

## 模块说明

规则引擎基础模块

### 基础模块  

### 高阶模块  

### 权限定义  

### 功能定义  

### 配置定义  

### 如何使用


```csharp

    [DependsOn(
        typeof(AbpRulesModule))]
    public class YouProjectModule : AbpModule
    {
    }

    public class YouService 
    {
        private readonly IRuleProvider _ruleProvider;

        public YouService(IRuleProvider ruleProvider)
        {
            _ruleProvider = ruleProvider;
        }

        public async Task DoAsync()
        {
            var input = new YouInput();
            // 规则校验
            await _ruleProvider.ExecuteAsync(input);

            var inputParams = new object[1]
            {
                new InputParam()
            };
            // 带参数规则校验
            await _ruleProvider.ExecuteAsync(input, inputParams);
        }
    }

```

### 更新日志 
