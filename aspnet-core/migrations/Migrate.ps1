# 数据库迁移脚本

# 导入必要的模块
using namespace System.Management.Automation.Host

# 加入环境变量FROM_MIGRATION,使其在Program.cs文件中可以进行判断
$env:FROM_MIGRATION = "true"

# 定义项目路径
$projectPath = Resolve-Path (Join-Path $PSScriptRoot "..")

# 定义可用的DbContext
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

# 显示DbContext选择菜单
function Show-DbContextMenu {
    $host.UI.RawUI.BackgroundColor = "Black"
    $host.UI.RawUI.ForegroundColor = "White"
    Clear-Host

    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host "     数据库迁移上下文选择" -ForegroundColor Yellow
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "请选择要迁移的数据库上下文:" -ForegroundColor Green
    foreach ($key in $dbContexts.Keys) {
        $context = $dbContexts[$key]
        Write-Host "[$key] " -NoNewline -ForegroundColor Magenta
        Write-Host "$($context.Name)" -ForegroundColor White
    }
    Write-Host ""
}

# 选择DbContext
function Select-DbContext {
    Show-DbContextMenu
    
    while ($true) {
        $choice = Read-Host "请输入数字选择"
        if ($dbContexts.ContainsKey($choice)) {
            return $dbContexts[$choice]
        }
        Write-Host "无效的选择，请重新输入。" -ForegroundColor Red
    }
}

# 获取迁移名称
function Get-MigrationName {
    $defaultName = "AddNewMigration_" + (Get-Date -Format "yyyyMMdd_HHmmss")
    $migrationName = Read-Host "请输入迁移名称 (留空将使用默认名称: $defaultName)"
    
    return ($migrationName ? $migrationName : $defaultName)
}

# 执行数据库迁移
function Invoke-DatabaseMigration($dbContext, $migrationName) {
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host "正在执行数据库迁移..." -ForegroundColor Green
    Write-Host "上下文: " -NoNewline -ForegroundColor Yellow
    Write-Host "$($dbContext.Name)" -ForegroundColor White
    Write-Host "迁移名称: " -NoNewline -ForegroundColor Yellow
    Write-Host "$migrationName" -ForegroundColor White
    Write-Host "项目路径: " -NoNewline -ForegroundColor Yellow
    Write-Host "$projectPath" -ForegroundColor White
    Write-Host "========================================" -ForegroundColor Cyan
    
    # 切换到项目目录并执行迁移
    Push-Location $projectPath
    try {
        dotnet ef migrations add $migrationName --project "migrations/$($dbContext.Name)" --context "$($dbContext.Context)"
    }
    finally {
        Pop-Location
    }
}

# 获取所有迁移文件
function Get-AllMigrations($dbContext) {
    Push-Location $projectPath
    try {
        $migrations = @(dotnet ef migrations list --project "migrations/$($dbContext.Name)" --context "$($dbContext.Context)")
        return $migrations | Where-Object { $_ -match '\S' } # 过滤空行
    }
    finally {
        Pop-Location
    }
}

# 选择起始迁移
function Select-FromMigration($dbContext) {
    $migrations = Get-AllMigrations -dbContext $dbContext
    
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host "可用的迁移文件:" -ForegroundColor Yellow
    Write-Host "========================================" -ForegroundColor Cyan
    
    for ($i = 0; $i -lt $migrations.Count; $i++) {
        Write-Host "[$i] " -NoNewline -ForegroundColor Magenta
        Write-Host "$($migrations[$i])" -ForegroundColor White
    }
    
    Write-Host "[A] " -NoNewline -ForegroundColor Magenta
    Write-Host "生成所有迁移的SQL脚本" -ForegroundColor White
    Write-Host "[L] " -NoNewline -ForegroundColor Magenta
    Write-Host "仅生成最新迁移的SQL脚本" -ForegroundColor White
    
    while ($true) {
        $choice = Read-Host "请选择起始迁移文件"
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
        Write-Host "无效的选择，请重新输入。" -ForegroundColor Red
    }
}

# 生成SQL脚本
function Export-SqlScript($dbContext, $migrationName) {
    $choice = Read-Host "是否生成SQL迁移脚本？(Y/N)"
    
    if ($choice -eq "Y" -or $choice -eq "y") {
        $sqlFileName = (Get-Date -Format "yyyyMMddHHmm") + ".sql"
        $initSqlPath = Join-Path $projectPath "InitSql"
        $contextSqlPath = Join-Path $initSqlPath $dbContext.Name
        
        # 确保InitSql和上下文特定目录存在
        if (-not (Test-Path $initSqlPath)) {
            New-Item -ItemType Directory -Path $initSqlPath | Out-Null
        }
        if (-not (Test-Path $contextSqlPath)) {
            New-Item -ItemType Directory -Path $contextSqlPath | Out-Null
        }
        
        $sqlFilePath = Join-Path $contextSqlPath $sqlFileName
        
        # 选择起始迁移
        $fromMigration = Select-FromMigration -dbContext $dbContext
        
        Write-Host "========================================" -ForegroundColor Cyan
        Write-Host "正在生成SQL脚本..." -ForegroundColor Green
        Write-Host "输出路径: " -NoNewline -ForegroundColor Yellow
        Write-Host "$sqlFilePath" -ForegroundColor White
        if ($fromMigration) {
            Write-Host "起始迁移: " -NoNewline -ForegroundColor Yellow
            Write-Host "$fromMigration" -ForegroundColor White
        }
        Write-Host "========================================" -ForegroundColor Cyan
        
        # 切换到项目目录并生成SQL脚本
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
        
        Write-Host "SQL脚本已生成: " -NoNewline -ForegroundColor Green
        Write-Host "$sqlFilePath" -ForegroundColor White
    }
}

# 主执行流程
try {
    $dbContext = Select-DbContext
    $migrationName = Get-MigrationName
    Invoke-DatabaseMigration -dbContext $dbContext -migrationName $migrationName
    
    # 生成SQL脚本
    Export-SqlScript -dbContext $dbContext -migrationName $migrationName
    
    Write-Host "迁移完成！" -ForegroundColor Green
}
catch {
    Write-Host "迁移过程中发生错误:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
}
finally {
    Write-Host "`n按任意键退出..."
    $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
}
