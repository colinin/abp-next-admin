# LINGYUN.Abp.IdentityServer.Domain

IdentityServer领域模块，扩展IdentityServer4的领域层功能。

## 功能特性

* 事件服务扩展
  * 自定义事件服务实现 - `AbpEventService`
  * 可配置的事件处理程序 - `IAbpIdentityServerEventServiceHandler`
  * 默认事件处理程序 - `AbpIdentityServerEventServiceHandler`
    * 支持失败事件记录
    * 支持信息事件记录
    * 支持成功事件记录
    * 支持错误事件记录
  * 事件处理程序注册机制
    * 通过`AbpIdentityServerEventOptions`配置事件处理程序

## 模块引用

```csharp
[DependsOn(
    typeof(AbpIdentityServerDomainModule)
)]
public class YourModule : AbpModule
{
    // ...
}
```

## 依赖模块

* `Volo.Abp.IdentityServer.AbpIdentityServerDomainModule` - ABP IdentityServer领域模块

## 配置使用

### 事件处理程序配置

```csharp
Configure<AbpIdentityServerEventOptions>(options =>
{
    // 添加自定义事件处理程序
    options.EventServiceHandlers.Add<YourEventServiceHandler>();
});
```

### 事件处理程序实现

```csharp
public class YourEventServiceHandler : IAbpIdentityServerEventServiceHandler
{
    public virtual bool CanRaiseEventType(EventTypes evtType)
    {
        // 实现事件类型判断逻辑
        return true;
    }

    public virtual Task RaiseAsync(Event evt)
    {
        // 实现事件处理逻辑
        return Task.CompletedTask;
    }
}
```

相关文档:
* [IdentityServer4文档](https://identityserver4.readthedocs.io/)
* [ABP IdentityServer文档](https://docs.abp.io/en/abp/latest/Modules/IdentityServer)

[查看英文文档](README.EN.md)
