using LINGYUN.Abp.BlobManagement.Dtos;
using LINGYUN.Abp.BlobManagement.Permissions;
using Shouldly;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Authorization;
using Volo.Abp.Content;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace LINGYUN.Abp.BlobManagement;

public class BlobAppService_Tests : AbpBlobManagementApplicationTestBase
{
    private const string ContainerName = "users";

    private IBlobAppService BlobAppService => GetRequiredService<IBlobAppService>();

    private BlobManager BlobManager => GetRequiredService<BlobManager>();

    private TestAuthorizationService Authorization => GetRequiredService<TestAuthorizationService>();

    private ICurrentTenant CurrentTenant => GetRequiredService<ICurrentTenant>();

    [Fact]
    public async Task Should_Download_File_Of_Current_Tenant()
    {
        var tenantId = Guid.NewGuid();
        var blob = await CreateFileAsync("current-tenant.txt", "TENANT-CONTENT", tenantId);

        Authorization.Grant(BlobManagementPermissionNames.Blob.Default);
        Authorization.GrantResource(BlobManagementPermissionNames.Blob.Resources.View, blob.Id);

        using (CurrentTenant.Change(tenantId))
        {
            var content = await BlobAppService.DownloadByNameAsync(
                new BlobDownloadByNameInput
                {
                    ContainerName = ContainerName,
                    BlobName = "/current-tenant.txt",
                });

            (await ReadAsStringAsync(content)).ShouldBe("TENANT-CONTENT");
        }
    }

    [Fact]
    public async Task Should_Not_Download_File_Of_Another_Tenant()
    {
        var tenantA = Guid.NewGuid();
        var tenantB = Guid.NewGuid();

        var blobOfTenantB = await CreateFileAsync("other-tenant.txt", "OTHER-TENANT-CONTENT", tenantB);
        // 两个租户都存在同名容器, 排除"容器不存在"造成的误判
        await EnsureContainerAsync(tenantA);

        // 即使授予所有功能权限以及该文件的资源级权限, 也不能跨租户读取
        Authorization.Grant(BlobManagementPermissionNames.Blob.Default);
        Authorization.GrantResource(BlobManagementPermissionNames.Blob.Resources.View, blobOfTenantB.Id);

        using (CurrentTenant.Change(tenantA))
        {
            var exception = await Should.ThrowAsync<BusinessException>(async () =>
                await BlobAppService.DownloadByNameAsync(
                    new BlobDownloadByNameInput
                    {
                        ContainerName = ContainerName,
                        BlobName = "/other-tenant.txt",
                    }));

            exception.Code.ShouldBe(BlobManagementErrorCodes.Blob.NameNotFound);
        }
    }

    [Fact]
    public void Should_Not_Expose_TenantId_In_Download_Input()
    {
        // 输入模型不允许携带租户标识(客户端可控租户是跨租户越权的根源)
        typeof(BlobDownloadByNameInput)
            .GetProperty("TenantId")
            .ShouldBeNull();
    }

    [Fact]
    public async Task Should_Require_Function_Permission_To_Download()
    {
        var tenantId = Guid.NewGuid();
        var blob = await CreateFileAsync("no-permission.txt", "PROTECTED", tenantId);

        // 未授予任何功能权限
        Authorization.Reset();

        using (CurrentTenant.Change(tenantId))
        {
            await Should.ThrowAsync<AbpAuthorizationException>(async () =>
                await BlobAppService.DownloadByNameAsync(
                    new BlobDownloadByNameInput
                    {
                        ContainerName = ContainerName,
                        BlobName = "/no-permission.txt",
                    }));
        }

        Authorization.CheckedPolicies.ShouldContain(BlobManagementPermissionNames.Blob.Default);
    }

    [Fact]
    public async Task Should_Require_Resource_Permission_To_Download()
    {
        var tenantId = Guid.NewGuid();
        var blob = await CreateFileAsync("private.txt", "PRIVATE", tenantId);

        // 功能权限已授予, 但未授予该文件的资源级权限
        Authorization.Grant(BlobManagementPermissionNames.Blob.Default);

        using (CurrentTenant.Change(tenantId))
        {
            await Should.ThrowAsync<AbpAuthorizationException>(async () =>
                await BlobAppService.DownloadByNameAsync(
                    new BlobDownloadByNameInput
                    {
                        ContainerName = ContainerName,
                        BlobName = "/private.txt",
                    }));
        }

        // 容器级(资源级)策略确实被调用
        Authorization.CheckedPolicies.ShouldContain(BlobManagementPermissionNames.Blob.Resources.View);
        Authorization.CheckedResources
            .OfType<Blob>()
            .Any(x => x.Id == blob.Id)
            .ShouldBeTrue();
    }

    [Fact]
    public async Task Should_Require_Resource_Permission_To_Delete()
    {
        var tenantId = Guid.NewGuid();
        var blob = await CreateFileAsync("to-delete.txt", "DELETE-ME", tenantId);

        Authorization.Grant(
            BlobManagementPermissionNames.Blob.Default,
            BlobManagementPermissionNames.Blob.Delete);

        using (CurrentTenant.Change(tenantId))
        {
            await Should.ThrowAsync<AbpAuthorizationException>(async () =>
                await BlobAppService.DeleteAsync(blob.Id));
        }

        Authorization.CheckedPolicies.ShouldContain(BlobManagementPermissionNames.Blob.Resources.Delete);
    }

