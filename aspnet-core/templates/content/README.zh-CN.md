# LINGYUN.Abp.MicroService.Template

[English](README.md) | [中文](README.zh-CN.md)

## 简介

LINGYUN.Abp.MicroService.Template 是一个基于 ABP Framework 的微服务项目模板。此模板提供了一个完整的微服务架构基础，包括必要的项目结构和配置。

## 功能特点

- 完整的微服务项目结构
- 集成身份认证
- 数据库集成（支持多种数据库）
- 统一的配置管理
- 分布式事件总线支持
- 后台任务处理

## 如何使用

### 安装模板

```bash
dotnet new install LINGYUN.Abp.MicroService.Template
```

### 创建新项目

```bash
labp create YourCompanyName.YourProjectName -pk YourPackageName -o /path/to/output --dbms MySql --cs "Server=127.0.0.1;Database=YourDatabase;User Id=your_user;Password=your_password;SslMode=None" --no-random-port
```

## 如何运行

创建项目后，您可以使用以下命令运行项目：

```bash
dotnet run --launch-profile "YourPackageName.YourCompanyName.YourProjectName.Development"
```

示例：
```bash
dotnet run --launch-profile "LY.MicroService.Applications.Single.Development"
```

## 如何打包发布

1. 克隆项目

```bash
git clone <repository-url>
cd <repository-path>/aspnet-core/templates/content
```

2. 修改版本号
   编辑 `../PackageName.CompanyName.ProjectName.csproj` 文件，更新 PackageVersion：

```xml
<PackageVersion>8.3.0</PackageVersion>
```

3. 执行打包脚本

```powershell
# Windows PowerShell
.\pack.ps1

# PowerShell Core (Windows/Linux/macOS)
pwsh pack.ps1
```

## 支持的数据库

- SqlServer
- MySQL
- PostgreSQL
- Oracle
- SQLite

## 注意事项

- 确保已安装 .NET SDK 8.0 或更高版本
- 打包时注意修改 NuGet 发布地址和密钥
- 建议在发布前进行完整的测试
