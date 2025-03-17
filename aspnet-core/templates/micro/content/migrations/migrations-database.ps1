Write-host "[ProjectName] - migrate database..."

Set-Location ./PackageName.CompanyName.ProjectName.DbMigrator.EntityFrameworkCore
dotnet ef migrations add Initial-ProjectName-Module
dotnet ef database update

Write-host "[ProjectName] - database migrations successfuly completed."

Write-host "[ProjectName] - seeding data to database..."

Set-Location ../PackageName.CompanyName.ProjectName.DbMigrator

dotnet run

Write-host "[ProjectName] - seed data successfuly completed."