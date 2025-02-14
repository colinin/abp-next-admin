# Monolithic Service Startup Guide

English | [简体中文](startup-aio-readme.md)

## Table of Contents

- [Quick Start with CLI](#quick-start-with-cli)
  - [Requirements](#cli-requirements)
  - [Install CLI Tool](#install-cli-tool)
  - [Create Project](#create-project)
  - [Run Project](#run-project)
- [Source Code Startup](#source-code-startup)
  - [Requirements](#requirements)
  - [Project Compilation](#project-compilation)
  - [Environment Configuration](#environment-configuration)
    - [Required Configuration](#required-configuration)
    - [Optional Configuration](#optional-configuration)
  - [Database Initialization](#database-initialization)
  - [Service Startup](#service-startup)
  - [Configuration Details](#configuration-details)
- [Common Issues](#common-issues)

## Quick Start with CLI

### CLI Requirements

- .NET 8.0 SDK
- Database (support any of the following):
  - MySQL
  - SQL Server
  - SQLite
  - Oracle
  - Oracle Devart
  - PostgreSQL
- Redis

### Install CLI Tool

```bash
dotnet tool install --global LINGYUN.Abp.Cli
```

### Create Project

```bash
# Short name: laa (LINGYUN Abp AllInOne)
labp create YourCompanyName.YourProjectName -pk YourPackageName -t laa -o /path/to/output --dbms MySql --cs "Server=127.0.0.1;Database=Platform-V70;User Id=root;Password=123456;SslMode=None" --no-random-port
```

Parameter Description:
- `-pk` or `--package-name`: Package name
- `-t` or `--template`: Template type, use `laa` for All-in-One template
- `-o` or `--output`: Output directory
- `--dbms`: Database type, supports MySql, SqlServer, Sqlite, Oracle, OracleDevart, PostgreSql
- `--cs`: Database connection string
- `--no-random-port`: Do not use random port

### Run Project

After creating the project, navigate to the project directory:

```bash
cd /path/to/output/host/YourPackageName.YourCompanyName.YourProjectName.AIO.Host
dotnet run --launch-profile "YourPackageName.YourCompanyName.YourProjectName.Development"
```

## Source Code Startup

### Requirements

- .NET 8.0 SDK
- Database (support any of the following):
  - PostgreSQL
  - MySQL
  - SQL Server (coming soon)
- Redis
- Docker (optional)

### Project Compilation

1. Ensure .NET 8.0 SDK is installed
2. Execute the following command in the project root directory to compile the entire project:

```shell
./build/build-aspnetcore-release.ps1
```

3. Open the `LY.MicroService.Applications.Single` solution in your IDE for debugging or publishing

### Environment Configuration

#### Required Configuration

#### 1. Database Configuration

Multiple databases are supported. Here are configuration examples for each:

##### PostgreSQL

```shell
# Start PostgreSQL using Docker
docker run -d --name postgres \
    -p 5432:5432 \
    -e POSTGRES_PASSWORD=postgres \
    -e PGDATA=/var/lib/postgresql/data \
    postgres:latest
```

##### MySQL

```shell
# Start MySQL using Docker
docker run -d --name mysql \
    -p 3306:3306 \
    -e MYSQL_ROOT_PASSWORD=mysql \
    mysql:latest
```

Create database:

```sql
CREATE DATABASE `Platform-V70`;
```

##### SQL Server (coming soon)

```shell
# Start SQL Server using Docker
docker run -d --name sqlserver \
    -p 1433:1433 \
    -e "ACCEPT_EULA=Y" \
    -e "SA_PASSWORD=yourStrong(!)Password" \
    mcr.microsoft.com/mssql/server:latest
```

#### 2. Configuration File Modification

Modify the database connection strings in the following configuration files according to your chosen database:

- `migrations/LY.MicroService.Applications.Single.DbMigrator/appsettings.json`
- `LY.MicroService.Applications.Single/appsettings.Development.json`

Database connection string examples:

PostgreSQL:

```json
{
  "ConnectionStrings": {
    "Default": "Host=127.0.0.1;Database=Platform-V70;Username=postgres;Password=123456;"
  }
}
```

MySQL:

```json
{
  "ConnectionStrings": {
    "Default": "Server=127.0.0.1;Database=Platform-V70;User Id=root;Password=123456;SslMode=None"
  }
}
```

SQL Server (coming soon):

```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost,1433;Database=Platform-V70;User Id=sa;Password=yourStrong(!)Password;TrustServerCertificate=True"
  }
}
```

#### 3. Redis Service

```shell
# Start Redis using Docker
docker run -d --name redis -p 6379:6379 redis:latest
```

Redis configuration example:

```json
{
  "DistributedLock": {
    "IsEnabled": true,
    "Redis": {
      "Configuration": "127.0.0.1,defaultDatabase=14"
    }
  },
  "Redis": {
    "IsEnabled": true,
    "Configuration": "127.0.0.1,defaultDatabase=15",
    "InstanceName": "LINGYUN.Abp.Application"
  },
  "Features": {
    "Validation": {
      "Redis": {
        "Configuration": "127.0.0.1,defaultDatabase=13",
        "InstanceName": "LINGYUN.Abp.Application"
      }
    }
  }
}
```

### Optional Configuration

The following configurations are applicable for monolithic distributed architecture:

#### 1. MinIO Distributed File Storage

```json
{
  "Minio": {
    "WithSSL": false,
    "BucketName": "blobs",
    "EndPoint": "127.0.0.1:19000",
    "AccessKey": "{AccessKey}",
    "SecretKey": "{SecretKey}",
    "CreateBucketIfNotExists": false
  }
}
```

#### 2. Elasticsearch Distributed Audit Logging

```json
{
  "Elasticsearch": {
    "NodeUris": "http://127.0.0.1:9200"
  }
}
```

### Database Initialization

1. Run database migration script:

Option 1 (Recommended):

```shell
./aspnet-core/migrations/Migrate.ps1
```

Follow the command line prompts to generate migration files and SQL scripts, then execute the SQL scripts to create database tables

Option 2:
Taking PostgreSQL as an example:

- Modify database connection information in `LY.MicroService.Applications.Single.DbMigrator/appsettings.PostgreSql.json`
- Navigate to `LY.MicroService.Applications.Single.EntityFrameworkCore.PostgreSql` project
- Run `dotnet ef database update`
- Wait for migration completion

2. Configure data initialization:

   - Modify database connection information in `LY.MicroService.Applications.Single.DbMigrator/appsettings.json`
   - Ensure the correct database provider is selected

3. Execute data migration:
   - Run the `LY.MicroService.Applications.Single.DbMigrator` project
   - Wait for migration to complete, basic table data will be initialized

### Service Startup

1. Run the `LY.MicroService.Applications.Single` project
2. Access Swagger API documentation in your browser:
   - URL: http://127.0.0.1:30000/swagger

### Configuration Details

#### 1. Basic Configuration

#### Application Configuration

```json
{
  "App": {
    "ShowPii": true,
    "SelfUrl": "http://127.0.0.1:30001/",
    "CorsOrigins": "http://127.0.0.1:3100,http://127.0.0.1:30001"
  }
}
```

#### Distributed Cache Configuration

```json
{
  "DistributedCache": {
    "HideErrors": true,
    "KeyPrefix": "LINGYUN.Abp.Application",
    "GlobalCacheEntryOptions": {
      "SlidingExpiration": "30:00:00",
      "AbsoluteExpirationRelativeToNow": "60:00:00"
    }
  }
}
```

### 2. Authentication Configuration

#### OpenIddict Configuration

```json
{
  "OpenIddict": {
    "Applications": {
      "VueAdmin": {
        "ClientId": "vue-admin-client",
        "RootUrl": "http://127.0.0.1:3100/"
      }
    },
    "Lifetime": {
      "AccessToken": "14:00:00",
      "IdentityToken": "00:20:00",
      "RefreshToken": "14:00:00"
    }
  }
}
```

#### Identity Authentication Configuration

```json
{
  "Identity": {
    "Password": {
      "RequiredLength": 6,
      "RequireNonAlphanumeric": false,
      "RequireLowercase": false,
      "RequireUppercase": false,
      "RequireDigit": false
    },
    "Lockout": {
      "AllowedForNewUsers": false,
      "LockoutDuration": 5,
      "MaxFailedAccessAttempts": 5
    },
    "SignIn": {
      "RequireConfirmedEmail": false,
      "RequireConfirmedPhoneNumber": false
    }
  }
}
```

### 3. Feature Management Configuration

```json
{
  "FeatureManagement": {
    "IsDynamicStoreEnabled": true
  },
  "SettingManagement": {
    "IsDynamicStoreEnabled": true
  },
  "PermissionManagement": {
    "IsDynamicStoreEnabled": true
  }
}
```

### 4. Logging Configuration

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "System": "Warning",
        "Microsoft": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "File",
        "Args": {
          "path": "Logs/Info-.log",
          "rollingInterval": "Day"
        }
      }
    ]
  }
}
```

## Common Issues

If you encounter problems, please check:

1. Database connection string is correct
2. Correct database provider is selected
3. Redis connection is working
4. Required ports are not occupied
5. Database migration completed successfully
6. Authentication server configuration is correct
7. CORS configuration is correct (if frontend access has cross-origin issues)
