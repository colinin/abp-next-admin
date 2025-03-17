# 清理之前的构建
if (Test-Path -Path "./aio/LocalNuget") {
    Remove-Item -Path "./aio/LocalNuget/*" -Recurse -Force
}
else {
    New-Item -ItemType Directory -Path "./aio/LocalNuget"
}

if (Test-Path -Path "./micro/LocalNuget") {
    Remove-Item -Path "./micro/LocalNuget/*" -Recurse -Force
}
else {
    New-Item -ItemType Directory -Path "./micro/LocalNuget"
}

# 显示选择菜单
Write-Host "请选择要打包的模板："
Write-Host "1. 微服务模板 (PackageName.CompanyName.ProjectName)"
Write-Host "2. AllInOne模板 (PackageName.CompanyName.ProjectName.AIO)"
Write-Host "3. 全部打包"

$choice = Read-Host "请输入选项 (1-3)"

switch ($choice) {
    "1" {
        Write-Host "正在打包微服务模板..."
        dotnet pack ./micro/PackageName.CompanyName.ProjectName.csproj -c Release -o ./micro/LocalNuget --nologo -p:NoDefaultExcludes=true
    }
    "2" {
        Write-Host "正在打包AllInOne模板..."
        dotnet pack ./aio/PackageName.CompanyName.ProjectName.AIO.csproj -c Release -o ./aio/LocalNuget --nologo -p:NoDefaultExcludes=true
    }
    "3" {
        Write-Host "正在打包所有模板..."
        dotnet pack ./micro/PackageName.CompanyName.ProjectName.csproj -c Release -o ./micro/LocalNuget --nologo -p:NoDefaultExcludes=true
        dotnet pack ./aio/PackageName.CompanyName.ProjectName.AIO.csproj -c Release -o ./aio/LocalNuget --nologo -p:NoDefaultExcludes=true
    }
    default {
        Write-Host "无效的选项，退出脚本"
        exit 1
    }
}

# 询问是否要发布到NuGet服务器
$publishChoice = Read-Host "是否要发布到NuGet服务器？(Y/N)"

if ($publishChoice -eq "Y" -or $publishChoice -eq "y") {
    # 根据之前的选择发布对应的包
    switch ($choice) {
        "1" {
            $packages = Get-ChildItem -Path "./micro/LocalNuget/*.nupkg"
        }
        "2" {
            $packages = Get-ChildItem -Path "./aio/LocalNuget/*.nupkg"
        }
        "3" {
            $packages = @()
            $packages += Get-ChildItem -Path "./micro/LocalNuget/*.nupkg"
            $packages += Get-ChildItem -Path "./aio/LocalNuget/*.nupkg"
        }
    }
    
    foreach ($package in $packages) {
        Write-Host "正在发布包：$($package.Name)"
        dotnet nuget push $package.FullName --source "https://custom.nuget.net/nuget/abp/v3/index.json" --api-key "" --skip-duplicate
    }
    Write-Host "发布完成！"
}
else {
    Write-Host "跳过发布步骤。"
    if ($choice -eq "1") {
        Write-Host "包文件已保存在 ./micro/LocalNuget 目录中。"
    }
    elseif ($choice -eq "2") {
        Write-Host "包文件已保存在 ./aio/LocalNuget 目录中。"
    }
    else {
        Write-Host "包文件已保存在 ./micro/LocalNuget 和 ./aio/LocalNuget 目录中。"
    }
}
