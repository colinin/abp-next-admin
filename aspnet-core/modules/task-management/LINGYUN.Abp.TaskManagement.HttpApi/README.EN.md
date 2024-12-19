# LINGYUN.Abp.TaskManagement.HttpApi

HTTP API implementation for task management module, providing RESTful endpoints for managing background jobs.

## Features

### Background Job Info API
- CRUD operations:
  - Create new jobs
  - Get job details
  - Update job properties
  - Delete jobs
- Job control operations:
  - Start jobs
  - Stop jobs
  - Pause jobs
  - Resume jobs
  - Trigger jobs
- Batch operations:
  - Bulk start
  - Bulk stop
  - Bulk pause
  - Bulk resume
  - Bulk trigger
  - Bulk delete
- Query operations:
  - Get job list with pagination
  - Get job definitions
  - Filter and sort jobs

### Background Job Action API
- Action management:
  - Add actions to jobs
  - Update action properties
  - Delete actions
  - Get action list
- Action definitions:
  - Get available action definitions
  - Query action definitions

### Background Job Log API
- Log operations:
  - Get log details
  - Get log list with pagination
  - Delete logs
- Log filtering:
  - Filter by job
  - Filter by time range
  - Filter by status

### Authorization
- Permission-based access control:
  - Create job permission
  - Update job permission
  - Delete job permission
  - Trigger job permission
  - Pause job permission
  - Resume job permission
  - Start job permission
  - Stop job permission
  - Delete log permission

### API Features
- RESTful endpoints
- HTTP method-based operations
- Route-based API versioning
- Standardized response formats
- Pagination support
- Dynamic filtering and sorting

### Localization
- Multi-language support
- Localized error messages
- Localized validation messages

### Integration
- ABP Framework integration
- MVC integration
- Dynamic query support
- Validation support
