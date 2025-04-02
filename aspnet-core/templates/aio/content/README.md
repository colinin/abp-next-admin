# PackageName.CompanyName.ProjectName

[English](README.md) | [中文](README.zh-CN.md)

## Quick Start Guide

This guide will help you quickly set up and run the project. Follow these steps to get started.

### Prerequisites

- .NET SDK 9.0 or higher
- A supported database (SQL Server, MySQL, PostgreSQL, Oracle, or SQLite)
- PowerShell 7.0+ (recommended for running migration scripts)

### Step 1: Restore and Build the Project

```bash
# Navigate to the project root directory
cd /path/to/project

# Restore dependencies
dotnet restore

# Build the solution
dotnet build
```

### Step 2: Create Database Schema

Use the Migrate.ps1 script to create the database tables structure:

```powershell
# Navigate to the migrations directory
cd migrations

# Run the migration script
./Migrate.ps1
```

The script will:

1. Detect available DbContext classes in the project
2. Ask you to select which DbContext to use for migration
3. Prompt for a migration name
4. Create the migration
5. Optionally generate SQL scripts for the migration

### Step 3: Initialize Seed Data

Run the DbMigrator project to initialize seed data:

```bash
# Navigate to the DbMigrator project directory
cd migrations/PackageName.CompanyName.ProjectName.AIO.DbMigrator

# Run the DbMigrator project
dotnet run
```

The DbMigrator will:

1. Apply all database migrations
2. Seed initial data (users, roles, etc.)
3. Set up tenant configurations if applicable

### Step 4: Launch the Application

After successfully setting up the database, you can run the host project:

```bash
# Navigate to the host project directory
cd host/PackageName.CompanyName.ProjectName.AIO.Host

# Run the host project
dotnet run --launch-profile "PackageName.CompanyName.ProjectName.Development"
```

The application will start and be accessible at the configured URL (typically [https://localhost:44300](https://localhost:44300)).

## Database-based Unit Testing

To run database-based unit tests, follow these steps:

### Step 1: Prepare Test Database

Before running tests, make sure the test database exists. The test database connection string is defined in the `ProjectNameEntityFrameworkCoreTestModule.cs` file.

The default connection string is:

```csharp
private const string DefaultPostgresConnectionString =
    "Host=127.0.0.1;Port=5432;Database=test_db;User Id=postgres;Password=postgres;";
```

You can either create this database manually or modify the connection string to use an existing database.

### Step 2: Configure Test Environment

Modify the connection string in `ProjectNameEntityFrameworkCoreTestModule.cs` if needed:

```csharp
// You can also set an environment variable to override the default connection string
var connectionString = Environment.GetEnvironmentVariable("TEST_CONNECTION_STRING") ??
                       DefaultPostgresConnectionString;
```

### Step 3: Run Tests

Run the Application.Tests project:

```bash
# Navigate to the test project directory
cd tests/PackageName.CompanyName.ProjectName.Application.Tests

# Run the tests
dotnet test
```

The test framework will:

1. Create a clean test database environment
2. Run all unit tests
3. Report test results

## Note About Naming

This is a template project, so all project names contain placeholders that will be replaced when the template is used to create a new project:

- `PackageName` will be replaced with your package name
- `CompanyName` will be replaced with your company name
- `ProjectName` will be replaced with your project name

When creating a new project from this template, you'll specify these values and they'll be substituted throughout the entire solution.
