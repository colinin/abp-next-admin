using Shouldly;
using Volo.Abp.BlobStoring;
using Xunit;

namespace LINGYUN.Abp.BlobStoring.Tencent;

public class DefaultTencentBlobNamingNormalizerProvider_Tests : AbpBlobStoringTencentTestBase
{
    private readonly IBlobNamingNormalizer _blobNamingNormalizer;

    public DefaultTencentBlobNamingNormalizerProvider_Tests()
    {
        _blobNamingNormalizer = GetRequiredService<IBlobNamingNormalizer>();
    }

    [Fact]
    public void NormalizeContainerName_Lowercase()
    {
        var filename = "ThisIsMyContainerName";
        filename = _blobNamingNormalizer.NormalizeContainerName(filename);
        filename.ShouldBe("thisismycontainername");
    }

    [Fact]
    public void NormalizeContainerName_Only_Letters_Numbers_Dash()
    {
        var filename = ",./this-i,./s-my-c,./ont,./ai+*/.=!@#$n^&*er-name.+/";
        filename = _blobNamingNormalizer.NormalizeContainerName(filename);
        filename.ShouldBe("this-is-my-container-name");
    }

    [Fact]
    public void NormalizeContainerName_Max_Length()
    {
        var filename = "abpabpabpabpabpabpabpabpabpabpabpabpabpabpabpabpabpabpabpabpabpabpabp";
        filename = _blobNamingNormalizer.NormalizeContainerName(filename);
        filename.Length.ShouldBeLessThanOrEqualTo(63);
    }

    [Fact]
    public void NormalizeContainerName_Max_Length_Dash()
    {
        var filename = "-this-is-my-container-name-abpabpabpabpabpabpabpabp-a-b-p-a--b-p-";
        filename = _blobNamingNormalizer.NormalizeContainerName(filename);
        filename.ShouldBe("this-is-my-container-name-abpabpabpabpabpabpabpabp-a-b-p-a--");
    }

}
