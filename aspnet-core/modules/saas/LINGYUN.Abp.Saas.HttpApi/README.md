# LINGYUN.Abp.Saas.HttpApi

SaaS HTTP API模块，实现了租户和版本管理的HTTP API接口。

## 功能特性

* 租户管理Controller(TenantController)
  * GET /api/saas/tenants：获取租户列表
  * GET /api/saas/tenants/{id}：获取租户详情
  * POST /api/saas/tenants：创建租户
  * PUT /api/saas/tenants/{id}：更新租户
  * DELETE /api/saas/tenants/{id}：删除租户
  * GET /api/saas/tenants/{id}/connection-strings：获取租户连接字符串
  * PUT /api/saas/tenants/{id}/connection-strings：更新租户连接字符串
  * DELETE /api/saas/tenants/{id}/connection-strings/{name}：删除租户连接字符串

* 版本管理Controller(EditionController)
  * GET /api/saas/editions：获取版本列表
  * GET /api/saas/editions/{id}：获取版本详情
  * POST /api/saas/editions：创建版本
  * PUT /api/saas/editions/{id}：更新版本
  * DELETE /api/saas/editions/{id}：删除版本

## API文档

所有API接口都支持Swagger文档，启动应用后可以通过Swagger UI查看详细的API文档。

## 更多

[English](README.EN.md)
