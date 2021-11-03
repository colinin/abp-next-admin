# LINGYUN.Abp.UI.Navigation

菜单导航模块，提供可扩展自定义的菜单项  

## 配置使用

应用初始化时扫描所有实现 **INavigationDefinitionProvider** 接口的用户定义菜单项  

通过 **INavigationDataSeeder** 接口初始化菜单种子数据  

**INavigationDataSeeder** 的实现交给具体的实现  

```csharp
[DependsOn(typeof(AbpUINavigationModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

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

## 其他
