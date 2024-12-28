# LINGYUN.Abp.TaskManagement.Domain

Domain module for task management, implementing core business logic and domain models.

## Features

### Background Job Management
- Job lifecycle management (create, update, delete)
- Job status control (start, stop, pause, resume, trigger)
- Support for different job types:
  - One-off jobs
  - Periodic jobs (with cron expressions)
  - Persistent jobs (with intervals)

### Job Store
- Store job information and execution status
- Track job execution history
- Clean up expired jobs
- Support for multi-tenancy

### Job Synchronization
- Synchronize job status across distributed systems
- Handle job creation, update, and deletion events
- Maintain job queue consistency

### Job Actions
- Manage job-related actions
- Store action parameters
- Enable/disable actions

### Job Filtering and Specifications
- Filter jobs by multiple criteria:
  - Type
  - Group
  - Name
  - Status
  - Priority
  - Source
  - Creation time
  - Last run time
- Support for complex job queries

### Job Logging
- Log job execution details
- Track execution results and exceptions
- Support for multi-tenancy in logging

### Domain Events
- Job status change events
- Job execution events
- Distributed event handling

### Job Priority Management
- Support multiple priority levels:
  - Low
  - Below Normal
  - Normal
  - Above Normal
  - High

### Job Source Management
- Support different job sources:
  - User jobs
  - System jobs

### Multi-tenancy Support
- Tenant-specific job management
- Cross-tenant job operations
- Tenant isolation in job execution

### Domain Services
- Background job manager
- Job store service
- Job action service
- Job log service
