using Shouldly;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace LINGYUN.Abp.BlobManagement;

public class BlobRepository_Tests : AbpBlobManagementEntityFrameworkCoreTestBase
{
    private const string TestProvider = "FileSystem";

    private IBlobRepository BlobRepository => GetRequiredService<IBlobRepository>();

    private IBlobContainerRepository BlobContainerRepository => GetRequiredService<IBlobContainerRepository>();

    private ICurrentTenant CurrentTenant => GetRequiredService<ICurrentTenant>();

    [Fact]
    public async Task Should_Persist_And_Query_Blob_Of_Current_Tenant()
    {
        var tenantId = Guid.NewGuid();

        var container = new TestBlobContainer("users", tenantId);
        var blob = new TestBlob(Guid.NewGuid(), container.Id, "/tenant.txt", tenantId);

        await SeedAsync((container, blob));

        await WithUnitOfWorkAsync(async () =>
        {
            using (CurrentTenant.Change(tenantId))
            {
                var found = await BlobRepository.FindByFullNameAsync("/tenant.txt", container.Id);

                found.ShouldNotBeNull();
                found!.Id.ShouldBe(blob.Id);
                found.TenantId.ShouldBe(tenantId);
            }
        });
    }

    [Fact]
    public async Task Should_Filter_Blobs_By_Current_Tenant()
    {
        var tenantA = Guid.NewGuid();
        var tenantB = Guid.NewGuid();

        var containerOfA = new TestBlobContainer("users", tenantA);
        var containerOfB = new TestBlobContainer("users", tenantB);

        var blobOfA = new TestBlob(Guid.NewGuid(), containerOfA.Id, "/shared.txt", tenantA);
        var blobOfB = new TestBlob(Guid.NewGuid(), containerOfB.Id, "/shared.txt", tenantB);

        await SeedAsync(
            (containerOfA, blobOfA),
            (containerOfB, blobOfB));

        await WithUnitOfWorkAsync(async () =>
        {
            using (CurrentTenant.Change(tenantA))
            {
                var found = await BlobRepository.FindByFullNameAsync("/shared.txt");

                found.ShouldNotBeNull();
                found!.Id.ShouldBe(blobOfA.Id);
            }
        });

        await WithUnitOfWorkAsync(async () =>
        {
            using (CurrentTenant.Change(tenantB))
            {
                var found = await BlobRepository.FindByFullNameAsync("/shared.txt");

                found.ShouldNotBeNull();
                found!.Id.ShouldBe(blobOfB.Id);
            }
        });

        await WithUnitOfWorkAsync(async () =>
        {
            using (CurrentTenant.Change(null))
            {
                // 宿主租户下不存在这些数据
                (await BlobRepository.FindByFullNameAsync("/shared.txt")).ShouldBeNull();
            }
        });
    }

    [Fact]
    public async Task Should_Filter_Containers_By_Current_Tenant()
    {
        var tenantA = Guid.NewGuid();
        var tenantB = Guid.NewGuid();

        var containerOfA = new TestBlobContainer("users", tenantA);
        var containerOfB = new TestBlobContainer("users", tenantB);

        await SeedAsync(
            (containerOfA, null),
            (containerOfB, null));

        await WithUnitOfWorkAsync(async () =>
        {
            using (CurrentTenant.Change(tenantA))
            {
                var found = await BlobContainerRepository.FindByNameAsync(TestProvider, "users");

                found.ShouldNotBeNull();
                found!.Id.ShouldBe(containerOfA.Id);
            }
        });

        await WithUnitOfWorkAsync(async () =>
        {
            using (CurrentTenant.Change(tenantB))
            {
                var found = await BlobContainerRepository.FindByNameAsync(TestProvider, "users");

                found.ShouldNotBeNull();
                found!.Id.ShouldBe(containerOfB.Id);
            }
        });
    }

    [Fact]
    public async Task Should_Not_Find_Blob_Of_Another_Tenant_By_Id()
    {
        var tenantA = Guid.NewGuid();
        var tenantB = Guid.NewGuid();

        var containerOfB = new TestBlobContainer("users", tenantB);
        var blobOfB = new TestBlob(Guid.NewGuid(), containerOfB.Id, "/by-id.txt", tenantB);

        await SeedAsync((containerOfB, blobOfB));

        await WithUnitOfWorkAsync(async () =>
        {
            using (CurrentTenant.Change(tenantA))
            {
                (await BlobRepository.FindAsync(blobOfB.Id)).ShouldBeNull();
            }
        });

        await WithUnitOfWorkAsync(async () =>
        {
            using (CurrentTenant.Change(tenantB))
            {
                (await BlobRepository.FindAsync(blobOfB.Id)).ShouldNotBeNull();
            }
        });
    }

    [Fact]
    public async Task Should_Filter_Blobs_By_Container()
    {
        var tenantId = Guid.NewGuid();

        var usersContainer = new TestBlobContainer("users", tenantId);
        var iconsContainer = new TestBlobContainer("icons", tenantId);

        var blobOfUsers = new TestBlob(Guid.NewGuid(), usersContainer.Id, "/logo.png", tenantId);
        var blobOfIcons = new TestBlob(Guid.NewGuid(), iconsContainer.Id, "/logo.png", tenantId);

        await SeedAsync(
            (usersContainer, blobOfUsers),
            (iconsContainer, blobOfIcons));

        await WithUnitOfWorkAsync(async () =>
        {
            using (CurrentTenant.Change(tenantId))
            {
                var found = await BlobRepository.FindByFullNameAsync("/logo.png", usersContainer.Id);

                found.ShouldNotBeNull();
                found!.Id.ShouldBe(blobOfUsers.Id);
            }
        });
    }

    [Fact]
    public async Task Should_Return_Null_For_Unknown_FullName()
    {
        var tenantId = Guid.NewGuid();
        var container = new TestBlobContainer("users", tenantId);

        await SeedAsync((container, null));

        await WithUnitOfWorkAsync(async () =>
        {
            using (CurrentTenant.Change(tenantId))
            {
                (await BlobRepository.FindByFullNameAsync("/not-exists.txt", container.Id)).ShouldBeNull();
            }
        });
    }

    private async Task SeedAsync(params (TestBlobContainer Container, TestBlob? Blob)[] items)
    {
        await WithUnitOfWorkAsync(async () =>
        {
            foreach (var item in items)
            {
                using (CurrentTenant.Change(item.Container.TenantId))
                {
                    await BlobContainerRepository.InsertAsync(item.Container);

                    if (item.Blob != null)
                    {
                        await BlobRepository.InsertAsync(item.Blob);
                    }
                }
            }
        });
    }

    private sealed class TestBlobContainer : BlobContainer
    {
        public TestBlobContainer(string name, Guid? tenantId)
        {
            Id = Guid.NewGuid();
            TenantId = tenantId;
            SetName(name);
            SetProvider(TestProvider);
        }
    }

    private sealed class TestBlob : Blob
    {
        public TestBlob(Guid id, Guid containerId, string fullName, Guid? tenantId)
        {
            Id = id;
            TenantId = tenantId;
            ContainerId = containerId;
            SetName(Path.GetFileName(fullName));
            SetFullName(fullName);
            Type = BlobType.File;
            Size = 1;
            SetProvider(TestProvider);
        }
    }
}
