# PostgreSQL 数据库迁移指南

本指南将帮助您使用迁移脚本来管理 PostgreSQL 数据库的迁移操作。

## 前置条件

1. 确保已安装 .NET Core SDK
2. 确保已安装 Entity Framework Core 工具
   ```powershell
   dotnet tool install --global dotnet-ef
   ```
3. 确保已正确配置 PostgreSQL 连接字符串

## 使用说明

### 1. 创建新的迁移

1. 在 `aspnet-core/migrations` 目录下运行迁移脚本：
   ```powershell
   # 使用中文版本
   .\Migrate.ps1
   
   # 或使用英文版本
   .\MigrateEn.ps1
   ```

2. 在菜单中选择 PostgreSQL 数据库上下文：
   ```
   [2] LY.MicroService.Applications.Single.EntityFrameworkCore.PostgreSql
   ```

3. 输入迁移名称（可选）：
   - 直接回车将使用默认名称：`AddNewMigration_yyyyMMdd_HHmmss`
   - 或输入自定义名称，如：`AddNewFeature`

### 2. 生成 SQL 脚本

在创建迁移后，脚本会询问是否需要生成 SQL 脚本：

1. 选择是否生成 SQL 脚本 (Y/N)
2. 如果选择 Y，将提供以下选项：
   - `[A]` - 生成所有迁移的 SQL 脚本
   - `[L]` - 仅生成最新迁移的 SQL 脚本
   - `[0-9]` - 从指定的迁移版本开始生成

生成的 SQL 脚本将保存在：
```
aspnet-core/InitSql/LY.MicroService.Applications.Single.EntityFrameworkCore.PostgreSql/
```

### 3. 应用迁移

生成的 SQL 脚本可以通过以下方式应用到数据库：

1. 使用 pgAdmin 或其他 PostgreSQL 客户端工具执行 SQL 脚本
2. 或使用命令行：
   ```bash
   psql -U your_username -d your_database -f your_script.sql
   ```
