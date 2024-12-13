# common 模块概述

## 模块简介
`common`模块是ABP框架的基础模块，提供了一系列通用功能和服务，旨在支持各种应用程序的开发和扩展。该模块包含多个子模块，每个子模块实现了特定的功能，帮助开发者快速构建高效的应用程序。

## 包含的项目列表
1. **LINGYUN.Abp.Aliyun.Authorization**
   - 提供阿里云基础认证功能，支持AppKeyId和AccessKeySecret的配置。

2. **LINGYUN.Abp.AspNetCore.HttpOverrides**
   - 实现HTTP传输标头的重写，支持获取反向代理中的真实客户地址。

3. **LINGYUN.Abp.AspNetCore.Mvc.Client**
   - 提供可配置的用户配置缓存时间，支持多租户接口。

4. **LINGYUN.Abp.BackgroundJobs.Hangfire**
   - 基于Hangfire实现的后台作业模块，支持即时、延迟和周期性任务。

5. **LINGYUN.Abp.ExceptionHandling**
   - 提供统一的异常处理和通知机制，支持自定义异常处理程序。

6. **LINGYUN.Abp.Location**
   - 提供地理编码、反向地理编码和IP地理位置解析功能。

7. **LINGYUN.Abp.IdGenerator**
   - 实现分布式唯一ID生成器，支持雪花算法。

8. **LINGYUN.Abp.Wrapper**
   - 统一包装API返回结果和异常处理。

## 每个项目的主要功能概述
- **阿里云认证模块**: 提供阿里云的认证功能，简化了对阿里云服务的访问。
- **HTTP重写模块**: 处理HTTP请求中的标头，确保获取真实的客户端地址。
- **MVC客户端模块**: 提供用户配置缓存，支持多租户架构。
- **后台作业模块**: 支持任务的调度和执行，确保后台任务的可靠性。
- **异常处理模块**: 处理应用中的异常，提供统一的通知机制。
- **位置服务模块**: 提供地理位置相关的功能，支持地址与坐标之间的转换。
- **ID生成模块**: 生成分布式唯一ID，确保在高并发环境下的唯一性。
- **包装器模块**: 统一处理API的返回结果和异常，提升API的可用性。

## 模块的整体用途和重要性
`common`模块为ABP框架提供了基础设施，支持开发者在构建应用时快速集成常用功能，减少重复工作，提高开发效率。通过这些模块，开发者能够更专注于业务逻辑的实现，而无需担心底层的实现细节。

## 如何使用或集成该模块的简要说明
在项目中使用`common`模块时，只需在模块类中添加相应的依赖项，并在`ConfigureServices`方法中进行必要的配置。例如：

```csharp
[DependsOn(typeof(LINGYUN.Abp.Aliyun.Authorization))]
public class YourProjectModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // 配置服务
    }
}
```

## 提示
本项目中的README是由AI模型分析代码逻辑后自动生成的，如有误，请提issues或PR。
