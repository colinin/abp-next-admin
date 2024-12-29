# LY.MicroService.Applications.Single.DbMigrator

单体应用数据库迁移工具，用于自动执行数据库迁移和初始化数据。

[English](./README.EN.md)

## 功能特性

* 自动执行数据库迁移
* 支持多环境配置
* 集成Serilog日志记录
* 支持数据迁移环境配置
* 自动检查和应用数据库迁移
* 控制台应用程序，方便集成到CI/CD流程

## 配置项

```json
{
  "ConnectionStrings": {
    "Default": "你的数据库连接字符串"
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "Volo.Abp": "Warning"
      }
    }
  }
}
```

## 基本用法

1. 配置数据库连接
   * 在appsettings.json中配置数据库连接字符串
   * 可以通过appsettings.{Environment}.json配置不同环境的连接字符串

2. 运行迁移工具
   ```bash
   dotnet run
   ```

3. 查看迁移日志
   * 控制台输出
   * Logs/migrations.txt文件

## 环境变量

* `ASPNETCORE_ENVIRONMENT`: 设置运行环境（Development、Staging、Production等）
* `DOTNET_ENVIRONMENT`: 同上，用于兼容性

## 注意事项

* 确保数据库连接字符串中包含足够的权限
* 建议在执行迁移前备份数据库
* 查看migrations.txt日志文件以了解迁移详情
* 如果迁移失败，检查日志中的错误信息

## 开发调试

1. 设置环境变量
   ```bash
   export ASPNETCORE_ENVIRONMENT=Development
   ```

2. 使用Visual Studio或Visual Studio Code进行调试
   * 可以设置断点
   * 查看详细的迁移过程

3. 自定义迁移逻辑
   * 修改SingleDbMigrationService类
   * 添加新的数据种子
