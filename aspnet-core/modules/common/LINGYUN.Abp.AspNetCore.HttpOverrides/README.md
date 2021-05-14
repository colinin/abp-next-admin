# LINGYUN.Abp.AspNetCore.HttpOverrides

## 模块说明

对于Http传输标头的一些重写类模块  

目前实现通过解析 **X-Forwarded-For** 标头获取反向代理中的真实客户地址,通常获取标头中的最后一个设置值  

### 基础模块  

### 高阶模块  

### 权限定义  

### 功能定义  

### 配置定义  

### 如何使用


```csharp

    [DependsOn(
        typeof(AbpAspNetCoreHttpOverridesModule))]
    public class YouProjectModule : AbpModule
    {
    }

```

### 更新日志 
