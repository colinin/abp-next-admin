# LINGYUN.Abp.TaskManagement.Application.Contracts

Application contracts for task management module, defining interfaces and DTOs for application services.

## Features

### Application Service Interfaces
- Background Job Info Service:
  - CRUD operations
  - Job control operations
  - Batch operations
  - Query operations
- Background Job Action Service:
  - Action management
  - Action definitions
  - Parameter handling
- Background Job Log Service:
  - Log retrieval
  - Log filtering
  - Log deletion

### Data Transfer Objects (DTOs)
- Background Job Info DTOs:
  - Job info DTO
  - Job creation DTO
  - Job update DTO
  - Job list DTO
  - Job batch input DTO
- Background Job Action DTOs:
  - Action DTO
  - Action creation DTO
  - Action update DTO
  - Action definition DTO
  - Action parameter DTO
- Background Job Log DTOs:
  - Log DTO
  - Log list DTO
  - Log filter DTO

### Permissions
- Background Jobs:
  - Create permission
  - Update permission
  - Delete permission
  - Trigger permission
  - Pause permission
  - Resume permission
  - Start permission
  - Stop permission
- Background Job Logs:
  - View permission
  - Delete permission

### Remote Service Configuration
- Service name constants
- Service endpoint configuration
- Client proxy settings

### Validation
- Input validation
- Data annotations
- Custom validation rules

### Integration Features
- ABP Framework integration
- Dynamic query support
- Application service layer abstraction

### Module Configuration
- Module dependencies
- Service registration
- Feature management
