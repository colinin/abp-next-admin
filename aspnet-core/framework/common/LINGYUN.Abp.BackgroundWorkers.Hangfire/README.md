# LINGYUN.Abp.BackgroundWorkers.Hangfire

[English](README.EN.md) | 简体中文

## 1. 介绍

`LINGYUN.Abp.BackgroundWorkers.Hangfire` 是一个基于Hangfire实现的ABP后台工作者模块。它提供了一种可靠的方式来管理和执行长期运行的后台任务，支持自动启动、停止和定期执行等功能。

## 2. 功能特性

* 支持后台工作者的自动启动和停止
* 支持定期执行的后台任务
* 与ABP后台工作者系统无缝集成
* 支持工作者状态管理
* 支持依赖注入

## 3. 安装

```bash
dotnet add package LINGYUN.Abp.BackgroundWorkers.Hangfire
```

## 4. 使用方法

1. 添加 `AbpBackgroundWorkersHangfireModule` 到模块依赖中:

```csharp
[DependsOn(typeof(AbpBackgroundWorkersHangfireModule))]
public class YourModule : AbpModule
{
}
```

2. 创建后台工作者:

```csharp
public class YourBackgroundWorker : AsyncPeriodicBackgroundWorkerBase
{
    public YourBackgroundWorker(
        AbpAsyncTimer timer,
        IServiceScopeFactory serviceScopeFactory) 
        : base(timer, serviceScopeFactory)
    {
        Timer.Period = 5000; // 每5秒执行一次
    }

    protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        // 在这里实现你的后台任务逻辑
        await Task.CompletedTask;
    }
}
```

3. 注册后台工作者:

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    context.Services.AddTransient<YourBackgroundWorker>();
    Configure<AbpBackgroundWorkerOptions>(options =>
    {
        options.IsEnabled = true; // 启用后台工作者
    });
}
```

## 5. 配置项

### 5.1 工作者配置

* `AbpBackgroundWorkerOptions.IsEnabled`: 是否启用后台工作者。默认值: `true`

## 6. 依赖项

* Volo.Abp.BackgroundWorkers
* LINGYUN.Abp.BackgroundJobs.Hangfire

## 7. 文档和资源

* [Hangfire官方文档](https://docs.hangfire.io/)
* [ABP框架文档](https://docs.abp.io/)
