# Database Migration Script

# Import required modules
using namespace System.Management.Automation.Host

# Add environment variable FROM_MIGRATION for Program.cs to use
$env:FROM_MIGRATION = "true"

# Define project path
$projectPath = Resolve-Path (Join-Path $PSScriptRoot "..")

# Define available DbContexts
$dbContexts = @{
    "1" = @{
        Name = "LY.MicroService.Applications.Single.EntityFrameworkCore.MySql"
        Context = "SingleMigrationsDbContext"
        Factory = "SingleMigrationsDbContextFactory"
    }
    "2" = @{
        Name = "LY.MicroService.Applications.Single.EntityFrameworkCore.PostgreSql"
        Context = "SingleMigrationsDbContext"
        Factory = "SingleMigrationsDbContextFactory"
    }
}

# Display DbContext selection menu
function Show-DbContextMenu {
    $host.UI.RawUI.BackgroundColor = "Black"
    $host.UI.RawUI.ForegroundColor = "White"
    Clear-Host

    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host "     Database Migration Context Selection" -ForegroundColor Yellow
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Please select a database context to migrate:" -ForegroundColor Green
    foreach ($key in $dbContexts.Keys) {
        $context = $dbContexts[$key]
        Write-Host "[$key] " -NoNewline -ForegroundColor Magenta
        Write-Host "$($context.Name)" -ForegroundColor White
    }
    Write-Host ""
}

# Select DbContext
function Select-DbContext {
    Show-DbContextMenu
    
    while ($true) {
        $choice = Read-Host "Please enter a number to select"
        if ($dbContexts.ContainsKey($choice)) {
            return $dbContexts[$choice]
        }
        Write-Host "Invalid selection, please try again." -ForegroundColor Red
    }
}

# Get migration name
function Get-MigrationName {
    $defaultName = "AddNewMigration_" + (Get-Date -Format "yyyyMMdd_HHmmss")
    $migrationName = Read-Host "Enter migration name (press Enter to use default: $defaultName)"
    
    return ($migrationName ? $migrationName : $defaultName)
}

# Execute database migration
function Invoke-DatabaseMigration($dbContext, $migrationName) {
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host "Executing database migration..." -ForegroundColor Green
    Write-Host "Context: " -NoNewline -ForegroundColor Yellow
    Write-Host "$($dbContext.Name)" -ForegroundColor White
    Write-Host "Migration Name: " -NoNewline -ForegroundColor Yellow
    Write-Host "$migrationName" -ForegroundColor White
    Write-Host "Project Path: " -NoNewline -ForegroundColor Yellow
    Write-Host "$projectPath" -ForegroundColor White
    Write-Host "========================================" -ForegroundColor Cyan
    
    # Switch to project directory and execute migration
    Push-Location $projectPath
    try {
        dotnet ef migrations add $migrationName --project "migrations/$($dbContext.Name)" --context "$($dbContext.Context)"
    }
    finally {
        Pop-Location
    }
}

# Get all migration files
function Get-AllMigrations($dbContext) {
    Push-Location $projectPath
    try {
        $migrations = @(dotnet ef migrations list --project "migrations/$($dbContext.Name)" --context "$($dbContext.Context)")
        return $migrations | Where-Object { $_ -match '\S' } # Filter empty lines
    }
    finally {
        Pop-Location
    }
}

# Select migration to start from
function Select-FromMigration($dbContext) {
    $migrations = Get-AllMigrations -dbContext $dbContext
    
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host "Available migrations:" -ForegroundColor Yellow
    Write-Host "========================================" -ForegroundColor Cyan
    
    for ($i = 0; $i -lt $migrations.Count; $i++) {
        Write-Host "[$i] " -NoNewline -ForegroundColor Magenta
        Write-Host "$($migrations[$i])" -ForegroundColor White
    }
    
    Write-Host "[A] " -NoNewline -ForegroundColor Magenta
    Write-Host "Generate SQL script for all migrations" -ForegroundColor White
    Write-Host "[L] " -NoNewline -ForegroundColor Magenta
    Write-Host "Generate SQL script for latest migration only" -ForegroundColor White
    
    while ($true) {
        $choice = Read-Host "Select a migration to start from"
        if ($choice -eq "A" -or $choice -eq "a") {
            return $null
        }
        if ($choice -eq "L" -or $choice -eq "l") {
            if ($migrations.Count -gt 1) {
                return $migrations[$migrations.Count - 2]
            }
            return $null
        }
        if ([int]::TryParse($choice, [ref]$null)) {
            $index = [int]$choice
            if ($index -ge 0 -and $index -lt $migrations.Count) {
                return $migrations[$index]
            }
        }
        Write-Host "Invalid selection, please try again." -ForegroundColor Red
    }
}

# Generate SQL script
function Export-SqlScript($dbContext, $migrationName) {
    $choice = Read-Host "Do you want to generate SQL migration script? (Y/N)"
    
    if ($choice -eq "Y" -or $choice -eq "y") {
        $sqlFileName = (Get-Date -Format "yyyyMMddHHmm") + ".sql"
        $initSqlPath = Join-Path $projectPath "InitSql"
        $contextSqlPath = Join-Path $initSqlPath $dbContext.Name
        
        # Ensure InitSql and context-specific directories exist
        if (-not (Test-Path $initSqlPath)) {
            New-Item -ItemType Directory -Path $initSqlPath | Out-Null
        }
        if (-not (Test-Path $contextSqlPath)) {
            New-Item -ItemType Directory -Path $contextSqlPath | Out-Null
        }
        
        $sqlFilePath = Join-Path $contextSqlPath $sqlFileName
        
        # Select starting migration
        $fromMigration = Select-FromMigration -dbContext $dbContext
        
        Write-Host "========================================" -ForegroundColor Cyan
        Write-Host "Generating SQL script..." -ForegroundColor Green
        Write-Host "Output path: " -NoNewline -ForegroundColor Yellow
        Write-Host "$sqlFilePath" -ForegroundColor White
        if ($fromMigration) {
            Write-Host "Starting from migration: " -NoNewline -ForegroundColor Yellow
            Write-Host "$fromMigration" -ForegroundColor White
        }
        Write-Host "========================================" -ForegroundColor Cyan
        
        # Switch to project directory and generate SQL script
        Push-Location $projectPath
        try {
            if ($fromMigration) {
                dotnet ef migrations script $fromMigration --project "migrations/$($dbContext.Name)" --context "$($dbContext.Context)" --output $sqlFilePath
            }
            else {
                dotnet ef migrations script --project "migrations/$($dbContext.Name)" --context "$($dbContext.Context)" --output $sqlFilePath
            }
        }
        finally {
            Pop-Location
        }
        
        Write-Host "SQL script generated: " -NoNewline -ForegroundColor Green
        Write-Host "$sqlFilePath" -ForegroundColor White
    }
}

# Main execution
try {
    $dbContext = Select-DbContext
    $migrationName = Get-MigrationName
    Invoke-DatabaseMigration -dbContext $dbContext -migrationName $migrationName
    
    # Generate SQL script
    Export-SqlScript -dbContext $dbContext -migrationName $migrationName
    
    Write-Host "Migration completed successfully!" -ForegroundColor Green
}
catch {
    Write-Host "An error occurred during migration:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
}
finally {
    Write-Host "`nPress any key to exit..."
    $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
}
