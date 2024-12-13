# 单体服务启动指南

[English](startup-aio-readme.en.md) | 简体中文

## 目录

- [环境要求](#环境要求)
- [项目编译](#项目编译)
- [环境配置](#环境配置)
  - [必选配置](#必选配置)
  - [可选配置](#可选配置)
- [数据库初始化](#数据库初始化)
- [服务启动](#服务启动)
- [配置说明](#配置说明)

## 环境要求

- .NET 8.0 SDK
- 数据库（支持以下任一种）：
  - PostgreSQL
  - MySQL
  - SQL Server（即将支持）
- Redis
- Docker（可选）

## 项目编译

1. 确保已安装 .NET 8.0 SDK
2. 在项目根目录执行以下命令编译整个项目：

```shell
./build/build-aspnetcore-release.ps1
```

3. 使用 IDE 打开 `LY.MicroService.Applications.Single` 解决方案进行调试或发布

## 环境配置

### 必选配置

#### 1. 数据库配置

支持多种数据库，以下是各种数据库的配置示例：

##### PostgreSQL

```shell
# 使用Docker启动PostgreSQL
docker run -d --name postgres \
    -p 5432:5432 \
    -e POSTGRES_PASSWORD=postgres \
    -e PGDATA=/var/lib/postgresql/data \
    postgres:latest
```

##### MySQL

```shell
# 使用Docker启动MySQL
docker run -d --name mysql \
    -p 3306:3306 \
    -e MYSQL_ROOT_PASSWORD=mysql \
    mysql:latest
```

创建数据库：

```sql
CREATE DATABASE `Platform-V70`;
```

##### SQL Server（即将支持）

```shell
# 使用Docker启动SQL Server
docker run -d --name sqlserver \
    -p 1433:1433 \
    -e "ACCEPT_EULA=Y" \
    -e "SA_PASSWORD=yourStrong(!)Password" \
    mcr.microsoft.com/mssql/server:latest
```

#### 2. 配置文件修改

需要根据选择的数据库类型修改以下配置文件中的数据库连接字符串：

- `migrations/LY.MicroService.Applications.Single.DbMigrator/appsettings.json`
- `LY.MicroService.Applications.Single/appsettings.Development.json`

数据库连接字符串示例：

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

SQL Server（即将支持）:

```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost,1433;Database=Platform-V70;User Id=sa;Password=yourStrong(!)Password;TrustServerCertificate=True"
  }
}
```

#### 3. Redis 服务

```shell
# 使用Docker启动Redis
docker run -d --name redis -p 6379:6379 redis:latest
```

Redis 配置示例：

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

### 可选配置

以下配置适用于单体分布式架构：

#### 1. MinIO 分布式文件存储

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

#### 2. Elasticsearch 分布式审计日志

```json
{
  "Elasticsearch": {
    "NodeUris": "http://127.0.0.1:9200"
  }
}
```

## 数据库初始化

1. 运行数据库迁移脚本：

方案一（推荐）：

```shell
./aspnet-core/migrations/Migrate.ps1
```

根据命令行提示生成迁移文件和 sql 脚本，然后执行 sql 脚本来创建数据表

方案二：
以 pgsql 为例

- 修改 `LY.MicroService.Applications.Single.DbMigrator/appsettings.PostgreSql.json` 中的数据库连接信息
- 进入`LY.MicroService.Applications.Single.EntityFrameworkCore.PostgreSql`项目
- 运行 `dotnet ef database update`
- 等待数据迁移完成

2. 配置数据初始化：

   - 修改 `LY.MicroService.Applications.Single.DbMigrator/appsettings.json` 中的数据库连接信息
   - 确保选择了正确的数据库提供程序

3. 执行数据迁移：
   - 运行 `LY.MicroService.Applications.Single.DbMigrator` 项目
   - 等待数据迁移完成，基础表数据将被初始化

## 服务启动

1. 运行 `LY.MicroService.Applications.Single` 项目
2. 在浏览器中访问 Swagger 接口文档：
   - URL: http://127.0.0.1:30000/swagger

## 配置说明

### 1. 基础配置

#### 应用程序配置

```json
{
  "App": {
    "ShowPii": true,
    "SelfUrl": "http://127.0.0.1:30001/",
    "CorsOrigins": "http://127.0.0.1:3100,http://127.0.0.1:30001"
  }
}
```

#### 分布式缓存配置

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

### 2. 认证配置

#### OpenIddict 配置

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

#### 身份认证配置

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

### 3. 功能开关配置

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

### 4. 日志配置

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

## 常见问题

如果遇到问题，请检查：

1. 数据库连接字符串是否正确
2. 是否选择了正确的数据库提供程序
3. Redis 连接是否正常
4. 必要的端口是否被占用
5. 数据库迁移是否成功完成
6. 认证服务器配置是否正确
7. CORS 配置是否正确（如果前端访问出现跨域问题）
