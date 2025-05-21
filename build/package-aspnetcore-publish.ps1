$ErrorActionPreference = "Stop"

$NugetSource = "https://api.nuget.org/v3/index.json"
$ApiKey = "${{ secrets.NUGETKEY }}"

Get-ChildItem -Filter "*.nupkg" | Where-Object { $_.Name -notlike "*.symbols.nupkg" } | ForEach-Object {
    $pkg = $_.FullName
    Write-Host "🔍 Checking $pkg..."

    $tempDir = New-Item -ItemType Directory -Path ([System.IO.Path]::GetTempPath()) -Name ("nupkg_" + [System.Guid]::NewGuid().ToString())
    $zipCopy = Join-Path $tempDir "$($_.BaseName).zip"
    Copy-Item -Path $pkg -Destination $zipCopy

    Expand-Archive -Path $zipCopy -DestinationPath $tempDir.FullName

    $nuspec = Get-ChildItem -Path $tempDir.FullName -Filter "*.nuspec" | Select-Object -First 1
    if (-not $nuspec) {
        Write-Warning "❌ No .nuspec file found in $pkg. Skipping."
        Remove-Item -Recurse -Force $tempDir
        return
    }

    [xml]$nuspecXml = Get-Content $nuspec.FullName
    $packageId = $nuspecXml.package.metadata.id
    $packageVersion = $nuspecXml.package.metadata.version

    $lowerId = $packageId.ToLowerInvariant()
    $lowerVersion = $packageVersion.ToLowerInvariant()
    $checkUrl = "https://api.nuget.org/v3-flatcontainer/$lowerId/$lowerVersion/$lowerId.$lowerVersion.nupkg"

    $exists = $false
    try {
        $response = Invoke-WebRequest -Uri $checkUrl -Method Head -ErrorAction Stop
        if ($response.StatusCode -eq 200) {
            $exists = $true
        }
    } catch {
        if ($_.Exception.Response.StatusCode -ne 404) {
            Write-Warning "⚠️ Unexpected error while checking NuGet: $($_.Exception.Message)"
        }
    }

    if ($exists) {
        Write-Host "⚠️  Package $packageId@$packageVersion already exists. Skipping push."
    } else {
        Write-Host "🚀 Pushing $packageId@$packageVersion..."
        dotnet nuget push "$pkg" --api-key "$ApiKey" --source "$NugetSource" --skip-duplicate --no-symbols
    }

    Remove-Item -Recurse -Force $tempDir
}
