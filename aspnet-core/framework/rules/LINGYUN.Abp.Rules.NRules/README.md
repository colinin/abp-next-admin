# LINGYUN.Abp.Rules.NRules

## 模块说明

基于 [NRules](https://github.com/NRules/NRules) 的规则引擎实现模块。

### 基础模块  

* LINGYUN.Abp.Rules

### 功能定义  

* 提供基于 NRules 的规则引擎实现
* 支持通过依赖注入自动注册规则
* 支持规则的动态加载和执行

### 配置定义  

* AbpNRulesOptions
  * DefinitionRules - 定义规则列表，用于存储所有通过依赖注入注册的规则类型

### 如何使用

1. 添加 `AbpNRulesModule` 依赖

```csharp
[DependsOn(typeof(AbpNRulesModule))]
public class YouProjectModule : AbpModule
{
}
```

2. 创建规则类

```csharp
public class YourRule : RuleBase
{
    public override void Define()
    {
        // 定义规则条件和动作
        When()
            .Match<YourInput>(x => x.SomeCondition);
        Then()
            .Do(ctx => /* 执行规则动作 */);
    }
}
```

3. 注册规则并执行

```csharp
public class YourService 
{
    private readonly IRuleProvider _ruleProvider;

    public YourService(IRuleProvider ruleProvider)
    {
        _ruleProvider = ruleProvider;
    }

    public async Task ProcessAsync()
    {
        var input = new YourInput();
        // 执行规则验证
        await _ruleProvider.ExecuteAsync(input);
    }
}
```

[返回目录](../../../README.md)
