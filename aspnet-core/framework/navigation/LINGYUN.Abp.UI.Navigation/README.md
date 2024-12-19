# LINGYUN.Abp.UI.Navigation

简体中文 | [English](./README.EN.md)

菜单导航模块，提供可扩展自定义的菜单项  

## 功能特性

* 支持自定义菜单项定义和扩展
* 支持多租户
* 支持菜单项的层级结构
* 支持菜单项的排序
* 支持菜单项的可见性和禁用状态
* 支持菜单项的图标和重定向
* 支持菜单种子数据的初始化

## 模块引用

```csharp
[DependsOn(typeof(AbpUINavigationModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 基本用法

### 1. 定义菜单提供者

通过实现 `INavigationDefinitionProvider` 接口来定义菜单项：

```csharp
public class FakeNavigationDefinitionProvider : NavigationDefinitionProvider
{
    public override void Define(INavigationDefinitionContext context)
    {
        context.Add(GetNavigationDefinition());
    }

    private static NavigationDefinition GetNavigationDefinition()
    {
        var dashboard = new ApplicationMenu(
            name: "Vben Dashboard",
            displayName: "仪表盘",
            url: "/dashboard",
            component: "",
            description: "仪表盘",
            icon: "ion:grid-outline",
            redirect: "/dashboard/analysis");

         dashboard.AddItem(
            new ApplicationMenu(
            name: "Analysis",
            displayName: "分析页",
            url: "/dashboard/analysis",
            component: "/dashboard/analysis/index",
            description: "分析页"));

         return new NavigationDefinition(dashboard);
    }
}
```

### 2. 初始化菜单数据

通过实现 `INavigationSeedContributor` 接口来初始化菜单种子数据：

```csharp
public class YourNavigationDataSeeder : INavigationSeedContributor
{
    public async Task SeedAsync(NavigationSeedContext context)
    {
        // 在这里初始化菜单数据
    }
}
```

## 菜单项属性说明

* Name - 菜单项名称（唯一标识）
* DisplayName - 显示名称
* Description - 说明
* Url - 路径
* Component - 组件路径
* Redirect - 重定向路径
* Icon - 图标
* Order - 排序（默认1000）
* IsDisabled - 是否禁用
* IsVisible - 是否可见
* Items - 子菜单项集合
* ExtraProperties - 扩展属性
* MultiTenancySides - 多租户选项

## 最佳实践

1. 菜单项命名建议使用有意义的名称，便于管理和维护
2. 合理使用菜单项的排序，保持界面的整洁
3. 使用图标增强用户体验
4. 适当使用重定向功能优化导航体验
5. 根据实际需求控制菜单项的可见性和禁用状态

## 注意事项

1. 菜单项的Name必须唯一
2. 子菜单项会继承父菜单项的多租户设置
3. 菜单项的排序值越小越靠前
4. 禁用的菜单项仍然可见，但无法点击
5. 组件路径应与前端路由配置保持一致
