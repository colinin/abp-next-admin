# LINGYUN.Abp.Serilog.Enrichers.UniqueId

简体中文 | [English](./README.EN.md)

日志属性添加唯一标识，使用雪花算法生成唯一ID。

## 功能特性

* 为每个日志事件添加唯一标识
* 基于雪花算法（Snowflake）生成分布式唯一ID
* 支持自定义雪花算法配置
* 支持代码配置和JSON配置两种方式
* 与Serilog无缝集成

## 模块引用

```csharp
[DependsOn(typeof(AbpSerilogEnrichersUniqueIdModule))]
public class YouProjectModule : AbpModule
{
  // other
}
```

## 配置项

### 常量配置

* AbpSerilogUniqueIdConsts.UniqueIdPropertyName - 唯一标识字段的名称，默认为"UniqueId"

### 雪花算法配置

通过 `AbpSerilogEnrichersUniqueIdOptions` 配置雪花算法参数：

```json
{
  "UniqueId": {
    "Snowflake": {
      "WorkerId": 1,        // 工作机器ID
      "DatacenterId": 1,    // 数据中心ID
      "Sequence": 0,        // 序列号
      "BaseTime": "2020-01-01 00:00:00"  // 基准时间
    }
  }
}
```

## 使用方法

### 代码配置方式

```csharp
Log.Logger = new LoggerConfiguration()
    .Enrich.WithUniqueId()
    // ...其他配置...
    .CreateLogger();
```

### JSON配置方式

```json
{
   "Serilog": {
    "MinimumLevel": {
      "Default": "Information"
    },
    "Enrich": [ "WithUniqueId" ]
  }
}
```

## 实现细节

该enricher使用雪花算法为每个日志事件生成一个唯一的ID。雪花算法的特点是：

* 生成的ID是64位的长整型
* ID由时间戳、数据中心ID、工作机器ID和序列号组成
* 保证在分布式环境下的唯一性
* 基于时间戳的有序性

## 最佳实践

1. 合理配置工作机器ID和数据中心ID，避免在分布式环境中产生冲突：
```json
{
  "UniqueId": {
    "Snowflake": {
      "WorkerId": 1,
      "DatacenterId": 1
    }
  }
}
```

2. 设置合适的基准时间以最大化ID的可用时间范围：
```json
{
  "UniqueId": {
    "Snowflake": {
      "BaseTime": "2020-01-01 00:00:00"
    }
  }
}
```

3. 在日志查询时可以使用UniqueId字段进行精确定位。

## 注意事项

1. 雪花算法生成的ID是趋势递增的，但不保证严格递增
2. 工作机器ID和数据中心ID在集群中必须唯一
3. 基准时间一旦设置就不能修改，否则可能导致ID重复
4. 每个日志事件都会生成新的唯一ID，这可能会增加一些性能开销
