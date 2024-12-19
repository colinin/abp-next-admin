# LINGYUN.Abp.Saas.Application

SaaS application service layer module, implementing application service interfaces for tenant and edition management.

## Features

* Tenant Management Service (TenantAppService)
  * Create tenant
  * Update tenant
  * Delete tenant
  * Get tenant list
  * Get tenant details
  * Manage tenant connection strings
  * Manage tenant features

* Edition Management Service (EditionAppService)
  * Create edition
  * Update edition
  * Delete edition
  * Get edition list
  * Get edition details
  * Manage edition features

## Permission Validation

All application service methods have added corresponding permission validation to ensure that only users with corresponding permissions can access.

## Object Mapping

The following object mappings are implemented using AutoMapper:
* Edition <-> EditionDto
* Tenant <-> TenantDto
* TenantConnectionString <-> TenantConnectionStringDto

## More

[简体中文](README.md)
