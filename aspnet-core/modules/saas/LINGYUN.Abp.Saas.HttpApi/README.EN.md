# LINGYUN.Abp.Saas.HttpApi

SaaS HTTP API module, implementing HTTP API interfaces for tenant and edition management.

## Features

* Tenant Management Controller (TenantController)
  * GET /api/saas/tenants: Get tenant list
  * GET /api/saas/tenants/{id}: Get tenant details
  * POST /api/saas/tenants: Create tenant
  * PUT /api/saas/tenants/{id}: Update tenant
  * DELETE /api/saas/tenants/{id}: Delete tenant
  * GET /api/saas/tenants/{id}/connection-strings: Get tenant connection strings
  * PUT /api/saas/tenants/{id}/connection-strings: Update tenant connection strings
  * DELETE /api/saas/tenants/{id}/connection-strings/{name}: Delete tenant connection string

* Edition Management Controller (EditionController)
  * GET /api/saas/editions: Get edition list
  * GET /api/saas/editions/{id}: Get edition details
  * POST /api/saas/editions: Create edition
  * PUT /api/saas/editions/{id}: Update edition
  * DELETE /api/saas/editions/{id}: Delete edition

## API Documentation

All API interfaces support Swagger documentation. After starting the application, you can view detailed API documentation through Swagger UI.

## More

[简体中文](README.md)
