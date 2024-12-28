# LINGYUN.Abp.TaskManagement.Application

The application layer implementation of the task management module, providing core functionality for background job management.

## Features

### Background Job Management Service
- Background Job Info Service (BackgroundJobInfoAppService)
  - Implements CRUD operations for jobs
  - Provides job control functions (start, stop, pause, resume, etc.)
  - Supports batch operations
  - Implements job querying and filtering

### Background Job Action Service (BackgroundJobActionAppService)
- Action Management Features:
  - Add job actions
  - Update job actions
  - Delete job actions
  - Get list of job actions
- Action Definition Management:
  - Get available action definitions
  - Action parameter configuration
  - Action enable/disable control

### Background Job Log Service (BackgroundJobLogAppService)
- Log Management Features:
  - Get log details
  - Get log list
  - Delete log records
- Log Query Features:
  - Support for multiple condition queries
  - Pagination
  - Sorting functionality
  - Advanced filtering

### Object Mapping Configuration
- AutoMapper Profile:
  - Mapping from BackgroundJobInfo to BackgroundJobInfoDto
  - Mapping from BackgroundJobLog to BackgroundJobLogDto
  - Mapping from BackgroundJobAction to BackgroundJobActionDto

### Module Configuration
- Dependencies:
  - AbpAutoMapper
  - AbpDynamicQueryable
  - TaskManagementDomain
  - TaskManagementApplication.Contracts
- Service Configuration:
  - Automatic object mapping configuration
  - Validation configuration

### Extended Features
- Expression Extensions:
  - AndIf conditional expression
  - OrIf conditional expression
- Dynamic query support
- Localization resource integration

## Usage

1. Add module dependency:
```csharp
[DependsOn(typeof(TaskManagementApplicationModule))]
public class YourModule : AbpModule
{
    // ...
}
```

2. Inject and use services:
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
