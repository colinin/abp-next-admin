# LINGYUN.Platform.Domain

平台管理模块

#### 注意

> 动态菜单管理

  ## IDataSeedContributor
    说明: 用于预置菜单数据的接口,模块默认已初始化与vue-admin相关的菜单

  ## 其他注意事项

    1、不论是布局(path)还是菜单(component),都不需要添加 @/ 的前缀(这通常在前端定义路由时需要),因为前端不支持这样的形式  

    2、请在运行平台服务之前,执行 dotnet ef database update 更新平台服务数据结构
  

## 配置使用


```csharp
[DependsOn(typeof(PlatformDomainModule))]
public class YouProjectModule : AbpModule
{
  // other
}
