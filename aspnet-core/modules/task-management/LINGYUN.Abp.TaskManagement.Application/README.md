# LINGYUN.Abp.TaskManagement.Application

任务管理模块的应用层实现，提供后台作业管理的核心功能实现。

## 功能实现

### 后台作业管理服务
- 后台作业信息服务 (BackgroundJobInfoAppService)
  - 实现作业的CRUD操作
  - 提供作业控制功能（启动、停止、暂停、恢复等）
  - 支持批量操作功能
  - 实现作业查询和过滤

### 后台作业行为服务 (BackgroundJobActionAppService)
- 行为管理功能：
  - 添加作业行为
  - 更新作业行为
  - 删除作业行为
  - 获取作业行为列表
- 行为定义管理：
  - 获取可用的行为定义
  - 行为参数配置
  - 行为启用/禁用控制

### 后台作业日志服务 (BackgroundJobLogAppService)
- 日志管理功能：
  - 获取日志详情
  - 获取日志列表
  - 删除日志记录
- 日志查询功能：
  - 支持多条件组合查询
  - 分页查询
  - 排序功能
  - 高级过滤

### 对象映射配置
- AutoMapper配置文件：
  - BackgroundJobInfo到BackgroundJobInfoDto的映射
  - BackgroundJobLog到BackgroundJobLogDto的映射
  - BackgroundJobAction到BackgroundJobActionDto的映射

### 模块配置
- 依赖模块：
  - AbpAutoMapper
  - AbpDynamicQueryable
  - TaskManagementDomain
  - TaskManagementApplication.Contracts
- 服务配置：
  - 自动对象映射配置
  - 验证配置

### 扩展功能
- 表达式扩展：
  - AndIf条件表达式
  - OrIf条件表达式
- 动态查询支持
- 本地化资源集成

## 使用方式

1. 添加模块依赖：
```csharp
[DependsOn(typeof(TaskManagementApplicationModule))]
public class YourModule : AbpModule
{
    // ...
}
```

2. 注入并使用服务：
```csharp
public class YourService
{
    private readonly IBackgroundJobInfoAppService _jobInfoAppService;
    private readonly IBackgroundJobActionAppService _jobActionAppService;
    private readonly IBackgroundJobLogAppService _jobLogAppService;

    public YourService(
        IBackgroundJobInfoAppService jobInfoAppService,
        IBackgroundJobActionAppService jobActionAppService,
        IBackgroundJobLogAppService jobLogAppService)
    {
        _jobInfoAppService = jobInfoAppService;
        _jobActionAppService = jobActionAppService;
        _jobLogAppService = jobLogAppService;
    }
}
```
