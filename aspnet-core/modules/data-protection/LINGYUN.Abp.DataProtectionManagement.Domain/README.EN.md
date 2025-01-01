# LINGYUN.Abp.DataProtectionManagement.Domain

Data protection management domain module, providing core business logic for data protection management.

## Features

* Data Protection Management
  * Create Data Protection
  * Update Data Protection
  * Delete Data Protection
  * Query Data Protection
* Data Protection Resource Management
  * Resource Definition
  * Resource Grouping
  * Resource Properties
* Data Protection Rule Management
  * Rule Definition
  * Rule Grouping
  * Rule Operators

## Domain Services

* `IDataProtectionManager` - Data Protection Management Service
  * `CreateAsync` - Create Data Protection
  * `UpdateAsync` - Update Data Protection
  * `DeleteAsync` - Delete Data Protection
  * `GetAsync` - Get Data Protection
  * `GetListAsync` - Get Data Protection List

## Entities

* `DataProtection` - Data Protection Entity
  * `Id` - Primary Key
  * `Name` - Name
  * `DisplayName` - Display Name
  * `Description` - Description
  * `AllowProperties` - List of Allowed Properties
  * `FilterGroup` - Filter Rule Group

## Repositories

* `IDataProtectionRepository` - Data Protection Repository Interface
  * `GetListAsync` - Get Data Protection List
  * `FindByNameAsync` - Find Data Protection by Name
  * `GetCountAsync` - Get Data Protection Count

## Related Links

* [中文文档](./README.md)
