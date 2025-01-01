# LINGYUN.Abp.TaskManagement.Domain.Shared

Domain shared module for task management, containing shared domain models, enums, and constants.

## Features

### Permission Management
- Task Management permissions
- Background job permissions
- Job action permissions
- Job log permissions

### Job Types and Status
- Job Types:
  - One-off jobs (executed once)
  - Periodic jobs (executed periodically)
  - Persistent jobs (executed repeatedly)
- Job Status:
  - None
  - Completed
  - Running
  - Queuing
  - Paused
  - Failed Retry
  - Stopped

### Priority Levels
- Low
- Below Normal
- Normal
- Above Normal
- High

### Job Properties
- Basic Information:
  - Group
  - Name
  - Description
  - Type
  - Status
  - Begin/End Time
- Execution Settings:
  - Interval (in seconds)
  - Cron expression
  - Lock timeout
  - Priority
  - Maximum trigger count
  - Maximum retry count
- Tracking Information:
  - Creation time
  - Last run time
  - Next run time
  - Trigger count
  - Try count
  - Execution result

### Localization
- Support for multiple languages
- Error code localization
- UI text localization

### Multi-tenancy Support
- Tenant-specific job management
- System-level job management

### Source Types
- User jobs
- System jobs
