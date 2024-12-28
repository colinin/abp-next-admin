# PostgreSQL Database Migration Guide

This guide will help you manage PostgreSQL database migrations using the migration scripts.

## Prerequisites

1. Ensure .NET Core SDK is installed
2. Ensure Entity Framework Core tools are installed
   ```powershell
   dotnet tool install --global dotnet-ef
   ```
3. Ensure PostgreSQL connection string is properly configured

## Usage Instructions

### 1. Create New Migration

1. Run the migration script in the `aspnet-core/migrations` directory:
   ```powershell
   # Use English version
   .\MigrateEn.ps1
   
   # Or use Chinese version
   .\Migrate.ps1
   ```

2. Select PostgreSQL database context from the menu:
   ```
   [2] LY.MicroService.Applications.Single.EntityFrameworkCore.PostgreSql
   ```

3. Enter migration name (optional):
   - Press Enter to use default name: `AddNewMigration_yyyyMMdd_HHmmss`
   - Or enter custom name, e.g.: `AddNewFeature`

### 2. Generate SQL Script

After creating the migration, the script will ask if you want to generate SQL script:

1. Choose whether to generate SQL script (Y/N)
2. If Y is selected, following options will be available:
   - `[A]` - Generate SQL script for all migrations
   - `[L]` - Generate SQL script for latest migration only
   - `[0-9]` - Generate from specified migration version

Generated SQL scripts will be saved in:
```
aspnet-core/InitSql/LY.MicroService.Applications.Single.EntityFrameworkCore.PostgreSql/
```

### 3. Apply Migration

Generated SQL scripts can be applied to database through:

1. Using pgAdmin or other PostgreSQL client tools to execute SQL script
2. Or using command line:
   ```bash
   psql -U your_username -d your_database -f your_script.sql
   ```
