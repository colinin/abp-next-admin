using Shouldly;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Testing;
using Xunit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LINGYUN.Abp.OssManagement;
public abstract class OssContainer_Tests<TStartupModule> : AbpIntegratedTest<TStartupModule>
        where TStartupModule : IAbpModule
{
    private readonly ICurrentTenant _currentTenant;
    private readonly IOssContainerFactory _ossContainerFactory;

    protected OssContainer_Tests()
    {
        _currentTenant = GetRequiredService<ICurrentTenant>();
        _ossContainerFactory = GetRequiredService<IOssContainerFactory>();
    }

    [Theory]
    [InlineData("test-bucket")]
    public async virtual Task Should_Creat_And_Get_Bucket(string bucket)
    {
        var container = _ossContainerFactory.Create();

        var ossContainer = await container.CreateAsync(bucket);

        ossContainer.ShouldNotBeNull();
        ossContainer.Name.ShouldBe(bucket);

        await container.DeleteAsync(bucket);
    }

    [Theory]
    [InlineData("test-bucket")]
    public async virtual Task Should_Get_Exists_Bucket(string bucket)
    {
        var container = _ossContainerFactory.Create();

        await container.CreateAsync(bucket);

        var getContainer = await container.GetAsync(bucket);

        getContainer.ShouldNotBeNull();
        getContainer.Name.ShouldBe(bucket);

        await container.DeleteAsync(bucket);
    }

    [Theory]
    [InlineData("test-bucket")]
    public async virtual Task Should_Delete_Bucket(string bucket)
    {
        var container = _ossContainerFactory.Create();

        await container.CreateAsync(bucket);

        await container.DeleteAsync(bucket);

        var getNotFoundException = await Assert.ThrowsAsync<BusinessException>(async () =>
            await container.GetAsync(bucket)
        );

        getNotFoundException.Code.ShouldBe(OssManagementErrorCodes.ContainerNotFound);

        var deleteNotFoundException = await Assert.ThrowsAsync<BusinessException>(async () =>
            await container.DeleteAsync(bucket)
        );

        deleteNotFoundException.Code.ShouldBe(OssManagementErrorCodes.ContainerNotFound);
    }

    [Theory]
    [InlineData("test-bucket", "test-object-1")]
    [InlineData("test-bucket", "test-folder/test-object-1")]
    public async virtual Task Should_Save_And_Get_Object(string bucket, string @object)
    {
        var container = _ossContainerFactory.Create();

        if (!await container.ExistsAsync(bucket))
        {
            await container.CreateAsync(bucket);
        }

        var testContent = "test content".GetBytes();
        using var stream = new MemoryStream(testContent);

        var createObject = await container.CreateObjectAsync(
            new CreateOssObjectRequest(
                bucket,
                @object,
                stream));

        var getOssObject = await container.GetObjectAsync(
            new GetOssObjectRequest(
                bucket,
                @object));

        var  result = await getOssObject.Content.GetAllBytesAsync();
        result.SequenceEqual(testContent).ShouldBeTrue();

        await container.DeleteObjectAsync(
            new GetOssObjectRequest(
                bucket,
                @object));

        await container.DeleteAsync(bucket);
    }

    [Theory]
    [InlineData("test-bucket", "test-object-1")]
    [InlineData("test-bucket", "test-folder/test-object-1")]
    public async virtual Task Should_Delete_Object(string bucket, string @object)
    {
        var container = _ossContainerFactory.Create();

        if (!await container.ExistsAsync(bucket))
        {
            await container.CreateAsync(bucket);
        }

        var testContent = "test content".GetBytes();
        using var stream = new MemoryStream(testContent);

        await container.CreateObjectAsync(
            new CreateOssObjectRequest(
                bucket,
                @object,
                stream));

        var getOssObjectRequest = new GetOssObjectRequest(
            bucket,
            @object);
        await container.DeleteObjectAsync(getOssObjectRequest);

        var getNotFoundException = await Assert.ThrowsAsync<BusinessException>(async () =>
            await container.GetObjectAsync(getOssObjectRequest)
        );

        getNotFoundException.Code.ShouldBe(OssManagementErrorCodes.ObjectNotFound);

        await container.DeleteAsync(bucket);
    }

    [Theory]
    [InlineData("test-bucket")]
    public async virtual Task Should_Bulk_Delete_Object(string bucket)
    {
        var container = _ossContainerFactory.Create();

        if (!await container.ExistsAsync(bucket))
        {
            await container.CreateAsync(bucket);
        }

        string[] testObjects = ["test-object-1", "test-object-2", "test-object-3"];
        var testContent = "test content".GetBytes();
        using var stream = new MemoryStream(testContent);

        foreach (var testObject in testObjects)
        {
            await container.CreateObjectAsync(
                new CreateOssObjectRequest(
                    bucket,
                    testObject,
                    stream));
        }

        await container.BulkDeleteObjectsAsync(
            new BulkDeleteObjectRequest(bucket, testObjects, "/"));

        var getNotFoundException = await Assert.ThrowsAsync<BusinessException>(async () =>
            await container.GetObjectAsync(
                 new GetOssObjectRequest(
                    bucket,
                    testObjects[0])
            )
        );

        getNotFoundException.Code.ShouldBe(OssManagementErrorCodes.ObjectNotFound);

        await container.BulkDeleteObjectsAsync(
            new BulkDeleteObjectRequest(
                bucket, testObjects, "/"));

        await container.DeleteAsync(bucket);
    }

    [Theory]
    [InlineData("test-bucket")]
    public async virtual Task Should_List_Object(string bucket)
    {
        var container = _ossContainerFactory.Create();

        if (!await container.ExistsAsync(bucket))
        {
            await container.CreateAsync(bucket);
        }

        string[] testObjects = [
            "test-object-1", 
            "test-object-2", 
            "test-object-3", 
            "test-object-4", 
            "test-object-5"];
        var testContent = "test content".GetBytes();
        using var stream = new MemoryStream(testContent);

        foreach (var testObject in testObjects)
        {
            await container.CreateObjectAsync(
                new CreateOssObjectRequest(
                    bucket,
                    testObject,
                    stream));
        }

        var result1 = await container.GetObjectsAsync(
            new GetOssObjectsRequest(
                bucket,
                "/",
                maxKeys: 1));

        result1.Objects.Count.ShouldBe(1);
        result1.Objects[0].Name.ShouldBe(testObjects[0]);

        var result2 = await container.GetObjectsAsync(
           new GetOssObjectsRequest(
               bucket,
                "/",
               current: 2,
               maxKeys: 2));
        result2.Objects.Count.ShouldBe(2);
        result2.Objects[1].Name.ShouldBe(testObjects[3]);

        await container.BulkDeleteObjectsAsync(
            new BulkDeleteObjectRequest(
                bucket, testObjects, "/"));

        await container.DeleteAsync(bucket);
    }
}
