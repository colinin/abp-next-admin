# LINGYUN.Abp.DataProtectionManagement.HttpApi

Data protection management HTTP API module, providing REST API interfaces for data protection management.

## Features

* Data Protection Management REST API
  * Create Data Protection
  * Update Data Protection
  * Delete Data Protection
  * Query Data Protection

## API Controllers

* `DataProtectionController` - Data Protection Controller
  * `GET /api/data-protection-management/data-protection/{id}` - Get Specific Data Protection
  * `GET /api/data-protection-management/data-protection` - Get Data Protection List
  * `POST /api/data-protection-management/data-protection` - Create Data Protection
  * `PUT /api/data-protection-management/data-protection/{id}` - Update Data Protection
  * `DELETE /api/data-protection-management/data-protection/{id}` - Delete Data Protection

## Permission Validation

* All APIs require authentication
* Creating data protection requires `DataProtectionManagement.DataProtection.Create` permission
* Updating data protection requires `DataProtectionManagement.DataProtection.Update` permission
* Deleting data protection requires `DataProtectionManagement.DataProtection.Delete` permission
* Querying data protection requires `DataProtectionManagement.DataProtection` permission

## Related Links

* [中文文档](./README.md)
