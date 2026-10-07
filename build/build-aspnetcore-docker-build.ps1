. "./build-aspnetcore-common.ps1"

$solutionFolder = (Get-Item -Path "../" -Verbose).FullName

Set-Location $solutionFolder

# Build all solutions
foreach ($docker in $dockerArray) {    
    $image = $docker.Image + ":" + $docker.Version
    Write-host "docker build -f " $docker.Dockerfile " -t " $image " ."
    docker build -f $docker.Dockerfile -t $image .
}

Set-Location $rootFolder