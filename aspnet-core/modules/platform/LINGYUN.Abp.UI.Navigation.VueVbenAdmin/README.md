# LINGYUN.Abp.UI.Navigation.VueVbenAdmin

适用于 **abp-vue-vben-admin** 的初始化菜单数据模块  

## 配置使用

```csharp
[DependsOn(typeof(AbpUINavigationVueVbenAdminModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

*	AbpUINavigationVueVbenAdminOptions.UI				UI名称,默认值: Vue Vben Admin,不建议变更,否则需要改变前端  
*	AbpUINavigationVueVbenAdminOptions.LayoutName		布局名称,默认值: Vben Admin Layout,不建议变更,否则需要改变前端  
*	AbpUINavigationVueVbenAdminOptions.LayoutPath		布局组件,默认值: LAYOUT,不建议变更,否则需要改变前端  

## 其他
