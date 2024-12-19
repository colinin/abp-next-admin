# LINGYUN.Abp.Security

## 模块说明

扩展 Abp Security 模块，提供额外的安全功能。

### 基础模块  

* Volo.Abp.Security

### 功能定义  

* 提供 JWT Claims 类型映射功能
* 扩展了标准的 JWT Claims 类型定义

### 配置定义  

无特殊配置项

### 如何使用

1. 添加 `AbpSecurityModule` 依赖

```csharp
[DependsOn(typeof(AbpSecurityModule))]
public class YouProjectModule : AbpModule
{
}
```

2. 使用 JWT Claims 映射

```csharp
// JWT Claims 类型映射会自动完成，无需手动配置
// 如果需要自定义映射，可以在模块的 ConfigureServices 方法中进行配置
```

[返回目录](../../../README.md)
