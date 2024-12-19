# LINGYUN.Abp.DataProtectionManagement.Application

Data protection management application service module, providing application service implementation for data protection management.

## Features

* Data Protection Management Application Service
  * Create Data Protection
  * Update Data Protection
  * Delete Data Protection
  * Query Data Protection
* Auto Mapping Configuration
* Permission Validation

## Application Service Implementation

* `DataProtectionAppService` - Data Protection Application Service
  * Implements `IDataProtectionAppService` interface
  * Provides CRUD operations for data protection
  * Includes permission validation
  * Includes data validation

## Auto Mapping Configuration

* `DataProtectionManagementApplicationAutoMapperProfile` - Auto Mapping Configuration Profile
  * `DataProtection` -> `DataProtectionDto`
  * `DataProtectionCreateDto` -> `DataProtection`
  * `DataProtectionUpdateDto` -> `DataProtection`

## Permission Validation

* Creating data protection requires `DataProtectionManagement.DataProtection.Create` permission
* Updating data protection requires `DataProtectionManagement.DataProtection.Update` permission
* Deleting data protection requires `DataProtectionManagement.DataProtection.Delete` permission
* Querying data protection requires `DataProtectionManagement.DataProtection` permission

## Related Links

* [中文文档](./README.md)
