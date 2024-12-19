# LINGYUN.Abp.MultiTenancy.Editions

多租户版本管理模块，提供租户版本（Edition）的基础功能支持。

## 功能特性

- 版本管理
  - 支持为租户分配不同版本
  - 版本信息包含 ID 和显示名称
  - 支持版本信息的存储和检索
- 全局功能开关
  - 支持通过全局功能开关控制版本功能
  - 可灵活配置版本功能的启用/禁用
- 身份认证集成
  - 自动将版本信息添加到用户 Claims 中
  - 支持在应用程序中获取当前租户的版本信息
- 可扩展性
  - 提供 IEditionStore 接口用于自定义版本存储
  - 支持自定义版本配置提供程序

## 模块引用

```csharp
[DependsOn(typeof(AbpMultiTenancyEditionsModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置说明

### 1. 全局功能配置

```csharp
GlobalFeatureManager.Instance.Modules.Editions(editions =>
{
    // 在这里配置版本相关的全局功能
});
```

### 2. 实现版本存储

```csharp
public class YourEditionStore : IEditionStore
{
    public async Task<EditionInfo> FindByTenantAsync(Guid tenantId)
    {
        // 实现从存储中获取租户版本信息的逻辑
        return new EditionInfo(
            id: Guid.NewGuid(),
            displayName: "Standard Edition"
        );
    }
}
```

### 3. 获取版本信息

```csharp
public class YourService
{
    private readonly IEditionConfigurationProvider _editionProvider;

    public YourService(IEditionConfigurationProvider editionProvider)
    {
        _editionProvider = editionProvider;
    }

    public async Task<EditionConfiguration> GetEditionAsync(Guid? tenantId)
    {
        return await _editionProvider.GetAsync(tenantId);
    }
}
```

## 版本信息在 Claims 中的使用

当启用版本功能时，模块会自动将版本信息添加到用户的 Claims 中：

```csharp
public class YourController
{
    public async Task<IActionResult> GetEditionInfo()
    {
        var editionId = User.FindFirstValue(AbpClaimTypes.EditionId);
        // 使用版本ID进行相关操作
    }
}
```

## 注意事项

1. 使用版本功能前需要确保已启用全局功能开关。
2. 版本存储的实现需要考虑性能和并发访问的问题。
3. 版本信息的变更可能会影响租户的功能访问权限。
4. Claims 中的版本信息会在用户认证时自动更新。

## 更多文档

- [English Documentation](README.EN.md)
