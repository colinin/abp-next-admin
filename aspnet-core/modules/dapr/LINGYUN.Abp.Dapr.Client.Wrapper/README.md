# LINGYUN.Abp.Dapr.Client.Wrapper

Dapr服务间调用，对包装后的响应结果解包  

## 配置使用

模块按需引用

```csharp
[DependsOn(typeof(AbpDaprClientModule))]
public class AbpDaprClientWrapperModule : AbpModule
{
}
```

## 其他
