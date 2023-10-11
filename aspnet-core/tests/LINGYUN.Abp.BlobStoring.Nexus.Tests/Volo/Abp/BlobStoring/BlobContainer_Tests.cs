using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.BlobStoring.TestObjects;
using Volo.Abp.Clients;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.BlobStoring;

public abstract class BlobContainer_Tests<TStartupModule> : AbpIntegratedTest<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected IBlobContainer<TestContainer1> Container { get; }

    protected ICurrentTenant CurrentTenant { get; }

    protected BlobContainer_Tests()
    {
        Container = GetRequiredService<IBlobContainer<TestContainer1>>();
        CurrentTenant = GetRequiredService<ICurrentTenant>();
    }

    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }

    [Theory]
    [InlineData("Should_Save_And_Get_Blobs")]
    [InlineData("Should_Save_And_Get_Blobs.txt")]
    [InlineData("test-folder/Should_Save_And_Get_Blobs")]
    public async Task Should_Save_And_Get_Blobs(string blobName)
    {
        var testContent = "test content".GetBytes();
        await Container.SaveAsync(blobName, testContent);
        await Task.Delay(3000);
        var result = await Container.GetAllBytesAsync(blobName);
        result.SequenceEqual(testContent).ShouldBeTrue();
        await Container.DeleteAsync(blobName);
    }

    [Fact]
    public async Task Should_Save_And_Get_Blobs_In_Different_Tenant()
    {
        var blobName = "Should_Save_And_Get_Blobs_In_Different_Tenant";
        var testContent = "test content".GetBytes();

        using (CurrentTenant.Change(Guid.NewGuid()))
        {
            await Container.SaveAsync(blobName, testContent);
            await Task.Delay(2000);
            (await Container.GetAllBytesAsync(blobName)).SequenceEqual(testContent).ShouldBeTrue();
            await Container.DeleteAsync(blobName);
            await Task.Delay(2000);
        }

        using (CurrentTenant.Change(Guid.NewGuid()))
        {
            await Container.SaveAsync(blobName, testContent);
            await Task.Delay(2000);
            (await Container.GetAllBytesAsync(blobName)).SequenceEqual(testContent).ShouldBeTrue();

            using (CurrentTenant.Change(null))
            {
                // Could not found the requested BLOB...
                await Assert.ThrowsAsync<AbpException>(async () =>
                    await Container.GetAllBytesAsync(blobName)
                );
            }

            await Container.DeleteAsync(blobName);
            await Task.Delay(2000);
        }

        using (CurrentTenant.Change(null))
        {
            await Container.SaveAsync(blobName, testContent);
            await Task.Delay(2000);
            (await Container.GetAllBytesAsync(blobName)).SequenceEqual(testContent).ShouldBeTrue();
            await Container.DeleteAsync(blobName);
        }
    }

    [Fact]
    public async Task Should_Overwrite_Pre_Saved_Blob_If_Requested()
    {
        var blobName = "Should_Overwrite_Pre_Saved_Blob_If_Requested";

        var testContent = "test content".GetBytes();
        await Container.SaveAsync(blobName, testContent);
        await Task.Delay(2000);
        var testContentOverwritten = "test content overwritten".GetBytes();
        await Container.SaveAsync(blobName, testContentOverwritten, true);
        await Task.Delay(2000);
        var result = await Container.GetAllBytesAsync(blobName);
        result.SequenceEqual(testContentOverwritten).ShouldBeTrue();
        await Container.DeleteAsync(blobName);
    }

    [Fact]
    public async Task Should_Not_Allow_To_Overwrite_Pre_Saved_Blob_By_Default()
    {
        var blobName = "Should_Not_Allow_To_Overwrite_Pre_Saved_Blob_By_Default";

        var testContent = "test content".GetBytes();
        await Container.SaveAsync(blobName, testContent);
        await Task.Delay(2000);
        var testContentOverwritten = "test content overwritten".GetBytes();
        await Assert.ThrowsAsync<BlobAlreadyExistsException>(() =>
            Container.SaveAsync(blobName, testContentOverwritten)
        );

        await Container.DeleteAsync(blobName);
    }

    [Theory]
    [InlineData("Should_Delete_Saved_Blobs")]
    [InlineData("Should_Delete_Saved_Blobs.txt")]
    [InlineData("test-folder/Should_Delete_Saved_Blobs")]
    public async Task Should_Delete_Saved_Blobs(string blobName)
    {
        await Container.SaveAsync(blobName, "test content".GetBytes());
        await Task.Delay(2000);
        (await Container.GetAllBytesAsync(blobName)).ShouldNotBeNull();

        await Container.DeleteAsync(blobName);
        await Task.Delay(2000);
        (await Container.GetAllBytesOrNullAsync(blobName)).ShouldBeNull();
    }

    [Theory]
    [InlineData("Saved_Blobs_Should_Exists")]
    [InlineData("Saved_Blobs_Should_Exists.txt")]
    [InlineData("test-folder/Saved_Blobs_Should_Exists")]
    public async Task Saved_Blobs_Should_Exists(string blobName)
    {
        await Container.SaveAsync(blobName, "test content".GetBytes());
        await Task.Delay(2000);
        (await Container.ExistsAsync(blobName)).ShouldBeTrue();

        await Container.DeleteAsync(blobName);
        await Task.Delay(2000);
        (await Container.ExistsAsync(blobName)).ShouldBeFalse();
    }

    [Theory]
    [InlineData("Unknown_Blobs_Should_Not_Exists")]
    [InlineData("Unknown_Blobs_Should_Not_Exists.txt")]
    [InlineData("test-folder/Unknown_Blobs_Should_Not_Exists")]
    public async Task Unknown_Blobs_Should_Not_Exists(string blobName)
    {
        await Container.DeleteAsync(blobName);
        await Task.Delay(2000);
        (await Container.ExistsAsync(blobName)).ShouldBeFalse();
    }
}
