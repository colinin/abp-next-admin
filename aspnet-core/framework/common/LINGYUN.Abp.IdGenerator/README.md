# LINGYUN.Abp.IdGenerator

## 介绍

`LINGYUN.Abp.IdGenerator` 是一个分布式ID生成器模块，主要实现了雪花算法（Snowflake）来生成分布式唯一ID。

## 功能

* 雪花算法ID生成器 (`SnowflakeIdGenerator`)
  * 支持自定义工作机器ID和数据中心ID
  * 支持自定义序列号位数
  * 支持时间回退处理
  * 提供分布式环境下的唯一ID生成

## 配置项

### SnowflakeIdOptions

* `WorkerIdBits` (默认: 5) - 工作机器ID位数
* `DatacenterIdBits` (默认: 5) - 数据中心ID位数
* `Sequence` (默认: 0) - 序列号起始值
* `SequenceBits` (默认: 12) - 序列号位数
* `UsePreviousInTimeRollback` (默认: true) - 是否在时间回退时使用上一个时间戳
* `WorkerId` - 工作机器ID，如未指定则从环境变量WORKERID获取或随机生成
* `DatacenterId` - 数据中心ID，如未指定则从环境变量DATACENTERID获取或随机生成

## 安装

```bash
dotnet add package LINGYUN.Abp.IdGenerator
```

## 使用

1. 添加 `[DependsOn(typeof(AbpIdGeneratorModule))]` 到你的模块类上。

```csharp
[DependsOn(typeof(AbpIdGeneratorModule))]
public class YourModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<SnowflakeIdOptions>(options =>
        {
            options.WorkerId = 1;
            options.DatacenterId = 1;
        });
    }
}
```

2. 注入并使用ID生成器：

```csharp
public class YourService
{
    private readonly IDistributedIdGenerator _idGenerator;

    public YourService(IDistributedIdGenerator idGenerator)
    {
        _idGenerator = idGenerator;
    }

    public long CreateId()
    {
        return _idGenerator.Create();
    }
}
```

## 链接

* [English document](./README.EN.md)
