# LINGYUN.Abp.Saas.Domain

SaaS domain layer module, defining core domain models, repository interfaces and domain services for tenants and editions.

## Core Features

* Tenant Entity: Contains tenant basic information, status, expiration time, etc.
* Edition Entity: Contains edition basic information
* TenantConnectionString: Manages tenant database connections
* TenantManager: Handles tenant-related business logic
* EditionManager: Handles edition-related business logic
* EditionDataSeeder: Provides default edition data initialization

## Domain Events

* Tenant creation, update, deletion events
* Edition creation, update, deletion events
* Tenant connection string change events

## Cache Management

* Tenant Cache: Cache tenant information to improve query performance
* Edition Cache: Cache edition information to improve query performance

## More

[简体中文](README.md)
