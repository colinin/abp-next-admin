using Shouldly;
using System.Linq;
using Xunit;

namespace LINGYUN.Abp.IdGenerator;

public class IdGeneratorTests : AbpIdGeneratorModuleTestBase
{
    public IdGeneratorTests()
    {
        DistributedIdGenerator = GetRequiredService<IDistributedIdGenerator>();
    }

    [Fact]
    public void IdShouldBeUnique_SingleThread()
    {
        const int countIds = 50000;
        long[] ids = GenerateIdInSingleThread(countIds);

        var finalIds = ids.GroupBy(id => id).Select(d => d.Key).ToArray();
        finalIds.Length.ShouldBe(countIds);
    }

    [Fact]
    public void IdShouldBeUnique_MultiThread()
    {
        const int countIdsPerThread = 50000;
        const int countThreads = 8;
        long[] ids = GenerateIdInMutiThread(countThreads, countIdsPerThread);

        var finalIds = ids.GroupBy(id => id).Select(d => d.Key).ToArray();
        finalIds.Length.ShouldBe(countThreads * countIdsPerThread);
    }
}
