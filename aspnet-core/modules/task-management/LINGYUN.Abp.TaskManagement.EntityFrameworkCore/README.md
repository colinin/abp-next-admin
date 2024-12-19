# LINGYUN.Abp.TaskManagement.EntityFrameworkCore

任务管理模块的Entity Framework Core实现，提供数据库访问和持久化。

## 功能

### 数据库上下文
- TaskManagementDbContext用于管理数据库操作
- 可配置的表前缀和架构
- 支持多租户

### 实体配置
- 后台作业信息：
  - 表名：{prefix}BackgroundJobs
  - 对Name和Group建立索引
  - 带长度约束的属性
  - 支持额外属性
- 后台作业日志：
  - 表名：{prefix}BackgroundJobLogs
  - 对JobGroup和JobName建立索引
  - 带长度约束的属性
- 后台作业行为：
  - 表名：{prefix}BackgroundJobActions
  - 对Name建立索引
  - 支持参数的额外属性

### 仓储实现
- 后台作业信息仓储：
  - CRUD操作
  - 作业状态管理
  - 作业过滤和查询
  - 支持作业过期
  - 等待作业列表管理
  - 周期性任务管理
- 后台作业日志仓储：
  - 日志存储和检索
  - 日志过滤和查询
  - 分页支持
- 后台作业行为仓储：
  - 行为存储和检索
  - 参数管理

### 查询功能
- 动态排序
- 分页
- 按规范过滤
- 异步操作
- 只读操作的无跟踪查询

### 性能优化
- 高效索引
- 支持批量操作
- 针对作业状态的优化查询

### 多租户支持
- 租户特定的数据隔离
- 跨租户操作
- 租户感知的仓储

### 集成功能
- ABP框架集成
- Entity Framework Core约定
- 复杂类型的值转换器
- 额外属性支持
