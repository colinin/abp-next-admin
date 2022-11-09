using LINGYUN.Abp.IdGenerator.Snowflake;
using System;
using System.Linq;
using Shouldly;
using Xunit;

namespace LINGYUN.Abp.IdGenerator;

public class SnowflakeIdGeneratorTests : AbpIdGeneratorModuleTestBase
{
    public SnowflakeIdGeneratorTests()
    {
        DistributedIdGenerator = null;
    }

    [Theory]
    [InlineData(-1, 5)]
    [InlineData(0, 5)]
    [InlineData(10, 5)]
    [InlineData(31, 5)]
    [InlineData(32, 5)]
    [InlineData(30010, 5)]
    public void WorkerIdShouldInRange(int workerId, int workerIdBits)
    {
        var snowflakeIdGenerator = SnowflakeIdGenerator.Create(new SnowflakeIdOptions
        { 
            WorkerId = workerId, 
            WorkerIdBits = workerIdBits 
        });

        snowflakeIdGenerator.WorkerId.ShouldBeInRange(0, (long)Math.Pow(2, workerIdBits) - 1);
    }

    [Fact]
    public void BiggerWorkerIdMakeIdRepeated()
    {
        var snowflakeIdGenerator = SnowflakeIdGenerator.Create(new SnowflakeIdOptions
        {
            WorkerId = 30010,
            WorkerIdBits = 5
        });
        snowflakeIdGenerator.GetType().GetProperty("WorkerId").SetValue(snowflakeIdGenerator, 30010);
        DistributedIdGenerator = snowflakeIdGenerator;

        const int countIds = 50000;
        long[] ids = GenerateIdInSingleThread(countIds);

        var finalIds = ids.GroupBy(id => id).Select(d => d.Key).ToArray();
        finalIds.Length.ShouldBeLessThan(countIds);
    }
}
