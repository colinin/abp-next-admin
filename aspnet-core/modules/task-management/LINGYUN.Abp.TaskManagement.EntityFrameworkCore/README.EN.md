# LINGYUN.Abp.TaskManagement.EntityFrameworkCore

Entity Framework Core implementation for task management module, providing database access and persistence.

## Features

### Database Context
- TaskManagementDbContext for managing database operations
- Configurable table prefix and schema
- Support for multi-tenancy

### Entity Configurations
- Background Job Info:
  - Table name: {prefix}BackgroundJobs
  - Indexes on Name and Group
  - Properties with length constraints
  - Extra properties support
- Background Job Log:
  - Table name: {prefix}BackgroundJobLogs
  - Indexes on JobGroup and JobName
  - Properties with length constraints
- Background Job Action:
  - Table name: {prefix}BackgroundJobActions
  - Index on Name
  - Extra properties support for parameters

### Repository Implementations
- Background Job Info Repository:
  - CRUD operations
  - Job status management
  - Job filtering and querying
  - Support for job expiration
  - Waiting job list management
  - Period task management
- Background Job Log Repository:
  - Log storage and retrieval
  - Log filtering and querying
  - Pagination support
- Background Job Action Repository:
  - Action storage and retrieval
  - Parameter management

### Query Features
- Dynamic sorting
- Pagination
- Filtering by specifications
- Asynchronous operations
- No-tracking queries for read-only operations

### Performance Optimizations
- Efficient indexing
- Batch operations support
- Optimized queries for job status

### Multi-tenancy Support
- Tenant-specific data isolation
- Cross-tenant operations
- Tenant-aware repositories

### Integration Features
- ABP Framework integration
- Entity Framework Core conventions
- Value converters for complex types
- Extra properties support
