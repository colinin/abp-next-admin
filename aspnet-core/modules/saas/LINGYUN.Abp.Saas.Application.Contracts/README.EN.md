# LINGYUN.Abp.Saas.Application.Contracts

SaaS application service contract module, defining application service interfaces, DTO objects and permission definitions for tenant and edition management.

## Features

* Tenant Management Interface (ITenantAppService)
  * Defines all service interfaces for tenant management
  * Contains tenant-related DTO object definitions
  * Tenant connection string management interface

* Edition Management Interface (IEditionAppService)
  * Defines all service interfaces for edition management
  * Contains edition-related DTO object definitions

* Permission Definition (AbpSaasPermissions)
  * Edition management permissions
  * Tenant management permissions
  * Feature management permissions
  * Connection string management permissions

* DTO Objects
  * EditionCreateDto/EditionUpdateDto
  * TenantCreateDto/TenantUpdateDto
  * TenantConnectionStringCreateDto/TenantConnectionStringUpdateDto

## More

[简体中文](README.md)
