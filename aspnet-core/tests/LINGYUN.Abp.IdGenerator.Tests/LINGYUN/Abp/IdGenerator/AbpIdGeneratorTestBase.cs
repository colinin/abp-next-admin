using LINGYUN.Abp.Tests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LINGYUN.Abp.IdGenerator;

public class AbpIdGeneratorModuleTestBase : AbpTestsBase<AbpIdGeneratorModuleTestModule>
{
    protected IDistributedIdGenerator DistributedIdGenerator { get; set; }

    private void GenerateId(long[] idArray, int startIndex, int endIndex)
    {
        for (int i = startIndex; i < endIndex; i++)
        {
            idArray[i] = DistributedIdGenerator.Create();
        }
    }

    protected long[] GenerateIdInSingleThread(int countIds)
    {
        long[] ids = new long[countIds];

        GenerateId(ids, 0, ids.Length);

        return ids;
    }

    protected long[] GenerateIdInMutiThread(int countThreads, int countIdsPerThread)
    {
        List<Task> tasks = new();
        long[] idArray = new long[countThreads * countIdsPerThread];

        for (int i = 0; i < countThreads; i++)
        {
            int threadId = i;
            tasks.Add(Task.Run(() => GenerateId(idArray, threadId * countIdsPerThread, (threadId + 1) * countIdsPerThread)));
        }
        Task.WaitAll(tasks.ToArray());

        return idArray;
    }
}
