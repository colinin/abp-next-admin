# LY.MicroService.Applications.Single.DbMigrator

Single application database migration tool for automatically executing database migrations and initializing data.

[简体中文](./README.md)

## Features

* Automatic database migration execution
* Multi-environment configuration support
* Integrated Serilog logging
* Data migration environment configuration support
* Automatic database migration check and application
* Console application, easy to integrate into CI/CD pipelines

## Configuration

```json
{
  "ConnectionStrings": {
    "Default": "your-database-connection-string"
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

## Basic Usage

1. Configure Database Connection
   * Configure database connection string in appsettings.json
   * Use appsettings.{Environment}.json for different environment configurations

2. Run Migration Tool
   ```bash
   dotnet run
   ```

3. View Migration Logs
   * Console output
   * Logs/migrations.txt file

## Environment Variables

* `ASPNETCORE_ENVIRONMENT`: Set runtime environment (Development, Staging, Production, etc.)
* `DOTNET_ENVIRONMENT`: Same as above, for compatibility

## Notes

* Ensure database connection string includes sufficient permissions
* Recommended to backup database before executing migrations
* Check migrations.txt log file for migration details
* If migration fails, check error messages in logs

## Development and Debugging

1. Set Environment Variables
   ```bash
   export ASPNETCORE_ENVIRONMENT=Development
   ```

2. Debug with Visual Studio or Visual Studio Code
   * Set breakpoints
   * View detailed migration process

3. Customize Migration Logic
   * Modify SingleDbMigrationService class
   * Add new data seeds
