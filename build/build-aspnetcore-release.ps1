. "./build-aspnetcore-common.ps1"

# Build all solutions
foreach ($batchCommandPath in $batchCommandPaths) {    
    $file = [io.fileinfo]$batchCommandPath;
    Write-Host $file.DirectoryName
    Set-Location $file.DirectoryName
    CMD /c $file.Name --publish -Wait
}

Set-Location $rootFolder