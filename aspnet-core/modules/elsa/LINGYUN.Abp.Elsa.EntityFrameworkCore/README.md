# LINGYUN.Abp.Elsa.EntityFrameworkCore

Elsa工作流的EntityFrameworkCore集成模块

## 功能

* 提供Elsa工作流的EntityFrameworkCore集成
* 依赖于 `AbpElsaModule` 和 `AbpEntityFrameworkCoreModule`

## 配置使用

```csharp
[DependsOn(
    typeof(AbpElsaEntityFrameworkCoreModule)
    )]
public class YouProjectModule : AbpModule
{
}
```

## 数据库提供程序

本模块是数据库提供程序无关的基础模块，你需要根据实际使用的数据库选择以下对应的模块：

* [SqlServer](../LINGYUN.Abp.Elsa.EntityFrameworkCore.SqlServer/README.md)
* [PostgreSql](../LINGYUN.Abp.Elsa.EntityFrameworkCore.PostgreSql/README.md)
* [MySql](../LINGYUN.Abp.Elsa.EntityFrameworkCore.MySql/README.md)

[English](./README.EN.md)
