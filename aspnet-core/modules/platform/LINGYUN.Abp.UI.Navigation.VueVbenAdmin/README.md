# LINGYUN.Abp.UI.Navigation.VueVbenAdmin

适用于 **abp-vue-vben-admin** 的导航菜单初始化模块。本模块提供了与VueVbenAdmin前端框架集成所需的菜单数据初始化功能。

## 功能特性

* 菜单数据初始化
  * 预设菜单结构
  * 自动注册布局
  * 动态菜单配置

* VueVbenAdmin集成
  * 布局组件配置
  * UI主题适配
  * 菜单项定制

* 扩展性支持
  * 自定义菜单提供者
  * 菜单数据重写
  * 布局配置扩展

## 快速开始

1. 引用模块
```csharp
[DependsOn(typeof(AbpUINavigationVueVbenAdminModule))]
public class YouProjectModule : AbpModule
{
    // other
}
```

2. 配置选项
```csharp
Configure<AbpUINavigationVueVbenAdminOptions>(options =>
{
    // UI名称配置
    options.UI = "Vue Vben Admin";
    // 布局名称配置
    options.LayoutName = "Vben Admin Layout";
    // 布局组件配置
    options.LayoutPath = "LAYOUT";
});
```

## 配置项说明

* `AbpUINavigationVueVbenAdminOptions.UI`
  * 说明：UI名称
  * 默认值：Vue Vben Admin
  * 注意：不建议变更，需要与前端保持一致

* `AbpUINavigationVueVbenAdminOptions.LayoutName`
  * 说明：布局名称
  * 默认值：Vben Admin Layout
  * 注意：不建议变更，需要与前端保持一致

* `AbpUINavigationVueVbenAdminOptions.LayoutPath`
  * 说明：布局组件路径
  * 默认值：LAYOUT
  * 注意：不建议变更，需要与前端保持一致

## 使用说明

1. 菜单初始化
   * 模块会自动注册默认的菜单数据
   * 可以通过实现 `INavigationDataSeedContributor` 接口来添加自定义菜单

2. 布局配置
   * 布局配置需要与前端的路由配置相匹配
   * 修改布局配置时需要同步修改前端相关配置

3. 扩展开发
   * 可以通过继承 `VueVbenAdminNavigationDataSeeder` 类来自定义菜单初始化逻辑
   * 支持通过依赖注入覆盖默认实现

## 更多信息

* [VueVbenAdmin官方文档](https://doc.vvbin.cn/)
* [ABP官方文档](https://docs.abp.io/)
