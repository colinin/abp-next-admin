# LINGYUN.Abp.DataProtectionManagement.Application.Contracts

Data protection management application service contracts module, providing application service interfaces and DTOs for data protection management.

## Features

* Data Protection Management Application Service Interfaces
* Data Protection DTO Definitions
* Data Protection Query Definitions

## Application Service Interfaces

* `IDataProtectionAppService` - Data Protection Application Service Interface
  * `GetAsync` - Get Data Protection
  * `GetListAsync` - Get Data Protection List
  * `CreateAsync` - Create Data Protection
  * `UpdateAsync` - Update Data Protection
  * `DeleteAsync` - Delete Data Protection

## DTO Definitions

* `DataProtectionDto` - Data Protection DTO
  * `Id` - Primary Key
  * `Name` - Name
  * `DisplayName` - Display Name
  * `Description` - Description
  * `AllowProperties` - List of Allowed Properties
  * `FilterGroup` - Filter Rule Group

* `DataProtectionCreateDto` - Create Data Protection DTO
* `DataProtectionUpdateDto` - Update Data Protection DTO
* `DataProtectionGetListInput` - Get Data Protection List Input DTO

## Related Links

* [中文文档](./README.md)
