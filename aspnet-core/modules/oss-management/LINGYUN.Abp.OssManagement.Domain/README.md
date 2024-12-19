# LINGYUN.Abp.OssManagement.Domain

对象存储管理模块领域层。

## 功能

* 提供对象存储管理的核心领域模型和业务逻辑
* 定义对象存储容器和对象的基本操作接口
* 提供文件处理和验证的核心逻辑
* 支持文件分片上传和断点续传
* 支持多种存储提供程序的扩展

## 配置项

### AbpOssManagementOptions

* StaticBuckets: 静态容器列表，这些容器不允许被删除
* IsCleanupEnabled: 是否启用清理功能，默认：true
* CleanupPeriod: 清理周期，默认：3,600,000 ms
* DisableTempPruning: 是否禁用缓存目录清除作业，默认：false
* MaximumTempSize: 每批次清理数量，默认：100
* MinimumTempLifeSpan: 最小缓存对象寿命，默认：30分钟
* Processers: 文件流处理器列表

## 链接

* [English documentation](./README.EN.md)
* [模块说明](../README.md)
