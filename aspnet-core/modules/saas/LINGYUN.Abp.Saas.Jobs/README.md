# LINGYUN.Abp.Saas.Jobs

SaaS后台作业模块，提供租户资源监控和管理的后台作业实现。

## 功能特性

* 租户使用监控作业(TenantUsageMonitoringJob)
  * 监控租户资源使用情况
  * 处理租户过期预警
  * 处理过期租户资源回收

## 配置说明

### 作业配置

```json
{
  "Hangfire": {
    "TenantUsageMonitoring": {
      "CronExpression": "0 0 * * *",  // 每天执行一次
      "Queue": "default",              // 作业队列
      "Enabled": true                  // 是否启用
    }
  }
}
```

### 作业参数

* Saas:AdminEmail：管理员邮件地址，用于接收预警通知
* Saas:TenantId：租户标识

## 更多

[English](README.EN.md)
