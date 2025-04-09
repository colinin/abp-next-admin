# PackageName.CompanyName.ProjectName

[English](README.md) | [中文](README.zh-CN.md)

## 快速启动指南

本指南将帮助您快速设置和运行项目。请按照以下步骤开始。

### 前提条件

- .NET SDK 9.0 或更高版本
- 支持的数据库（SQL Server、MySQL、PostgreSQL、Oracle 或 SQLite）
- PowerShell 7.0+（推荐用于运行迁移脚本）

### 第一步：还原和构建项目

```bash
# 导航到项目根目录
cd /path/to/project

# 还原依赖项
dotnet restore

# 构建解决方案
dotnet build
```

### 第二步：创建数据库结构

使用 Migrate.ps1 脚本创建数据库表结构：

```powershell
# 导航到 migrations 目录
cd migrations

# 运行迁移脚本
./Migrate.ps1
```

该脚本将：

1. 检测项目中可用的 DbContext 类
2. 要求您选择用于迁移的 DbContext
3. 提示输入迁移名称
4. 创建迁移
5. 可选地为迁移生成 SQL 脚本

### 第三步：初始化种子数据

运行 DbMigrator 项目来初始化种子数据：

```bash
# 导航到 DbMigrator 项目目录
cd migrations/PackageName.CompanyName.ProjectName.AIO.DbMigrator

# 运行 DbMigrator 项目
dotnet run
```

DbMigrator 将：

1. 应用所有数据库迁移
2. 初始化种子数据（用户、角色等）
3. 如适用，设置租户配置

### 第四步：启动应用程序

成功设置数据库后，您可以运行 host 项目：

```bash
# 导航到 host 项目目录
cd host/PackageName.CompanyName.ProjectName.AIO.Host

# 运行 host 项目
dotnet run --launch-profile "PackageName.CompanyName.ProjectName.Development"
```

应用程序将启动并可通过配置的 URL 访问（通常是 [https://localhost:44300](https://localhost:44300)）。

## 基于数据库的单元测试

要运行基于数据库的单元测试，请按照以下步骤操作：

### 第一步：准备测试数据库

在运行测试之前，确保测试数据库存在。测试数据库连接字符串在 `ProjectNameEntityFrameworkCoreTestModule.cs` 文件中定义。

默认连接字符串是：

```csharp
private const string DefaultPostgresConnectionString =
    "Host=127.0.0.1;Port=5432;Database=test_db;User Id=postgres;Password=postgres;";
```

您可以手动创建此数据库或修改连接字符串以使用现有数据库。

### 第二步：配置测试环境

如需修改 `ProjectNameEntityFrameworkCoreTestModule.cs` 中的连接字符串：

```csharp
// 您也可以设置环境变量来覆盖默认连接字符串
var connectionString = Environment.GetEnvironmentVariable("TEST_CONNECTION_STRING") ??
                       DefaultPostgresConnectionString;
```

### 第三步：运行测试

运行 Application.Tests 项目：

```bash
# 导航到测试项目目录
cd tests/PackageName.CompanyName.ProjectName.Application.Tests

# 运行测试
dotnet test
```

测试框架将：

1. 创建清洁的测试数据库环境
2. 运行所有单元测试
3. 报告测试结果

## 关于命名的说明

这是一个模板项目，所以所有项目名称包含的占位符在使用模板创建新项目时将被替换：

- `PackageName` 将被替换为您的包名
- `CompanyName` 将被替换为您的公司名
- `ProjectName` 将被替换为您的项目名

当从此模板创建新项目时，您将指定这些值，它们将在整个解决方案中进行替换。
