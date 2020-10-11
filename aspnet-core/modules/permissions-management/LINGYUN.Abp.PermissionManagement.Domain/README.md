# LINGYUN.Abp.PermissionManagement.Domain

重写 **Volo.Abp.PermissionManagement.PermissionManager**, 在查询权限的时候优先检查缓存

大部分重写的模块都和官方模块名称保持一致,通过命名空间区分,主要是只改写了一小部分或者增加额外的功能
如果大部分模块代码都重写,或者完全就是扩展模块,才会定义自己的名字

#### 注意

当使用了此模块,可能会出现您不愿意见到的场景,因为当您只需要查看某个实体拥有的权限,然后却为此建立了它的全部缓存项

在一些特定场景下(**比如云缓存**),请避免引用此模块,以免产生额外的费用

## 配置使用


```csharp
[DependsOn(typeof(LINGYUN.Abp.PermissionManagement.AbpPermissionManagementDomainModule))]
public class YouProjectModule : AbpModule
{
  // other
}
