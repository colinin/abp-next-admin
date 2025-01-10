# LINGYUN.Abp.BackgroundJobs.Hangfire

[English](README.EN.md) | 简体中文

## 1. 介绍

`LINGYUN.Abp.BackgroundJobs.Hangfire` 是基于 [Hangfire](https://www.hangfire.io/) 实现的ABP后台作业模块。它提供了一个可靠的后台作业执行框架，支持即时任务、延迟任务和周期性任务的执行。

## 2. 功能特性

* 支持即时任务执行
* 支持延迟任务执行
* 支持周期性任务（使用Cron表达式）
* 与ABP后台作业系统无缝集成
* 支持作业执行状态跟踪
* 支持作业重试机制

## 3. 安装

```bash
dotnet add package LINGYUN.Abp.BackgroundJobs.Hangfire
```

## 4. 使用方法

1. 添加 `AbpBackgroundJobsHangfireModule` 到模块依赖中:

```csharp
[DependsOn(typeof(AbpBackgroundJobsHangfireModule))]
public class YourModule : AbpModule
{
}
```

2. 配置后台作业:

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    Configure<AbpBackgroundJobOptions>(options =>
    {
        options.IsJobExecutionEnabled = true; // 启用作业执行
    });
}
```

3. 使用后台作业:

```csharp
public class YourService
{
    private readonly IBackgroundJobManager _backgroundJobManager;

    public YourService(IBackgroundJobManager backgroundJobManager)
    {
        _backgroundJobManager = backgroundJobManager;
    }

    public async Task CreateJobAsync()
    {
        // 创建即时任务
        await _backgroundJobManager.EnqueueAsync(new YourArgs());

        // 创建延迟任务
        await _backgroundJobManager.EnqueueAsync(
            new YourArgs(),
            delay: TimeSpan.FromMinutes(5)
        );

        // 创建周期性任务
        await _backgroundJobManager.EnqueueAsync(
            "0 0 * * *", // Cron表达式：每天0点执行
            new YourArgs()
        );
    }
}
```

## 5. 配置项

### 5.1 作业配置

* `AbpBackgroundJobOptions.IsJobExecutionEnabled`: 是否启用作业执行。默认值: `true`

## 6. 依赖项

* Volo.Abp.BackgroundJobs
* Hangfire.Core
* Hangfire.AspNetCore

## 7. 文档和资源

* [Hangfire官方文档](https://docs.hangfire.io/)
* [ABP框架文档](https://docs.abp.io/)
