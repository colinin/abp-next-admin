# LINGYUN.Abp.Templates

[English](README.md) | [中文](README.zh-CN.md)

## 简介

LINGYUN.Abp.Templates 基于 ABP Framework 提供两种项目模板：

1. **微服务模板**：完整的分布式微服务架构模板
2. **单体应用模板**：将所有服务集成到一个项目中的单体应用模板

## 特性

### 共同特性

- 集成身份认证（支持 IdentityServer4/OpenIddict）
- 数据库集成（支持多种数据库）
- 统一配置管理
- 分布式事件总线支持
- 后台作业处理

### 微服务模板特性

- 完整的微服务项目结构
- 服务发现与注册
- 支持分布式部署

### 单体应用模板特性

- 简化的部署流程
- 更容易的维护
- 更低的资源需求

## 使用方法

### 安装模板

```bash
# 安装微服务模板
dotnet new install LINGYUN.Abp.MicroService.Templates

# 安装单体应用模板
dotnet new install LINGYUN.Abp.AllInOne.Templates
```

### 创建新项目

#### 创建微服务项目

```bash
# 简写名称：lam (LINGYUN Abp Microservice)
dotnet new lam -n YourCompanyName.YourProjectName -pk YourPackageName -o /path/to/output --dbms MySql --cs "Server=127.0.0.1;Database=YourDatabase;User Id=your_user;Password=your_password;SslMode=None" --no-random-port
```

#### 创建单体应用项目

```bash
# 简写名称：laa (LINGYUN Abp AllInOne)
labp create MyCompanyName.MyProjectName -pk MyPackageName -t laa -o /Users/feijie/Projects/Tests --dbms MySql --cs "Server=127.0.0.1;Database=Platform-V70;User Id=root;Password=123456;SslMode=None" --no-random-port
```

## 运行项目

创建项目后，可以使用以下命令运行：

### 运行微服务项目

```bash
cd /path/to/output/host/YourPackageName.YourCompanyName.YourProjectName.HttpApi.Host
dotnet run --launch-profile "YourPackageName.YourCompanyName.YourProjectName.Development"
```

### 运行单体应用项目

```bash
cd /path/to/output/host/YourPackageName.YourCompanyName.YourProjectName.AIO.Host
dotnet run --launch-profile "YourPackageName.YourCompanyName.YourProjectName.Development"
```

## 打包与发布

1. 克隆项目

```bash
git clone <repository-url>
cd <repository-path>/aspnet-core/templates/content
```

2. 修改版本号
   编辑项目文件更新版本号：
   - 微服务模板：`../PackageName.CompanyName.ProjectName.csproj`
   - 单体应用模板：`../PackageName.CompanyName.ProjectName.AIO.csproj`

```xml
<Version>8.3.0</Version>
```

3. 执行打包脚本

```powershell
# Windows PowerShell
.\pack.ps1

# PowerShell Core (Windows/Linux/macOS)
pwsh pack.ps1
```

脚本会提示您选择要打包的模板：

1. 微服务模板
2. 单体应用模板
3. 两种模板都打包

## 支持的数据库

- SqlServer
- MySQL
- PostgreSQL
- Oracle
- SQLite

## 注意事项

- 确保已安装 .NET SDK 8.0 或更高版本
- 根据需求选择合适的模板：
  - 微服务模板：适用于大规模分布式应用
  - 单体应用模板：适用于小型应用或简单部署需求
- 打包时注意 NuGet 发布地址和密钥
- 发布前建议进行完整测试
