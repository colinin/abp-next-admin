Set-Location ".\migrations\LY.MicroService.BackendAdmin.EntityFrameworkCore"
dotnet ef database update

Set-Location "..\LY.MicroService.Platform.EntityFrameworkCore"
dotnet ef database update

Set-Location "..\LY.MicroService.LocalizationManagement.EntityFrameworkCore"
dotnet ef database update

Set-Location "..\LY.MicroService.RealtimeMessage.EntityFrameworkCore"
dotnet ef database update

Set-Location "..\LY.MicroService.IdentityServer.EntityFrameworkCore"
dotnet ef database update

Set-Location "..\LY.MicroService.TaskManagement.EntityFrameworkCore"
dotnet ef database update

Set-Location "..\LY.MicroService.AuthServer.EntityFrameworkCore"
dotnet ef database update
