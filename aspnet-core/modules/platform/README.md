# LINGYUN.Platform.Domain

平台管理模块

#### 注意

> 动态菜单管理

  ## IDataSeedContributor
    说明: 用于预置菜单数据的接口,模块默认已初始化与vue-admin相关的菜单

  ## 其他注意事项
    不论是布局(path)还是菜单(component),都不需要添加 @/ 的前缀(这通常在前端定义路由时需要),因为前端不支持这样的形式
  

## 配置使用


```csharp
[DependsOn(typeof(PlatformDomainModule))]
public class YouProjectModule : AbpModule
{
  // other
}
