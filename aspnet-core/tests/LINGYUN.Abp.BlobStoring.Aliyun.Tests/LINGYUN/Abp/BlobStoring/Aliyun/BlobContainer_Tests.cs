using LINGYUN.Abp.BlobStoring.TestObjects;
using Shouldly;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;
using Xunit;

namespace LINGYUN.Abp.BlobStoring.Aliyun
{
    public class BlobContainer_Tests : AbpBlobStoringAliyunTestBase
    {
        protected IBlobContainer<TestContainer1> Container { get; }
        public BlobContainer_Tests()
        {
            Container = GetRequiredService<IBlobContainer<TestContainer1>>();
        }

        [Theory]
        [InlineData("test-blob-1")]
        [InlineData("test-blob-1.txt")]
        [InlineData("test-folder/test-blob-1")]
        public async Task Should_Save_And_Get_Blobs(string blobName)
        {
            var testContent = "test content".GetBytes();
            await Container.SaveAsync(blobName, testContent);
            try
            {
                var resultBytes = await Container.GetAllBytesAsync(blobName);
                resultBytes.SequenceEqual(testContent).ShouldBeTrue();
            }
            finally
            {
                await Container.DeleteAsync(blobName);
            }
        }

        [Fact]
        public async Task Should_Overwrite_Pre_Saved_Blob_If_Requested()
        {
            var blobName = "test-blob-1";

            var testContent = "test content".GetBytes();
            await Container.SaveAsync(blobName, testContent);

            var testContentOverwritten = "test content overwritten".GetBytes();
            await Container.SaveAsync(blobName, testContentOverwritten, true);

            var result = await Container.GetAllBytesAsync(blobName);
            result.SequenceEqual(testContentOverwritten).ShouldBeTrue();

            await Container.DeleteAsync(blobName);
        }


        [Fact]
        public async Task Should_Not_Allow_To_Overwrite_Pre_Saved_Blob_By_Default()
        {
            var blobName = "test-blob-1";

            var testContent = "test content".GetBytes();
            await Container.SaveAsync(blobName, testContent);

            var testContentOverwritten = "test content overwritten".GetBytes();
            await Assert.ThrowsAsync<BlobAlreadyExistsException>(() =>
                Container.SaveAsync(blobName, testContentOverwritten)
            );

            await Container.DeleteAsync(blobName);
        }

        [Theory]
        [InlineData("test-blob-1")]
        [InlineData("test-blob-1.txt")]
        [InlineData("test-folder/test-blob-1")]
        public async Task Should_Delete_Saved_Blobs(string blobName)
        {
            await Container.SaveAsync(blobName, "test content".GetBytes());
            (await Container.GetAllBytesAsync(blobName)).ShouldNotBeNull();

            await Container.DeleteAsync(blobName);
            (await Container.GetAllBytesOrNullAsync(blobName)).ShouldBeNull();
        }

        [Theory]
        [InlineData("test-blob-1")]
        [InlineData("test-blob-1.txt")]
        [InlineData("test-folder/test-blob-1")]
        public async virtual Task Saved_Blobs_Should_Exists(string blobName)
        {
            await Container.SaveAsync(blobName, "test content".GetBytes());
            (await Container.ExistsAsync(blobName)).ShouldBeTrue();

            await Container.DeleteAsync(blobName);
            (await Container.ExistsAsync(blobName)).ShouldBeFalse();
        }
    }
}
