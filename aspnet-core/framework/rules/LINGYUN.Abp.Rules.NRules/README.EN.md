# LINGYUN.Abp.Rules.NRules

## Module Description

Rule engine implementation module based on [NRules](https://github.com/NRules/NRules).

### Base Modules  

* LINGYUN.Abp.Rules

### Features  

* Provides rule engine implementation based on NRules
* Supports automatic rule registration through dependency injection
* Supports dynamic rule loading and execution

### Configuration  

* AbpNRulesOptions
  * DefinitionRules - List of defined rules, used to store all rule types registered through dependency injection

### How to Use

1. Add `AbpNRulesModule` dependency

```csharp
[DependsOn(typeof(AbpNRulesModule))]
public class YouProjectModule : AbpModule
{
}
```

2. Create a rule class

```csharp
public class YourRule : RuleBase
{
    public override void Define()
    {
        // Define rule conditions and actions
        When()
            .Match<YourInput>(x => x.SomeCondition);
        Then()
            .Do(ctx => /* Execute rule action */);
    }
}
```

3. Register and execute rules

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
        // Execute rule validation
        await _ruleProvider.ExecuteAsync(input);
    }
}
```

[Back to TOC](../../../README.md)
