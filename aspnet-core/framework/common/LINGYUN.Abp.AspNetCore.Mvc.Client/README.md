# LINGYUN.Abp.AspNetCore.Mvc.Client

参照 Volo.Abp.AspNetCore.Mvc.Client 进行重写  

实现可配置的用户配置缓存时间  
实现订阅配置刷新事件清除用户配置缓存  
实现基于用户缓存的权限、特性、配置、本地化、语言接口  
引用 LINGYUN.Abp.MultiTenancy.RemoteService 可实现多租户接口  
完全脱离具体数据库接口  

#### 注意


## 配置使用


``` json

{
	"AbpMvcClient": {
		"Cache": {
			"UserCacheExpirationSeconds": 300,
			"AnonymousCacheExpirationSeconds": 300
		}
	}
}

```

```csharp
[DependsOn(typeof(AbpAspNetCoreMvcClientModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```