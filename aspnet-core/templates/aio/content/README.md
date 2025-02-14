# LINGYUN.Abp.Templates

[English](README.md) | [中文](README.zh-CN.md)

## Introduction

LINGYUN.Abp.Templates provides two types of project templates based on ABP Framework:

1. **Microservice Template**: A complete microservice architecture template with distributed services.
2. **All-in-One Template**: A single-application template that combines all services into one project.

## Features

### Common Features

- Integrated authentication (IdentityServer4/OpenIddict)
- Database integration (multiple databases supported)
- Unified configuration management
- Distributed event bus support
- Background job processing

### Microservice Template Features

- Complete microservice project structure
- Service discovery and registration
- Distributed deployment support

### All-in-One Template Features

- Simplified deployment
- Easier maintenance
- Lower resource requirements

## How to Use

### Install labp CLI Tool

```bash
dotnet tool install --global LINGYUN.Abp.Cli
```

### Install Templates

```bash
# Install Microservice Template
dotnet new install LINGYUN.Abp.MicroService.Templates

# Install All-in-One Template
dotnet new install LINGYUN.Abp.AllInOne.Templates
```

### Create New Project

#### For Microservice Project

```bash
# Short name: lam (LINGYUN Abp Microservice)
labp create YourCompanyName.YourProjectName -pk YourPackageName -t lam -o /path/to/output --dbms MySql --cs "Server=127.0.0.1;Database=Platform-V70;User Id=root;Password=123456;SslMode=None" --no-random-port
```

#### For All-in-One Project

```bash
# Short name: laa (LINGYUN Abp AllInOne)
labp create YourCompanyName.YourProjectName -pk YourPackageName -t laa -o /path/to/output --dbms MySql --cs "Server=127.0.0.1;Database=Platform-V70;User Id=root;Password=123456;SslMode=None" --no-random-port
```

## How to Run

After creating your project, you can run it using the following command:

### For Microservice Project

```bash
cd /path/to/output/host/YourPackageName.YourCompanyName.YourProjectName.HttpApi.Host
dotnet run --launch-profile "YourPackageName.YourCompanyName.YourProjectName.Development"
```

### For All-in-One Project

```bash
cd /path/to/output/host/YourPackageName.YourCompanyName.YourProjectName.AIO.Host
dotnet run --launch-profile "YourPackageName.YourCompanyName.YourProjectName.Development"
```

## How to Package and Publish

1. Clone the Project

```bash
git clone <repository-url>
cd <repository-path>/aspnet-core/templates/content
```

2. Modify Version
   Edit the project files to update versions:
   - For Microservice: `../PackageName.CompanyName.ProjectName.csproj`
   - For All-in-One: `../PackageName.CompanyName.ProjectName.AIO.csproj`

```xml
<Version>8.3.0</Version>
```

3. Execute Packaging Script

```powershell
# Windows PowerShell
.\pack.ps1

# PowerShell Core (Windows/Linux/macOS)
pwsh pack.ps1
```

The script will prompt you to choose which template to package:

1. Microservice Template
2. All-in-One Template
3. Both Templates

## Supported Databases

- SqlServer
- MySQL
- PostgreSQL
- Oracle
- SQLite

## Notes

- Ensure .NET SDK 8.0 or higher is installed
- Choose the appropriate template based on your needs:
  - Microservice Template: For large-scale distributed applications
  - All-in-One Template: For smaller applications or simpler deployment requirements
- Pay attention to NuGet publish address and key when packaging
- Complete testing is recommended before publishing