    [Fact]
    public async Task Should_Delete_File_With_Resource_Permission()
    {
        var tenantId = Guid.NewGuid();
        var blob = await CreateFileAsync("delete-own.txt", "DELETE-ME", tenantId);

        Authorization.Grant(
            BlobManagementPermissionNames.Blob.Default,
            BlobManagementPermissionNames.Blob.Delete);
        Authorization.GrantResource(BlobManagementPermissionNames.Blob.Resources.Delete, blob.Id);

        using (CurrentTenant.Change(tenantId))
        {
            await BlobAppService.DeleteAsync(blob.Id);

            var repository = GetRequiredService<IBlobRepository>();
            await WithUnitOfWorkAsync(async () =>
            {
                (await repository.FindAsync(blob.Id)).ShouldBeNull();
            });
        }
    }

    [Fact]
    public async Task Should_Require_Resource_Permission_To_Generate_Download_Url()
    {
        var tenantId = Guid.NewGuid();
        var blob = await CreateFileAsync("url.txt", "URL-CONTENT", tenantId);

        Authorization.Grant(
            BlobManagementPermissionNames.Blob.Default,
            BlobManagementPermissionNames.Blob.GenerateDownloadUrl);

        using (CurrentTenant.Change(tenantId))
        {
            await Should.ThrowAsync<AbpAuthorizationException>(async () =>
                await BlobAppService.GenerateDownloadUrlAsync(blob.Id));
        }
    }

    [Fact]
    public async Task Should_Download_By_Generated_Key()
    {
        var tenantId = Guid.NewGuid();
        var blob = await CreateFileAsync("by-key.txt", "BY-KEY-CONTENT", tenantId);

        Authorization.Grant(
            BlobManagementPermissionNames.Blob.Default,
            BlobManagementPermissionNames.Blob.GenerateDownloadUrl);
        Authorization.GrantResource(BlobManagementPermissionNames.Blob.Resources.View, blob.Id);

        string url;
        using (CurrentTenant.Change(tenantId))
        {
            url = await BlobAppService.GenerateDownloadUrlAsync(blob.Id);
        }

        url.ShouldContain("/download/");

        var key = url.Split('/').Last();

        // 宿主(无租户)上下文使用下载密钥, 租户信息由密钥携带
        Authorization.Reset();

        var content = await BlobAppService.DownloadAsync(new BlobDownloadByIdInput { Key = key });
        (await ReadAsStringAsync(content)).ShouldBe("BY-KEY-CONTENT");
    }

    [Fact]
    public async Task Should_Not_Download_By_Tampered_Key()
    {
        var tenantId = Guid.NewGuid();
        var blob = await CreateFileAsync("tampered.txt", "TAMPERED", tenantId);

        Authorization.Grant(
            BlobManagementPermissionNames.Blob.Default,
            BlobManagementPermissionNames.Blob.GenerateDownloadUrl);
        Authorization.GrantResource(BlobManagementPermissionNames.Blob.Resources.View, blob.Id);

        string key;
        using (CurrentTenant.Change(tenantId))
        {
            var url = await BlobAppService.GenerateDownloadUrlAsync(blob.Id);
            key = url.Split('/').Last();
        }

        var tamperedKey = key[..^1] + (key[^1] == 'A' ? 'B' : 'A');

        var content = await BlobAppService.DownloadAsync(new BlobDownloadByIdInput { Key = tamperedKey });

        (await ReadAsStringAsync(content)).ShouldBeEmpty();
    }

    [Fact]
    public async Task Should_Not_Get_Blob_Of_Another_Tenant_By_Id()
    {
        var tenantA = Guid.NewGuid();
        var tenantB = Guid.NewGuid();

        var blobOfTenantB = await CreateFileAsync("by-id.txt", "BY-ID-CONTENT", tenantB);

        Authorization.Grant(BlobManagementPermissionNames.Blob.Default);
        Authorization.GrantResource(BlobManagementPermissionNames.Blob.Resources.View, blobOfTenantB.Id);

        using (CurrentTenant.Change(tenantA))
        {
            await Should.ThrowAsync<EntityNotFoundException>(async () =>
                await BlobAppService.GetAsync(blobOfTenantB.Id));
        }
    }

    private async Task EnsureContainerAsync(Guid? tenantId)
    {
        await WithUnitOfWorkAsync(async () =>
        {
            using (CurrentTenant.Change(tenantId))
            {
                if (await BlobManager.FindContainerAsync(ContainerName) == null)
                {
                    await BlobManager.CreateContainerAsync(ContainerName);
                }
            }
        });
    }

    private async Task<Blob> CreateFileAsync(string name, string content, Guid? tenantId)
    {
        Blob? blob = null;

        await WithUnitOfWorkAsync(async () =>
        {
            using (CurrentTenant.Change(tenantId))
            {
                var container = await BlobManager.FindContainerAsync(ContainerName)
                    ?? await BlobManager.CreateContainerAsync(ContainerName);

                using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

                blob = await BlobManager.CreateBlobAsync(
                    container,
                    name,
                    BlobType.File,
                    stream.Length);

                await BlobManager.UploadBlobAsync(container, blob, stream);
            }
        });

        return blob!;
    }

    private static async Task<string> ReadAsStringAsync(IRemoteStreamContent content)
    {
        using var stream = content.GetStream();
        using var reader = new StreamReader(stream);

        return await reader.ReadToEndAsync();
    }
}
