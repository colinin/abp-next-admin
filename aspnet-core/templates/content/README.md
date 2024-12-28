# LINGYUN.Abp.MicroService.Template

[English](README.md) | [中文](README.zh-CN.md)

## Introduction

LINGYUN.Abp.MicroService.Template is a microservice project template based on ABP Framework. This template provides a complete microservice architecture foundation, including necessary project structure and configurations.

## Features

- Complete microservice project structure
- Integrated authentication
- Database integration (multiple databases supported)
- Unified configuration management
- Distributed event bus support
- Background job processing

## How to Use

### Install Template

```bash
dotnet new install LINGYUN.Abp.MicroService.Template
```

### Create New Project

```bash
labp create YourCompanyName.YourProjectName -pk YourPackageName -o /path/to/output --dbms MySql --cs "Server=127.0.0.1;Database=YourDatabase;User Id=your_user;Password=your_password;SslMode=None" --no-random-port
```

## How to Run

After creating your project, you can run it using the following command:

```bash
dotnet run --launch-profile "YourPackageName.YourCompanyName.YourProjectName.Development"
```

For example:
```bash
dotnet run --launch-profile "LY.MicroService.Applications.Single.Development"
```

## How to Package and Publish

1. Clone the Project

```bash
git clone <repository-url>
cd <repository-path>/aspnet-core/templates/content
```

2. Modify Version
   Edit `../PackageName.CompanyName.ProjectName.csproj` file, update PackageVersion:

```xml
<PackageVersion>8.3.0</PackageVersion>
```

3. Execute Packaging Script

```powershell
# Windows PowerShell
.\pack.ps1

# PowerShell Core (Windows/Linux/macOS)
pwsh pack.ps1
```

## Supported Databases

- SqlServer
- MySQL
- PostgreSQL
- Oracle
- SQLite

## Notes

- Ensure .NET SDK 8.0 or higher is installed
- Pay attention to NuGet publish address and key when packaging
- Complete testing is recommended before publishing
