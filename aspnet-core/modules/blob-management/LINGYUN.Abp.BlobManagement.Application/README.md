# Blob container authorization

Container operations require an authenticated caller.

- Create and delete require the corresponding `AbpBlobManagement.BlobContainer.Create` and `AbpBlobManagement.BlobContainer.Delete` permissions.
- Get and list accept either `AbpBlobManagement.BlobContainer` or the effective `AbpBlobManagement.Blob` permission, so file selection remains available to authorized file users.
- The file permission includes the existing Blob feature requirement. Container management uses its own permission.
- Existing permission definitions determine tenant eligibility; repository filters continue to determine data scope. Static-container deletion protection also applies to authorized callers.

Run the focused authorization tests from the repository root:

```sh
dotnet test aspnet-core/tests/LINGYUN.Abp.BlobManagement.Application.Tests/LINGYUN.Abp.BlobManagement.Application.Tests.csproj
```
