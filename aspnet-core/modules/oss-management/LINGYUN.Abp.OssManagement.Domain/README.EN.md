# LINGYUN.Abp.OssManagement.Domain

Object Storage Management Module Domain Layer.

## Features

* Provides core domain models and business logic for object storage management
* Defines basic operation interfaces for object storage containers and objects
* Provides core logic for file processing and validation
* Supports file sharding upload and breakpoint continuation
* Supports extension of multiple storage providers

## Configuration

### AbpOssManagementOptions

* StaticBuckets: List of static containers that cannot be deleted
* IsCleanupEnabled: Whether to enable cleanup functionality, default: true
* CleanupPeriod: Cleanup period, default: 3,600,000 ms
* DisableTempPruning: Whether to disable cache directory cleanup job, default: false
* MaximumTempSize: Number of items to clean per batch, default: 100
* MinimumTempLifeSpan: Minimum cache object lifespan, default: 30 minutes
* Processers: List of file stream processors

## Links

* [中文文档](./README.md)
* [Module documentation](../README.md)
